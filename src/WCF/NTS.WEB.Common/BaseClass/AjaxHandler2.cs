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
        //��ʹ�û���
        public static object CreateObjectNoCache(string AssemblyPath, string classNamespace)
        {
            try
            {
                object objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                return objType;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;// ��¼������־
                return null;
            }
        }
        /// <summary>
        /// ����IHttpHandler����
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
            try//version 3.0//֧�ַ���������(����get��post����)
            {
                //���÷���,ִ��Ajax���� 
                string method = string.Empty;
                string dll = string.Empty;
                if (Request["dll"] == "NTS.WEB.Common")
                {
                    method = Request["method"];
                    dll = Request["dll"];
                }else
                {
                    method = Strings.Decrypt(Request["method"], "njtsbecm");//namespace.class.method//����.����.������
                    dll = Strings.Decrypt(Request["DLL"], "njtsbecm");
                }
                 
                
                string ClassNamespace = method.Substring(0, method.LastIndexOf("."));// "</SPAN>< style="FONT-FAMILY: ����; COLOR: green; FONT-SIZE: 9pt" SPAN >?;< SPAN>
                //object objType = CreateObjectNoCache(Strings.Decrypt(Request["DLL"]), ClassNamespace);//����
                object objType = CreateObjectNoCache(dll, ClassNamespace);//����
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
                //���䷽������,������
                object[] obj = new object[length];// ����
                Type[] objTypes = new Type[length];//����

                for (int i = 0; i < length; i++)
                {
                    string ReqKey = likey[i].Substring(12);//Request.Form.AllKeys[i];  ȥ��������˵������[__DOTNET__]
                    Type tType = null;
                    if (ReqKey.IndexOf("[") != -1)
                    {
                        tType = System.Web.Compilation.BuildManager.GetType(ReqKey.Remove(ReqKey.LastIndexOf("[")), true, true);
                        // tType.MakeGenericType(typeof(T)); //ָ���������� 
                    }
                    else
                    {
                        tType = System.Web.Compilation.BuildManager.GetType(ReqKey, true, true);
                    }
                    objTypes[i] = tType;//���͸�ֵ
                    //�������ֵ����,����String
                    if (tType.Equals(typeof(string)) || tType.Equals(typeof(string[])) || (!tType.IsInterface && !tType.IsClass))
                    {
                        obj[i] = Convert.ChangeType(context.Server.UrlDecode(Request.Params[likey[i]]), tType);
                    }
                    else if (tType.IsClass || tType.IsGenericType)//�������,������json�ַ���ת���ɶ���
                    {
                        obj[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(context.Server.UrlDecode(Request.Params[likey[i]]), tType); //Serialize.Deserialize(Request.Form[i], tType);
                    }
                }
                //TODO
                //ִ�з���
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
                                throw new Exception("��û�е�½,��û��Ȩ��ִ�и÷���!503");
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
                //ִ�з���
                //object returnValue = methodInfo.Invoke(objType, obj);
                if (returnValue == null)
                    throw new Exception("��û��Ȩ��ִ�и÷�����\\n\\r��������Ϊ�գ�");


                #region ������ֵ
                if (returnValue.GetType().IsClass && !returnValue.GetType().Equals(typeof(string)))
                {
                    if (returnValue.GetType().Equals(typeof(XmlDocument)))
                    {
                        //�����XML�ĵ�,��д��xml�ĵ�
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
