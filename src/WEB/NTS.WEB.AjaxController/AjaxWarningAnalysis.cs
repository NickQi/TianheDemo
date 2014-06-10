using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using NTS.WEB.Common;
using NTS.WEB.Model;

namespace NTS.WEB.AjaxController
{
    public class AjaxWarningAnalysis
    {
       private readonly HttpContext _ntsPage = HttpContext.Current;
      
        [Framework.Common.CustomAjaxMethod]
        public string GetWarningListByPage()
       {
           string resultStr = "";
            try
            {
                var inputValue = _ntsPage.Request["input"]; ;
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<WarningAnalysisModel>(inputValue);
                var dt =
                    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IWarningAnalysisService>("WarningAnalysis").GetWarningListByPage(query);
                int Total = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IWarningAnalysisService>("WarningAnalysis").GetWarningPageCount(query);

                string jsonstring = dt.Rows.Count > 0 ? Strings.ToJsonWithOrder((query.PageCurrent - 1) * query.PageSize, dt) : "{}";
                resultStr = "{\"ActionInfo\":[{ \"Success\": true,\"ExceptionMsg\": \" \"}],\"Data\":{\"Rows\":" + jsonstring + ",\"Page\":{\"Current\":" + query.PageCurrent + ",\"Total\":" + Total + "}}}";
            }
            catch(Exception ee)
            {
                resultStr = "{\"ActionInfo\":[{ \"Success\": false,\"ExceptionMsg\": " + ee.Message + "}]}";
            }

         
            return resultStr;

        }
        [Framework.Common.CustomAjaxMethod]
        public string GetWarningTypeList()
        {
            string resultStr = "";
            try
            {
                var result =
            Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IWarningAnalysisService>("WarningAnalysis").GetWarningTypeList();

                //string warningstring = NTS_BECM.Common.BaseClass.Json.SerializeObject(service.GetWarningTypeList());
                var warningstring = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                //string resultStr = "{\"Success\":true,\"Msg\":\"\",\"Data\":{\"Rows\":" + warningstring + "}}";
                resultStr = "{\"ActionInfo\":[{ \"Success\": true,\"ExceptionMsg\": \" \"}],\"Data\":" + warningstring + "}";

            }
            catch(Exception ee)
            {
                resultStr = "{\"ActionInfo\":[{ \"Success\": false,\"ExceptionMsg\": " + ee.Message + "}]}";
            }
          

            return resultStr;
        }
        
    }
}
