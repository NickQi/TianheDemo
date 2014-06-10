using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    [DataContract]
    public class QuotaDataContact
    {
        [DataMember]
        public int QuotaId { get; set; }

        [DataMember]
        public int ObjectType { get; set; }

        [DataMember]
        public int ObjectId { get; set; }

        [DataMember]
        public string ObjectDesc { get; set; }

        [DataMember]
        public int QuotaType { get; set; }

        [DataMember]
        public float QuotaValue { get; set; }

        [DataMember]
        public string Reserved { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime QuotaDate { get; set; }
    }
}
