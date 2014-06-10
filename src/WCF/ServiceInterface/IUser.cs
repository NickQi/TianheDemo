using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
   [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IUser
    {
        [OperationContract]
        LoginResult UserLogin(QueryLogin login);

        [OperationContract]
        void Logout();
        [OperationContract]
        LoginCookiesInfo GetLoginCookiesInfo();

        [OperationContract]
        UserResult GetUserInfo(string userName);


        [OperationContract]
        UserListResult GetUsers();
        [OperationContract]
        ExecuteProcess AddUser(QueryUser model);
        [OperationContract]
        ExecuteProcess UpdateUser(QueryUser model);
        [OperationContract]
        ExecuteProcess DeleteUser(int id);
        [OperationContract]
        SingleUserResult GetSingleUser(int userid);

       [OperationContract]
        List<string> GetSingleUserMenu(string username);

       [OperationContract]
       int GetUserGroupID(string username);
    }
}
