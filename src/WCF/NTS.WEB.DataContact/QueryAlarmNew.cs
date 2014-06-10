using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace NTS.WEB.DataContact
{
     [DataContract]
    public class QueryAlarmNew
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 对象id
        /// </summary>
        [DataMember]
        public AreaType ObjType { get; set; }

        /// <summary>
        /// 对象id
        /// </summary>
        [DataMember]
        public int ObjectId { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        [DataMember]
        public string AlarmLevel { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMember]
        public string AlarmType { get; set; }

        /// <summary>
        /// 告警状态
        /// </summary>
        [DataMember]
        public string AlarmStatus { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int Particle { get; set; }

        [DataMember]
        public bool AllAlarm { get; set; }
    }
}
