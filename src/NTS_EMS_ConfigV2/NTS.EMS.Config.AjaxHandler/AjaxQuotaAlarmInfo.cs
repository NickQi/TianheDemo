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
    public class AjaxQuotaAlarmInfo
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        /// <summary>
        /// 定额告警列表查询
        /// </summary>
        /// <returns></returns>
        [Framework.Ajax.CustomAjaxMethod]
        [Framework.LogAndException.CustomException]
        public ResultQuotaAlarmList GetQuotaAlarmList()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryQuotaAlarmContact>(inputValue);
            var result = new OperateQuotaAlarmBll().GetQuotaAlarmList(query);
            return result;
        }

        /// <summary>
        /// 定额告警信息
        /// </summary>
        /// <returns></returns>
        [Framework.Ajax.CustomAjaxMethod]
        [Framework.LogAndException.CustomException]
        public ResultQuotaAlarm GetQuotaAlarmInfo()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryQuotaAlarmSingle>(inputValue);
            var result = new OperateQuotaAlarmBll().GetQuotaAlarmInfo(query);
            return result;
        }

        /// <summary>
        /// 保存定额告警数据
        /// </summary>
        /// <returns></returns>
        //[AjaxAopBussinessLog(ModelName = "定额告警配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult SaveQuotaAlarm()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QuotaAlarmDataContact>(inputValue);
            var result = new OperateQuotaAlarmBll().SaveQuotaAlarm(query);
            return result;
        }

        /// <summary>
        /// 删除定额告警
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult DeleteQuotaAlarm()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(inputValue);
            var result = new NTS.EMS.Config.BLL.OperateQuotaAlarmBll().DeleteQuotaAlarm(query);
            return result;
        }
    }
}
