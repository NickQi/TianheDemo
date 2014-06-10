using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{

    public class TB_User
    {
        public TB_User()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [DataMapping("Id", "ID", DbType.Int32)]
        public int Id{ get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMapping("Name", "CNAME", DbType.String)]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMapping("PassWord", "PASSWORD", DbType.String)]
        public string PassWord { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMapping("Status", "STATUS", DbType.Int16)]
        public int Status { get; set; }

        /// <summary>
        /// 所属用户组
        /// </summary>
        [DataMapping("GroupId", "GROUPS", DbType.String)]
        public string GroupId { get; set; }

    }

}
