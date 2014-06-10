using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;

namespace NTS.WEB.Common
{
    public class Page : System.Web.UI.Page
    {
       // public string EIWindowTitle = System.Configuration.ConfigurationSettings.AppSettings["SiteMainTitle"];
        public static void LoadJavascript()
        {
          //  HttpContext.Current.Response.Write("<script src=\"JQueryEasyUi/jquery-1.4.2.min.js\" type=\"text/javascript\"></script>");
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
            HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
            HttpContext.Current.Response.Write("function animate(){");
            HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
            HttpContext.Current.Response.Write("if(elem != null) {");
            HttpContext.Current.Response.Write("if (pos==0) len += dir;");
            HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
            HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
            HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
            HttpContext.Current.Response.Write("elem.style.left = pos;");
            HttpContext.Current.Response.Write("elem.style.width = len;");
            HttpContext.Current.Response.Write("}}");
            HttpContext.Current.Response.Write("function remove_loading() {");
            HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
            HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
            HttpContext.Current.Response.Write("targelem.style.display='none';");
            HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
            HttpContext.Current.Response.Write("}");
            HttpContext.Current.Response.Write("</script>");
            HttpContext.Current.Response.Write("<style>");
            HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
            HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
            HttpContext.Current.Response.Write("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
            HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("<div id='loader_container'>");
            HttpContext.Current.Response.Write("<div id=loader>");
            HttpContext.Current.Response.Write("<div align=center>页面正在加载中 </div>");
            HttpContext.Current.Response.Write("<div id=loader_bg><div id=progress> </div></div>");
            HttpContext.Current.Response.Write("</div></div>");
            HttpContext.Current.Response.Flush();
           // Alerts.Alert("sssss");
        }
        public void UnloadJavascript()
        {
           // HttpContext.Current.Response.Write("<script src=\"JQueryEasyUi/jquery-1.4.2.min.js\" type=\"text/javascript\"></script>");
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), null, "remove_loading();", true);
            this.Response.Write("<script>remove_loading();</script>");
        }
        protected override void OnInit(EventArgs e)
        {

            if (new NTS_BECM.Common.BaseClass.Cookie().getCookie("islogin") == null)
            {
                Response.Redirect("/default.aspx");
                return;
            }
            
            /*
            if (Session["IsLogin"] == null)
            {
                Response.Redirect("Error.aspx?flag=NullLogin&errMsg=" + HtmlEncode("请在登录页面输入用户名和密码访问该系统"));
                return;
            }
            int RoleId = int.Parse(Session["RoleID"].ToString());
            string StrRoleId = ConvertOperator(RoleId);
            if (!StrRoleId.Contains("1")) //判断是否是管理员
            {
                string url = System.IO.Path.GetFileName(Request.PhysicalPath);//取得访问的物理地址
                if (!PermisionManag(StrRoleId).Contains(url)) //判断是否有访问此页面权限
                {
                    Response.Redirect("Error.aspx?flag=NullRole&errMsg=" + HtmlEncode("对不起您无权限访问该页面，请联系管理员"));
                }
            }
            Error += new EventHandler(BaseWebPage_Error);
             * */
            base.OnInit(e);
       //     LoadJavascript();
          //  UnloadJavascript();
        }

        #region 后台URL编码
        public static string HtmlEncode(string strurl)
        {
            return Microsoft.JScript.GlobalObject.escape(strurl);
        }
        #endregion

        void BaseWebPage_Error(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            Exception currentError = HttpContext.Current.Server.GetLastError();
            errMsg += "<h1>系统错误：</h1><hr/>系统发生错误， " +
            "该信息已被系统记录，请稍后重试或与管理员联系。<br/>" +
            "错误地址： " + Request.Url.ToString() + "<br/>" +
            "错误信息： " + currentError.Message.ToString() + "<hr/>" +
            "<b>Stack Trace:</b><br/>" + currentError.ToString();
            Response.Redirect("Error.aspx?errmsg=" +HtmlEncode(errMsg));
        }

        private string ConvertOperator(int roleid)
        {
            StringBuilder sprole = new StringBuilder();
            int[] roleArrary = new int[] { 1, 2, 4, 8 };//权限标志位，权限0 无法登录登录之前已经判断
            for (int i = 0; i < roleArrary.Length; i++)
            {
                if ((roleid & roleArrary[i]) != 0)
                {
                    sprole.Append(roleArrary[i].ToString());
                }
            }
            return sprole.ToString();
        }

        /// <summary>
        /// 根据权限获取访问页面的权限
        /// </summary>
        /// <param name="roleid">权限值</param>
        /// <returns></returns>
        private string PermisionManag(string roleid)
        {
            StringBuilder sp = new StringBuilder();
            sp.Append("Default.aspx,");
            foreach (char ch in roleid)
            {
                switch (ch)
                {
                    case '2':
                        sp.Append("PersonManage.aspx,LabInfoManage.aspx,DeviceManage.aspx,DepartManage.aspx,EditPassWord.aspx,");
                        break;
                    case '4':
                        sp.Append("LabInfoSearch.aspx,EditPassWord.aspx");
                        break;
                }
            }
            return sp.ToString();
        }
    }
}
