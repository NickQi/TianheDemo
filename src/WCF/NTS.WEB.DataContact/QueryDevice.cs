using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
     [DataContract]
    public class QueryDevice
    {
        [DataMember]
        public int AreaID
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
    }
}
