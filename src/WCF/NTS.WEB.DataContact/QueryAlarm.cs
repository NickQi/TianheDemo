using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryAlarm
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
        public int ObjectId { get; set; }

        [DataMember]
        public AreaType ObjType { get; set; }


    }


}
