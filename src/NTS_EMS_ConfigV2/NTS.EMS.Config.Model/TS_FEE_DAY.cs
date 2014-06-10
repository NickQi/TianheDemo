using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 费用历史表
    /// </summary>
    public class TS_FEE_DAY
    {
        [DataMapping("ID", "ID", DbType.Int64)]
        public long ID { get; set; }

        [DataMapping("TOTAL", "TOTAL", DbType.Double)]
        public double TOTAL { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [DataMapping("TOTAL_COST", "TOTAL_COST", DbType.Double)]
        public double TOTAL_COST { get; set; }

        /// <summary>
        /// 对象ID 
        /// </summary>
        [DataMapping("OBJECTID", "OBJECTID", DbType.Int16)]
        public int OBJECTID { get; set; }

        /// <summary>
        /// 费率类型 32代表区域
        /// </summary>
        [DataMapping("PAYMENT_TYPE", "PAYMENT_TYPE", DbType.Int16)]
        public int PAYMENT_TYPE { get; set; }

        /// <summary>
        /// 能源类型
        /// </summary>
        [DataMapping("ITEMCODE", "ITEMCODE", DbType.String)]
        public string ITEMCODE { get; set; }
    }
}
