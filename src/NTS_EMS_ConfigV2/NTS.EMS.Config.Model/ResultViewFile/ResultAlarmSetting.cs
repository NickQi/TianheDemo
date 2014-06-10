using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model.ResultViewFile
{
    public class ResultAlarmTypes
    {
        public List<AlarmType> AlarmTypeList { get; set; }
        public Padding Page { get; set; }
    }

    public class ResultAlarmEvents
    {
       public List<TB_AlarmEvent> ListAlarmEvent { get; set; }
    }

    public class ResultAlarmEvent
    {
        /// <summary>
        /// 标识新增或编辑的返回状态
        /// </summary>
        public bool IsSucess { get; set; }
    }

    public class AlarmType
    {
        /// <summary>
        /// 索引号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 工程号
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 告警等级ID
        /// </summary>
        public int AlarmLevelId { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        public int AlarmLevel { get; set; }

        /// <summary>
        /// 告警触发事件(插件名称)
        /// </summary>
        public List<AlarmEvent> PlugIns { get; set; }
    }

    public class AlarmEvent
    {
        /// <summary>
        /// 索引号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 工程号
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        public int AlarmTypeId { get; set; }

        /// <summary>
        /// 插件
        /// </summary>
        public string PlugIn { get; set; }

        /// <summary>
        /// 触发方式
        /// </summary>
        public int TrigMode { get; set; }

        /// <summary>
        /// 执行方式
        /// </summary>
        public int RunMode { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunCount { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public int RunTime { get; set; }

        /// <summary>
        /// 配置域
        /// </summary>
        public string Options { get; set; }
    }

    public class ResultAlarmScaleTypes
    {
        public List<AlarmScale> AlarmScaleList { get; set; }
        public Padding Page { get; set; }
    }

    public class AlarmScale
    {
        public int ID { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public int Scale { get; set; }

        public string AlarmName { get; set; }
    }
}
