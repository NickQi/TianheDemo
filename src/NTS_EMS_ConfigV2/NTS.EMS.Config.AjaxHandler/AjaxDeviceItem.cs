using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.EMS.Config.Model;

namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxDeviceItem
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        /// <summary>
        /// 保存设备
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "设备-能源配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public int SaveDeviceProp()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceItemDataContact>(inputValue);
            return new NTS.EMS.Config.BLL.OperateDeviceItemBll().UpdateDeviceProp(query);
            
        }

       /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultDevicePropList GetDeviceProp()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryDeviceItemContact>(inputValue);
            return new NTS.EMS.Config.BLL.OperateDeviceItemBll().GetDevicePropInfo(query);
        }
    }
}
