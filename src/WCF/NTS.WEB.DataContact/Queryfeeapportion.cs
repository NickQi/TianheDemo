using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    public class Queryfeeapportion
    {
        [DataMember]
        public int ObjectId { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public AreaType ObjType { get; set; }

        [DataMember]
        public string ItemCode { get; set; }
    }
}
