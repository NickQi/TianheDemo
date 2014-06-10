using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 计费方式小项表
    /// </summary>
    public class TB_BillItem
    {
        public TB_BillItem()
        { }

        /// <summary>
        /// 计费小项的ID 自动编号
        /// </summary>
        [DataMapping("ItemID", "ItemID", DbType.Int32)]
        public int ItemID { get; set; }

        /// <summary>
        /// 计费小项的名称
        /// </summary>
        [DataMapping("ItemName", "ItemName", DbType.String)]
        public string ItemName { get; set; }

        /// <summary>
        /// 能源分项的代码
        /// </summary>
        [DataMapping("EnergyCode", "EnergyCode", DbType.String)]
        public string EnergyCode { get; set; }

        /// <summary>
        /// 计费的方式ID
        /// </summary>
        [DataMapping("TypeID","TypeID",DbType.Int32)]
        public int TypeID { get; set; }
    }
}
