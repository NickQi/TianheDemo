using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 用能计费方式配置表
    /// </summary>
    public class EneryBillingConfig
    {
        public EneryBillingConfig()
        { }

        /// <summary>
        /// 配置ID 自动编号
        /// </summary>
        [DataMapping("ConfigID", "ConfigID", DbType.Int32)]
        public int ConfigID { get; set; }

        /// <summary>
        /// 计费的方式ID 
        /// </summary>
        [DataMapping("TypeID", "TypeID", DbType.Int32)]
        public int TypeID { get; set; }

        /// <summary>
        /// 计费的方式展示名称
        /// </summary>
        [DataMapping("DisplayName", "DisplayName", DbType.String)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 能源分项的代码
        /// </summary>
        [DataMapping("EneryCode", "EneryCode", DbType.String)]
        public string EneryCode { get; set; }
    }
}
