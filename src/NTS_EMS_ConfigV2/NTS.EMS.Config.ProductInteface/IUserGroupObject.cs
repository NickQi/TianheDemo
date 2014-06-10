using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IUserGroupObject
    {
        /// <summary>
        /// 用户组列表
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        List<Model.TB_UserGroup> GetUserGroupList(string whereStr);

        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <param name="userGroup">用户组信息</param>
        /// <param name="menuIds">菜单IDs</param>
        /// <param name="objectIds">对象IDs</param>
        /// <param name="objectTypes">对象类型</param>
        /// <returns></returns>
        int SaveUserGroupInfo(Model.UserGroup userGroup, string menuIds, string objectIds, string objectTypes);

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        int DeleteUserGroup(int userGroupId);

        /// <summary>
        /// 是否包含用户
        /// </summary>
        /// <param name="uerGroupId"></param>
        /// <returns></returns>
        bool ContainUser(int uerGroupId);

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        Model.TB_UserGroup GetUserGroupInfo(int userGroupId);

        /// <summary>
        /// 获取用户组菜单权限列表
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        List<Model.TB_UserGroupMenuRight> GetUserGroupMenuRightList(int userGroupId);

        /// <summary>
        /// 获取用户组对象列表
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        List<Model.TB_UserGroupObjectRight> GetUserGroupObjectRightList(int userGroupId);

        /// <summary>
        /// 获取最大ID
        /// </summary>
        /// <returns></returns>
        int GetMaxId();

        /// <summary>
        /// 获取菜单表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        List<Model.TB_Menu> GetMenuList(string where, string order);
    }
}
    