using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NTS.WEB.DataContact
{
    [Serializable]
    [DataContract]
    public class QueryLogin
    {
        [DataMember]
        public bool IsRemeberPass
        {
            get;
            set;
        }

        [DataMember]
        public string LoginUser
        {
            get;
            set;
        }

        [DataMember]
        public string LoginPass
        {
            get;
            set;
        }
    }
}
