using System.Configuration;
using System.Reflection;

namespace NTS.EMS.Config.ProductInteface
{
    public sealed class DataSwitchConfig
    {
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["ThisDataCoreName"];

        #region CreateObject

        //不使用缓存
        private static object CreateObject(string classNamespace)
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
        #endregion
        /* example create interface
        public static IBussinessLog CreateBussinessLog()
        {
            string classNamespace = AssemblyPath + ".LogAndExpiction";
            object objType = CreateObject(classNamespace);
            return (IBussinessLog)objType;
        }
        */

        #region MyRegion

        public static ISysLogObject CreateSysLog()
        {
            string classNamespace = AssemblyPath + ".SysLogObject";
            object objType = CreateObject(classNamespace);
            return (ISysLogObject)objType;
        }

        public static IQuotaObject CreateQuotaObject()
        {
            string classNamespace = AssemblyPath + ".QuotaObject";
            object objType = CreateObject(classNamespace);
            return (IQuotaObject)objType;
        }

        public static IBussinessLog CreateBussinessLog()
        {
            string classNamespace = AssemblyPath + ".LogAndExpiction";
            object objType = CreateObject(classNamespace);
            return (IBussinessLog)objType;
        }

        public static IRate CreateRateData()
        {
            string classNamespace = AssemblyPath + ".Rate";
            object objType = CreateObject(classNamespace);
            return (IRate)objType;
        }

        /// <summary>
        /// 用户模块
        /// </summary>
        /// <returns></returns>
        public static IUserObject CreateUserObject()
        {
            string classNamespace = AssemblyPath + ".UserObject";
            object objType = CreateObject(classNamespace);
            return (IUserObject)objType;
        }

        /// <summary>
        /// 用户组模块
        /// </summary>
        /// <returns></returns>
        public static IUserGroupObject CreateUserGroupObject()
        {
            string classNamespace = AssemblyPath + ".UserGroupObject";
            object objType = CreateObject(classNamespace);
            return (IUserGroupObject)objType;
        }


        public static IAlloction CreateAlloctionData()
        {
            string classNamespace = AssemblyPath + ".Alloction";
            object objType = CreateObject(classNamespace);
            return (IAlloction)objType;
        }

        /// <summary>
        /// 人工导入
        /// </summary>
        /// <returns></returns>
        public static IImport CreateImport()
        {
            string classNamespace = AssemblyPath + ".Import";
            object objType = CreateObject(classNamespace);
            return (IImport)objType;
        }

        /// <summary>
        /// 权限
        /// </summary>
        /// <returns></returns>
        public static IRightObject CreateRightObject()
        {
            string classNamespace = AssemblyPath + ".RightObject";
            object objType = CreateObject(classNamespace);
            return (IRightObject)objType;
        }

        /// <summary>
        /// 设备属性
        /// </summary>
        /// <returns></returns>
        public static IDevicePropObject CreateDevicePropObject()
        {
            string classNamespace = AssemblyPath + ".DevicePropObject";
            object objType = CreateObject(classNamespace);
            return (IDevicePropObject)objType;
        }


        public static IAlarmSetting CreateAlarmSetting()
        {
            string classNamespace = AssemblyPath + ".AlarmSetting";
            object objType = CreateObject(classNamespace);
            return (IAlarmSetting)objType;
        }

        public static IQuotaAlarmObject CreateQuotaAlarmObject()
        {
            string classNamespace = AssemblyPath + ".QuotaAlarmObject";
            object objType = CreateObject(classNamespace);
            return (IQuotaAlarmObject)objType;
        }
        #endregion

    }
}
