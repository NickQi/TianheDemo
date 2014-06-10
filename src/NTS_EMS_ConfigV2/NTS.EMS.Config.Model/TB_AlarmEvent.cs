using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    public class TB_AlarmEvent
    {
        public TB_AlarmEvent()
        {
        }

        /// <summary>
        /// 索引号
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID{ get; set; }

        /// <summary>
        /// 工程号
        /// </summary>
        [DataMapping("ProjectId", "PROJECTID", DbType.Int16)]
        public int ProjectId { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        [DataMapping("SystemId", "SYSTEMID", DbType.Int16)]
        public int SystemId { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMapping("AlarmTypeId", "ALARMTYPE", DbType.Int16)]
        public int AlarmTypeId { get; set; }

        /// <summary>
        /// 插件
        /// </summary>
        [DataMapping("PlugIn", "PLUGIN", DbType.String)]
        public string PlugIn { get; set; }

        /// <summary>
        /// 触发方式
        /// </summary>
        [DataMapping("TrigMode", "TRIGMODE", DbType.Int16)]
        public int TrigMode { get; set; }

        /// <summary>
        /// 执行方式
        /// </summary>
        [DataMapping("RunMode", "RUNMODE", DbType.Int16)]
        public int RunMode { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        [DataMapping("RunCount", "RUNCOUNT", DbType.Int16)]
        public int RunCount { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        [DataMapping("RunTime", "RUNTIME", DbType.Int16)]
        public int RunTime { get; set; }

        /// <summary>
        /// 配置域
        /// </summary>
        [DataMapping("Options", "OPTIONS", DbType.String)]
        public string Options { get; set; }

    }
}
