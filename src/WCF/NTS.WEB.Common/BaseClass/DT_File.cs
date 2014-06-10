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
        /// �ļ��ϴ�
        /// </summary>
        /// <param name="f">�ļ��ϴ��ؼ�</param>
        /// <param name="folderPath">�ļ������·��</param>
        /// <param name="enableFileExtendName">֧�ֵ���չ�� PS������ǿգ�������</param>
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
                    //throw new UpLoadFileException("�ļ��ϴ�ʧ��");               
                    Alerts.Alert("�ļ��ϴ�ʧ��",null);
                }
            }
            else
            {
                //throw new UpLoadFileException();
                Alerts.Alert("�ļ��ϴ�ʧ��", null);              
            }
            return fileName;
        }

        /// <summary>
        /// �õ�һ��������ļ���
        /// </summary>
        /// <param name="fileName">ԭʼ�ļ���</param>
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
        /// �õ��ļ���չ��
        /// </summary>
        /// <param name="fileName">�ļ���</param>
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

        #region �����ļ��Ĳ���
        /// <summary>
        /// Ҫ���ص��ļ���
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static void DownLoadFile(string FileName)
        {
            try
            {
                //��Ҫ���ص��ļ�
                string FilePath = FileName;
                System.IO.FileStream r = new System.IO.FileStream(FilePath, System.IO.FileMode.Open);
                //���û�����Ϣ
                System.Web.HttpContext.Current.Response.Buffer = false;
                //System.Web.UI.Page.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
                //HttpUtility.UrlEncode��������ļ�����������
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(System.IO.Path.GetFileName(FilePath)));
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", r.Length.ToString());
                while (true)
                {
                    //���ٻ������ռ�
                    byte[] buffer = new byte[1024];
                    //��ȡ�ļ�������
                    int leng = r.Read(buffer, 0, 1024);
                    if (leng == 0)//���ļ�β������
                        break;
                    if (leng == 1024)//�������ļ����ݳ��ȵ��ڻ��������ȣ�ֱ�ӽ�����������д��
                        System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
                    else
                    {
                        //�����ļ����ݱȻ�����С�����¶��建������С��ֻ���ڶ�ȡ�ļ������һ�����ݿ�
                        byte[] b = new byte[leng];
                        for (int i = 0; i < leng; i++)
                            b[i] = buffer[i];
                        System.Web.HttpContext.Current.Response.BinaryWrite(b);
                    }
                }
                r.Close();//�ر������ļ�
                System.Web.HttpContext.Current.Response.End();//�����ļ�����
                //return true;
            }
            catch (Exception e)
            {
                throw e;
                //return false;
            }
        }
        #endregion

        #region ԭ�������ļ�
        public static void Copy(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            DirectoryInfo rootdir = new DirectoryInfo(source);

            //�����ļ�   
            FileInfo[] fileinfo = rootdir.GetFiles();
            foreach (FileInfo file in fileinfo)
            {
                file.CopyTo(Path.Combine(destination, file.Name), true);
            }

            //�ݹ�   
            DirectoryInfo[] childdir = rootdir.GetDirectories();
            foreach (DirectoryInfo dir in childdir)
            {
                Copy(dir.FullName, Path.Combine(destination, dir.Name));
            }
        }
        #endregion

    }
}
