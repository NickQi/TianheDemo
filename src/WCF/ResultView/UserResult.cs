using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
namespace NTS.WEB.ResultView
{
    public class UserResult
    {
        public string UserName { get; set; }
        public string UserPass { get; set; }
    }
    /// <summary>
    /// 用户列表结果
    /// </summary>
    public class UserListResult
    {
        public ExecuteProcess ActionInfo;
        public List<UserList> UserList;
    }
    /// <summary>
    /// 单个用户结果
    /// </summary>
    public class SingleUserResult
    {
        public ExecuteProcess ActionInfo;
        public QueryUser QueryUser;
    }
    /// <summary>
    /// 用户组列表结果
    /// </summary>
    public class UserGroupListResult
    {
        public ExecuteProcess ActionInfo;
        public List<UserGroupModel> UserGroupList;
    }
    /// <summary>
    /// 单个用户组结果
    /// </summary>
    public class SingleUserGroupResult
    {
        public ExecuteProcess ActionInfo;
        public QueryUserGroup QueryUserGroup;
    }
  
}
