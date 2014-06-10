using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    /// <summary>
    /// 多对象
    /// </summary>
    [DataContract]
    public class QueryContrastObjects
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
        /// 多个区域id
        /// </summary>
        [DataMember]
        public List<int> AreaIdLst { get; set; }

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
        /// 0:日（时对比）
        /// 1:0-90天（日对比）
        /// 2:>=90天AND小于3年（月对比） 
        /// 3：>3年（年对比）
        /// </summary>
        [DataMember]
        public int Particle { get; set; }
    }
    /// <summary>
    /// 区域类型
    /// </summary>
    public enum AreaType
    {
        /// <summary>
        /// 区域
        /// </summary>
        Area=1,
        
        /// <summary>
        /// 业态
        /// </summary>
        Liquid=2
    }

}
