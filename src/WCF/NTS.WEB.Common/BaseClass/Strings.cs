using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Converters;
using System.Web.UI;
using System.Drawing;
using System.Collections;
using System.Web;


namespace NTS.WEB.Common
{
    public enum RequestDataType
    {
        DataString,
        DataInt,
        DataTime,
        DataDecimal
    }

    public class Strings
    {
        /// <summary>
        /// 获取时间类型
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int GetDateClass(DateTime date1, DateTime date2)
        {
            int timetype = 0;
            /* 关于周的特殊处理开始*/
            if (date1.Second == 59)
            {
                return 0;
            }
            /* 关于周的特殊处理结束*/
            if (date1.Date.Equals(date2.Date))
            {
                timetype = 1;
            }
            else
            {
                if (date1.Year == date2.Year && date1.Month == date2.Month)
                {
                    // 同月
                    timetype = 0;
                }
                else
                {
                    timetype = 2;
                }
            }


            return timetype; 
        }

        /// <summary>
        /// 获取对应的查询表
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetTName(int num)
        {
            switch (num)
            { 
                case 0:
                    return "o";
                case 1:
                    return "d";
                default:
                    return "m";
            }
        }
        public static string GetMixPwd(int num)//生成混合随机数
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// string 数组转化为decimal数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static decimal[]  ConvertDecimal(string [] arr)
        {
            decimal[] darr=new decimal[arr.Length];
            for(int i=0;i<darr.Length;i++)
            {
                try
                {
                    darr[i] = decimal.Parse(arr[i]);
                }
                catch (Exception e)
                {
                    darr[i] = 0;
                }
            }
            return darr;
        }

        /// <summary>
        /// 带单位量转换
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal[] ConvertUnitDecimal(string[] arr,decimal p)
        {
            decimal[] darr = new decimal[arr.Length];
            for (int i = 0; i < darr.Length; i++)
            {
                if (p > 0)
                {
                    darr[i] = decimal.Round(decimal.Parse(arr[i])/p, 2);
                }
                else
                {
                    darr[i] = 0.00M;
                }
            }
            return darr;
        }

