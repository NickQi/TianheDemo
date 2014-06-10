using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IUserObject
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        List<Model.TB_User> GetUserList(string whereStr);

        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int InsertUser(Model.User user);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int UpdateUser(Model.User user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteUser(int userId);

        /// <summary>
        /// 是否已有同名用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool IsContainUser(string userName);

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Model.TB_User GetUserInfo(int userId);

        /// <summary>
        /// 获取用户组名称
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        string GetUserGroupName(int userGroupId);

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        int GetMaxId();
    }
}
