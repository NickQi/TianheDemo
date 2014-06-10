using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.ProductInteface
{
   public interface IUser
   {
       List<UserList> GetUsers();
       QueryUser GetSingleUser(int userid);
       List<QueryUserMenu> GetSingleUserMenu(string username);
       //void AddUser(QueryUser model);
       //void UpdateUser(QueryUser model);
       //void DeleteUser(int  id);
       //bool IsExistUserName(QueryUser model);
   }
}
