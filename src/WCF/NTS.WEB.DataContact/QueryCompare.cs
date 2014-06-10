using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
   public class QueryCompare
    {
        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public List<int> ObjectNum { get; set; }

        [DataMember]
        public int Unit { get; set; }

        [DataMember]
        public int ClassId { get; set; }

        /// <summary>
        /// 对象类型 1:区域树，2：页态树
        /// </summary>
        [DataMember]
        public AreaType ObjType { get; set; }
    }
}
