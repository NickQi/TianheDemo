using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Framework.DataConfiguration;


namespace NTS.WEB.DataContact
{
    [DataContract]
    public class QueryUser
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        [DataMapping("UserID", "ID", DbType.Int32)]
        public int UserID
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        [DataMapping("UserName", "CNAME", DbType.String)]
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        [DataMapping("Password", "PASSWORD", DbType.String)]
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        [DataMapping("Status", "STATUS", DbType.Int32)]
        public int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        [DataMember]
        [DataMapping("GroupId", "GROUPS", DbType.String)]
        public String GroupId
        {

            get;
            set;
        }
    }

    [DataContract]
    public class QueryUserMenu
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        [DataMapping("LINKNAME", "LINKNAME", DbType.String)]
        public string LINKNAME
        {
            get;
            set;
        }
       
    }
}
