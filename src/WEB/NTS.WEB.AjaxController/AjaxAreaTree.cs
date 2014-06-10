using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using NTS.WEB.Common;
using NTS.WEB.Model;

namespace NTS.WEB.AjaxController
{
    public class AjaxAreaTree
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public string GetAreaTree()
        {
            string resultStr = "";
            try
            {
                //var inputValue = _ntsPage.Request["input"];
                //var query = Newtonsoft.Json.JsonConvert.DeserializeObject<BalanceAnalysisModel>(inputValue);
               
                var dt =
                    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAreaTreeService>("AreaTree").GetAreaTree();

                if (dt == null)
                {
                    return "";
                }
                int levellimit = Int32.MaxValue;
                int expandedlimit = -1;
                if (_ntsPage.Request.QueryString["levellimit"] != null)
                {
                    try
                    {
                        levellimit = Convert.ToInt32(_ntsPage.Request.QueryString["levellimit"]);
                    }
                    catch
                    {
                     
                    }

                }
                if (_ntsPage.Request.QueryString["expandedlevel"] != null)
                {
                    try
                    {
                        expandedlimit = Convert.ToInt32(_ntsPage.Request.QueryString["expandedlevel"]);
                     
                    }
                    catch
                    {
                        //return TableToTreeJson(dt, "layerobjectparentid", "0", "layerobjectid", "layerobjectname", 0);
                    }

                }

                return TableToTreeJson(dt, "layerobjectparentid", "0", "layerobjectid", "layerobjectname", 0, levellimit, expandedlimit);


                // return TableToTreeJson(dt, "layerobjectparentid", "0", "layerobjectid", "layerobjectname", 0);
               

            }
            catch (Exception ee)
            {
                resultStr = "";
            }
            return resultStr;
        }

        private static string TableToTreeJson(DataTable dt, string pField, string pValue, string kField, string TextField, int level)
        {
            string result = TableToTreeJson3(dt, pField, pValue, kField, TextField, level);
            if (result.IndexOf('[') > 0)
            {
                result = result.Substring(result.IndexOf('['));
            }
            return result;
        }
        private static string TableToTreeJson(DataTable dt, string pField, string pValue, string kField, string TextField, int level, int limit, int expandedlimit)
        {
            string result = TableToTreeJson3(dt, pField, pValue, kField, TextField, level, limit, expandedlimit);
            if (result.IndexOf('[') > 0)
            {
                result = result.Substring(result.IndexOf('['));
            }
            return result;
        }
        private static string TableToTreeJson3(DataTable dt, string pField, string pValue, string kField, string TextField, int level)
        {
            level++;
            StringBuilder sb = new StringBuilder();
            string filter = String.Format("{0}='{1}' ", pField, pValue);//获取顶级目录.
            DataRow[] drs = dt.Select(filter);
            if (drs.Length < 1)
            {
                return "";
            }
            sb.Append(",\"children\":[");
            foreach (DataRow dr in drs)
            {
                string pcv = dr[kField].ToString();
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",", dr[kField].ToString());
                sb.AppendFormat("\"text\":\"{0}\",", dr[TextField].ToString());
                sb.AppendFormat("\"classes\":\"{0}\",", GetAreaTreeCss(level));

                sb.AppendFormat("\"hasChildren\":false,");

                if (level < 2)
                {
                    sb.AppendFormat("\"expanded\":true");
                }
                else
                {
                    sb.AppendFormat("\"expanded\":false");
                }

                sb.Append(TableToTreeJson3(dt, pField, pcv, kField, TextField, level).TrimEnd(','));
              

                sb.Append("},");
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");
            return sb.ToString();


        }

        private static string TableToTreeJson3(DataTable dt, string pField, string pValue, string kField, string TextField, int level, int limit, int expandedlimit)
        {
           
            level++;
            if (level > limit)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            string filter = String.Format("{0}='{1}' ", pField, pValue);//获取顶级目录.
            DataRow[] drs = dt.Select(filter);
            if (drs.Length < 1)
            {
                return "";
            }
            sb.Append(",\"children\":[");
            foreach (DataRow dr in drs)
            {
                string pcv = dr[kField].ToString();
                sb.Append("{");
                sb.AppendFormat("\"id\":\"{0}\",", dr[kField].ToString());
                sb.AppendFormat("\"text\":\"{0}\",", dr[TextField].ToString());
                sb.AppendFormat("\"classes\":\"{0}\",", GetAreaTreeCss(level));

                sb.AppendFormat("\"hasChildren\":false,");
                //sb.AppendFormat("\"expanded\":false");
                if (expandedlimit > 0)
                {
                    if (level < expandedlimit)
                    {
                        sb.AppendFormat("\"expanded\":true");
                    }
                    else
                    {
                        sb.AppendFormat("\"expanded\":false");
                    }
                }
                else
                {
                    sb.AppendFormat("\"expanded\":true");
                }
               

                sb.Append(TableToTreeJson3(dt, pField, pcv, kField, TextField, level, limit, expandedlimit).TrimEnd(','));


                sb.Append("},");
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");
            return sb.ToString();


        }

        private static string GetAreaTreeCss(int level)
        {
            switch (level)
            {
                case 1:
                    return "project";
                case 2:
                    return "build";
                case 3:
                    return "floor";
                case 4:
                    return "house";
                default:
                    return "equip";
            }
        }
    }
}
