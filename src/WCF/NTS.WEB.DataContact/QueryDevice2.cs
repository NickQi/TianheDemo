using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public   class QueryDevice2
    {
        /// <summary>
        /// 对象类型 1位置区域、2业态区域
        /// </summary>
        [DataMember]
        public AreaType ObjType { get; set; }

        /// <summary>
        /// 区域id
        /// </summary>
        [DataMember]
        public int ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 分类分项id
        /// </summary>
        [DataMember]
        public string ItemCode
        {
            get;
            set;
        }

    }
}
