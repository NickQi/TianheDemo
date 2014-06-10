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
    public class QueryUserGroup
    {
        /// <summary>
        ///用户组ID
        /// </summary>
        [DataMember]
        [DataMapping("UserGroupID", "ID", DbType.Int32)]
        public int UserGroupID
        {
            get;
            set;
        }
        /// <summary>
        /// 用户组名称
        /// </summary>
        [DataMember]
        [DataMapping("UserGroupName", "CNAME", DbType.String)]
        public string UserGroupName
        {
            get;
            set;
        }
        /// <summary>
        /// 用户组描述
        /// </summary>
        [DataMember]
        [DataMapping("Description", "DESCRIPTION", DbType.String)]
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 用户组菜单权限
        /// </summary>
        [DataMember]
        public List<int> UserGroupMenuRights { get; set; }
        /// <summary>
        /// 用户组液态权限
        /// </summary>
        [DataMember]
        public List<int> UserGroupLiquidRights { get; set; }
        /// <summary>
        /// 用户组区域权限
        /// </summary>
        [DataMember]
        public List<int> UserGroupAreaRights { get; set; }

    }

    public class GroupRight
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        [DataMember]
        [DataMapping("RightID", "RIGHTID", DbType.Int32)]
        public int RightID
        {
            get;
            set;
        }

    }


}
