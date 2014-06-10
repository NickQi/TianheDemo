using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryQuota
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        ///// <summary>
        ///// 结束时间
        ///// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 分类分项
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// 对象id
        /// </summary>
        [DataMember]
        public int ObjectId { get; set; }
        /// <summary>
        /// 区域类型
        /// </summary>
        [DataMember]
        public AreaType ObjType { get; set; }
        /// <summary>
        /// 时间单位
        /// </summary>
        [DataMember]
        public Particle Particle { get; set; }
    }

    /// <summary>
    /// 0:日（时对比）
    /// 1:0-90天（日对比）
    /// 2:>=90天AND小于3年（月对比） 
    /// 3：>3年（年对比）
    /// </summary>
    public enum Particle
    {
        /// <summary>
        /// 0:小时（时对比）
        /// </summary>
        Hour=0,
        /// <summary>
        /// 0-90天（日对比）
        /// </summary>
        Day=1,
        /// <summary>
        /// :>=90天AND小于3年（月对比） 
        /// </summary>
        Month=2,
        /// <summary>
        /// >3年（年对比）
        /// </summary>
        Year=3
      
    }


}
