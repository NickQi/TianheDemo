using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    [DataContract]
    public class QuerySysLogContact
    {
        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public string OperatorName { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}
