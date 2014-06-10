using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.WEB.Common;
using NTS.WEB.Model;

namespace NTS.WEB.AjaxController
{
    public class AjaxBalanceAnalysis
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public string GetBalanaceValueByMonth()
        {
            string resultStr = "";
            try
            {
                var inputValue = _ntsPage.Request["input"];
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<BalanceAnalysisModel>(inputValue);
                var dt =
                    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IBalanceAnalysisService>("BalanceAnalysis").
                        GetBalanaceValueByMonth(query);
                int Total =
                    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IBalanceAnalysisService>("BalanceAnalysis").
                        GetChildAreaCount(query.PageSize, query.ObjectNum);




                string jsonstring = dt.Rows.Count > 0 ? Strings.ToJsonWithOrder((query.PageCurrent - 1) * query.PageSize, dt) : "{}";
                resultStr = "{\"ActionInfo\":[{ \"Success\": true,\"ExceptionMsg\": \" \"}],\"Data\":{\"Rows\":" + jsonstring + ",\"Page\":{\"Current\":" + query.PageCurrent + ",\"Total\":" + Total + "}}}";
            }
            catch (Exception ee)
            {
                resultStr = "{\"ActionInfo\":[{ \"Success\": false,\"ExceptionMsg\": " + ee.Message + "}]}";
            }
            return resultStr;
        }
    }
}
