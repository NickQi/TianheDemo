using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{

    public class TS_Quota_Log
    {
        public TS_Quota_Log()
        {
        }

        /// <summary>
        /// 定额id
        /// </summary>
        [DataMapping("QuotaId", "QUOTAID", DbType.Int32)]
        public int QuotaId{ get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMapping("UserName", "USERNAME", DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 配置时间
        /// </summary>
        [DataMapping("LogTime", "LOGTIME", DbType.DateTime)]
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 定额值
        /// </summary>
        [DataMapping("QuotaValue", "QUOTAVALUE", DbType.Double)]
        public double QuotaValue { get; set; }

        /// <summary>
        /// 预留
        /// </summary>
        [DataMapping("Reserved", "RESERVED", DbType.String)]
        public string Reserved { get; set; }

    }
}
