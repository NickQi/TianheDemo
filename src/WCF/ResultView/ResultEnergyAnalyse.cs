using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultEnergyAnalyse
    {
        public ExecuteProcess ActionInfo;

        public string Unit { get; set; }
        /// <summary>
        /// 绑定LineHighChart
        /// </summary>
        public List<EneryAnalyseSeries> series;

        //表格数据
        public List<EnergyOrder> OrderLst { get; set; }
    }
    /// <summary>
    /// 能耗分析比较结果
    /// </summary>
    public class EnergyAnalyseCompare
    {
        public ExecuteProcess ActionInfo;
        public string Unit { get; set; }
        public decimal TotalValue { get; set; }
        public decimal LastMonthTotalValue { get; set; }
        public string LastMonthCompare { get; set; }
        public decimal LastYearTotalValue { get; set; }
        public string LastYearCompare { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal AverageValue { get; set; }
    }

    public class ResultEnergyAnalysePie
    {

        public ExecuteProcess ActionInfo { get; set; }
        public string Unit { get; set; }
        public PieHighChart LayerPie;
        public PieHighChart ItemCodePie;
        //public List<PieItem> LayerPie { get; set; }
        //public List<PieItem> ItemCodePie { get; set; }
    }

    public class PieItem
    {
        public string ObjectName;
        public decimal EnergyValue;
    }

    public class EneryAnalyseSeries
    {
        public string name;
        public List<decimal> data;
    }

    //public class EnergyAnalysePoint
    //{
    //    public string time;
    //    public decimal data;
    //}
}
