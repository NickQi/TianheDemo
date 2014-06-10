using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NTS.EMS.Config.Model
{
    [DataContract]
    public class QueryBussinessLog
    {
        /// <summary>
        /// 操作模块名称
        /// </summary>
        [DataMember]
        public string ModelName { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMember]
        public int ModelType { get; set; }
        /// <summary>
        /// 操作的用户
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [DataMember]
        public DateTime OperatorTime { get; set; }
        /// <summary>
        /// 操作的内容
        /// </summary>
        [DataMember]
        public string OperatorContent { get; set; }
    }
}
