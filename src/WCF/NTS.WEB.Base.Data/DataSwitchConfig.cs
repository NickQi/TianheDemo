using System.Configuration;
using System.Reflection;

namespace NTS.WEB.ProductInteface
{
    public sealed class DataSwitchConfig
    {

        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["DataCoreName"];
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
        ////使用缓存
        //private static object CreateObject(string classNamespace)
        //{			
        //    object objType = DataCache.GetCache(classNamespace);
        //    if (objType == null)
        //    {
        //        try
        //        {
        //            objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);					
        //            DataCache.SetCache(classNamespace, objType);// 写入缓存
        //        }
        //        catch//(System.Exception ex)
        //        {
        //            //string str=ex.Message;// 记录错误日志
        //        }
        //    }
        //    return objType;
        //}
        #endregion

        /// <summary>
        /// 反射生成层级对象
        /// </summary>
        /// <returns></returns>
        public static IBaseLayerObject CreateLayer()
        {
            string classNamespace = AssemblyPath + ".BaseLayerObject";
            object objType = CreateObject(classNamespace);
            return (IBaseLayerObject)objType;
            
        }

        /// <summary>
        /// 反射生成用户信息模块
        /// </summary>
        /// <returns></returns>
        public static IAccount CreateAccount()
        {
            string classNamespace = AssemblyPath + ".Account";
            object objType = CreateObject(classNamespace);
            return (IAccount)objType;
            
        }

    
        public static IReportBase CreateReportBase()
        {
            string classNamespace = AssemblyPath + ".ReportBase";
            object objType = CreateObject(classNamespace);
            return (IReportBase)objType;
        }

        public static IItemcode CreateItemcode()
        {
            string classNamespace = AssemblyPath + ".Itemcode";
            object objType = CreateObject(classNamespace);
            return (IItemcode)objType;
        }

        public static  IDevice CreateDevice()
        {
            string classNamespace = AssemblyPath + ".Device";
            object objType = CreateObject(classNamespace);
            return (IDevice)objType;
        }

        public static  IRealTimeData CreateRealTimeData()
        {
            string classNamespace = AssemblyPath + ".RealTimeData";
            object objType = CreateObject(classNamespace);
            return (IRealTimeData)objType;
        }


        public static IMonthDataObject CreateMonthData()
        {
            string classNamespace = AssemblyPath + ".MonthDataObject";
            object objType = CreateObject(classNamespace);
            return (IMonthDataObject)objType;

        }

        public static IBalanceAnalysis CreateBalanceAnalysis()
        {
            string classNamespace = AssemblyPath + ".BalanceAnalysis";
            object objType = CreateObject(classNamespace);
            return (IBalanceAnalysis)objType;
        }
        public static IWarningAnalysis CreateWarningAnalysis()
        {
            string classNamespace = AssemblyPath + ".WarningAnalysis";
            object objType = CreateObject(classNamespace);
            return (IWarningAnalysis)objType;
        }

        public static IComplexReport CreateComplexReport()
        {
            string classNamespace = AssemblyPath + ".ComplexReport";
            object objType = CreateObject(classNamespace);
            return (IComplexReport)objType;
        }

        public static IEnergyContrast CreateEnergyContrast()
        {
            string classNamespace = AssemblyPath + ".EnergyContrast";
            object objType = CreateObject(classNamespace);
            return (IEnergyContrast)objType;
        }

        public static IAreaTree CreateAreaTree()
        {
            string classNamespace = AssemblyPath + ".AreaTree";
            object objType = CreateObject(classNamespace);
            return (IAreaTree)objType;
        }
        public static IMenu CreateMenuTree()
        {
            string classNamespace = AssemblyPath + ".Menu";
            object objType = CreateObject(classNamespace);
            return (IMenu)objType;
        }
        public static IUser CreateUser()
        {
            string classNamespace = AssemblyPath + ".User";
            object objType = CreateObject(classNamespace);
            return (IUser)objType;
        }
        public static IUserGroup CreateUserGroup()
        {
            string classNamespace = AssemblyPath + ".UserGroup";
            object objType = CreateObject(classNamespace);
            return (IUserGroup)objType;
        }

        public static IQuotaAnalyse CreateQuotaAnalyse()
        {
            string classNamespace = AssemblyPath + ".QuotaAnalyse";
            object objType = CreateObject(classNamespace);
            return (IQuotaAnalyse)objType;
        }
        /*onlyfor9000 begin*/

        public static ILoadForecast CreateLoadForecast()
        {
            string classNamespace = AssemblyPath + ".LoadForecast";
            object objType = CreateObject(classNamespace);
            return (ILoadForecast)objType;
        }
        public static IAlarmDiagnose CreateAlarmDiagnose()
        {
            string classNamespace = AssemblyPath + ".AlarmDiagnose";
            object objType = CreateObject(classNamespace);
            return (IAlarmDiagnose)objType;
        }
       

        public static IAlarmAccess CreateAlarmAccess()
        {
            string classNamespace = AssemblyPath + ".AlarmAccess";
            object objType = CreateObject(classNamespace);
            return (IAlarmAccess)objType;
        }
        public static ICostQuery CreateCostQuery()
        {
            string classNamespace = AssemblyPath + ".CostQuery";
            object objType = CreateObject(classNamespace);
            return (ICostQuery)objType;
        }

        public static IFee_Apportion CreateFee_Apportion()
        {
            string classNamespace = AssemblyPath + ".Fee_Apportion";
            object objType = CreateObject(classNamespace);
            return (IFee_Apportion)objType;
        }


        public static IAccessCommon CreateAccessCommon()
        {
            string classNamespace = AssemblyPath + ".AccessCommon";
            object objType = CreateObject(classNamespace);
            return (IAccessCommon)objType;
        }

        //public static IElePrice CreateTB_ElePrice()
        //{
        //    string classNamespace = AssemblyPath + ".ElePrice";
        //    object objType = CreateObject(classNamespace);
        //    return (IElePrice)objType;
        //}
        ///*onlyfor9000 end*/

        //public static IPadding CreatePadding()
        //{
        //    string classNamespace = AssemblyPath + ".Padding";
        //    object objType = CreateObject(classNamespace);
        //    return (IPadding)objType;
        //}


        //public static IMaxValue CreateMaxValue()
        //{
        //    string classNamespace = AssemblyPath + ".MaxValue";
        //    object objType = CreateObject(classNamespace);
        //    return (IMaxValue)objType;
        //}
    }
}
