using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.Model;

namespace NTS.EMS.Config.BLL
{
    public class QuerySysLogBll
    {
        NTS.EMS.Config.ProductInteface.ISysLogObject sysLogObject = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateSysLog();
        public ResultSysLog GetSysLog(QuerySysLogContact query)
        {

           /* string cacheKey = string.Empty;
            cacheKey = query.StartTime.ToString() + query.EndTime.ToString() + query.ModelName;
            if (CacheHelper.GetCache(cacheKey) != null)
            {
                return (ResultSysLog)CacheHelper.GetCache(cacheKey);
            }
            */
            #region 返回类型
            ResultSysLog resultSysLog = new ResultSysLog();
            resultSysLog.SysLogList = new List<BaseSysLog>();
            resultSysLog.Page = new Padding();
            #endregion

            #region 查询条件
            string where = string.Empty;
            if (query.StartTime != null && query.StartTime.ToShortDateString() != "1900-1-1" && !query.StartTime.Equals(DateTime.MaxValue) && !query.StartTime.Equals(DateTime.MinValue))
            {
                where += " and LogTime>='" + query.StartTime + "' ";
            }
            if (query.EndTime != null && query.EndTime.ToShortDateString() != "1900-1-1" && !query.EndTime.Equals(DateTime.MinValue) && !query.EndTime.Equals(DateTime.MaxValue))
            {
                where += " and LogTime<='" + query.EndTime.AddDays(1) + "' ";
            }
            if (!string.IsNullOrEmpty(query.ModelName))
            {
                where += " and ModelName like '%" + query.ModelName.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(query.OperatorName))
            {
                //操作人员
                where += " and UserName like '%" + query.OperatorName.Trim() + "%'";
            }

            #endregion

            #region 组织数据

            var sysLogList = sysLogObject.GetSysLogList(where);
            int total = sysLogList.Count();
            resultSysLog.Page.Total = total;
            resultSysLog.Page.Current = query.PageCurrent;
            resultSysLog.SysLogList = sysLogList.Select(p =>
                new BaseSysLog
                {
                    SysNo = p.SysNo,
                    ModelName = p.ModelName,
                    LogContent = p.LogContent,
                    LogTime = p.LogTime,
                    OpType = p.OpType == 1 ? OpType.Operate : OpType.Configure,
                    UserName = p.UserName
                }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();

         /*   if (CacheHelper.GetCache(cacheKey) == null)
            {
                CacheHelper.SetCache(cacheKey, resultSysLog);
            }
            */
            #endregion

            return resultSysLog;
        }
    }
}
