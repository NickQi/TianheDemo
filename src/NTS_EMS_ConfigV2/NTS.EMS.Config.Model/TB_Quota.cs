using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{

    public class TB_Quota
    {
        public TB_Quota()
        {
        }

        /// <summary>
        /// 定额id
        /// </summary>
        [DataMapping("QuotaId", "QUOTAID", DbType.Int32)]
        public int QuotaId{ get; set; }

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
        /// 定额时间
        /// </summary>
        [DataMapping("QuotaTime", "QUOTATIME", DbType.Date)]
        public DateTime QuotaTime { get; set; }

        /// <summary>
        /// 定额值
        /// </summary>
        [DataMapping("QuotaValue", "QUOTAVALUE", DbType.Double)]
        public double QuotaValue { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        [DataMapping("Reserved", "RESERVED", DbType.String)]
        public string Reserved { get; set; }

        /// <summary>
        /// ItemCode
        /// </summary>
        [DataMapping("ItemCode", "ITEMCODE", DbType.String)]
        public string ItemCode { get; set; }

    }

    public class TS_DataCenter_Area_Month
    {
        public TS_DataCenter_Area_Month()
        {
        }

        /// <summary>
        /// 对象id
        /// </summary>
        [DataMapping("ObjectId", "CountID", DbType.Int32)]
        public int ObjectId { get; set; }

        /// <summary>
        /// 所有月份值
        /// </summary>
        [DataMapping("Value", "Value30", DbType.String)]
        public string Value { get; set; }

        /// <summary>
        /// ItemCode
        /// </summary>
        [DataMapping("ItemCode", "ItemCode", DbType.String)]
        public string ItemCode { get; set; }
    }
}
