using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class RealQuery
    {
        /// <summary>
        /// 设备id
        /// </summary>
        [DataMember]
        public int ObjectId { get; set; }

        /// <summary>
        /// 是否需要设备数值 0：不需要  1：需要
        /// </summary>
        [DataMember]
        public int IsDetail { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        [DataMember]
        public EnergyAnalyseQueryType QueryType { get; set; }
    }
}
