using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 定额告警数据查询
    /// </summary>
    [DataContract]
    public class QueryQuotaAlarmContact
    {
        /// <summary>
        /// 对象名称
        /// </summary>
        [DataMember]
        public string ObjectName { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMember]
        public int AlarmType { get; set; }

        /// <summary>
        /// 1---月定额， 2---年定额
        /// </summary>
        [DataMember]
        public int QuotaType { get; set; }

        /// <summary>
        /// 分类分项
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 定额告警
    /// </summary>
    [DataContract]
    public class QueryQuotaAlarmSingle
    {
        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMember]
        public int AlarmType { get; set; }

        /// <summary>
        /// 1---月定额， 2---年定额
        /// </summary>
        [DataMember]
        public int QuotaType { get; set; }

        /// <summary>
        /// 分类分项
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// 对象Id
        /// </summary>
        [DataMember]
        public int ObjectId { get; set; }

        /// <summary>
        /// 31---设备， 32---区域
        /// </summary>
        [DataMember]
        public int ObjectType { get; set; }

    }

    /// <summary>
    /// 定额告警数据契约
    /// </summary>
    [DataContract]
    public class QuotaAlarmDataContact
    {
        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMember]
        public int AlarmType { get; set; }

        /// <summary>
        /// 1---月定额， 2---年定额
        /// </summary>
        [DataMember]
        public int QuotaType { get; set; }

        /// <summary>
        /// 分类分项
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// 对象Id
        /// </summary>
        [DataMember]
        public int ObjectId { get; set; }

        /// <summary>
        /// 31---设备， 32---区域
        /// </summary>
        [DataMember]
        public int ObjectType { get; set; }

        /// <summary>
        /// 对象描述
        /// </summary>
        [DataMember]
        public string ObjectDesc { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        [DataMember]
        public double Percent { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
    }
}
