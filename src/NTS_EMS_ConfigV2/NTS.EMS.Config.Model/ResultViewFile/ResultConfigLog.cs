using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model.ResultViewFile
{
    public class ResultConfigLog
    {
        public List<BaseConfigLog> LogList { get; set; }
        public Padding Page { get; set; }
    }

    public class BaseConfigLog
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string OPTIONUSER { get; set; }

        /// <summary>
        /// 分摊时间
        /// </summary>
        public string CFGDATE { get; set; }

        /// <summary>
        /// 操作对象
        /// </summary>
        public int CFGOBJECT { get; set; }

        /// <summary>
        /// 操作对象名称
        /// </summary>
        public string CNAME { get; set; }

        /// <summary>
        /// 分摊内容
        /// </summary>
        public string CFGDEC { get; set; }

        /// <summary>
        /// 写日志时间
        /// </summary>
        public string OPTIONTIME { get; set; }
    }
}
