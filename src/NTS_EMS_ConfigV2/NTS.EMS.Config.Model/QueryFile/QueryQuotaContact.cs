using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 定额数据查询
    /// </summary>
    [DataContract]
    public class QueryQuotaContact
    {
        [DataMember]
        public int ObjectId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// 1---月定额， 2---年定额
        /// </summary>
        [DataMember]
        public int QuotaType { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 定额日志查询
    /// </summary>
    [DataContract]
    public class QueryQuotaLogContract
    {
        [DataMember]
        public int QuotaId { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; } 
    }
}
