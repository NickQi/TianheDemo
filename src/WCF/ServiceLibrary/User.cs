using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;
using Framework.Common;
namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class User : IUser
    {

        [Log(ModelName = "用户登录")]
        [CustomException]
        public LoginResult UserLogin(QueryLogin login)
        {
            var loginModel = new NTS.WEB.BLL.Account().GetAccount(login.LoginUser);
            List<string> menus = new List<string>();
            var menulist = new NTS.WEB.BLL.User().GetSingleUserMenu(login.LoginUser);
            menus = (from i in menulist select i.LINKNAME.ToLower()).ToList<string>();
            string redirecturl = (from i in menus where i.EndsWith("html") select i).FirstOrDefault();
            if (string.IsNullOrEmpty(redirecturl))
            {
                redirecturl = "";
            }
            if (loginModel != null)
            {
                if (loginModel.UserPass == login.LoginPass)
                {
                    return new LoginResult { Success = true, Msg = "", Menus = menus, RedirectUrl = redirecturl };


                }
                return new LoginResult { Success = false, Msg = "用户的密码不正确。" };
            }
            return new LoginResult { Success = false, Msg = "用户名不存在。" };
        }

        [Log(ModelName = "用户登出")]
        [CustomException]
        public void Logout()
        {
            NTS.WEB.Common.CacheHelper.RemoveCache("username");
        }
        private static void WriteCookie(string strName, string strValue)
        {

            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }
        private static string GetCookie(string strName)
        {
            string result = "";
            if (HttpContext.Current.Request.Cookies.Count > 0)
            {
                if (HttpContext.Current.Request.Cookies[strName] != null)
                {
                    result = HttpContext.Current.Request.Cookies[strName].Value;
                }
            }
            else
            {
                result = "";
            }
            return result;
        }
        public LoginCookiesInfo GetLoginCookiesInfo()
        {
            var loginCookiesInfo = new LoginCookiesInfo
            {
                LoginUser = "admin",
                LoginPass = "admin888",
                IsRember = true
            };
            return loginCookiesInfo;
        }

        public UserResult GetUserInfo(string userName)
        {
            var loginModel = new NTS.WEB.BLL.Account().GetAccount(userName);
            if (loginModel != null)
            {
                return new UserResult()
                    {
                        UserName = loginModel.UserName,
                        UserPass = loginModel.UserPass
                    };
            }
            return null;
        }

        [Log(ModelName = "用户列表")]
        [CustomException]
        public UserListResult GetUsers()
        {
            UserListResult result = new UserListResult();
            var pAction = new ExecuteProcess();
            try
            {
                var userlist = new NTS.WEB.BLL.User().GetUsers();

                if (userlist.Count > 0)
                {
                    pAction.Success = true;
                    result.UserList = userlist;
                    result.ActionInfo = pAction;
                    return result;
                }
                else
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    result.ActionInfo = pAction;
                    return result;

                }


            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                result.ActionInfo = pAction;
                return result;
            }


        }

        [Log(ModelName = "获取单个用户")]
        [CustomException]
        public SingleUserResult GetSingleUser(int userid)
        {
            SingleUserResult result = new SingleUserResult();
            var pAction = new ExecuteProcess();
            try
            {
                var query = new NTS.WEB.BLL.User().GetSingleUser(userid);

                if (query != null)
                {

                    //query.Password = DESEncrypt.Decrypt(query.Password);
                    pAction.Success = true;
                    pAction.ExceptionMsg = "获取单个用户成功";
                    result.QueryUser = query;
                    result.ActionInfo = pAction;
                    return result;
                }
                else
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    result.ActionInfo = pAction;
                    return result;

                }


            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                result.ActionInfo = pAction;
                return result;
            }
        }

        [Log(ModelName = "新增用户")]
        [CustomException]
        public ExecuteProcess AddUser(QueryUser model)
        {
            var pAction = new ExecuteProcess();
            try
            {
                if (new NTS.WEB.BLL.User().IsExistUserName(model))
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "该用户名已存在";
                    return pAction;
                }
                else
                {


                    new NTS.WEB.BLL.User().AddUser(model);
                    pAction.Success = true;
                    pAction.ExceptionMsg = "新增用户成功";
                    return pAction;
                }

            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return pAction;
            }

        }

        [Log(ModelName = "更新用户")]
        [CustomException]
        public ExecuteProcess UpdateUser(QueryUser model)
        {
            var pAction = new ExecuteProcess();
            try
            {
                if (new NTS.WEB.BLL.User().IsExistUserName(model))
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "该用户名已存在";
                    return pAction;
                }
                else
                {
                    new NTS.WEB.BLL.User().UpdateUser(model);
                    pAction.Success = true;
                    pAction.ExceptionMsg = "更新用户成功";
                    return pAction;
                }
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return pAction;
            }

        }
        [Log(ModelName = "删除用户")]
        [CustomException]
        public ExecuteProcess DeleteUser(int id)
        {
            var pAction = new ExecuteProcess();
            try
            {
                new NTS.WEB.BLL.User().DeleteUser(id);
                pAction.Success = true;
                pAction.ExceptionMsg = "删除用户成功";
                return pAction;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return pAction;
            }

        }

        [Log(ModelName = "获取用户菜单")]
        [CustomException]
        public List<string> GetSingleUserMenu(string username)
        {
            List<string> result = new List<string>();
            var menulist = new NTS.WEB.BLL.User().GetSingleUserMenu(username);
            result = (from i in menulist select  i.LINKNAME.ToLower()).ToList<string>();

            return result;


        }

        [Log(ModelName = "获取用户组")]
        [CustomException]
        public int GetUserGroupID(string username)
        {
            return new NTS.WEB.BLL.User().GetUserGroupID(username);
        }
    }
}
