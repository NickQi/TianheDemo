using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.DataConfiguration;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 分摊配置日志表
    /// </summary>
    public class TB_ALLOCTION_CONFIG_History
    {
        /// <summary>
        /// 主键ID 
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 操作用户 
        /// </summary>
        [DataMapping("OPTIONUSER", "OPTIONUSER", DbType.String)]
        public string OPTIONUSER { get; set; }

        /// <summary>
        /// 操作对象 
        /// </summary>
        [DataMapping("CFGOBJECT", "CFGOBJECT", DbType.Int32)]
        public int CFGOBJECT { get; set; }


        /// <summary>
        /// 分摊内容
        /// </summary>
        [DataMapping("CFGDEC", "CFGDEC", DbType.String)]
        public string CFGDEC { get; set; }

        /// <summary>
        /// 分摊月份时间
        /// </summary>
        [DataMapping("CFGDATE", "CFGDATE", DbType.DateTime)]
        public DateTime CFGDATE { get; set; }

        /// <summary>
        /// 写日志时间
        /// </summary>
        [DataMapping("OPTIONTIME", "OPTIONTIME", DbType.DateTime)]
        public DateTime OPTIONTIME { get; set; }

        /// <summary>
        /// 分摊方式
        /// </summary>
        [DataMapping("PAYClass", "PAYClass", DbType.Int32)]
        public int PAYClass { get; set; }

        [DataMapping("CNAME", "CNAME", DbType.String)]
        public string CNAME { get; set; }
    }
}
