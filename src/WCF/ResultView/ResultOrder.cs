using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultOrder
    {
        public ExecuteProcess ActionInfo;
        ////Highcharts
        //public string LineJson { get; set; }
        //单位
        public string Unit { get; set; }
        //表格数据
        public List<EnergyOrder> OrderLst { get; set; }
        /// <summary>
        /// 绑定LineHighChart
        /// </summary>
        public LineHighChart lineHighChart;

        /// <summary>
        /// 绑定PieHighChart
        /// </summary>
        public PieHighChart pieHighChart;
    }

    public class EnergyOrder
    {
        //排名
        public int Order { get; set; }
        //时间
        public string Tm { get; set; }
        //对象
        public string Obj { get; set; }
        ////对象ID
        //public int ObjID { get; set; }
        //能耗类型
        public string EneType { get; set; }
        //能耗值
        public decimal Val { get; set; }
    }

    public class LineHighChart
    {
        public List<Series> series;
    }
    public class PieHighChart
    {
        public List<Series> series;
    }

    public class Series
    {
        public List<EneryHighChart> data { get; set; }
    }

    public class EneryHighChart
    {
        public string name;
        public decimal y;
        public string id;
    }

}
