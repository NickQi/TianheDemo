using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 计费方式小项规则集合表（分时模式）
    /// </summary>
    public class TB_Rule_TimeBill
    {
        public TB_Rule_TimeBill()
        { }

        /// <summary>
        /// 分时规则ID  自动编号
        /// </summary>
        [DataMapping("RuleID", "RuleID", DbType.Int32)]
        public int RuleID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMapping("StartTime", "StartTime", DbType.Int32)]
        public int StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMapping("EndTime", "EndTime", DbType.Int32)]
        public int EndTime { get; set; }

        /// <summary>
        /// 计费小项的ID
        /// </summary>
        [DataMapping("ItemID", "ItemID", DbType.Int32)]
        public int ItemID { get; set; }
    }
}
