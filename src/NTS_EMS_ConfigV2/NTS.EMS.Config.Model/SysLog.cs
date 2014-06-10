using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.EMS.Config.Model
{
    public class SysLog
    {
        public SysLog()
        {
        }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMapping("SysNo", "SysNo", DbType.Int32)]
        public int SysNo { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [DataMapping("ModelName", "ModelName", DbType.String)]
        public string ModelName { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [DataMapping("LogContent", "LogContent", DbType.String)]
        public string LogContent { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        [DataMapping("LogTime", "LogTime", DbType.DateTime)]
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMapping("OpType", "OpType", DbType.Int16)]
        public int OpType { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        [DataMapping("UserName", "UserName", DbType.String)]
        public string UserName { get; set; }
    }
}
