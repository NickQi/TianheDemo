using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.EMS.Config.Model;
using System.Data;
using System.IO;
using NTS.EMS.Config.BLL;


namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxQuotaInfo
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        /// <summary>
        /// 定额查询
        /// </summary>
        /// <returns></returns>
        [Framework.Ajax.CustomAjaxMethod]
        [Framework.LogAndException.CustomException]
        public ResultQuotaInfo GetQuotaInfo()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryQuotaContact>(inputValue);
            var result = new OperateQuotaBll().GetQuotaInfo(query);
            return result;
        }

        /// <summary>
        /// 定额日志查询
        /// </summary>
        /// <returns></returns>
        [Framework.Ajax.CustomAjaxMethod]
        [Framework.LogAndException.CustomException]
        public ResultQuotaLogs GetQuotaLogs()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryQuotaLogContract>(inputValue);
            var result = new OperateQuotaBll().GetQuotaLogs(query);
            return result;
        }

        /// <summary>
        /// 添加或修改定额数据
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "定额配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public int DealQuota()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QuotaDataContact>(inputValue);
            query.UserName = Framework.Common.Utils.GetCookie("userid");
            var result = new OperateQuotaBll().InsertOrEditQuota(query);

            return result;
        }

    }
}
