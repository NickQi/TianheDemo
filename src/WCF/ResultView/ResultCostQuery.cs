using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ResultView
{
    public class ResultCostQuery
    {
        public ExecuteProcess ActionInfo;
       
        public string Unit { get; set; }
        public string FeeType { get; set; }
        public FeeTbl FeeTbl { get; set; }
        public FeeAnalyses FeeAnalyses { get; set; }
        public QuotaHighChart FeeQueryCharts;
        public PieHighChart FeePie;
        public List<int> StepSettingID { get; set; }
       
    }
  
    public class FeeAnalyses
    {
        public decimal TotalVal;
        public decimal MaxVal;
        public decimal MinVal;
        public decimal AvgVal;
        public decimal TotalEnergy;
        public decimal EnergyLastMonth;
        public string CompareLastMonth;
        public string EnergyUnit;
    }

    public class FeeTbl
    {
        public string EneType;
        public string Unit;
        public List<List<string>> FeeList;

    }

    public class FeeList
    {
        public List<string> List;
    }

    
    public enum FeeType
    {
        分时计费=1,
        平时计费=2,
        阶梯计费=3

    }

   



}
