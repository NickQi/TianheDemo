using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Ajax;
using System.Web;
using NTS.EMS.Config.BLL;
using NTS.EMS.Config.Model.QueryFile;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.Model.ResultViewFile;
using NTS.WEB.Common;

namespace NTS.EMS.Config.AjaxHandler
{
    /// <summary>
    /// 分摊配置
    /// </summary>
    public class AjaxAlloction
    {
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string GetTreeObj()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            QueryTreeObj model = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryTreeObj>(inputValue);
            if (model == null)
                return null;

            var result = new AlloctionBLL().GetTreeObjByID(model);

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [AjaxAopBussinessLog(ModelName = "分摊配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string SaveAlloctionAndLog()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var memo = HttpContext.Current.Request.Form["Memo"];

            QueryAlloction model = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlloction>(inputValue);
            if (model == null)
                return null;

            for (int i = 1; i <= model.ListConfig.Count; i++)
            {
                TB_ALLOCTION_CONFIG m = model.ListConfig[(i - 1)];
                m.ALLOCTION_StartDate = m.ALLOCTION_EndDate.AddMonths(-1);
            }
            memo = memo.Replace('\'', '"');

            TB_ALLOCTION_CONFIG_History log = new TB_ALLOCTION_CONFIG_History();
            log.CFGDATE = model.ListConfig[0].ALLOCTION_EndDate;
            log.CFGDEC = memo;
            log.OPTIONUSER = Framework.Common.Utils.GetCookie("userid");
            log.CFGOBJECT = model.ListConfig[0].ParentAREAID;
            log.OPTIONTIME = DateTime.Now;
            log.PAYClass = model.ListConfig[0].PAYClass;
            model.ConfigLog = log;
            var result = new AlloctionBLL().SaveAlloctionAndLog(model);
            result.TreeName = model.ParentName;
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public ResultConfigLog GetConfigLog()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var StartTime = HttpContext.Current.Request.Form["StartTime"];
            var endTime = HttpContext.Current.Request.Form["EndTime"];
            var areaId = HttpContext.Current.Request.Form["AreaID"];
            QueryConfigLog model = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryConfigLog>(inputValue);
            if (model == null)
                return null;

            if (!string.IsNullOrEmpty(StartTime))
            {
                DateTime dt = DateTime.Parse(StartTime.ToString());
                model.StartTime = dt;
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime dt = DateTime.Parse(endTime.ToString());
                model.EndTime = dt;
            }
            if (!string.IsNullOrEmpty(areaId))
            {
                model.AreaID = int.Parse(areaId.ToString());
            }
            ResultConfigLog result = new AlloctionBLL().GetConfigLog(model);
            System.Web.Caching.Cache c = new System.Web.Caching.Cache();

            CacheHelper.SetCache("ConfigLog", result);

            return result;
        }

        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string GetConfigLogDetail()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            ResultConfigLog logList = new ResultConfigLog();
            if (CacheHelper.GetCache("ConfigLog") != null)
            {
                logList = (ResultConfigLog)CacheHelper.GetCache("ConfigLog");
            }
            else
            {
                logList = new AlloctionBLL().GetConfigLog(null);
                CacheHelper.SetCache("ConfigLog", logList);
            }

            BaseConfigLog model = logList.LogList.Where(a => a.SysNo == int.Parse(inputValue.ToString())).FirstOrDefault();
            if (model != null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(model.CFGDEC);
            }
            return "";
        }
    }
}
