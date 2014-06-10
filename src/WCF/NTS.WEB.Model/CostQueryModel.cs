using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    /// <summary>
    /// 管理诊断
    /// </summary>
    public class CostQueryModel
    {

        /// <summary>
        /// 时间
        /// </summary>
        [DataMapping("TIMEID", "TIMEID", DbType.DateTime)]
        public DateTime TIMEID
        {
            get;
            set;
        }
        /// <summary>
        /// 总用量
        /// </summary>
        [DataMapping("TOTAL", "TOTAL", DbType.Double)]
        public double TOTAL
        {
            get;
            set;
        }
        /// <summary>
        /// 总费用
        /// </summary>
        [DataMapping("TOTAL_COST", "TOTAL_COST", DbType.Double)]
        public double TOTAL_COST
        {
            get;
            set;
        }
        /// <summary>
        /// 尖时用量
        /// </summary>
        [DataMapping("SHARP", "SHARP", DbType.Double)]
        public double SHARP
        {
            get;
            set;
        }
        /// <summary>
        /// 尖时费用
        /// </summary>
        [DataMapping("SHARP_COST", "SHARP_COST", DbType.Double)]
        public double SHARP_COST
        {
            get;
            set;
        }
        /// <summary>
        /// 峰时电量
        /// </summary>
        [DataMapping("HIGH", "HIGH", DbType.Double)]
        public double HIGH
        {
            get;
            set;
        }
        /// <summary>
        /// 峰时电费
        /// </summary>
        [DataMapping("HIGH_COST", "HIGH_COST", DbType.Double)]
        public double HIGH_COST
        {
            get;
            set;
        }
       
        /// <summary>
        /// 平时电量
        /// </summary>
        [DataMapping("MID", "MID", DbType.Double)]
        public double MID
        {
            get;
            set;
        }
        /// <summary>
        /// 平时电费
        /// </summary>
        [DataMapping("MID_COST", "MID_COST", DbType.Double)]
        public double MID_COST
        {
            get;
            set;
        }
        /// <summary>
        /// 谷时电量
        /// </summary>
        [DataMapping("LOW", "LOW", DbType.Double)]
        public double LOW
        {
            get;
            set;
        }
        /// <summary>
        /// 谷时电费
        /// </summary>
        [DataMapping("LOW_COST", "LOW_COST", DbType.Double)]
        public double LOW_COST
        {
            get;
            set;
        }
        /// <summary>
        /// 谷时电费
        /// </summary>
        [DataMapping("PAYMENT_TYPE", "PAYMENT_TYPE", DbType.Int32)]
        public int PAYMENT_TYPE
        {
            get;
            set;
        }
    }


}
