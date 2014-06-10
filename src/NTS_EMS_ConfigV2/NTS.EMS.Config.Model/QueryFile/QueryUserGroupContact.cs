using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 用户组数据增加、修改
    /// </summary>
    [DataContract]
    public class UserGroupDataContact
    {
        [DataMember]
        public UserGroupData UGData { get; set; }

        [DataMember]
        public List<UserGroupMenuRightData> UGMenuRightDataList { get; set; }

        [DataMember]
        public List<UserGroupObjectRightData> UGObjectRightDataList { get; set; }

    }

    /// <summary>
    /// Tb_UserGroup 表对应数据
    /// </summary>
    [DataContract]
    public class UserGroupData
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
        /// 说明
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }

    /// <summary>
    /// Tb_UserGroupMenuRight 表对应数据
    /// </summary>
    [DataContract]
    public class UserGroupMenuRightData
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [DataMember]
        public int MenuId { get; set; }
    }

    /// <summary>
    /// Tb_UserGroupObjectRight 表对应数据
    /// </summary>
    [DataContract]
    public class UserGroupObjectRightData
    {
        /// <summary>
        /// 对象ID
        /// </summary>
        [DataMember]
        public int ObjdetId { get; set; }

        /// <summary>
        /// 类型： 2--业态、 1--区域
        /// </summary>
        [DataMember]
        public int Type { get; set; }

    }

    /// <summary>
    /// 用户组详细信息
    /// </summary>
    [DataContract]
    public class QueryUserGroupDetailContact
    {
        /// <summary>
        /// 用户组ID值
        /// </summary>
        [DataMember]
        public int UserGroupId { get; set; }
    }


    /// <summary>
    /// 用户组查询
    /// </summary>
    [DataContract]
    public class QueryUserGroupContact
    {
        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}
