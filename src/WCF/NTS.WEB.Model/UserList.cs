using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class UserList
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMapping("UserID", "USERID", DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMapping("UserName", "USERNAME", DbType.String)]
        public string UserName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMapping("Status", "STATUS", DbType.String)]
        public string Status { get; set; }
        /// <summary>
        /// 用户组
        /// </summary>
        [DataMapping("GroupName", "GROUPNAME", DbType.String)]
        public string GroupName { get; set; }
    }
}
