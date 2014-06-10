using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class Device
    {
        /// <summary>
        /// 设备的id
        /// </summary>
        [DataMapping("DeviceID", "deviceid", DbType.Int32)]
        public int DeviceID { get; set; }
        /// <summary>
        /// 设备的编号
        /// </summary>
        [DataMapping("DeviceNumber", "DeviceNumber", DbType.String)]
        public string DeviceNumber { get; set; }
        /// <summary>
        /// 设备的名称
        /// </summary>
        [DataMapping("DeviceName", "DeviceName", DbType.String)]
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备的性质
        /// </summary>
        [DataMapping("DEVMODE", "DEVMODE", DbType.String)]
        public string DEVMODE { get; set; }
        ///<summary>
        /// 所属区域id
        /// </summary>
        [DataMapping("AreaID", "AreaID", DbType.Int32)]
        public int AreaID { get; set; }
        /// <summary>
        /// 设备分类分项id
        /// </summary>
        [DataMapping("ItemCodeID", "ItemCodeID", DbType.String)]
        public string ItemCodeID { get; set; }
        /// <summary>
        /// 设备的地址
        /// </summary>
        [DataMapping("Location", "Location", DbType.String)]
        public string Location { get; set; }
        /// <summary>
        /// 设备的状态
        /// </summary>
        [DataMapping("Status", "Status", DbType.Int32)]
        public int Status { get; set; }
        /// <summary>
        /// 设备的转速
        /// </summary>
        [DataMapping("Rating", "Rating", DbType.Int32)]
        public int Rating { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        [DataMapping("Factory", "Factory", DbType.String)]
        public string Factory { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        [DataMapping("FactoryDate", "FactoryDate", DbType.DateTime)]
        public DateTime FactoryDate { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        [DataMapping("LimitYear", "LimitYear", DbType.DateTime)]
        public DateTime LimitYear { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMapping("StartDate", "StartDate", DbType.DateTime)]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 设备的类型
        /// </summary>
        [DataMapping("DeviceType", "DeviceType", DbType.String)]
        public string DeviceType { get; set; }
        /// <summary>
        /// 设备的组织单位
        /// </summary>
        [DataMapping("Organization", "Organization", DbType.String)]
        public string Organization { get; set; }
    }
}
