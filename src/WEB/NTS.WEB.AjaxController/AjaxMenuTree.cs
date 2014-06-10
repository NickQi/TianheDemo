using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using CacheHelper = NTS.WEB.Common.CacheHelper;

namespace NTS.WEB.AjaxController
{
    public class AjaxMenuTree
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public string GetMenuTree()
        {
            string resultStr = "";
            try
            {
                //string username = Utils.GetCookie("userid");
                //var dt =
                //    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IMenuTreeService>("MenuTreeService").GetMenuTree(username);

                //if (dt == null)
                //{
                //    return "";
                //}
                //return TableToTreeJson(dt, "ParentID", "0", "MenuID", "MenuName");


                // return TableToTreeJson(dt, "layerobjectparentid", "0", "layerobjectid", "layerobjectname", 0);
               

            }
            catch (Exception ee)
            {
                resultStr = "";
            }
            return resultStr;
        }


        [Framework.Common.CustomAjaxMethod]
        public ResultMenus GetMenuModule()
        {
            //string keyCatch = "MenuModule";
            //if (CacheHelper.GetCache(keyCatch) != null)
            //{
            //    return (MenuModule)CacheHelper.GetCache(keyCatch);
            //}
            string username = Utils.GetCookie("userid");
            if (!string.IsNullOrEmpty(username))
            {
                var res = new ResultMenus();
               //var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IMenuTreeService>("MenuTreeService").GetMenuModule(username);
                //if (CacheHelper.GetCache(keyCatch) == null)
                //{
                //    CacheHelper.SetCache(keyCatch, res);
                //}
                return res;
            }
            return new ResultMenus();

        }


        [Framework.Common.CustomAjaxMethod]
        public ResultMenus GetMenus()
        {
            //string keyCatch = "Menus";
            //if (CacheHelper.GetCache(keyCatch) != null)
            //{
            //    return (ResultMenus)CacheHelper.GetCache(keyCatch);
            //}
            string username = Utils.GetCookie("userid");
            if (!string.IsNullOrEmpty(username))
            {
                var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IMenuTreeService>("MenuTreeService").GetMenus(username);
                //if (CacheHelper.GetCache(keyCatch) == null)
                //{
                   // CacheHelper.SetCache(keyCatch, res);
                //}
                return res;
            }
            return new ResultMenus();

        }

        private static string TableToTreeJson(DataTable dt, string pField, string pValue, string kField, string TextField)
        {
            string result = TableToTreeJson3(dt, pField, pValue, kField, TextField);
            if (result.IndexOf('[') > 0)
            {
                result = result.Substring(result.IndexOf('['));
            }
            return result;
        }
     
        private static string TableToTreeJson3(DataTable dt, string pField, string pValue, string kField, string TextField)
        {
            
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
                sb.AppendFormat("\"classes\":\"{0}\",", dr["Class"].ToString());
                sb.AppendFormat("\"linkname\":\"{0}\"", dr["LinkName"].ToString());
                sb.Append(TableToTreeJson3(dt, pField, pcv, kField, TextField).TrimEnd(','));
                sb.Append("},");
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");
            return sb.ToString();


        }

       

       
    }
}
