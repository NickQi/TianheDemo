using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    /// <summary>
    /// 多时间
    /// </summary>
     [DataContract]
    public class QueryContrastPeriods
    {
        /// <summary>
        /// 查询类型 1默认 2单位面积 3人均
        /// </summary>
        [DataMember]
         public QueryOrderType QueryType { get; set; }

        //期间列表
        [DataMember]
         public List<TimePeriod> PeriodLst { get; set; }
 

        //分类分项
        [DataMember]
        public string ItemCode { get; set; }
            
        //区域id
        [DataMember]
        public int AreaId { get; set; }


        //对象类型 1 区域树
        [DataMember]
        public AreaType ObjType { get; set; }

        /// <summary>
        /// 0:日（时对比）
        /// 1:0-90天（日对比）
        /// 2:>=90天AND小于3年（月对比） 
        /// 3：>3年（年对比）
        /// </summary>
        [DataMember]
        public int particle { get; set; }
    }

     public class TimePeriod
     {
         public DateTime StartTime { get; set; }


         public DateTime EndTime { get; set; }
     }



}
