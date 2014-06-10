using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class TB_AreaInfo
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
        /// id
        /// </summary>
        [DataMapping("CName", "CName", DbType.String)]
        public string CName
        {
            get;
            set;
        }
    }
}
