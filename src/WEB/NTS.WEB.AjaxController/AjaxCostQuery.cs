using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using Framework.Common;
using Framework.Configuration;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
namespace NTS.WEB.AjaxController
{

    public class AjaxCostQuery
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public ResultCostQuery GetCostQuery()
        {
            try
            {
                var inputValue = _ntsPage.Request.Form["Inputs"];
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryCost>(inputValue);
                var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.ICostQueryService>("CostQueryService").GetCostQuery(query);
            
                return res;
            }
            catch (Exception ee)
            {
                throw ee;
            }

        }

       
    }

   
}
