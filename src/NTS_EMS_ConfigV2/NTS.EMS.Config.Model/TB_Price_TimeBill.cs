using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 计费方式价格表（分时模式）
    /// </summary>
    public class TB_Price_TimeBill
    {
        public TB_Price_TimeBill()
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
        /// 计费分项的ID
        /// </summary>
        [DataMapping("ItemID", "ItemID", DbType.Int32)]
        public int ItemID { get; set; }
    }

    public class TB_Price_TimeBillExend
    {
        public TB_Price_TimeBillExend()
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
        /// 计费分项的ID
        /// </summary>
        [DataMapping("ItemID", "ItemID", DbType.Int32)]
        public int ItemID { get; set; }

        [DataMapping("ItemName", "ItemName", DbType.Int32)]
        public string ItemName { get; set; }
    }
}
