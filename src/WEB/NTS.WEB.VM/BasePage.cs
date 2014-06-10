using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using Framework.Common.VM;


namespace NTS.WEB.VM
{
    public class BasePage : PageCore, IHttpHandler
    {
        /// <summary>
        /// 系统公共的全局信息数据
        /// </summary>
        /// <returns></returns>
        public Hashtable GetCommon()
        {
            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("gb2312");
         
            var ht = new Hashtable
                {
                  
                    {"ProjectName", NTS.WEB.AjaxController.ProjectInfo.Project.ProjectName},
                    {"ProjectContent", NTS.WEB.AjaxController.ProjectInfo.Project.ProjectContent},
                    {"ProjectUnit", NTS.WEB.AjaxController.ProjectInfo.Project.ProjectUnit},
                    //{"LoginUser", string.IsNullOrEmpty(Utils.GetCookie("userid")) ? "" :HttpUtility.UrlDecode(Utils.GetCookie("userid"), enc)},
                   {"LoginUser", string.IsNullOrEmpty(Utils.GetCookie("userid")) ? "" :Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(Utils.GetCookie("userid")))},
                    {"serverDate",DateTime.Now.ToString("yyyy-MM-dd")}
                    
                };
            return ht;
        }
   
        /// <summary>
        /// 渲染页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Handler_Load(object sender, EventArgs e)
        {
            string callIndex = Request.QueryString["CallIndex"];
           
            if (callIndex != "login")
            {
                

                if (string.IsNullOrEmpty(GetCookie("islogin")) || callIndex == null  )
                {
                    Response.Redirect("Login.html");
                    Response.End();
                }
                //else
                //{
                //    string key = Utils.GetCookie("userid");
                //    bool verify = false;
                //    string url = Request.RawUrl.Replace('/', ' ').Trim();
                //    if (url == "index.html" || url == "login.html")
                //    {
                //        verify = true;
                //    }
                //    else
                //    {
                //        List<string> menulist = NTS.WEB.Common.CacheHelper.GetCache(key) as List<string>;
                //        if (menulist != null)
                //        {
                //            if (menulist.Contains(url))
                //            {
                //                verify = true;
                //            }
                //        }
                //    }
                //    if(verify==false)
                //    {
                       
                //        Response.Redirect("Login.html");
                //        Response.End();
                //    }
                //}
            }

            var page = new PageCore(callIndex);
            TemplateFile = page._TemplateFile;
            TemplateData = page._TemplateData;


            string key = Utils.GetCookie("userid");
            bool verify = false;
            string url = Request.RawUrl.Replace('/', ' ').Trim().ToLower().Split('?')[0];
            if (  url == "login.html")
            {
                verify = true;
            }
            else
            {
                List<string> menulist = NTS.WEB.Common.CacheHelper.GetCache(key) as List<string>;
                
                if (menulist != null)
                {
                    if (menulist.Contains(url))
                    {
                        verify = true;
                    }
                }
                else
                {
                    Response.Redirect("Login.html");
                    Response.End();
                }
            }
            if (verify == false)
            {

                Response.Write("<script>alert('您没有权限访问该页面');</script>");
                Response.Write("<script language='javascript'>window.parent.location.href='login.html';</script>");
              
            }
           
           
            
        }

    
        private static string GetCookie(string strName)
        {
            string result="";
            if (HttpContext.Current.Request.Cookies.Count>0)
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
    }
}