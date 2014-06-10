using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model.QueryFile
{
    public class QueryAlloction
    {
        /// <summary>
        /// 分摊配置
        /// </summary>
        [DataMember]
        public List<TB_ALLOCTION_CONFIG> ListConfig { get; set; }

        /// <summary>
        /// 分摊日志
        /// </summary>
        [DataMember]
        public TB_ALLOCTION_CONFIG_History ConfigLog { get; set; }

        /// <summary>
        /// 左侧树对象上的名称
        /// </summary>
        [DataMember]
        public string ParentName { get; set; }

    }
}
