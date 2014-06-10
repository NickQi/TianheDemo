using System;
using System.Collections.Generic;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Reflection;
using Newtonsoft.Json;
using System.Xml;
using System.Data;

namespace NTS.WEB.Common
{
    public class AjaxHandler2 : IHttpHandler, IRequiresSessionState
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
        //不使用缓存
        public static object CreateObjectNoCache(string AssemblyPath, string classNamespace)
        {
            try
            {
                object objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                return objType;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;// 记录错误日志
                return null;
            }
        }
        /// <summary>
        /// 处理IHttpHandler请求
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Session = context.Session;
            Response.CacheControl = "no-cache";
            //Request.ContentEncoding = Encoding.Default;
            //Response.ContentEncoding = Encoding.Default;
            //Request.ContentEncoding = Encoding.Default;
            Response.Clear();
            try//version 3.0//支持方法的重载(兼容get，post方法)
            {
                //运用反射,执行Ajax方法 
                string method = string.Empty;
                string dll = string.Empty;
                if (Request["dll"] == "NTS.WEB.Common")
                {
                    method = Request["method"];
                    dll = Request["dll"];
                }else
                {
                    method = Strings.Decrypt(Request["method"], "njtsbecm");//namespace.class.method//包名.类名.方法名
                    dll = Strings.Decrypt(Request["DLL"], "njtsbecm");
                }
                 
                
                string ClassNamespace = method.Substring(0, method.LastIndexOf("."));// "</SPAN>< style="FONT-FAMILY: 宋体; COLOR: green; FONT-SIZE: 9pt" SPAN >?;< SPAN>
                //object objType = CreateObjectNoCache(Strings.Decrypt(Request["DLL"]), ClassNamespace);//反射
                object objType = CreateObjectNoCache(dll, ClassNamespace);//反射
                Type ht = objType.GetType();
                List<string> likey = new List<string>();
                foreach (string item in Request.Params.AllKeys)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.StartsWith("[__DOTNET__]"))
                            likey.Add(item);
                    }
                }
                int length = likey.Count;//Request.Form.AllKeys.Length;
                //int length=Request
                //反射方法参数,和类型
                object[] obj = new object[length];// 数据
                Type[] objTypes = new Type[length];//类型

                for (int i = 0; i < length; i++)
                {
                    string ReqKey = likey[i].Substring(12);//Request.Form.AllKeys[i];  去掉参数的说明文字[__DOTNET__]
                    Type tType = null;
                    if (ReqKey.IndexOf("[") != -1)
                    {
                        tType = System.Web.Compilation.BuildManager.GetType(ReqKey.Remove(ReqKey.LastIndexOf("[")), true, true);
                        // tType.MakeGenericType(typeof(T)); //指定泛型类型 
                    }
                    else
                    {
                        tType = System.Web.Compilation.BuildManager.GetType(ReqKey, true, true);
                    }
                    objTypes[i] = tType;//类型赋值
                    //如果它是值类型,或者String
                    if (tType.Equals(typeof(string)) || tType.Equals(typeof(string[])) || (!tType.IsInterface && !tType.IsClass))
                    {
                        obj[i] = Convert.ChangeType(context.Server.UrlDecode(Request.Params[likey[i]]), tType);
                    }
                    else if (tType.IsClass || tType.IsGenericType)//如果是类,将它的json字符串转换成对象
                    {
                        obj[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(context.Server.UrlDecode(Request.Params[likey[i]]), tType); //Serialize.Deserialize(Request.Form[i], tType);
                    }
                }
                //TODO
                //执行方法
                MethodInfo methodInfo = ht.GetMethod(method.Substring(method.LastIndexOf(".") + 1), objTypes);
                object returnValue = null;
                foreach (object attribute in methodInfo.GetCustomAttributes(true))
                {
                    if (attribute is AjaxSessionMethod)
                    {
                        if (Session["ID"] == null)
                        {
                            if (new NTS_BECM.Common.BaseClass.Cookie().getCookie("islogin").Equals(""))
                            {
                                throw new Exception("你没有登陆,你没用权限执行该方法!503");
                            }
                            else
                            {
                                returnValue = methodInfo.Invoke(objType, obj);
                            }
                        }
                        else
                        {
                            returnValue = methodInfo.Invoke(objType, obj);
                        }
                        break;
                    }

                    else if (attribute is AjaxMethod)
                    {
                        returnValue = methodInfo.Invoke(objType, obj);
                        break;
                    }
                }
                //执行方法
                //object returnValue = methodInfo.Invoke(objType, obj);
                if (returnValue == null)
                    throw new Exception("你没用权限执行该方法，\\n\\r返回数据为空！");


                #region 处理返回值
                if (returnValue.GetType().IsClass && !returnValue.GetType().Equals(typeof(string)))
                {
                    if (returnValue.GetType().Equals(typeof(XmlDocument)))
                    {
                        //如果是XML文档,则写入xml文档
                        Response.ContentType = "text/xml";
                        Response.Write(((XmlDocument)returnValue).OuterXml);
                    }
                    else if (returnValue.GetType().IsClass && returnValue.GetType().Equals(typeof(DataTable)))
                    {
                        if (!string.IsNullOrEmpty(Request["callback"]) && Request["callback"].StartsWith("jsonp"))
                        {
                            Response.ContentType = "application/json";
                            Response.ContentEncoding = Encoding.Default;
                            Response.Write(Request["callback"] + "(" + Newtonsoft.Json.JsonConvert.SerializeObject((DataTable)returnValue) + ")");
                        }
                        else
                            //Response.Write(DotNet.DT_Web.Json.DT_Json.DataTableToJson("Rows", (DataTable)returnValue));
                            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject((DataTable)returnValue));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request["callback"]) && Request["callback"].StartsWith("jsonp"))
                        {
                            Response.ContentType = "application/json";
                            Response.ContentEncoding = Encoding.Default;
                            Response.Write(Request["callback"] + "(" + Newtonsoft.Json.JsonConvert.SerializeObject(returnValue) + ")");
                        }
                        else
                            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(returnValue));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request["callback"]) && Request["callback"].StartsWith("jsonp"))
                    {
                        Response.ContentType = "application/json";
                        Response.ContentEncoding = Encoding.Default;
                        Response.Write(Request["callback"] + "(\"" + HttpContext.Current.Server.HtmlEncode(returnValue.ToString()) + "\")");
                    }
                    else
                    {
                        Response.ContentType = "text/css";
                        Response.Write(returnValue.ToString());
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(Request["callback"]) && Request["callback"].StartsWith("jsonp"))
                {
                    if (e.InnerException != null)
                    {
                        Response.Write(Request["callback"] + "({Exception:\"" + e.InnerException.Message.Replace("\"", "\\\"") + "\"})");
                    }
                    else
                    {
                        Response.Write(Request["callback"] + "({Exception:\"" + e.Message.Replace("\"", "\\\"") + "\"})");
                    }

                }
                else
                {
                    if (e.InnerException != null)
                    {
                        Response.Write("{Exception:\"" + e.InnerException.Message.Replace("\"", "\\\"") + "\"}");
                    }
                    else
                    {
                        Response.Write("{Exception:\"" + e.Message.Replace("\"", "\\\"") + "\"}");
                    }
                }

            }
            Response.End();
        }
    }
    public class AjaxMethod : Attribute
    {
        public override string ToString()
        {
            return "AjaxMethod";
        }
    }
    public class AjaxSessionMethod : Attribute
    {
        public override string ToString()
        {
            return "AjaxSessionMethod";
        }
    }
}
