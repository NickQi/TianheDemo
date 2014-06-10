using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NTS.WEB.Common
{
  
        public class DataTool
    {
        /// <summary>
        /// 合并datable
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newdt"></param>
        /// <returns></returns>
        public static DataTable UniteDataTable(DataTable old, DataTable newdt)
        {
            object[] obj = new object[old.Columns.Count];

            for (int i = 0; i < newdt.Rows.Count; i++)
            {
                newdt.Rows[i].ItemArray.CopyTo(obj, 0);
                old.Rows.Add(obj);
            }
            return old;
        }

        public static DataTable GetPaddingLimitTable(DataTable dt, int pages, int pagesize)
        {
            int startnum = (pages - 1) * pagesize;

            if (dt.Rows.Count > 0)
            {
                DataTable new_dt = dt.Clone();
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int others = dt.Rows.Count - startnum;
                    others = others < pagesize ? others : pagesize;
                    if (i >= startnum && i < startnum + others)
                    {
                        new_dt.ImportRow(dr);
                    }
                    //else break;
                    i++;
                }
                return new_dt;
            }
            return dt;
        }

        /// <summary>
        /// DataView转为datatable
        /// </summary>
        /// <param name="obDataView"></param>
        /// <returns></returns>
        public static DataTable DataView2DataTable(DataView obDataView)
        {
            if (null == obDataView)
            {
                throw new ArgumentNullException
                ("DataView", "Invalid DataView object specified");
            }

            DataTable obNewDt = obDataView.Table.Clone();
            int idx = 0;
            string[] strColNames = new string[obNewDt.Columns.Count];

            foreach (DataColumn col in obNewDt.Columns)
            {
                strColNames[idx++] = col.ColumnName;
            }

            IEnumerator viewEnumerator = obDataView.GetEnumerator();
            while (viewEnumerator.MoveNext())
            {
                DataRowView drv = (DataRowView)viewEnumerator.Current;
                DataRow dr = obNewDt.NewRow();
                try
                {
                    foreach (string strName in strColNames)
                    {
                        dr[strName] = drv[strName];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                obNewDt.Rows.Add(dr);
            }

            return obNewDt;
        }
    }
}
