using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;

namespace NTS.WEB.BLL
{
    public class User
    {
        private readonly IUser dal = DataSwitchConfig.CreateUser();
        public List<UserList> GetUsers()
        {
            return dal.GetUsers();
        }
        public bool IsExistUserName(QueryUser model)
        {
            return true;
            //return dal.IsExistUserName(model);
        }

        public void AddUser(QueryUser model)
        {
            // dal.AddUser(model);
        }

        public void UpdateUser(QueryUser model)
        {
            //dal.UpdateUser(model);
        }

        public void DeleteUser(int id)
        {
            //dal.DeleteUser(id);
        }
        public QueryUser GetSingleUser(int userid)
        {
            return dal.GetSingleUser(userid);
        }
        public List<QueryUserMenu> GetSingleUserMenu(string username)
        {
            return dal.GetSingleUserMenu(username);
        }

        public int GetUserGroupID(string username)
        {
            IAccessCommon dalCommon = DataSwitchConfig.CreateAccessCommon();
            return dalCommon.GetUserGroupID(username);
        }

    }
}
