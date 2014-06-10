using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Ajax;
using System.Web;
using NTS.EMS.Config.Model.QueryFile;
using NTS.EMS.Config.BLL;
using NTS.EMS.Config.Model.ResultViewFile;

namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxAlarmSetting
    {
        #region 告警配置
        /// <summary>
        /// 获取告警类型表所有数据
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public ResultAlarmTypes GetAlarmTypeList()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmSetting>(inputValue);
            var result = new AlarmSettingBLL().GetAlarmTypeList(query);
            return result;
        }

        /// <summary>
        /// 获取告警类型表所有数据
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "告警配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public ResultAlarmEvent UpdateAlarmEvent()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmEventUpdate>(inputValue);
            
            var result = new AlarmSettingBLL().UpdateAlarmEvent(query);
            return result;
        }

        [CustomAjaxMethod]
        public ResultAlarmEvent UpdateAlarmEventByID()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmEvent>(inputValue);

            var result = new AlarmSettingBLL().UpdateAlarmEventByID(query);
            return result;
        }

        [CustomAjaxMethod]
        public ResultAlarmEvents GetAlarmEvent()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var result = new AlarmSettingBLL().GetAlarmEventList(int.Parse(inputValue.ToString()));
            return result;
        }
        #endregion

        #region 告警分值配置
        [CustomAjaxMethod]
        public ResultAlarmScaleTypes GetAlarmScaleList()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmSetting>(inputValue);
            var result = new AlarmSettingBLL().GetAlarmScaleList(query);
            return result;
        }

        [CustomAjaxMethod]
        public ResultRate DeleteAlarmScaleByID()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            ResultRate result = new AlarmSettingBLL().DeleteAlarmScaleByID(int.Parse(inputValue));
            return result;
        }

        [CustomAjaxMethod]
        public ResultRate SaveAlarmScale()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmScaleSetting>(inputValue);
            ResultRate result = new AlarmSettingBLL().SaveAlarmScale(query);
            return result;
        }
        #endregion

    }
}
