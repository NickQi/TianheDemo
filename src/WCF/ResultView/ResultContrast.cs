using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultContrast
    {
        public ExecuteProcess ActionInfo;
        //Highcharts
        public LineJson lineJson { get; set; }
        //单位
        public string Unit { get; set; }

        //表格数据
        public List<EnergyContrst> ContrastLst { get; set; }
    }

    public class LineJson
    {
        public List<EneryCompare> series;

        public string Unit;

        public string CompareType;
    }

    //public class SeriesEnerCompare
    //{
    //    public List<EneryCompare> serData{ get; set; }
    //}

    public class EneryCompare
    {
        public string id;
        public string name;
        public List<decimal> data;
    }

    public class EnergyContrst
    {
        //时间
        public string Tm { get; set; }
        //对象
        public string Obj { get; set; }
        //能耗类型
        public string EneType { get; set; }
        //能耗值
        public decimal Val { get; set; }
    }
}
