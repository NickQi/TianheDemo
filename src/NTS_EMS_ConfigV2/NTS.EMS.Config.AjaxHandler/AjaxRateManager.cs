using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Ajax;
using NTS.EMS.Config.BLL;
using NTS.EMS.Config.Model.QueryFile;
using NTS.EMS.Config.Model.ResultViewFile;

namespace NTS.EMS.Config.AjaxHandler
{
    /// <summary>
    /// 费率设置
    /// </summary>
    public class AjaxRateManager
    {
        /// <summary>
        /// 平价模式 新增、更新
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "费率配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string SaveCommPrice()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            QueryComm model = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComm>(inputValue);
            if (model == null)
                return null;
            //ResultRate result = new RateBLL().SaveCommPrice(model);

            ResultRate result = new RateBLL().SaveParValue(model);

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 阶梯模式 新增、修改
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "费率配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string SaveRatePrice()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            List<QueryRate> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QueryRate>>(inputValue);
            if (list == null)
                return null;
            //ResultRate result = new RateBLL().SaveRatePrice(list);
            ResultRate result = new RateBLL().SaveMultiStep(list);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 分时模式 新增/修改
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "费率配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string SaveTimePrice()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            List<QueryTime> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QueryTime>>(inputValue);
            if (list == null)
                return null;
            //ResultRate result = new RateBLL().SaveTimePrice(model);
            ResultRate result = new RateBLL().SavePeriod(list);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 根据能源类型获取费率信息
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string GetRateInfoList()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            //ResultRatePriceList result = new RateBLL().GetRateInfoList(inputValue, "2", "3");
            ResultRatePriceList result = new RateBLL().GetRateList(inputValue);
            result.PeroidFlag = GetPeroidList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string GetPeriodEnum()
        {
            List<PeroidFlag> listPeriod = GetPeroidList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(listPeriod);
        }

        public List<PeroidFlag> GetPeroidList()
        {
            List<PeroidFlag> listPeriod = new List<PeroidFlag>();
            foreach (NTS.EMS.Config.Model.RateType d in Enum.GetValues(typeof(NTS.EMS.Config.Model.RateType)))
            {
                PeroidFlag m = new PeroidFlag();
                m.ID = (int)d;
                m.Name = d.ToString();
                listPeriod.Add(m);
            }
            return listPeriod;
        }

        /// <summary>
        /// 分类分项联动
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string IndexItem()
        {
            //var inputValue = HttpContext.Current.Request.Form["Inputs"];
            //var query = Newtonsoft.Json.JsonConvert.DeserializeObject<NTS.WEB.DataContact.QueryEnergyIterm>(inputValue);
            ItemList result = new RateBLL().GetItemcodeList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 删除阶梯
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "费率配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string DeleteStepByID()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var result = new RateBLL().DeleteStepByID(int.Parse(inputValue));
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 删除分时
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "费率配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string DeletePeriodByID()
        {
            var inputValue = HttpContext.Current.Request.Form["Inputs"];
            var result = new RateBLL().DeletePeriodByID(int.Parse(inputValue));
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
