using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultQuota
    {
        public ExecuteProcess ActionInfo;

        public string Unit { get; set; }

        public QuotaAnalysePie Pie;
        public QuotaHighChart BalanceHighChart { get; set; }
        public QuotaHighChart TrendHighChart { get; set; }
    }
    /// <summary>
    /// 定额分析饼图结果
    /// </summary>
    public class QuotaAnalysePie
    {
       
        /// <summary>
        /// 定额值
        /// </summary>
        public decimal QuotaValue { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public decimal ActualValue { get; set; }
        /// <summary>
        /// 剩余值
        /// </summary>
        public decimal OverPlusValue { get; set; }
        /// <summary>
        /// 剩余百分比
        /// </summary>
        public string OverPlusPercent { get; set; }
        /// <summary>
        /// 历史节能率
        /// </summary>
        public string LastYearSavingPercent { get; set; }
        /// <summary>
        /// 历史同期能耗值
        /// </summary>
        public decimal LastYearActualValue { get; set; }
        /// <summary>
        /// 历史同期定额值
        /// </summary>
        public decimal LastYearQuotaValue { get; set; }
        /// <summary>
        /// 预测节能率
        /// </summary>
        public string ForecastSavingPercent { get; set; }
      
    }

    public class QuotaHighChart
    {
        public List<EneryAnalyseSeries> series{ get; set; }
    }

    public class BalanceHighChart
    {
        //public QuotaHighChart QuotaLine { get; set; }
        //public QuotaHighChart ActualLine { get; set; }
        //public QuotaHighChart BalanceLine { get; set; }

    }
    public class TrendHighChart
    {
        public QuotaHighChart QuotaLine { get; set; }
        public QuotaHighChart ActualLine { get; set; }
        public QuotaHighChart ForecastLine { get; set; }
    }



}
