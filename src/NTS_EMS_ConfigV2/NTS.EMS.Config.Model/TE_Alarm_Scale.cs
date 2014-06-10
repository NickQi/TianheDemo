using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.DataConfiguration;

namespace NTS.EMS.Config.Model
{
   public class TE_Alarm_Scale
    {
        /// <summary>
        /// 索引号
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMapping("AlarmType", "ALARMTYPE", DbType.Int16)]
        public int AlarmType { get; set; }

        /// <summary>
        /// 告警分值
        /// </summary>
        [DataMapping("Scale", "SCALE", DbType.Int16)]
        public int Scale { get; set; }

       [DataMapping("AlarmName", "AlarmName", DbType.String)]
        public string AlarmName { get; set; }
    }
}
