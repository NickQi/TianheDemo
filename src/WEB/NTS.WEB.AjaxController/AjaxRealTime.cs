using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.WEB.DataContact;

namespace NTS.WEB.AjaxController
{
    public class AjaxRealTime
    {
         private readonly HttpContext _ntsPage = HttpContext.Current;
        [Framework.Common.CustomAjaxMethod]
        public ResultView.ResultReal GetRealTime()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<RealQuery>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery").GetRealTime(query);
            return res;

        }
    }
}
