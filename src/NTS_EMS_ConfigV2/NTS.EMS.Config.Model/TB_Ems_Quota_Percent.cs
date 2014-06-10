using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{

    public class TB_Ems_Quota_Percent
    {
        public TB_Ems_Quota_Percent()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [DataMapping("Id", "ID", DbType.Int32)]
        public int Id{ get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMapping("AlarmType", "ALARMTYPE", DbType.Int16)]
        public int AlarmType { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        [DataMapping("ObjectType", "OBJECTTYPE", DbType.Int16)]
        public int ObjectType { get; set; }

        /// <summary>
        /// 对象id
        /// </summary>
        [DataMapping("ObjectId", "OBJECTID", DbType.Int32)]
        public int ObjectId { get; set; }

        /// <summary>
        /// 对象描述
        /// </summary>
        [DataMapping("ObjectDesc", "OBJECTDESC", DbType.String)]
        public string ObjectDesc { get; set; }

        /// <summary>
        /// 定额类型
        /// </summary>
        [DataMapping("QuotaType", "QUOTATYPE", DbType.Int16)]
        public int QuotaType { get; set; }

        /// <summary>
        /// ItemCode
        /// </summary>
        [DataMapping("ItemCode", "ITEMCODE", DbType.String)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        [DataMapping("Percent", "PERCENT", DbType.Double)]
        public double Percent { get; set; }

    }
}
