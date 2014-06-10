using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    public class TB_AlarmType
    {
        public TB_AlarmType()
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
        [DataMapping("Type", "TYPE", DbType.Int16)]
        public int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMapping("Name", "NAME", DbType.String)]
        public string Name { get; set; }

        /// <summary>
        /// 告警发生时的动作
        /// </summary>
        [DataMapping("ACTIONA", "ACTIONA", DbType.String)]
        public string ACTIONA { get; set; }

        /// <summary>
        /// 告警恢复时的动作
        /// </summary>
        [DataMapping("ACTIONB", "ACTIONB", DbType.String)]
        public string ACTIONB { get; set; }

        /// <summary>
        /// 0-不记录实时告警状态；1-记录实时告警状态
        /// </summary>
        [DataMapping("RTALARM", "RTALARM", DbType.Int16)]
        public int RTALARM { get; set; }

        /// <summary>
        /// 告警等级ID
        /// </summary>
        [DataMapping("AlarmLevel", "ALARMLEVEL", DbType.Int16)]
        public int AlarmLevel { get; set; }

    }
}
