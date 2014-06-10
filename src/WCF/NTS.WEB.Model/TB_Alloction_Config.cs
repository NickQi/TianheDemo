using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class TB_Alloction_Config
    {
        /// <summary>
        /// id
        /// </summary>
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// ParentAreaId
        /// </summary>
        [DataMapping("ParentAreaId", "ParentAreaId", DbType.Int32)]
        public int ParentAreaId
        {
            get;
            set;
        }


        /// <summary>
        /// ParentAreaId
        /// </summary>
        [DataMapping("AreaID", "AreaID", DbType.Int32)]
        public int AreaID
        {
            get;
            set;
        }

        /// <summary>
        /// ParentAreaId
        /// </summary>
        [DataMapping("ALLoction_Fee", "ALLoction_Fee", DbType.Decimal)]
        public Double ALLoction_Fee
        {
            get;
            set;
        }

        /// <summary>
        /// ParentAreaId
        /// </summary>
        [DataMapping("CfgPercent", "CfgPercent", DbType.Double)]
        public Double CfgPercent
        {
            get;
            set;
        }
        
        /// <summary>
        /// ALLoction_StartDate
        /// </summary>
        [DataMapping("ALLoction_StartDate", "ALLoction_StartDate", DbType.DateTime)]
        public DateTime ALLoction_StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// ALLoction_EndDate
        /// </summary>
        [DataMapping("ALLoction_EndDate", "ALLoction_EndDate", DbType.DateTime)]
        public DateTime ALLoction_EndDate
        {
            get;
            set;
        }
    }
}
