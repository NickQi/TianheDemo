using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{

    public class TB_DeviceProp
    {
        public TB_DeviceProp()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [DataMapping("Id", "ID", DbType.Int32)]
        public int Id{ get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        [DataMapping("DeviceId", "DEVICEID", DbType.Int32)]
        public int DeviceId { get; set; }

        /// <summary>
        /// ItemCodeID
        /// </summary>
        [DataMapping("ItemCodeId", "ITEMCODEID", DbType.Int16)]
        public int ItemCodeId { get; set; }

        /// <summary>
        /// AreaId1
        /// </summary>
        [DataMapping("AreaId1", "AREAID1", DbType.Int16)]
        public int AreaId1 { get; set; }

        /// <summary>
        /// AreaId2
        /// </summary>
        [DataMapping("AreaId2", "AREAID2", DbType.Int16)]
        public int AreaId2 { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMapping("DeviceName", "CNAME", DbType.String)]
        public string DeviceName { get; set; }

        /// <summary>
        /// 分类分项名称
        /// </summary>
        [DataMapping("ItemCodeName", "ItemCodeName", DbType.String)]
        public string ItemCodeName { get; set; }

        /// <summary>
        /// 费率名称
        /// </summary>
        [DataMapping("PayTypeName", "PayTypeName", DbType.String)]
        public string PayTypeName { get; set; }

    }

    /// <summary>
    /// tb_payment_type 表
    /// </summary>
    public class TB_PAYMENT_TYPE
    {
        // <summary>
        /// ID
        /// </summary>
        [DataMapping("Id", "ID", DbType.Int16)]
        public int Id { get; set; }

        // <summary>
        /// paytype
        /// </summary>
        [DataMapping("PayType", "PAYTYPE", DbType.Int16)]
        public int PayType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMapping("Name", "CNAME", DbType.String)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMapping("Description", "DESC", DbType.String)]
        public string Description { get; set; }
    }
}
