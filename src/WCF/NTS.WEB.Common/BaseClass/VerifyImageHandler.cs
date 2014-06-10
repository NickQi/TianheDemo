using System;
using System.Collections.Generic;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace NTS.WEB.Common
{
    public class VerifyImageHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 您将需要在您网站的 web.config 文件中配置此处理程序，
        /// 并向 IIS 注册此处理程序，然后才能进行使用。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }
        private HttpRequest Request;
        private HttpResponse Response;
        private HttpSessionState Session;
        /// <summary>
        /// 处理IHttpHandler请求
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Session = context.Session;

            VerifyImage dt_verifyImage = new VerifyImage();
            dt_verifyImage._Random = new Random();
            dt_verifyImage._Code = dt_verifyImage.GetRandomCode();
            dt_verifyImage.BackColor = ColorTranslator.FromHtml("#edf8fe");
            Session["SystemCode"] = dt_verifyImage._Code;

            ///设置没有缓存
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Pragma", "No-Cache");

            Bitmap objBitmap = dt_verifyImage.GetVerifyImage();
            objBitmap.Save(Response.OutputStream, ImageFormat.Gif);
            if (null != objBitmap)
                objBitmap.Dispose();
            Response.ContentType = "image/gif";
            Response.Write(Response.OutputStream);
        }
    }
}
