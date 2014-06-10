using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryAnalyse
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
        /// 是否设备（1=设备）
        /// </summary>
        [DataMember]
        public int IsDevice { get; set; }
        /// <summary>
        /// 查询类型 1默认 2同比值 3环比值 4单位面积 5人均
        /// </summary>
        [DataMember]
        public EnergyAnalyseQueryType QueryType { get; set; }

        /// <summary>
        /// 区域类型
        /// </summary>
        [DataMember]
        public AreaType ObjType { get; set; }

        /// <summary>
        /// 当前选中项的子节点
        /// </summary>
        [DataMember]
        public List<int> ObjectChildren { get; set; }

        [DataMember]
        public int particle { get; set; }
    }

    public enum EnergyAnalyseQueryType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 1,
        /// <summary>
        /// 同比
        /// </summary>
        YearCompare = 4,
        /// <summary>
        /// 环比
        /// </summary>
        MonthCompare = 5,
        /// <summary>
        /// 单位面积
        /// </summary>
        UnitArea = 2,
        /// <summary>
        /// 人均
        /// </summary>
        UnitPerson = 3,
        /// <summary>
        /// 转化为标准煤
        /// </summary>
        Convert2Coal = 6,
        /// <summary>
        /// 转化为二氧化碳
        /// </summary>
        Convert2Co2 = 7,
        /// <summary>
        /// 转化为人名币
        /// </summary>
        Convert2Money = 8,

    }
}
