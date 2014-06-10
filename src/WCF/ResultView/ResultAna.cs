using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultAna
    {
        public ExecuteProcess ActionInfo;
       
        public string Unit { get; set; }
        /// <summary>
        /// 绑定LineHighChart
        /// </summary>
        public LineHighChart lineHighChart;

        //表格数据
        public List<EnergyOrder> OrderLst { get; set; }
    }

    //public class EnergyVal
    //{
    //    //
    //    public int No { get; set; }
    //    //
    //    public string  Tm { get; set; }
    //    //
    //    public string Obj { get; set; }
    //    //
    //    public string Val { get; set; }
    //}
}
