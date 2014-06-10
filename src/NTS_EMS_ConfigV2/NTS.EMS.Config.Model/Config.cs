using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class Config
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get { return ConfigurationManager.AppSettings["SystemName"]; } }
        /// <summary>
        /// 系统皮肤路径
        /// </summary>
        public string Skin {
            get { return ConfigurationManager.AppSettings["Skin"]; }
        }
    }
}
