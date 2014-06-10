namespace NTS.WEB.ResultView
{
    public class LoginCookiesInfo
    {
        /// <summary>
        /// Cookies登录的用户
        /// </summary>
        public string LoginUser { get; set; }
        /// <summary>
        /// Cookies 登录的密码信息（已加密）
        /// </summary>
        public string LoginPass { get; set; }
        /// <summary>
        /// Cookies 是否保存密码
        /// </summary>
        public bool IsRember { get; set; }
    }
}
