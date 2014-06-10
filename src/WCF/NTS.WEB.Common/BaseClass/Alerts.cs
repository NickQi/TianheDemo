///作者：徐佳佳
///时间：2009/07/29
///功能：服务器弹框
///
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Web;

namespace NTS.WEB.Common
{
    /// <summary>
    /// Alert 的说明,此类专门负责弹出各种各样的窗口。
    /// </summary>
    public class Alerts
    {
        #region 通过js跳转至指定的页面
        /// <summary>
        /// 通过js跳转至指定的页面
        /// </summary>
        /// <param name="url"></param>
        /// <param name="page"></param>
        public static void Loaction(string url)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script defer=\"defer\">window.location='" + url + "';</script>");
        }
        #endregion

        #region 通过js刷新本页
        /// <summary>
        /// 通过js刷新本页
        /// </summary>
        /// <param name="url"></param>
        /// <param name="page"></param>
        public static void Refresh()
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script defer=\"defer\">window.location='" + page.Request.Url + "';</script>");
        }
        #endregion

        #region 服务器端弹出alert对话框
        /// <summary>
        /// 服务器端弹出alert对话框,如果类型是JQuery需要JavaScriptManage
        /// </summary>
        public static void Alert(string str_Message)
        {
            Alert(str_Message, null);
        }
        /// <summary>
        /// 服务器端弹出alert对话框,如果类型是JQuery需要JavaScriptManage
        /// </summary>
        public static void Alert(string str_Message, string code)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script defer=\"defer\">alert('" + str_Message + "');" + code + "</script>");
        }
        #endregion

        #region 执行制定的js脚本
        /// <summary>
        /// 执行制定的js脚本
        /// </summary>
        /// <param name="code">js脚本</param>
        /// <param name="page">page类</param>
        public static void exec(string code)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script type=\"text/javascript\" defer=\"defer\">" + code + "</script>");
        }
        #endregion

        #region 服务器端弹出confirm对话框.
        /// <summary>
        /// 服务器端弹出confirm对话框.
        /// </summary>
        /// <param name="button">隐藏Botton按钮Id值,比如：btn_Flow</param>
        /// <param name="confirmString">提示信息,例子："您是否确认删除!"</param>
        public static void Confirm(Button button, string confirmString)
        {
            button.Attributes.Add("onClick", "return confirm('" + System.Web.HttpUtility.HtmlEncode(confirmString) + "');");
        }
        #endregion
    }
}