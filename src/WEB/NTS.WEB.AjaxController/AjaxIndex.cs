using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.WEB.Common;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.WEB.AjaxController
{
    public class AjaxIndex
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;
        [Framework.Common.CustomAjaxMethod]
        public ResultView.IndexWindowResult IndexMap()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryIndexWindow>(inputValue);
            var res= Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IIndexEnery>("Index").GetIndexWindowResult(query);
            return res;
        }


        [Framework.Common.CustomAjaxMethod]
        public ResultView.IndexMonthEnery IndexMonthEnery()
        {
            var res= Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IIndexEnery>("Index").GetIndexMonthEneryResult();
            return res;
        }
        
        [Framework.Common.CustomAjaxMethod]
        public ResultView.IndexCompareEnery IndexCompare()
        {
            var res= Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IIndexEnery>("Index").GetIndexCompareEnery();
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultView.MainInfo IndexCompareNew()
        {

            string keyCatch = "NTS" + DateTime.Now.ToString("yyyyMMddhh");
            if (CacheHelper.GetCache(keyCatch) != null)
            {
                return (MainInfo)CacheHelper.GetCache(keyCatch);
            }
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IIndexEnery>("Index").GetIndexCompareEneryNew();
            if (CacheHelper.GetCache(keyCatch) == null)
            {
                CacheHelper.SetCache(keyCatch, res);
            }
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public IndexShopOrder IndexOrderList()
        {
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IIndexEnery>("Index").GetIndexShopOrder();
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public IndexLimit IndexLook()
        {
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IIndexEnery>("Index").GetIndexLimit();
            return res;
        }

        
    }
}
