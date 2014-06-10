using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Ajax;
using NTS.EMS.Config.BLL;

namespace NTS.EMS.Config.AjaxHandler
{

    public class AjaxTree
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;
        [CustomAjaxMethod]
        public string ObjectTree()
        {
            var cacheTree = NTS.WEB.Common.CacheHelper.GetCache("object-tree");
            if (cacheTree != null)
            {

                return cacheTree.ToString();
            }
            //  var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetObjectTree();
            var res = new BaseTree().GetObjectTree();
            NTS.WEB.Common.CacheHelper.SetCache("object-tree", res.TreeJson);
            return res.TreeJson;
        }

        /// <summary>
        /// add by jy （区域树+页树）
        /// </summary>
        /// <returns></returns>
        [CustomAjaxMethod]
        public string objectItemTree()
        {
            //string itemcode = "01A00";
            //int classid = 1;

            string itemcode = _ntsPage.Request["ItemCode"].ToString();
            int classid = int.Parse(_ntsPage.Request["ClassId"].ToString());
            string strCacheName = "object-tree" + itemcode + classid;

            var cacheTree = NTS.WEB.Common.CacheHelper.GetCache(strCacheName);

            if (cacheTree != null)
            {

                return cacheTree.ToString();
            }
            //  var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetObjectTree();
            var res = new BaseTree(itemcode, classid, "").GetObjectTree();
            NTS.WEB.Common.CacheHelper.SetCache(strCacheName, res.TreeJson);
            return res.TreeJson;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private bool IsJoinItemCode(string areaid)
        //{

        //}

        [CustomAjaxMethod]
        public string DeviceTree()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            if (string.IsNullOrEmpty(inputValue))
            {
                var cacheTree = NTS.WEB.Common.CacheHelper.GetCache("device-tree");
                if (cacheTree != null)
                {
                    //var treeObject = cacheTree.ToString();
                    return cacheTree.ToString();
                }
                var res = new BaseTree().GetDeviceTree();
                NTS.WEB.Common.CacheHelper.SetCache("device-tree", res.TreeJson);
                // Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetDeviceTree();
                return res.TreeJson;
            }
            else
            {

                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<NTS.WEB.DataContact.QueryDevice>(inputValue);
                var res = new BaseTree().GetDeviceListByArea(query);
                //Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetDeviceListByArea(query);
                return res;
                // return Newtonsoft.Json.JsonConvert.SerializeObject(res);
            }
            //return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
    }
}
