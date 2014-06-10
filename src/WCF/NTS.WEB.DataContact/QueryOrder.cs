using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryOrder
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
        public string OrderWay { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
        
        [DataMember]
        public string Particle { get; set; }
    }
}