        /// <summary>
        /// 获取部分datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TopNum"></param>
        /// <returns></returns>
        public static DataTable GetLimitTable(DataTable dt, int TopNum)
        {   
            if (dt.Rows.Count > 0)
            {
                DataTable new_dt = dt.Clone();
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i < TopNum)
                    {
                        new_dt.ImportRow(dr);
                    }
                    else break;
                    i++;
                }
                return new_dt;
            }
            return dt;
        }

        public static string getRequestString(string v, RequestDataType T)
        {
            string ErrorMsg = "Error";
            if (string.IsNullOrEmpty(v))
            {
                return ErrorMsg;
            }
            else
            {
                switch (T)
                {
                    case RequestDataType.DataString:
                        return GetSafeStr(FilterHTML(v));
                    case RequestDataType.DataInt:
                        int tempint;
                        return int.TryParse(v, out tempint) ? tempint.ToString() : ErrorMsg;
                    case RequestDataType.DataTime:
                        DateTime tempdate;
                        return DateTime.TryParse(v, out tempdate) ? tempdate.ToString() : ErrorMsg;
                    case RequestDataType.DataDecimal:
                        decimal tempdecimal;
                        return decimal.TryParse(v, out tempdecimal) ? tempdecimal.ToString() : ErrorMsg;
                    default:
                        return ErrorMsg;
                }
            }
        }

        /// <summary>
        /// 分页显示能耗排名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pages"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static DataTable GetPaddingLimitTable(DataTable dt,int pages, int pagesize)
        {
            int startnum = (pages - 1) * pagesize;

            if (dt.Rows.Count > 0)
            {
                DataTable new_dt = dt.Clone();
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int others = dt.Rows.Count - startnum;
                    others = others < pagesize ? others : pagesize;
                    if (i >=startnum && i < startnum+others)
                    {
                        new_dt.ImportRow(dr);
                    }
                    //else break;
                    i++;
                }
                return new_dt;
            }
            return dt;
        }


        public static DataTable GetDataTable(DataView obDataView)
        {
            if (null == obDataView)
            {
                throw new ArgumentNullException
                ("DataView", "Invalid DataView object specified");
            }

            DataTable obNewDt = obDataView.Table.Clone();
            int idx = 0;
            string[] strColNames = new string[obNewDt.Columns.Count];
            foreach (DataColumn col in obNewDt.Columns)
            {
                strColNames[idx++] = col.ColumnName;
            }

            IEnumerator viewEnumerator = obDataView.GetEnumerator();
            while (viewEnumerator.MoveNext())
            {
                DataRowView drv = (DataRowView)viewEnumerator.Current;
                DataRow dr = obNewDt.NewRow();
                try
                {
                    foreach (string strName in strColNames)
                    {
                        dr[strName] = drv[strName];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                obNewDt.Rows.Add(dr);
            }

            return obNewDt;
        }     
        /// <summary>
        /// 以汉字形式返回今天是星期几
        /// </summary>
        /// <returns></returns>
        public static string GetWeek()
        {
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
            string week = "";
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    week = "星期四";
                    break;
                case DayOfWeek.Friday:
                    week = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    week = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    week = "星期日";
                    break;
            }
            return week;
        }

        /// <summary>
        /// 增加空格的长度
        /// </summary>
        /// <param name="i">长度</param>
        /// <returns></returns>
        public static string SpaceLength(int i)
        {
            string space = "";
            for (int j = 0; j < i; j++)
            {
                space += "　";//注意这里的空白是智能abc输入法状态下的v11字符；   
            }
            return space + "└ ";
        }

        /// <summary>
        /// 得到嵌入到dll中文件,以流的形式返回
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stream GetAssemblyResStream(string path)
        {
            System.Reflection.Assembly dll = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream = dll.GetManifestResourceStream(path);
            StreamReader srdPreview = new StreamReader(stream);
            return stream;
        }
        /// <summary>
        /// 得到嵌入到dll中文件,以StreamReader的形式返回
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static StreamReader GetAssemblyResStreamReader(string path)
        {
            StreamReader srdPreview = new StreamReader(GetAssemblyResStream(path));
            return srdPreview;
        }
        /// <summary>
        /// 得到嵌入到dll中文件,以string的形式返回
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAssemblyResString(string path)
        {
            return GetAssemblyResStreamReader(path).ReadToEnd();
        }

        /// <summary>
        /// 时差,如几分钟前...
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNowMore(DateTime dt, string formate)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToString(formate);
            }
            else
                if (span.TotalDays > 30)
                {
                    return
                    "1个月前";
                }
                else
                    if (span.TotalDays > 14)
                    {
                        return
                        "2周前";
                    }
                    else
                        if (span.TotalDays > 7)
                        {
                            return
                            "1周前";
                        }
                        else
                            if (span.TotalDays > 1)
                            {
                                return
                                string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            }
                            else
                                if (span.TotalHours > 1)
                                {
                                    return
                                    string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                }
                                else
                                    if (span.TotalMinutes > 1)
                                    {
                                        return
                                        string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    }
                                    else
                                        if (span.TotalSeconds >= 1)
                                        {
                                            return
                                            string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        }
                                        else
                                        {
                                            return
                                            "1秒前";
                                        }
        }

        public static string DateStringFromNow(DateTime dt, string formate)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 2)
            {
                return dt.ToString(formate);
            }
            else
                if (span.TotalHours > 12 && span.TotalDays < 1)
                {
                    return
                    string.Format("今天{0}", dt.ToString("HH:mm:ss"));
                }
                else
                    if (span.TotalDays > 1)
                    {
                        return
                        string.Format("昨天{0}", dt.ToString("HH:mm:ss"));
                    }
                    else
                        if (span.TotalHours > 1)
                        {
                            return
                            string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                        }
                        else
                            if (span.TotalMinutes > 1)
                            {
                                return
                                string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                            }
                            else
                                if (span.TotalSeconds >= 1)
                                {
                                    return
                                    string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                }
                                else
                                {
                                    return
                                    "1秒前";
                                }
        }


        #region 获取安全的字符串
        /// <summary>
        /// 获取安全的字符串
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string GetSafeStr(string Str)
        {
            if (Str.Length > 0)
            {
                string SqlStr = "'|and|exec|insert|select|delete|update|count|*|chr|mid|master|truncate|char|declare";

                string[] anySqlStr = SqlStr.ToString().ToLower().Split('|');
                for (int i = 0; i < anySqlStr.Length; i++)
                {
                    Str = Str.Replace(anySqlStr[i], "");
                }
            }
            return Str;
        }
        #endregion

        #region 过滤HTML字符串
        /// <summary>
        /// 过滤String中的HTML标签
        /// </summary>
        /// <param name="strHtml">要过滤的html字符串</param>
        /// <returns></returns>
        public static string FilterHTML(string strHtml)
        {
            string[] aryReg ={
								 @"<script[^>]*?>.*?</script>",
								 @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
								 @"([\r\n])[\s]+",
								 @"&(quot|#34);",
								 @"&(amp|#38);",
								 @"&(lt|#60);",
								 @"&(gt|#62);",
								 @"&(nbsp|#160);",
								 @"&(iexcl|#161);",
								 @"&(cent|#162);",
								 @"&(pound|#163);",
								 @"&(copy|#169);",
								 @"&#(\d+);",
								 @"-->",
								 @"<!--.*\n"

							 };
            string[] aryRep = {
								  "",
								  "",
								  "",
								  "\"",
								  "&",
								  "<",
								  ">",
								  " ",
								  "\xa1",//chr(161),
								  "\xa2",//chr(162),
								  "\xa3",//chr(163),
								  "\xa9",//chr(169),
								  "",
								  "\r\n",
								  ""
							  };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");

            return strOutput;
        }
        public static string StripHtmlXmlTags(string content)
        {
            return Regex.Replace(content, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        public static string StripAllTags(string stringToStrip)
        {
            stringToStrip = Regex.Replace(stringToStrip, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            stringToStrip = Regex.Replace(stringToStrip, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            stringToStrip = StripHtmlXmlTags(stringToStrip);
            return stringToStrip;
        }
        #endregion

        /// <summary>
        /// 过滤不安全的字符串
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string WipeScript(string html)
        {
            Regex regex1 = new Regex(@"<script[\s\s]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\s]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" on[\s\s]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\s]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\s]+</frameset *>", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<a>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            return html;
        }

        private static Random rand = new Random();
        [NTS.WEB.Common.AjaxSessionMethod]
        public static string GetStrColor()
        {
            Color color = Color.FromArgb(rand.Next());
            string strColor = "#" + Convert.ToString(color.ToArgb(), 16).PadLeft(8, '0').Substring(2, 6);
            return strColor;
        }

        /// <summary>
        /// datatable导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string OutExcel(DataTable dt,string filefolerpath,string filename)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            // Encoding.GetEncoding("gb2312");
          

            DataGrid excel = new DataGrid();
            //excel.AutoGenerateColumns = true; 
            excel.DataSource = dt.DefaultView;  //绑定到DataGrid       
            excel.DataBind();
            excel.RenderControl(htmlWriter);  //这里指定文件的路径   

            filename= filename == "" ? System.DateTime.Now.ToString().Replace(":", "").Replace("-", "").Replace(" ", "") : filename.Replace(" ","");
            string filestr = "/"+filefolerpath+"/"+filename + ".xls";
            int pos = System.Web.HttpContext.Current.Server.MapPath(filestr).LastIndexOf("\\");
            string file = System.Web.HttpContext.Current.Server.MapPath(filestr).Substring(0, pos);
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(file);
            }
            System.IO.StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath(filestr));
            sw.Write(stringWriter.ToString(), System.Text.Encoding.GetEncoding("utf-8"));
            
            sw.Close();
            return filestr;
        }


        



        private static bool DownFile(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            try
            {
                Response.ContentType = "application/octet-stream";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" +
                HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=GB2312");
                System.IO.FileStream fs = System.IO.File.OpenRead(fullPath);
                long fLen = fs.Length;
                int size = 102400;//每100K同时下载数据
                byte[] readData = new byte[size];//指定缓冲区的大小
                if (size > fLen) size = Convert.ToInt32(fLen);
                long fPos = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    if ((fPos + size) > fLen)
                    {
                        size = Convert.ToInt32(fLen - fPos);
                        readData = new byte[size];
                        isEnd = true;
                    }
                    fs.Read(readData, 0, size);//读入一个压缩块
                    Response.BinaryWrite(readData);
                    fPos += size;
                }
                fs.Close();
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 截取字符串的长度(重载1次)
        /// <summary>
        /// 截取字符串的长度
        /// </summary>
        /// <param name="Str">要截取的字符串</param>
        /// <param name="StrMaxLength">要截取的字符串的长度</param>
        /// <returns></returns>
        public static string CutStr(string Str, int StrMaxLength)
        {
            return CutStr(Str, StrMaxLength, "");
        }

        /// <summary>
        /// 截取字符串的长度
        /// </summary>
        /// <param name="Str">要截取的字符串</param>
        /// <param name="StrMaxLength">要截取的字符串的长度</param>
        /// <param name="ReplaceStr">多余的长度要替换的字符</param>
        /// <returns></returns>
        public static string CutStr(string strInput, int StrMaxLength, string ReplaceStr)
        {
            strInput = strInput.Trim();
            byte[] myByte = System.Text.Encoding.Default.GetBytes(strInput);
            if (myByte.Length > StrMaxLength)
            {
                //截取操作
                string resultStr = "";
                for (int i = 0; i < strInput.Length; i++)
                {
                    byte[] tempByte = System.Text.Encoding.Default.GetBytes(resultStr);
                    if (tempByte.Length < StrMaxLength)
                    {
                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr + ReplaceStr;
            }
            else
            {
                return strInput;
            }


            //if (Str.Length > StrMaxLength)
            //{
            //    Str = Str.Substring(0, StrMaxLength) + ReplaceStr;
            //}
            //return Str;
        }
        #endregion

        #region 日期随机函数
        /**********************************
         * 函数名称:DateRndName
         * 功能说明:日期随机函数
         * 参    数:ra:随机数
         * 调用示例:
         *          Random ra = new Random();
         *          string s = DT_String.RandDate(ra);
         * ********************************/
        /// <summary>
        /// 日期随机函数
        /// </summary>
        /// <param name="ra">随机数</param>
        /// <returns></returns>
        public static string RandDate()
        {
            Random ra = new Random(DateTime.Now.Millisecond);
            DateTime d = DateTime.Now;
            string s = d.ToString("yyyy-MM-dd-HH-mm-ss-");
            s += ra.Next(1000000, 9999999).ToString();
            return s;
        }
        #endregion

        #region 拆分字符串
        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="splitWith">以什么拆分</param>
        /// <returns></returns>
        public static string[] Split(string source, string splitWith)
        {
            return Regex.Split(source, splitWith);
        }
        #endregion

        #region 返回文件的扩展名
        /// <summary>
        /// 返回文件的扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>返回.xxx</returns>
        public static string Extend(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf("."));
        }
        #endregion

        #region 除去最后一个逗号
        /// <summary>
        /// 除去最后一个逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLastComma(string str)
        {
            if (str.LastIndexOf(',') == 1)
                return str.Remove(str.Length - 1);
            return str;
        }
        #endregion

        #region 查看以某某分割的字符串是否包含某个字符串
        /// <summary>
        /// 查看以某某分割的字符串是否包含某个字符串<br/>
        /// 如:a,b,c,d,   是否包含  a  调用HasContains("a,b,c,d, ","a",',')
        /// </summary>
        /// <param name="srcStr">原始字符串</param>
        /// <param name="contain">被包含的字符串</param>
        /// <param name="split">以什么分割</param>
        /// <returns></returns>
        public static bool HasContains(string srcStr, string contain, char split)
        {
            List<string> lstr = new List<string>();
            foreach (string str in srcStr.Split(split))
            {
                lstr.Add(str);
            }
            return lstr.Contains(contain);
        }
        #endregion

        //===============================================================================

        public static string GetContentTypeByExtName(string extName)
        {
            switch (extName.ToLower())
            {
                case ".001":
                    return "application/x-001";

                case ".301":
                    return "application/x-301";

                case ".323":
                    return "text/h323";

                case ".906":
                    return "application/x-906";

                case ".907":
                    return "drawing/907";

                case ".a11":
                    return "application/x-a11";

                case ".acp":
                    return "audio/x-mei-aac";

                case ".ai":
                    return "application/postscript";

                case ".aif":
                    return "audio/aiff";

                case ".aifc":
                    return "audio/aiff";

                case ".aiff":
                    return "audio/aiff";

                case ".anv":
                    return "application/x-anv";

                case ".asa":
                    return "text/asa";

                case ".asf":
                    return "video/x-ms-asf";

                case ".asp":
                    return "text/asp";

                case ".asx":
                    return "video/x-ms-asf";

                case ".au":
                    return "audio/basic";

                case ".avi":
                    return "video/avi";

                case ".awf":
                    return "application/vnd.adobe.workflow";

                case ".biz":
                    return "text/xml";

                case ".bmp":
                    return "image/bmp";

                case ".bot":
                    return "application/x-bot";

                case ".c4t":
                    return "application/x-c4t";

                case ".c90":
                    return "application/x-c90";

                case ".cal":
                    return "application/x-cals";

                case ".cat":
                    return "application/vnd.ms-pki.seccat";

                case ".cdf":
                    return "application/x-netcdf";

                case ".cdr":
                    return "application/x-cdr";

                case ".cel":
                    return "application/x-cel";

                case ".cer":
                    return "application/x-x509-ca-cert";

                case ".cg4":
                    return "application/x-g4";

                case ".cgm":
                    return "application/x-cgm";

                case ".cit":
                    return "application/x-cit";

                case ".class":
                    return "java/*";

                case ".cml":
                    return "text/xml";

                case ".cmp":
                    return "application/x-cmp";

                case ".cmx":
                    return "application/x-cmx";

                case ".cot":
                    return "application/x-cot";

                case ".crl":
                    return "application/pkix-crl";

                case ".crt":
                    return "application/x-x509-ca-cert";

                case ".csi":
                    return "application/x-csi";

                case ".css":
                    return "text/css";

                case ".cut":
                    return "application/x-cut";

                case ".dbf":
                    return "application/x-dbf";

                case ".dbm":
                    return "application/x-dbm";

                case ".dbx":
                    return "application/x-dbx";

                case ".dcd":
                    return "text/xml";

                case ".dcx":
                    return "application/x-dcx";

                case ".der":
                    return "application/x-x509-ca-cert";

                case ".dgn":
                    return "application/x-dgn";

                case ".dib":
                    return "application/x-dib";

                case ".dll":
                    return "application/x-msdownload";

                case ".doc":
                    return "application/msword";

                case ".dot":
                    return "application/msword";

                case ".drw":
                    return "application/x-drw";

                case ".dtd":
                    return "text/xml";

                case ".dwf":
                    return "Model/vnd.dwf";

                case ".dwg":
                    return "application/x-dwg";

                case ".dxb":
                    return "application/x-dxb";

                case ".dxf":
                    return "application/x-dxf";

                case ".edn":
                    return "application/vnd.adobe.edn";

                case ".emf":
                    return "application/x-emf";

                case ".eml":
                    return "message/rfc822";

                case ".ent":
                    return "text/xml";

                case ".epi":
                    return "application/x-epi";

                case ".eps":
                    return "application/postscript";

                case ".etd":
                    return "application/x-ebx";

                case ".exe":
                    return "application/x-msdownload";

                case ".fax":
                    return "image/fax";

                case ".fdf":
                    return "application/vnd.fdf";

                case ".fif":
                    return "application/fractals";

                case ".fo":
                    return "text/xml";

                case ".frm":
                    return "application/x-frm";

                case ".g4":
                    return "application/x-g4";

                case ".gbr":
                    return "application/x-gbr";

                case ".gcd":
                    return "application/x-gcd";

                case ".gif":
                    return "image/gif";

                case ".gl2":
                    return "application/x-gl2";

                case ".gp4":
                    return "application/x-gp4";

                case ".hgl":
                    return "application/x-hgl";

                case ".hmr":
                    return "application/x-hmr";

                case ".hpg":
                    return "application/x-hpgl";

                case ".hpl":
                    return "application/x-hpl";

                case ".hqx":
                    return "application/mac-binhex40";

                case ".hrf":
                    return "application/x-hrf";

                case ".hta":
                    return "application/hta";

                case ".htc":
                    return "text/x-component";

                case ".htm":
                    return "text/html";

                case ".html":
                    return "text/html";

                case ".htt":
                    return "text/webviewhtml";

                case ".htx":
                    return "text/html";

                case ".icb":
                    return "application/x-icb";

                case ".ico":
                    return "image/x-icon";

                case ".iff":
                    return "application/x-iff";

                case ".ig4":
                    return "application/x-g4";

                case ".igs":
                    return "application/x-igs";

                case ".iii":
                    return "application/x-iphone";

                case ".img":
                    return "application/x-img";

                case ".ins":
                    return "application/x-internet-signup";

                case ".isp":
                    return "application/x-internet-signup";

                case ".IVF":
                    return "video/x-ivf";

                case ".java":
                    return "java/*";

                case ".jfif":
                    return "image/jpeg";

                case ".jpe":
                    return "image/jpeg";

                case ".jpeg":
                    return "image/jpeg";

                case ".jpg":
                    return "image/jpeg";

                case ".jsp":
                    return "text/html";

                case ".la1":
                    return "audio/x-liquid-file";

                case ".lar":
                    return "application/x-laplayer-reg";

                case ".latex":
                    return "application/x-latex";

                case ".lavs":
                    return "audio/x-liquid-secure";

                case ".lbm":
                    return "application/x-lbm";

                case ".lmsff":
                    return "audio/x-la-lms";

                case ".ls":
                    return "application/x-javascript";

                case ".ltr":
                    return "application/x-ltr";

                case ".m1v":
                    return "video/x-mpeg";

                case ".m2v":
                    return "video/x-mpeg";

                case ".m3u":
                    return "audio/mpegurl";

                case ".m4e":
                    return "video/mpeg4";

                case ".mac":
                    return "application/x-mac";

                case ".man":
                    return "application/x-troff-man";

                case ".math":
                    return "text/xml";

                case ".mdb":
                    return "application/msaccess";

                case ".mfp":
                    return "application/x-shockwave-flash";

                case ".mht":
                    return "message/rfc822";

                case ".mhtml":
                    return "message/rfc822";

                case ".mi":
                    return "application/x-mi";

                case ".mid":
                    return "audio/mid";

                case ".midi":
                    return "audio/mid";

                case ".mil":
                    return "application/x-mil";

                case ".mml":
                    return "text/xml";

                case ".mnd":
                    return "audio/x-musicnet-download";

                case ".mns":
                    return "audio/x-musicnet-stream";

                case ".mocha":
                    return "application/x-javascript";

                case ".movie":
                    return "video/x-sgi-movie";

                case ".mp1":
                    return "audio/mp1";

                case ".mp2":
                    return "audio/mp2";

                case ".mp2v":
                    return "video/mpeg";

                case ".mp3":
                    return "audio/mp3";

                case ".mp4":
                    return "video/mpeg4";

                case ".mpa":
                    return "video/x-mpg";

                case ".mpd":
                    return "application/vnd.ms-project";

                case ".mpe":
                    return "video/x-mpeg";

                case ".mpeg":
                    return "video/mpg";

                case ".mpg":
                    return "video/mpg";

                case ".mpga":
                    return "audio/rn-mpeg";

                case ".mpp":
                    return "application/vnd.ms-project";

                case ".mps":
                    return "video/x-mpeg";

                case ".mpt":
                    return "application/vnd.ms-project";

                case ".mpv":
                    return "video/mpg";

                case ".mpv2":
                    return "video/mpeg";

                case ".mpw":
                    return "application/vnd.ms-project";

                case ".mpx":
                    return "application/vnd.ms-project";

                case ".mtx":
                    return "text/xml";

                case ".mxp":
                    return "application/x-mmxp";

                case ".net":
                    return "image/pnetvue";

                case ".nrf":
                    return "application/x-nrf";

                case ".nws":
                    return "message/rfc822";

                case ".odc":
                    return "text/x-ms-odc";

                case ".out":
                    return "application/x-out";

                case ".p10":
                    return "application/pkcs10";

                case ".p12":
                    return "application/x-pkcs12";

                case ".p7b":
                    return "application/x-pkcs7-certificates";

                case ".p7c":
                    return "application/pkcs7-mime";

                case ".p7m":
                    return "application/pkcs7-mime";

                case ".p7r":
                    return "application/x-pkcs7-certreqresp";

                case ".p7s":
                    return "application/pkcs7-signature";

                case ".pc5":
                    return "application/x-pc5";

                case ".pci":
                    return "application/x-pci";

                case ".pcl":
                    return "application/x-pcl";

                case ".pcx":
                    return "application/x-pcx";

                case ".pdf":
                    return "application/pdf";

                case ".pdx":
                    return "application/vnd.adobe.pdx";

                case ".pfx":
                    return "application/x-pkcs12";

                case ".pgl":
                    return "application/x-pgl";

                case ".pic":
                    return "application/x-pic";

                case ".pko":
                    return "application/vnd.ms-pki.pko";

                case ".pl":
                    return "application/x-perl";

                case ".plg":
                    return "text/html";

                case ".pls":
                    return "audio/scpls";

                case ".plt":
                    return "application/x-plt";

                case ".png":
                    return "image/x-png";

                case ".pot":
                    return "application/vnd.ms-powerpoint";

                case ".ppa":
                    return "application/vnd.ms-powerpoint";

                case ".ppm":
                    return "application/x-ppm";

                case ".pps":
                    return "application/vnd.ms-powerpoint";

                case ".ppt":
                    return "application/vnd.ms-powerpoint";

                case ".pr":
                    return "application/x-pr";

                case ".prf":
                    return "application/pics-rules";

                case ".prn":
                    return "application/x-prn";

                case ".prt":
                    return "application/x-prt";

                case ".ps":
                    return "application/x-ps";

                case ".ptn":
                    return "application/x-ptn";

                case ".pwz":
                    return "application/vnd.ms-powerpoint";

                case ".r3t":
                    return "text/vnd.rn-realtext3d";

                case ".ra":
                    return "audio/vnd.rn-realaudio";

                case ".ram":
                    return "audio/x-pn-realaudio";

                case ".ras":
                    return "application/x-ras";

                case ".rat":
                    return "application/rat-file";

                case ".rdf":
                    return "text/xml";

                case ".rec":
                    return "application/vnd.rn-recording";

                case ".red":
                    return "application/x-red";

                case ".rgb":
                    return "application/x-rgb";

                case ".rjs":
                    return "application/vnd.rn-realsystem-rjs";

                case ".rjt":
                    return "application/vnd.rn-realsystem-rjt";

                case ".rlc":
                    return "application/x-rlc";

                case ".rle":
                    return "application/x-rle";

                case ".rm":
                    return "application/vnd.rn-realmedia";

                case ".rmf":
                    return "application/vnd.adobe.rmf";

                case ".rmi":
                    return "audio/mid";

                case ".rmj":
                    return "application/vnd.rn-realsystem-rmj";

                case ".rmm":
                    return "audio/x-pn-realaudio";

                case ".rmp":
                    return "application/vnd.rn-rn_music_package";

                case ".rms":
                    return "application/vnd.rn-realmedia-secure";

                case ".rmvb":
                    return "application/vnd.rn-realmedia-vbr";

                case ".rmx":
                    return "application/vnd.rn-realsystem-rmx";

                case ".rnx":
                    return "application/vnd.rn-realplayer";

                case ".rp":
                    return "image/vnd.rn-realpix";

                case ".rpm":
                    return "audio/x-pn-realaudio-plugin";

                case ".rsml":
                    return "application/vnd.rn-rsml";

                case ".rt":
                    return "text/vnd.rn-realtext";

                case ".rtf":
                    return "application/x-rtf";

                case ".rv":
                    return "video/vnd.rn-realvideo";

                case ".sam":
                    return "application/x-sam";

                case ".sat":
                    return "application/x-sat";

                case ".sdp":
                    return "application/sdp";

                case ".sdw":
                    return "application/x-sdw";

                case ".sit":
                    return "application/x-stuffit";

                case ".slb":
                    return "application/x-slb";

                case ".sld":
                    return "application/x-sld";

                case ".slk":
                    return "drawing/x-slk";

                case ".smi":
                    return "application/smil";

                case ".smil":
                    return "application/smil";

                case ".smk":
                    return "application/x-smk";

                case ".snd":
                    return "audio/basic";

                case ".sol":
                    return "text/plain";

                case ".sor":
                    return "text/plain";

                case ".spc":
                    return "application/x-pkcs7-certificates";

                case ".spl":
                    return "application/futuresplash";

                case ".spp":
                    return "text/xml";

                case ".ssm":
                    return "application/streamingmedia";

                case ".sst":
                    return "application/vnd.ms-pki.certstore";

                case ".stl":
                    return "application/vnd.ms-pki.stl";

                case ".stm":
                    return "text/html";

                case ".sty":
                    return "application/x-sty";

                case ".svg":
                    return "text/xml";

                case ".swf":
                    return "application/x-shockwave-flash";

                case ".tdf":
                    return "application/x-tdf";

                case ".tg4":
                    return "application/x-tg4";

                case ".tga":
                    return "application/x-tga";

                case ".tif":
                    return "image/tiff";

                case ".tiff":
                    return "image/tiff";

                case ".tld":
                    return "text/xml";

                case ".top":
                    return "drawing/x-top";

                case ".torrent":
                    return "application/x-bittorrent";

                case ".tsd":
                    return "text/xml";

                case ".txt":
                    return "text/plain";

                case ".uin":
                    return "application/x-icq";

                case ".uls":
                    return "text/iuls";

                case ".vcf":
                    return "text/x-vcard";

                case ".vda":
                    return "application/x-vda";

                case ".vdx":
                    return "application/vnd.visio";

                case ".vml":
                    return "text/xml";

                case ".vpg":
                    return "application/x-vpeg005";

                case ".vsd":
                    return "application/x-vsd";

                case ".vss":
                    return "application/vnd.visio";

                case ".vst":
                    return "application/x-vst";

                case ".vsw":
                    return "application/vnd.visio";

                case ".vsx":
                    return "application/vnd.visio";

                case ".vtx":
                    return "application/vnd.visio";

                case ".vxml":
                    return "text/xml";

                case ".wav":
                    return "audio/wav";

                case ".wax":
                    return "audio/x-ms-wax";

                case ".wb1":
                    return "application/x-wb1";

                case ".wb2":
                    return "application/x-wb2";

                case ".wb3":
                    return "application/x-wb3";

                case ".wbmp":
                    return "image/vnd.wap.wbmp";

                case ".wiz":
                    return "application/msword";

                case ".wk3":
                    return "application/x-wk3";

                case ".wk4":
                    return "application/x-wk4";

                case ".wkq":
                    return "application/x-wkq";

                case ".wks":
                    return "application/x-wks";

                case ".wm":
                    return "video/x-ms-wm";

                case ".wma":
                    return "audio/x-ms-wma";

                case ".wmd":
                    return "application/x-ms-wmd";

                case ".wmf":
                    return "application/x-wmf";

                case ".wml":
                    return "text/vnd.wap.wml";

                case ".wmv":
                    return "video/x-ms-wmv";

                case ".wmx":
                    return "video/x-ms-wmx";

                case ".wmz":
                    return "application/x-ms-wmz";

                case ".wp6":
                    return "application/x-wp6";

                case ".wpd":
                    return "application/x-wpd";

                case ".wpg":
                    return "application/x-wpg";

                case ".wpl":
                    return "application/vnd.ms-wpl";

                case ".wq1":
                    return "application/x-wq1";

                case ".wr1":
                    return "application/x-wr1";

                case ".wri":
                    return "application/x-wri";

                case ".wrk":
                    return "application/x-wrk";

                case ".ws":
                    return "application/x-ws";

                case ".ws2":
                    return "application/x-ws";

                case ".wsc":
                    return "text/scriptlet";

                case ".wsdl":
                    return "text/xml";

                case ".wvx":
                    return "video/x-ms-wvx";

                case ".xdp":
                    return "application/vnd.adobe.xdp";

                case ".xdr":
                    return "text/xml";

                case ".xfd":
                    return "application/vnd.adobe.xfd";

                case ".xfdf":
                    return "application/vnd.adobe.xfdf";

                case ".xhtml":
                    return "text/html";

                case ".xls":
                    return "application/x-xls";

                case ".xlw":
                    return "application/x-xlw";

                case ".xml":
                    return "text/xml";

                case ".xpl":
                    return "audio/scpls";

                case ".xq":
                    return "text/xml";

                case ".xql":
                    return "text/xml";

                case ".xquery":
                    return "text/xml";

                case ".xsd":
                    return "text/xml";

                case ".xsl":
                    return "text/xml";

                case ".xslt":
                    return "text/xml";

                case ".xwd":
                    return "application/x-xwd";

                case ".x_b":
                    return "application/x-x_b";

                case ".x_t":
                    return "application/x-x_t";
            }
            return "application/octet-stream";
        }

        #region base64 解码
        /// <summary>
        /// base64 解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code)
        {
            string decode = "";
            string st = code + "000";//
            string strcode = st.Substring(0, (st.Length / 4) * 4);
            byte[] bytes = Convert.FromBase64String(strcode);
            try
            {
                decode = System.Text.Encoding.GetEncoding("GB2312").GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region md5 加密
        public static string EncryptMD5(string cleanString)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            return BitConverter.ToString(hashedBytes);
        }
        #endregion

        #region DEC 加密过程
        /// <summary>
        /// DEC 加密过程
        /// </summary>
        /// <param name="pToEncrypt">被加密的字符串</param>
        /// <param name="sKey">密钥（只支持8个字节的密钥）</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);　//建立加密对象的密钥和偏移量
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);//把字符串放到byte数组中

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　
            //定义将数据流链接到加密转换的流
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        public static string Encrypt(string pToEncrypt)
        {
            return Encrypt(pToEncrypt, ConfigurationManager.AppSettings["deckey"]);
        }
        #endregion

        #region DEC 解密过程
        ///<summary>
        /// DEC 解密过程
        /// </summary>
        /// <param name="pToDecrypt">被解密的字符串</param>
        /// <param name="sKey">密钥（只支持8个字节的密钥，同前面的加密密钥相同）</param>
        /// <returns>返回被解密的字符串</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            if (!string.IsNullOrEmpty(pToDecrypt))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);　//建立加密对象的密钥和偏移量，此值重要，不能修改
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            return string.Empty;
        }
        /// <summary>      
        /// dataTable转换成Json格式      
        /// </summary>      
        /// <param name="dt"></param>      
        /// <returns></returns>      
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            // jsonBuilder.Append("{\"");
            // jsonBuilder.Append(dt.TableName.ToString());
            //jsonBuilder.Append("\":[");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().FilterJosnChars());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            //   jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string ToJsonWithOrder(int startOrder,DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
 
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");

                jsonBuilder.Append("\"");
                jsonBuilder.Append("Order");
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append((startOrder+i + 1).ToString().FilterJosnChars());
                jsonBuilder.Append("\",");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().FilterJosnChars());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            //   jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }


        public static string Decrypt(string pToDecrypt)
        {
            return Decrypt(pToDecrypt, ConfigurationManager.AppSettings["deckey"]);
        }
        #endregion


    }

    /// <summary>
    /// 加密辅助类
    /// </summary>
    public static class PassWordEncrypt
    {
        public static string EncryptString(this string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
        }
        private static string str;
        public static string PassStr
        {
            get
            {
                return str.EncryptString();
            }
            set
            {
                str = value;
            }

        }
    }

    /// <summary>
    /// 获取系统定义的基本信息
    /// </summary>
    public static class GetLabManagerConfig
    {
        public static string ConfigEnum(this string configinfo)
        {
            switch (int.Parse(configinfo))
            {
                case 0:
                    return "对不起，你还未被赋予系统的角色。";
                case 1:
                    return "对不起，您登录的用户名或密码错误。";
                case 2:
                    return "对不起，用户名不存在。";
                default:
                    return "";
            }
        }
        private static string configinfo;
        public static string _configinfo
        {
            get
            {

                return configinfo.ConfigEnum();

            }
            set { configinfo = value; }

        }
    }

    
    /// <summary>
    /// 将对象转换为Json对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class JsonConvert<T>
    {
        public StringBuilder ToDataGird(List<T> list, int totalRows)
        {
            StringBuilder JsonObjce = new StringBuilder();

            JsonObjce.Append("{");
            JsonObjce.Append("\"total\":");
            JsonObjce.Append(totalRows);
            JsonObjce.Append(",");
            JsonObjce.Append("\"rows\":");
            JsonObjce.Append("[");

            foreach (T item in list)
            {
                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();        
                //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式          
                timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd";      
                //jsonObject是准备转换的对象     
                string jsonGroup = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented, timeConverter);
               // string jsonGroup = JavaScriptConvert.SerializeObject(item);
                JsonObjce.Append(jsonGroup);
                JsonObjce.Append(",");
            }

            if (list.Count > 0)
                JsonObjce = JsonObjce.Remove(JsonObjce.Length - 1, 1);

            JsonObjce.Append("]}");

            return JsonObjce;
        }

        /// <summary>
        /// 返回josn Jqgrid的格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string JsonForJqgrid(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"total\":" + dt.Rows.Count + ",\"rows");
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
    }

    public static class JsonChars
    {
        #region  过滤干扰json的字符
        public static string FilterJosnChars(this string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    return str.Replace("\'", "").Replace("\"", "").Replace("\n", "").Replace("\r", "");
                }
                return str;
            }
            catch {
                return "";
            }
        }
        #endregion
    }
}
