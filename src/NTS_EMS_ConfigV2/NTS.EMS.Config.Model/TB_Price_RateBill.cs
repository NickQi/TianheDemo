using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 计费方式价格表（阶梯模式)
    /// </summary>
    public class TB_Price_RateBill
    {
        public TB_Price_RateBill()
        { }

        /// <summary>
        /// 自动编号
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 单价数值
        /// </summary>
        [DataMapping("Price", "Price", DbType.Decimal)]
        public decimal Price { get; set; }

        /// <summary>
        /// 阶梯规则的ID
        /// </summary>
        [DataMapping("RuleID", "RuleID", DbType.Int32)]
        public int RuleID { get; set; }
    }
}
