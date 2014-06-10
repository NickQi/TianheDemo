using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 用户数据增加、修改
    /// </summary>
    [DataContract]
    public class UserDataContact
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// CName,用户名(也是主键）
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string PassWord { get; set; }

        /// <summary>
        /// 状态：0启用，1停用
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 所属用户组
        /// </summary>
        [DataMember]
        public string GroupId { get; set; }
    }


    /// <summary>
    /// 用户数据详细获取
    /// </summary>
    [DataContract]
    public class UserDataDetailContact
    {
        [DataMember]
        public int UserID { get; set; }
    }

    /// <summary>
    /// 用户查询
    /// </summary>
    [DataContract]
    public class QueryUserContact
    {
        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}
