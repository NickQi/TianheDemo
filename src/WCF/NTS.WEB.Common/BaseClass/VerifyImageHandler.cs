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
        /// ������Ҫ������վ�� web.config �ļ������ô˴������
        /// ���� IIS ע��˴������Ȼ����ܽ���ʹ�á��й���ϸ��Ϣ��
        /// ��μ����������: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public bool IsReusable
        {
            // ����޷�Ϊ�������������йܴ�������򷵻� false��
            // �����������ĳЩ״̬��Ϣ����ͨ���⽫Ϊ false��
            get { return true; }
        }
        private HttpRequest Request;
        private HttpResponse Response;
        private HttpSessionState Session;
        /// <summary>
        /// ����IHttpHandler����
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

            ///����û�л���
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
