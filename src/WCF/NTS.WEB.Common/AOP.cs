using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using PostSharp.Laos;
using DBUtility;
namespace NTS.WEB.Common
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class LogAttribute : OnMethodBoundaryAspect
    {
        public string ModelName { get; set; }
        public string LogContent { get; set; }
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {


            string SQL = string.Format(@"Insert into  SysLog
                                        (ModelName,LogContent,LogTime)
                                        values
                                        (@ModelName,@LogContent,@LogTime)");
            SqlParameter[] parms = { 
                                      new SqlParameter("@ModelName", ModelName),
                                      new SqlParameter("@LogContent", string.IsNullOrEmpty(LogContent) ? ModelName + "执行了操作。" : LogContent),
                                      new SqlParameter("@LogTime", DateTime.Now)};

            SqlHelper.ExecuteSql(SQL, parms);
            base.OnEntry(eventArgs);
            // eventArgs.FlowBehavior = FlowBehavior.Return;
            // eventArgs.ReturnValue = false;
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            //Console.Write("end....");
            base.OnExit(eventArgs);

        }
    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomExceptionAttribute : PostSharp.Aspects.OnExceptionAspect
    {
        public override void OnException(PostSharp.Aspects.MethodExecutionArgs args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("*********************************************************************************************\r\n");
            sb.Append("ErrorTime:" + System.DateTime.Now + "\r\n");
            sb.Append("ErrorFun:" + args.Exception.Source + "\r\n");
            sb.Append("ErrorDetail:" + args.Exception.StackTrace.ToString() + "," + args.Exception.Message + "\r\n");
            sb.Append("*********************************************************************************************\r\n\r\n\r\n");
            WriteFile(AppDomain.CurrentDomain.BaseDirectory + "\\SystemError.txt", sb.ToString());
            base.OnException(args);
            //args.FlowBehavior = PostSharp.Aspects.FlowBehavior.Return;
        }

        private static void WriteFile(string path, string contentStr)
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            //获得字节数组
            byte[] data = new UTF8Encoding().GetBytes(contentStr);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
    }
}
