using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    public class QueryLoadForecast
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        [DataMember]
        public AreaType ObjType { get; set; }

        /// <summary>
        /// 分类分项 总能耗为"00000"
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// ObjectId
        /// </summary>
        [DataMember]
        public int ObjectId { get; set; }

        /// <summary>
        /// 0:日（时对比）
        /// 1:0-90天（日对比）
        /// 2:>=90天AND小于3年（月对比） 
        /// 3：>3年（年对比）
        /// </summary>
        [DataMember]
        public int Particle { get; set; }


        // 0 日，1：周，2：月：3：任意。
        [DataMember]
        public int DateUnit { get; set; }
    }
}
