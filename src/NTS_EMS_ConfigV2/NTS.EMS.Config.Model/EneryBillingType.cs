using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 用能计费方式表
    /// </summary>
    public class EneryBillingType
    {
        public EneryBillingType()
        { }

        /// <summary>
        /// 计费的方式ID 自动编号
        /// </summary>
        [DataMapping("TypeID", "TypeID", DbType.Int32)]
        public int TypeID { get; set; }

        /// <summary>
        /// 计费的方式名称
        /// </summary>
        [DataMapping("BillingTypeName", "BillingTypeName", DbType.String)]
        public string BillingTypeName { get; set; }
    }
}
