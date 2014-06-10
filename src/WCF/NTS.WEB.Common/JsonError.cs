using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Common
{
    public class JsonError
    {
        /// <summary>
        /// 客户端异常参数传递json错误格式
        /// </summary>
        /// <param name="requestValue"></param>
        /// <param name="errorStr"></param>
        /// <returns></returns>
        public static bool RequestError(string requestValue, out string errorStr)
        {
            errorStr = string.Empty;
            if (requestValue.Equals("Error"))
            {
                errorStr = "{\"success\":false,\"msg\":\"客户端传递的参数个数不正确。\"}";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 系统运行时的异常信息
        /// </summary>
        /// <param name="exceptionStr"></param>
        /// <returns></returns>
        public static string RunError(string exceptionStr)
        {
            string errorStr = "{\"ActionInfo\":[{ \"Success\": false,\"ExceptionMsg\": \"" + exceptionStr.FilterJosnChars() + "\"}]}";
                
            //string errorStr = "{\"Success\":false,\"Msg\":\"" + exceptionStr.FilterJosnChars() + "\"}";
            return errorStr;
        }

        /// <summary>
        /// 普通错误
        /// </summary>
        /// <returns></returns>
        public static string CommReportError()
        {
            string errorStr = "{\"success\":false,\"msg\":\"无法找到对应的报表对象\"}";
            return errorStr;
        }
    }
}
