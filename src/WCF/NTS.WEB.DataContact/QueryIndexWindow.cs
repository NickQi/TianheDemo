using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryIndexWindow
    {
        /// <summary>
        /// 建筑的编号
        /// </summary>
        [DataMember]
        public int BuildingNumber { get; set; }
        /// <summary>
        /// 统计的时间
        /// </summary>
        [DataMember]
        public DateTime StatisticsDate { get; set; }
    }
}
