using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
    public class TB_AREA_Info
    {
        /// <summary>
        /// 主键ID 
        /// </summary>
        [DataMapping("AREAID", "AREAID", DbType.Int32)]
        public int AREAID { get; set; }

        [DataMapping("AREANUM", "AREANUM", DbType.Double)]
        public double AREANUM { get; set; }
    }
}
