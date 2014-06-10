using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.WEB.WebSite.Ajax
{
    /// <summary>
    /// ServerTimePaser 的摘要说明
    /// </summary>
    public class ServerTimePaser : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.Params["action"].ToString();
            string parseretult;
            switch (action)
            {
                case "GetServerTodayDate":
                    parseretult = GetServerTodayDate();
                    break;
                default:
                    parseretult = GetServerTodayDate();
                    break;
            }
            context.Response.Write(parseretult);
        }

        /// <summary>
        /// 获取服务器的系统时间，具体到日
        /// </summary>
        /// <returns></returns>
        public string GetServerTodayDate()
        {
            string dt = System.DateTime.Now.ToString("yyyy-MM-dd");
            return dt;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}