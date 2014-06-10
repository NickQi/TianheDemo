using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using Newtonsoft.Json;

namespace NTS_BECM.Common.BaseClass
{
    public class Json
    {
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("\"rows");
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }


        public static string TotalJson(int totalCount, DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append("total");
                json.Append("\":");
                json.Append(totalCount);
                json.Append(",");
                json.Append(DataTable2Json(ds.Tables[0]));
                json.Append("}");
            }
            return json.ToString();
        }


        #region jqueryeasyui_girdtree json
        public  string GridTreeJson(DataTable dt, string strParentColumn, int parentid, string  keyid)
        {
            DataView dvNodeSetsNew = new DataView(dt);
            DataView dvNodeSets = new DataView(dt);
            dvNodeSets.RowFilter = strParentColumn + "=" + parentid;
            dt = dvNodeSets.ToTable();
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
               // jsonBuilder.Append("\"checked\":true,\"state\":\"closed\",");
                jsonBuilder.Append("\"iconCls\":\"icon-ok\",");
                ReChild(dvNodeSetsNew, jsonBuilder, int.Parse(dt.Rows[i][keyid].ToString()), strParentColumn, keyid);
           
               // jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                if (jsonBuilder.ToString().EndsWith(","))
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    //jsonBuilder.Append("]");
                }
                jsonBuilder.Append("},");
            }
           // jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            if (jsonBuilder.ToString().EndsWith(","))
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                //jsonBuilder.Append("]");
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }



        void ReChild(DataView dvNodeSets, StringBuilder jsonBuilder, int parentid, string ParentName, string keyid)
        {
            DataView Old = dvNodeSets;
            dvNodeSets.RowFilter = ParentName + "=" + parentid;
            if (dvNodeSets.Count > 0)
            {
                jsonBuilder.Append("\"children\":[");
                foreach (DataRowView drv in dvNodeSets)
                {
                   // jsonBuilder.Append("{");
                    DataTable dtchild = dvNodeSets.ToTable();
                    for (int k = 0; k < dtchild.Rows.Count; k++)
                    {
                        jsonBuilder.Append("{");
                        //jsonBuilder.Append("{");
                        for (int kj = 0; kj < dtchild.Columns.Count; kj++)
                        {
                            jsonBuilder.Append("\"");
                            jsonBuilder.Append(dtchild.Columns[kj].ColumnName);
                            jsonBuilder.Append("\":\"");
                            jsonBuilder.Append(dtchild.Rows[k][kj].ToString());
                            jsonBuilder.Append("\",");
                        }
                        jsonBuilder.Append("\"state\":\"opened\",");

                        string pid = dtchild.Rows[k][keyid].ToString();
                        ReChild(Old, jsonBuilder, int.Parse(pid), ParentName, keyid);
                        if (jsonBuilder.ToString().EndsWith(","))
                        {
                            jsonBuilder.Remove(jsonBuilder.ToString().Length - 1, 1);
                        }
                        jsonBuilder.Append("},");
                    }
                }
                if (jsonBuilder.ToString().EndsWith(","))
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("]");
                }
                
            }
           // return jsonBuilder;
        }
        #endregion

        public static string TableToEasyUITreeJson(DataTable dt, string pField, string pValue, string kField, string TextField)
        {
            StringBuilder sb = new StringBuilder();
            string filter = String.Format(" {0}='{1}' ", pField, pValue);//获取顶级目录.
            DataRow[] drs = dt.Select(filter);
            if (drs.Length < 1)
                return "";
            sb.Append(",\"children\":[");
            foreach (DataRow dr in drs)
            {
                string pcv = dr[kField].ToString();
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",", dr[kField].ToString());
                sb.AppendFormat("\"text\":\"{0}\"", dr[TextField].ToString());
                sb.Append(TableToEasyUITreeJson(dt, pField, pcv, kField, TextField).TrimEnd(','));
                sb.Append("},");
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            
            return sb.ToString();
        }



        public static string SerializeObject(object obj) 
        {
          return   JsonConvert.SerializeObject(obj);
        }

        



    }
}
