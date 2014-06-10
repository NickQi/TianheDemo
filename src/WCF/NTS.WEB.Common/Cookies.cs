using System;
using System.Web;
namespace NTS.WEB.Common
{
    /// <summary>  
    /// Cookie 的摘要说明  
    /// </summary>  
    public class Cookie
    {
        /// <summary>  
        /// 创建Cookies  
        /// </summary>  
        /// <param name="strName">Cookie 主键</param>  
        /// <param name="strValue">Cookie 键值</param>  
        /// <param name="strDay">Cookie 天数</param>  
        /// <code>Cookie ck = new Cookie();</code>  
        /// <code>ck.setCookie("主键","键值","天数");</code>  
        public bool SetCookie(string strName, string strValue, int strDay)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(strName);
                if (strDay != -2)
                {
                cookie.Expires = DateTime.Now.AddDays(strDay);
                }
                    cookie.Value = strValue;
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>  
        /// 读取Cookies  
        /// </summary>  
        /// <param name="strName">Cookie 主键</param>  
        /// <code>Cookie ck = new Cookie();</code>  
        /// <code>ck.getCookie("主键");</code>  
        public string GetCookie(string strName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
                return cookie.Value;
            else
                return string.Empty;
        }

        /// <summary>  
        /// 删除Cookies  
        /// </summary>  
        /// <param name="strName">Cookie 主键</param>  
        /// <code>Cookie ck = new Cookie();</code>  
        /// <code>ck.delCookie("主键");</code>  
        public bool DelCookie(string strName)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(strName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
