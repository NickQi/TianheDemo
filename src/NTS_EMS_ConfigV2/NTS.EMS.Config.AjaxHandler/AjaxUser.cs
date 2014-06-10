using System.Web;
using Framework.Ajax;
using Framework.Common;
using Framework.LogAndException;
using NTS.WEB.DataContact;


namespace NTS.EMS.Config.AjaxHandler
{

    public class AjaxUser
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [CustomAjaxMethod]
        public string UserLogin()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var loginInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryLogin>(inputValue);
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

            #region 判断是否是禁用的用户
            string where = string.Format(" and cname='{0}' and password='{1}' and status=1 ", loginInfo.LoginUser, loginInfo.LoginPass);
            var result = new NTS.EMS.Config.BLL.OperateUserBll().GetUserListNotPage(where);
            if (result.Count >0)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new NTS.WEB.ResultView.LoginResult() { Success=false, Msg="此用户已被禁用，请联系管理员！" });
            }
            #endregion

            var loginResult =
                Framework.Service.BaseWcf.CreateChannel<WEB.ServiceInterface.IUser>("UserLogin").UserLogin(loginInfo);
            if (loginResult.Success)
            {
                Utils.WriteCookie("IsLogin", "1");
                Utils.WriteCookie("userid", loginInfo.LoginUser);
                
                if (loginInfo.IsRemeberPass)
                {
                    // 记住密码
                    Utils.WriteCookie("saveusername", loginInfo.LoginUser, 30); // 记住密码的用户名
                    Utils.WriteCookie("savepass", "yes", 30); // 是否记住密码
                }
                else
                {
                    Utils.WriteCookie("saveusername", loginInfo.LoginUser,-1); // 记住密码的用户名
                    Utils.WriteCookie("savepass", "yes",-1); // 是否记住密码
                }
            }
            else
            {
                if (loginInfo.IsRemeberPass)
                {
                    loginInfo.LoginPass = DESEncrypt.EncryptMd5(loginInfo.LoginPass);
                    var loginResultCookies =
                    Framework.Service.BaseWcf.CreateChannel<WEB.ServiceInterface.IUser>("UserLogin").UserLogin(loginInfo);
                    if (loginResultCookies.Success)
                    {
                        Utils.WriteCookie("userid", loginInfo.LoginUser);
                        Utils.WriteCookie("IsLogin", "1");
                        if (loginInfo.IsRemeberPass)
                        {
                            // 记住密码
                            Utils.WriteCookie("saveusername", loginInfo.LoginUser, 30); // 记住密码的用户名
                            Utils.WriteCookie("savepass", "yes", 30); // 是否记住密码
                        }
                        else
                        {
                            Utils.WriteCookie("saveusername", loginInfo.LoginUser,-1); // 记住密码的用户名
                            Utils.WriteCookie("savepass", "yes",-1); // 是否记住密码
                        }
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(loginResultCookies);
                }
            }
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
            
            if (!string.IsNullOrEmpty(Utils.GetCookie("saveusername")))
            {
                string userid = Utils.GetCookie("saveusername");

                WEB.ResultView.UserResult loginResult =
                    Framework.Service.BaseWcf.CreateChannel<WEB.ServiceInterface.IUser>("UserLogin").GetUserInfo(userid);
                if (loginResult != null)
                {
                    return
                        Newtonsoft.Json.JsonConvert.SerializeObject(new QueryLogin
                            {
                                LoginUser = userid,
                                LoginPass = "AWDWD1234Q"+loginResult.UserPass,
                                IsRemeberPass = true
                            });
                }
            }
            return 
                        Newtonsoft.Json.JsonConvert.SerializeObject(new QueryLogin
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
        [CustomAjaxMethod]
        [CustomException]
        public void Logout()
        {
            Utils.WriteCookie("IsLogin", "1", -1);
            Utils.WriteCookie("userid", Utils.GetCookie("userid"), -1);
            _ntsPage.Response.Redirect("/login.html");
            _ntsPage.Response.End();
        }
        #endregion
    }
}
