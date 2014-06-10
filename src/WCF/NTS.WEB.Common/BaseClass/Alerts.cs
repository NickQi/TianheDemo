///���ߣ���Ѽ�
///ʱ�䣺2009/07/29
///���ܣ�����������
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
    /// Alert ��˵��,����ר�Ÿ��𵯳����ָ����Ĵ��ڡ�
    /// </summary>
    public class Alerts
    {
        #region ͨ��js��ת��ָ����ҳ��
        /// <summary>
        /// ͨ��js��ת��ָ����ҳ��
        /// </summary>
        /// <param name="url"></param>
        /// <param name="page"></param>
        public static void Loaction(string url)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script defer=\"defer\">window.location='" + url + "';</script>");
        }
        #endregion

        #region ͨ��jsˢ�±�ҳ
        /// <summary>
        /// ͨ��jsˢ�±�ҳ
        /// </summary>
        /// <param name="url"></param>
        /// <param name="page"></param>
        public static void Refresh()
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script defer=\"defer\">window.location='" + page.Request.Url + "';</script>");
        }
        #endregion

        #region �������˵���alert�Ի���
        /// <summary>
        /// �������˵���alert�Ի���,���������JQuery��ҪJavaScriptManage
        /// </summary>
        public static void Alert(string str_Message)
        {
            Alert(str_Message, null);
        }
        /// <summary>
        /// �������˵���alert�Ի���,���������JQuery��ҪJavaScriptManage
        /// </summary>
        public static void Alert(string str_Message, string code)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script defer=\"defer\">alert('" + str_Message + "');" + code + "</script>");
        }
        #endregion

        #region ִ���ƶ���js�ű�
        /// <summary>
        /// ִ���ƶ���js�ű�
        /// </summary>
        /// <param name="code">js�ű�</param>
        /// <param name="page">page��</param>
        public static void exec(string code)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(page.GetType(), "", "<script type=\"text/javascript\" defer=\"defer\">" + code + "</script>");
        }
        #endregion

        #region �������˵���confirm�Ի���.
        /// <summary>
        /// �������˵���confirm�Ի���.
        /// </summary>
        /// <param name="button">����Botton��ťIdֵ,���磺btn_Flow</param>
        /// <param name="confirmString">��ʾ��Ϣ,���ӣ�"���Ƿ�ȷ��ɾ��!"</param>
        public static void Confirm(Button button, string confirmString)
        {
            button.Attributes.Add("onClick", "return confirm('" + System.Web.HttpUtility.HtmlEncode(confirmString) + "');");
        }
        #endregion
    }
}