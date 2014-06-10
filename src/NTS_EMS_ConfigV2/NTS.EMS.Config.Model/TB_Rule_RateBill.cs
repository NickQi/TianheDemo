using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 计费方式小项规则集合表（阶梯模式）
    /// </summary>
   public class TB_Rule_RateBill
    {
       public TB_Rule_RateBill()
        { }

        /// <summary>
        /// 阶梯规则ID  自动编号
        /// </summary>
        [DataMapping("RuleID", "RuleID", DbType.Int32)]
       public int RuleID { get; set; }

        /// <summary>
        /// 上限值
        /// </summary>
        [DataMapping("MaxValue", "MaxValue", DbType.Int32)]
        public int MaxValue { get; set; }

        /// <summary>
        /// 下限值
        /// </summary>
        [DataMapping("MinValue", "MinValue", DbType.Int32)]
        public int MinValue { get; set; }

        /// <summary>
        /// 计费小项的ID
        /// </summary>
        [DataMapping("ItemID", "ItemID", DbType.Int32)]
        public int ItemID { get; set; }
    }

   public class RatePriceModel
   {
       /// <summary>
       /// 计费小项ID
       /// </summary>
       [DataMapping("ItemID", "ItemID", DbType.Int32)]
       public int ItemID { get; set; }

       /// <summary>
       /// 阶梯规则自动编号
       /// </summary>
       [DataMapping("RuleID", "RuleID", DbType.Int32)]
       public int RuleID { get; set; }

       /// <summary>
       /// 上限值
       /// </summary>
       [DataMapping("MaxValue", "MaxValue", DbType.Int32)]
       public int MaxValue { get; set; }

       /// <summary>
       /// 下限值
       /// </summary>
       [DataMapping("MinValue", "MinValue", DbType.Int32)]
       public int MinValue { get; set; }

       [DataMapping("Price", "Price", DbType.Decimal)]
       public decimal Price { get; set; }

       /// <summary>
       /// 价格表中的ID
       /// </summary>
       [DataMapping("ID", "ID", DbType.Int32)]
       public int ID { get; set; }
   }
}
