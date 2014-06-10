using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 分摊配置
    /// </summary>
    public class TB_ALLOCTION_CONFIG
    {
        /// <summary>
        /// 主键ID 
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 父区域ID 
        /// </summary>
        [DataMapping("ParentAREAID", "ParentAREAID", DbType.Int32)]
        public int ParentAREAID { get; set; }

        /// <summary>
        /// 子区域ID 
        /// </summary>
        [DataMapping("AREAID", "AREAID", DbType.Int32)]
        public int AREAID { get; set; }

        /// <summary>
        /// 描述 
        /// </summary>
        [DataMapping("DEC", "DEC", DbType.String)]
        public string DEC { get; set; }

        /// <summary>
        /// 待分摊费用 
        /// </summary>
        [DataMapping("ALLOCTION_FEE", "ALLOCTION_FEE", DbType.Double)]
        public double ALLOCTION_FEE { get; set; }

        /// <summary>
        /// 分摊实际比例 
        /// </summary>
        [DataMapping("CFGPERCENT", "CFGPERCENT", DbType.Double)]
        public double CFGPERCENT { get; set; }

        /// <summary>
        /// 分摊开始时间 
        /// </summary>
        [DataMapping("ALLOCTION_StartDate", "ALLOCTION_StartDate", DbType.DateTime)]
        public DateTime ALLOCTION_StartDate { get; set; }

        /// <summary>
        /// 能源类型 
        /// </summary>
        [DataMapping("PAYTYPE", "PAYTYPE", DbType.String)]
        public string PAYTYPE { get; set; }

        /// <summary>
        /// 分摊结束时间 
        /// </summary>
        [DataMapping("ALLOCTION_EndDate", "ALLOCTION_EndDate", DbType.DateTime)]
        public DateTime ALLOCTION_EndDate { get; set; }

        /// <summary>
        /// 分摊方式 
        /// </summary>
        [DataMapping("PAYClass", "PAYClass", DbType.Int32)]
        public int PAYClass { get; set; }
    }
}
