using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryOrderObjects : QueryContrastObjects
    {
        /// <summary>
        /// 查询类型 1默认 2单位面积 3人均
        /// </summary>
        [DataMember]
        public EnergyAnalyseQueryType QueryType { get; set; }
    }

    public enum QueryOrderType
    {
        Default=1,
        UnitArea=2,
        UnitPerson=3,
        ConvCoal = 6,
        CarbanOut = 7,
        Renminbi = 8
    }
}
