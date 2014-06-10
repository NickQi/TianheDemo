using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{

    public class TB_UserGroup
    {
        public TB_UserGroup()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [DataMapping("Id", "ID", DbType.Int32)]
        public int Id{ get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMapping("Name", "CNAME", DbType.String)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMapping("Description", "DESCRIPTION", DbType.String)]
        public string Description { get; set; }

        /// <summary>
        /// 不用了
        /// </summary>
        [DataMapping("Groups", "GROUPS", DbType.String)]
        public string Groups { get; set; }

    }

    public class TB_UserGroupMenuRight
    {
        public TB_UserGroupMenuRight() { }

        /// <summary>
        /// 用户组ID
        /// </summary>
        [DataMapping("UserGroupId", "UserGroupID", DbType.Int32)]
        public int UserGroupId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [DataMapping("MenuId", "MenuID", DbType.Int32)]
        public int MenuId { get; set; }
    }

    public class TB_UserGroupObjectRight
    {
        public TB_UserGroupObjectRight() { }

        /// <summary>
        /// 用户组ID
        /// </summary>
        [DataMapping("UserGroupId", "UserGroupID", DbType.Int32)]
        public int UserGroupId { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        [DataMapping("ObjectId", "AreaID", DbType.Int32)]
        public int ObjectId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMapping("Type", "Type", DbType.Int32)]
        public int Type { get; set; }
    }

    public class TB_Menu
    {
        public TB_Menu() { }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [DataMapping("Id", "ID", DbType.Int32)]
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [DataMapping("Name", "MenuName", DbType.String)]
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [DataMapping("ParentId", "ParentID", DbType.Int32)]
        public int ParentId { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        [DataMapping("LinkName", "LinkName", DbType.String)]
        public string LinkName { get; set; }

        /// <summary>
        /// IconClass
        /// </summary>
        [DataMapping("IconClass", "IconClass", DbType.String)]
        public string IconClass { get; set; }

        /// <summary>
        /// callIndex
        /// </summary>
        [DataMapping("CallIndex", "CallIndex", DbType.String)]
        public string CallIndex { get; set; }
    }
}
