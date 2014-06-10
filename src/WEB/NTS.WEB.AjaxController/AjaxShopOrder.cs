using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.WEB.DataContact;

namespace NTS.WEB.AjaxController
{
    public class AjaxShopOrder
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;
        [Framework.Common.CustomAjaxMethod]
        public ResultView.ShopOrderResult GetShopOrder()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryOrder>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery").GetShopOrder(query);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultView.ResultOrder GetShopOrderNew()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var Param = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryOrderObjects>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery").GetShopOrderNew(Param);
            return res;
        }
    }
}
