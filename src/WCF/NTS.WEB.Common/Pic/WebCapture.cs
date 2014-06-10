using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace WindowsApplication1
{
    public class WebCapture
    {
        #region 字段、属性
        private int width = 0;
        private int height = 0;
        private string url;
        private WebBrowser browser = new WebBrowser();

        /// <summary>
        /// 属性：图片宽
        /// </summary>
        public int Width { get { return width; } }
        /// <summary>
        /// 属性：图片高
        /// </summary>
        public int Height { get { return height; } }
        /// <summary>
        /// 属性：Url
        /// </summary>
        public string Url { get { return url; } }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebCapture()
        {
            this.url = "http://www.mzwu.com/";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">Url</param>
        public WebCapture(string url)
        {
            this.url = url;
        }
        #endregion

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="destpath">保存路径</param>
        /// <returns></returns>
        public void Save(string destpath)
        {
            Save(this.url, destpath, this.width, this.height);
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="destpath">保存路径</param>
        /// <returns></returns>
        public void Save(string url, string destpath)
        {
            Save(url, destpath, this.width, this.height);
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="destpath">保存路径</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高</param>
        /// <returns></returns>
        /// <remarks>width,height为0时获取完整大小</remarks>
        public void Save(string url, string destpath, int width, int height)
        {
            ImageFormat picType;

            //图片格式
            switch (Path.GetExtension(destpath).ToLower())
            {
                case ".gif":
                    picType = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
                case ".jpg":
                case ".jpeg":
                    picType = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case ".png":
                    picType = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case ".bmp":
                    picType = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                default:
                    picType = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
            }

            //抓取图片
            InitComobject(url);//初始化窗体
            int scrollWidth = this.browser.Document.Body.ScrollRectangle.Width;
            int scrollHeight = this.browser.Document.Body.ScrollRectangle.Height;
            this.browser.Width = scrollWidth;
            this.browser.Height = scrollHeight;
            if (width == 0) width = scrollWidth;
            if (height == 0) height = scrollHeight;

            //核心语句
            Snapshot snap = new Snapshot();
            using (Bitmap bitmap = snap.TakeSnapshot(this.browser.ActiveXInstance, new Rectangle(0, 0, width, height)))
            {
                bitmap.Save(destpath, picType);
            }

            browser.Dispose();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        protected void InitComobject(string url)
        {
            this.browser.ScriptErrorsSuppressed = false;
            this.browser.ScrollBarsEnabled = false;
            this.browser.Navigate(url);

            //因为没有窗体，所以必须如此
            while (this.browser.ReadyState != WebBrowserReadyState.Complete)
                System.Windows.Forms.Application.DoEvents();
            this.browser.Stop();
            if (this.browser.ActiveXInstance == null)
                throw new Exception("实例不能为空");
        }

    }
}