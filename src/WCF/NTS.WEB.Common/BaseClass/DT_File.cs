using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

using System.Web;
using System.IO;
using NTS.WEB.Common;


namespace NTS.WEB.Common
{
    public class DT_File
    {
        public static String FormatFileSize(Int64 fileSize)
        {
            if (fileSize < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} bytes", fileSize);
            }
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="f">文件上传控件</param>
        /// <param name="folderPath">文件保存的路劲</param>
        /// <param name="enableFileExtendName">支持的扩展名 PS：如果是空，则都允许</param>
        /// <returns></returns>
        public static string UpLoadFile(FileUpload f, string folderPath, List<string> enableFileExtendName)
        {
            string fileName = "";            
            if (enableFileExtendName == null
                || enableFileExtendName.Count == 0
                || enableFileExtendName.Contains(FileExtendName(f.FileName)))
            {
                try
                {
                    fileName = GetRandFileName(f.FileName);
                    f.SaveAs(folderPath + "\\" + fileName);
                }
                catch(Exception ex)
                {
                    //throw new UpLoadFileException("文件上传失败");               
                    Alerts.Alert("文件上传失败",null);
                }
            }
            else
            {
                //throw new UpLoadFileException();
                Alerts.Alert("文件上传失败", null);              
            }
            return fileName;
        }

        /// <summary>
        /// 得到一个随机的文件名
        /// </summary>
        /// <param name="fileName">原始文件名</param>
        /// <returns></returns>
        public static string GetRandFileName(string fileName)
        {
            Random ra = new Random(DateTime.Now.Second);
            DateTime d = DateTime.Now;
            string s = d.ToString("yyyy-MM-dd-hh-mm-ss-");
            s += ra.Next(1000000, 9999999).ToString();
            return s + FileExtendName(fileName);
        }

        /// <summary>
        /// 得到文件扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string FileExtendName(string fileName)
        {
            try
            {
                return fileName.Substring(fileName.LastIndexOf(".")).ToUpper();
            }
            catch
            {
                return "";
            }

        }

        #region 下载文件的操作
        /// <summary>
        /// 要下载的文件名
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static void DownLoadFile(string FileName)
        {
            try
            {
                //打开要下载的文件
                string FilePath = FileName;
                System.IO.FileStream r = new System.IO.FileStream(FilePath, System.IO.FileMode.Open);
                //设置基本信息
                System.Web.HttpContext.Current.Response.Buffer = false;
                //System.Web.UI.Page.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
                //HttpUtility.UrlEncode解决中文文件名乱码问题
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(System.IO.Path.GetFileName(FilePath)));
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", r.Length.ToString());
                while (true)
                {
                    //开辟缓冲区空间
                    byte[] buffer = new byte[1024];
                    //读取文件的数据
                    int leng = r.Read(buffer, 0, 1024);
                    if (leng == 0)//到文件尾，结束
                        break;
                    if (leng == 1024)//读出的文件数据长度等于缓冲区长度，直接将缓冲区数据写入
                        System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
                    else
                    {
                        //读出文件数据比缓冲区小，重新定义缓冲区大小，只用于读取文件的最后一个数据块
                        byte[] b = new byte[leng];
                        for (int i = 0; i < leng; i++)
                            b[i] = buffer[i];
                        System.Web.HttpContext.Current.Response.BinaryWrite(b);
                    }
                }
                r.Close();//关闭下载文件
                System.Web.HttpContext.Current.Response.End();//结束文件下载
                //return true;
            }
            catch (Exception e)
            {
                throw e;
                //return false;
            }
        }
        #endregion

        #region 原样复制文件
        public static void Copy(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            DirectoryInfo rootdir = new DirectoryInfo(source);

            //遍历文件   
            FileInfo[] fileinfo = rootdir.GetFiles();
            foreach (FileInfo file in fileinfo)
            {
                file.CopyTo(Path.Combine(destination, file.Name), true);
            }

            //递归   
            DirectoryInfo[] childdir = rootdir.GetDirectories();
            foreach (DirectoryInfo dir in childdir)
            {
                Copy(dir.FullName, Path.Combine(destination, dir.Name));
            }
        }
        #endregion

    }
}
