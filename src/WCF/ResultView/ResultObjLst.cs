using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
   public  class ResultObjLst
    {
       public ExecuteProcess ActionInfo;
       //单位
       public string Unit { get; set; }

       public List<ObjRecord> ObjLst { get; set; }
    }

   public class  ObjRecord
   {
       //对象或者时间
       public string ObjTime { get; set; }
       //总能耗
       public decimal TotalValue { get; set; }
       //平均能耗
       public decimal AvgValue { get; set; }
       //最大值
       public decimal MinValue { get; set; }
       //最小值
       public decimal MaxValue { get; set; }
   }
}
