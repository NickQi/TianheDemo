using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class Account
    {
        public Account()
		{}

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMapping("UserName","username",DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMapping("UserPass", "userpass", DbType.String)]
        public string UserPass { get; set; }
    }
}
