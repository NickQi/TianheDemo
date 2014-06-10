using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
   public class MainInfo
    {
        public ExecuteProcess ActionInfo;
     
       
       /// <summary>
       /// 总能耗
       /// </summary>
        public decimal Total { get; set; }
        
       /// <summary>
        /// 总能耗环比
       /// </summary>
        public string TotalCompare { get; set; }
        
        /// <summary>
        /// 上月总能耗 
        /// </summary>
        public decimal TotalLastMon { get; set; }
       /// <summary>
       /// 是否只有电，True表示只有电
       /// </summary>
        public bool IsOnlyElec { get; set; }
        //各分类分项能耗
        public List<EneryStatistic> ItemValues { get; set; }
        //电能综合评价
        public List<EnerySum> PeriodValues { get; set; }
    }

   public class EneryStatistic
   {   //分类分项
       public string ItemCode { get; set; }
       //能耗名称
       public string CName { get; set; }
       //单位
       public string Unit { get; set; }
       //能耗值
       public decimal EneryValue { get; set; }
       //环比
       public string MonthCompare { get; set; }
       //上月能耗
       public decimal EnergyLastMonth { get; set; }

       /// <summary>
       /// 转换为标准煤
       /// </summary>
       public decimal EnergyValue2Coal { get; set; }
   }

   /// <summary>
   /// 综合评价
   /// </summary>
   public class EnerySum
   {  //期间类型 1日 2周 3月 4年
       public int PeriodType { get; set; }
       //环比
       public string MonthCompare { get; set; }
       //值1
       public decimal Value1 { get; set; }
       //值2
       public decimal Value2 { get; set; }
   }
}
