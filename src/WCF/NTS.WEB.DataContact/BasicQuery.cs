//查询基础条件类
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class BasicQuery
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
        /// 分类分项的编号
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// 查询的颗粒
        /// </summary>
        [DataMember]
        public int Unit { get; set; }

        /// <summary>
        /// 查询的对象
        /// </summary>
        [DataMember]
        public int ObjectNum { get; set; }

        /// <summary>
        /// 类型（暂时保留方便扩展）
        /// </summary>
        [DataMember]
        public int ObjectType { get; set; }

        /// <summary>
        /// 1: 区域树
        /// 2：页态树
        /// </summary>
        [DataMember]
        public AreaType AreaType { get; set; }

        [DataMember]
        public EnergyAnalyseQueryType QueryType;

    }


}
