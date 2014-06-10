using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryEnery
    {
        [DataMember]
        public int Unit
        {
            get;
            set;
        }

        [DataMember]
        public string ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime
        {
            get;
            set;
        }

        [DataMember]
        public string ObjectNum
        {

            get;
            set;
        }
        [DataMember]
        public int ObjectId
        {

            get;
            set;
        }
    }
}
