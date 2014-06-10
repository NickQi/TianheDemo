using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public  class ResultFeeapportion
    {
        public ExecuteProcess ActionInfo;

        public string FeeApportionType { get; set; }

        public FeeApportionChartsClass FeeApportionCharts { get; set; }

        public FeeApportionValClass FeeApportionVal { get; set; }

        public FeeApportionTblClass FeeApportionTbl { get; set; } 

    }


    public class FeeApportionChartsClass
    {
        public List<SeriesData> series;
    }

    public class SeriesData
    {
        public string name { get; set; }

        public List<SeriesClass> data { get; set; }
    }

    public class SeriesClass
    {
        public string name { get; set; }
        public double y { get; set; }
    }

    public class FeeApportionValClass
    {
        public double BeforeVal { get; set; }

        public double BeforeValLastMonth { get; set; }

        public string BeforeValCompare { get; set; }

        public double ApportionVal { get; set; }

        public double ApportionValLastMonth { get; set; }

        public string ApportionValCompare { get; set; }

        public double TotalVal { get; set; }

        public double TotalValLastMonth { get; set; }

        public string TotalValCompare { get; set; }
    }

    public class FeeApportionTblClass
    {
        public List<FeeApportionListClass> FeeApportionList { get; set; }
    }

    public class FeeApportionListClass
    {
        public int Id { get; set; }

        public string Tm { get; set; }

        public string Obj { get; set; }

        public double BeforeVal { get; set; }

        public double ApportionVal { get; set; }

        public double TotalVal { get; set; }
    }


}
