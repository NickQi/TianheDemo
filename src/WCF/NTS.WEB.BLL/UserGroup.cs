using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.BLL
{
    public class UserGroup
    {
        private readonly IUserGroup dal = DataSwitchConfig.CreateUserGroup();

        public List<UserGroupModel> GetUserGroups()
        {
            return dal.GetUserGroups();
        }
        public QueryUserGroup GetSingleUserGroup(int usergroupid)
        {
            return dal.GetSingleUserGroup(usergroupid);
        }
        public bool IsExistUserGroupName(QueryUserGroup model)
        {
            return true;
            //return dal.IsExistUserGroupName(model);
        }

        public void AddUserGroup(QueryUserGroup model)
        {
           //dal.AddUserGroup(model);
        }

        public void UpdateUserGroup(QueryUserGroup model)
        {
            //dal.UpdateUserGroup(model);
        }

        public void DeleteUserGroup(int id)
        {
           // dal.DeleteUserGroup(id);
        }
    }
}
