using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    /// <summary>
    /// 管理诊断
    /// </summary>
    public class AlarmDiagnoseModel
    {

        /// <summary>
        /// 报警类型
        /// </summary>
        [DataMapping("Item", "ALARMTYPE", DbType.Int32)]
        public int Item
        {
            get;
            set;
        }
        /// <summary>
        /// 报警个数
        /// </summary>
        [DataMapping("Anomaly", "ALARMCOUNT", DbType.Int32)]
        public int Anomaly
        {
            get;
            set;
        }
        /// <summary>
        /// 报警分数
        /// </summary>
        [DataMapping("AbnormalValue", "ALARMSCALE", DbType.Int32)]
        public int AbnormalValue
        {
            get;
            set;
        }
      
    }


}
