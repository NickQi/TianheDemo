using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryTree
    {
        /// <summary>
        /// 区域类别 1:位置区域  2:功能区域
        /// </summary>
        [DataMember]
        public AreaType ClassId { get; set; }

        /// <summary>
        /// 分类分项ID
        /// </summary>
        [DataMember]
        public int? Level { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [DataMember]
        public int? ParentID { get; set; }
    }
}
