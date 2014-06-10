using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NTS.WEB.Common
{
   public class DataTableToTree
    {
        public static string TableToTreeJson(DataTable dt, string pField, string pValue, string kField, string TextField)
        {
            string result = TableToTreeJson2(dt, pField, pValue, kField, TextField);
            if (result.IndexOf('[') > 0)
            {
                result = result.Substring(result.IndexOf('['));
            }

            return result;
        }

        public static string TableToTreeJson2(DataTable dt, string pField, string pValue, string kField, string TextField)
        {
            StringBuilder sb = new StringBuilder();
            string filter = String.Format(" {0}='{1}' ", pField, pValue);//获取顶级目录.
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
                sb.AppendFormat("\"text\":\"{0}\"", dr[TextField].ToString());
                sb.Append(TableToTreeJson2(dt, pField, pcv, kField, TextField).TrimEnd(','));
                sb.Append("},");
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");
            return sb.ToString();
            // return sb.ToString().Substring(sb.ToString().IndexOf('['));

        }
    }
}
