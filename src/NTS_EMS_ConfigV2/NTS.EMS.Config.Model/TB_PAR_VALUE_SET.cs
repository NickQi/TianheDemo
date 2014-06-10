using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 平价配置表
    /// </summary>
    public class TB_PAR_VALUE_SET
    {
        /// <summary>
        /// 主键 ID 
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 能源类型编号
        /// </summary>
        [DataMapping("TYPEID", "TYPEID", DbType.String)]
        public string TYPEID { get; set; }

        /// <summary>
        /// 名称（电平价）
        /// </summary>
        [DataMapping("CNAME", "CNAME", DbType.String)]
        public string CNAME { get; set; }

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

    public enum RateType
    {
        尖 = 1,
        峰 = 2,
        平 = 3,
        谷 = 4
    }
}
