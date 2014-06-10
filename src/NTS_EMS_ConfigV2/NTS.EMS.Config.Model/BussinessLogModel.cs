using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class BussinessLogModel
    {
        /// <summary>
        /// 操作日志id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 操作模块名称
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int ModelType { get; set; }
        /// <summary>
        /// 操作的用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }
        /// <summary>
        /// 操作的内容
        /// </summary>
        public string OperatorContent { get; set; }
    }
}
