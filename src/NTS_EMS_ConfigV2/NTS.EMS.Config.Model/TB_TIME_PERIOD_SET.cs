using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.DataConfiguration;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 尖峰平谷时段配置表
    /// </summary>
    public class TB_TIME_PERIOD_SET
    {
        /// <summary>
        /// 主键ID 
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 能源类型ID 
        /// </summary>
        [DataMapping("TYPEID", "TYPEID", DbType.String)]
        public string TYPEID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMapping("CNAME", "CNAME", DbType.String)]
        public string CNAME { get; set; }

        /// <summary>
        /// 开始时间 小时
        /// </summary>
        [DataMapping("STARTHOUR", "STARTHOUR", DbType.Int32)]
        public int STARTHOUR { get; set; }

        /// <summary>
        /// 开始时间 分钟
        /// </summary>
        [DataMapping("STARTMINUTE", "STARTMINUTE", DbType.Int32)]
        public int STARTMINUTE { get; set; }

        /// <summary>
        /// 结束时间 小时
        /// </summary>
        [DataMapping("ENDHOUR", "ENDHOUR", DbType.Int32)]
        public int ENDHOUR { get; set; }

        /// <summary>
        /// 结束时间 分钟
        /// </summary>
        [DataMapping("ENDMINUTE", "ENDMINUTE", DbType.Int32)]
        public int ENDMINUTE { get; set; }

        /// <summary>
        /// 尖、峰、平、谷类型ID 
        /// </summary>
        [DataMapping("TYPE", "TYPE", DbType.Int32)]
        public int TYPE { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DataMapping("PRICE", "PRICE", DbType.Double)]
        public double PRICE { get; set; }

        /// <summary>
        /// 结算日
        /// </summary>
        [DataMapping("DATE", "DATE", DbType.Int32)]
        public int DATE { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMapping("DESC", "DESC", DbType.String)]
        public string DESC { get; set; }
    }
}
