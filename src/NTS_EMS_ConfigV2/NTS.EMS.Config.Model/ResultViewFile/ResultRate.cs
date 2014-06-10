using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model.ResultViewFile
{
    public class ResultRate
    {
        /// <summary>
        /// 标识新增或编辑的返回状态
        /// </summary>
        public bool IsSucess { get; set; }

        /// <summary>
        /// 分摊配置添加日志时 使用
        /// </summary>
        public string TreeName { get; set; }
    }

    public class ResultRatePriceList
    {
        #region Old
        /// <summary>
        /// 平价
        /// </summary>
        public Model.TB_Price_CommBill CommModel { get; set; }

        /// <summary>
        /// 阶梯集合
        /// </summary>
        public List<Model.RatePriceModel> RatePriceList { get; set; }

        /// <summary>
        /// 分时模式中 价格集合
        /// </summary>
        public List<Model.TB_Price_TimeBillExend> PriceTimeList { get; set; }

        /// <summary>
        /// 分时模式中 规则集合
        /// </summary>
        public List<Model.TB_Rule_TimeBill> RuleTimeList { get; set; }
        #endregion

        #region New
        public Model.TB_PAR_VALUE_SET ParValueModel { get; set; }

        /// <summary>
        /// 阶梯集合
        /// </summary>
        public List<Model.TB_MULTI_STEP> MultiStepList { get; set; }

        /// <summary>
        /// 分时集合
        /// </summary>
        public List<Model.TB_TIME_PERIOD_SET> PeroidList { get; set; }

        public List<PeroidFlag> PeroidFlag { get; set; }
        
        /// <summary>
        /// 尖 的价格
        /// </summary>
        public double PriceJ { get; set; }

        /// <summary>
        /// 峰 的价格
        /// </summary>
        public double PriceF { get; set; }

        /// <summary>
        /// 平 的价格
        /// </summary>
        public double PriceP { get; set; }

        /// <summary>
        /// 谷 的价格
        /// </summary>
        public double PriceG { get; set; }
        #endregion
    }

    public class PeroidFlag
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
