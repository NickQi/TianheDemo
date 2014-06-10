using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
namespace NTS.WEB.AjaxController
{

    public class AjaxUser
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public string UserLogin()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var loginInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryLogin>(inputValue);
            //var loginFlag = false;
            #region 是否保存
            if (Utils.GetCookie("savepass") == "yes")
            {
                try
                {
                    loginInfo.LoginPass = loginInfo.LoginPass.Substring(10);
                }
                catch
                {
                    #region 文本框中的基本密码

                    loginInfo.LoginPass = DESEncrypt.EncryptMd5(loginInfo.LoginPass);

                    #endregion
                }
            }
            else
            {
                loginInfo.LoginPass = DESEncrypt.EncryptMd5(loginInfo.LoginPass);
            }
            #endregion

            var loginResult =Framework.Common.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").UserLogin(loginInfo);
            //UserServiceReferenceTest.UserClient u = new UserServiceReferenceTest.UserClient();
            //var loginResult = u.UserLogin(loginInfo);

            
            //var loginResult =

            //   Framework.Service.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").UserLogin(loginInfo);
           if (loginResult.Success)
            {
                Utils.WriteCookie("islogin", "1");
                Utils.WriteCookie("userid", loginInfo.LoginUser);
                HttpContext.Current.Session["userid"] = loginInfo.LoginUser;
              

               
                var menuList =
                           Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").
                               GetSingleUserMenu(loginInfo.LoginUser);
                string cachekey = loginInfo.LoginUser;
                NTS.WEB.Common.CacheHelper.RemoveCache(cachekey);
                NTS.WEB.Common.CacheHelper.SetCache(cachekey, menuList);
                if (loginInfo.IsRemeberPass)
                {
                    // 记住密码
                    Utils.WriteCookie("saveusername", loginInfo.LoginUser, 30); // 记住密码的用户名
                    Utils.WriteCookie("savepass", "yes", 30); // 是否记住密码
                }
                else
                {
                    Utils.WriteCookie("saveusername", loginInfo.LoginUser, -1); // 记住密码的用户名
                    Utils.WriteCookie("savepass", "yes", -1); // 是否记住密码
                }
            }
            else
            {
                if (loginInfo.IsRemeberPass)
                {
                    loginInfo.LoginPass = DESEncrypt.EncryptMd5(loginInfo.LoginPass);
                    var loginResultCookies =
                    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").UserLogin(loginInfo);


                    if (loginResultCookies.Success)
                    {
                       // System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
                        //loginInfo.LoginUser=  HttpUtility.UrlEncode(loginInfo.LoginUser, enc);
                        
                        Utils.WriteCookie("userid", loginInfo.LoginUser);
                        //HttpContext.Current.Session["userid"] = loginInfo.LoginUser;
                        Utils.WriteCookie("islogin", "1");
                        var menuList =
                            Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").
                                GetSingleUserMenu(loginInfo.LoginUser);
                        string cachekey = loginInfo.LoginUser;
                        NTS.WEB.Common.CacheHelper.RemoveCache(cachekey);
                        NTS.WEB.Common.CacheHelper.SetCache(cachekey, menuList);
                        if (loginInfo.IsRemeberPass)
                        {
                            // 记住密码
                            Utils.WriteCookie("saveusername", loginInfo.LoginUser, 30); // 记住密码的用户名
                            Utils.WriteCookie("savepass", "yes", 30); // 是否记住密码
                        }
                        else
                        {
                            Utils.WriteCookie("saveusername", loginInfo.LoginUser, -1); // 记住密码的用户名
                            Utils.WriteCookie("savepass", "yes", -1); // 是否记住密码
                        }
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(loginResultCookies);
                }
            }
           loginResult.RedirectUrl = "/html/main.htm";
            return Newtonsoft.Json.JsonConvert.SerializeObject(loginResult);
        }
        #endregion

        #region 获取记住的密码
        /// <summary>
        /// 获取记住的密码
        /// </summary>
        /// <returns></returns>
        [CustomAjaxMethod]
        public string RemeberMyPass()
        {

            if (!string.IsNullOrEmpty(Framework.Common.Utils.GetCookie("saveusername")))
            {
                string userid = Utils.GetCookie("saveusername");

                ResultView.UserResult loginResult =
                    Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").GetUserInfo(userid);
                if (loginResult != null)
                {
                    return
                        Newtonsoft.Json.JsonConvert.SerializeObject(new QueryLogin()
                            {
                                LoginUser = userid,
                                LoginPass = "AWDWD1234Q" + loginResult.UserPass,
                                IsRemeberPass = true
                            });
                }
            }
            return
                        Newtonsoft.Json.JsonConvert.SerializeObject(new QueryLogin()
                            {
                                LoginUser = string.Empty,
                                LoginPass = string.Empty,
                                IsRemeberPass = false
                            });
        }
        #endregion

        #region 用户退出系统
        /// <summary>
        /// 用户退出系统
        /// </summary>
        /// <returns></returns>
       
      
        [Framework.Common.CustomAjaxMethod]
        public string Logout()
        {
            try
            {
             
                Framework.Common.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").Logout();
                Utils.WriteCookie("islogin", "1", -1);
                Utils.WriteCookie("userid", Utils.GetCookie("userid"), -1);
                NTS.WEB.Common.CacheHelper.RemoveCache("username");
                _ntsPage.Response.Redirect("/login.html", false);
                return "";
            }
            catch (Exception ee)
            {
                return "";
            }


        }
        #endregion

        [Framework.Common.CustomAjaxMethod]
        public UserListResult GetUsers()
        {
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").GetUsers();
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ExecuteProcess AddUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUser>(inputValue);
            query.Password = DESEncrypt.EncryptMd5(query.Password);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").AddUser(query);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public SingleUserResult GetSingleUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUser>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").GetSingleUser(query.UserID);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public SingleUserResult GetSingleUser(int userid )
        {
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").GetSingleUser(userid);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ExecuteProcess UpdateUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUser>(inputValue);
            query.Password = DESEncrypt.EncryptMd5(query.Password);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").UpdateUser(query);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ExecuteProcess DeleteUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUser>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUser>("UserLogin").DeleteUser(query.UserID);
            return res;
        }
    }
}
