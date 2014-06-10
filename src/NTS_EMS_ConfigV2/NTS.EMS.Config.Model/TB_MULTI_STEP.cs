using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 阶梯电价配置表
    /// </summary>
    public class TB_MULTI_STEP
    {
        /// <summary>
        /// 主键ID 
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        [DataMapping("GEARSID", "GEARSID", DbType.Int32)]
        public int GEARSID { get; set; }

        /// <summary>
        /// 能源类型ID 
        /// </summary>
        [DataMapping("TYPEID", "TYPEID", DbType.String)]
        public string TYPEID { get; set; }

        /// <summary>
        /// 档位名称
        /// </summary>
        [DataMapping("GEARNAME", "GEARNAME", DbType.String)]
        public string GEARNAME { get; set; }

        /// <summary>
        /// 开始入档电度
        /// </summary>
        [DataMapping("START_GEARS_VALUE", "START_GEARS_VALUE", DbType.Int32)]
        public int START_GEARS_VALUE { get; set; }

        /// <summary>
        /// 结束入档电度
        /// </summary>
        [DataMapping("END_GEARS_VALUE", "END_GEARS_VALUE", DbType.Int32)]
        public int END_GEARS_VALUE { get; set; }

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

    }
}
