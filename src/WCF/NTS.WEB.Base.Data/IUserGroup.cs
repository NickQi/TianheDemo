using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IUserGroup
    {
        List<UserGroupModel> GetUserGroups();
        QueryUserGroup GetSingleUserGroup(int usergroupid);
        //void AddUserGroup(QueryUserGroup model);
        //void UpdateUserGroup(QueryUserGroup model);
        //void DeleteUserGroup(int id);
        //bool IsExistUserGroupName(QueryUserGroup model);
    }
}
