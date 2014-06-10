using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;

namespace NTS.EMS.Config.BLL
{
    public class OperateQuotaBll
    {
        NTS.EMS.Config.ProductInteface.IQuotaObject quotaOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateQuotaObject();

        #region 定额配置模块

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="quotaData"></param>
        /// <returns></returns>
        public int InsertOrEditQuota(NTS.EMS.Config.Model.QuotaDataContact quotaData)
        {
            try
            {
                string where = " and QUOTAID=" + quotaData.QuotaId;
                var quotaObject = quotaOperator.GetQuota(where);

                #region 组织数据
                DateTime quotaTime = DateTime.Now;
                TB_Quota tbQuota = new TB_Quota();
                tbQuota.ObjectDesc = quotaData.ObjectDesc;
                tbQuota.ObjectId = quotaData.ObjectId;
                tbQuota.ObjectType = quotaData.ObjectType;
                tbQuota.QuotaId = quotaData.QuotaId;
                tbQuota.QuotaTime = quotaData.QuotaDate.Date;
                tbQuota.QuotaType = quotaData.QuotaType;
                tbQuota.QuotaValue = quotaData.QuotaValue;
                tbQuota.Reserved = quotaData.Reserved;
                tbQuota.ItemCode = quotaData.ItemCode;

                TS_Quota_Log tsQuotaLog = new TS_Quota_Log();
                tsQuotaLog.LogTime = quotaTime;
                tsQuotaLog.QuotaId = quotaData.QuotaId;
                tsQuotaLog.QuotaValue = quotaData.QuotaValue;
                tsQuotaLog.Reserved = quotaData.Reserved;
                tsQuotaLog.UserName = quotaData.UserName;
                #endregion

                //using (var scope = new TransactionScope())
                //{
                //    if (quotaObject == null || quotaObject.Count == 0)
                //    {
                //        int quotaId = quotaOperator.GetMaxQuotaId();
                //        tbQuota.QuotaId = quotaId;
                //        tsQuotaLog.QuotaId = quotaId;
                //        // 插入定额表
                //        quotaOperator.InsertQuota(tbQuota, tsQuotaLog);
                //    }
                //    else
                //    {
                //        //更新定额表
                //        quotaOperator.UpdateQuota(tbQuota, tsQuotaLog);
                //    }
                //    quotaOperator.InsertQuotaLog(tsQuotaLog);
                //    scope.Complete();
                //    return tbQuota.QuotaId;
                //}
                int count = 0;
                if (quotaObject == null || quotaObject.Count == 0)
                {
                    int quotaId = quotaOperator.GetMaxQuotaId();
                    tbQuota.QuotaId = quotaId;
                    tsQuotaLog.QuotaId = quotaId;
                    // 插入定额表
                    count = quotaOperator.InsertQuota(tbQuota, tsQuotaLog);

                }
                else
                {
                    //更新定额表
                    count = quotaOperator.UpdateQuota(tbQuota, tsQuotaLog);
                }
                if (count > 0)
                {
                    return tbQuota.QuotaId;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 定额加载
        /// </summary>
        /// <param name="queryQuota"></param>
        /// <returns></returns>
        public ResultQuotaInfo GetQuotaInfo(QueryQuotaContact queryQuota)
        {
            #region 定义变量
            ResultQuotaInfo quotaInfo = new ResultQuotaInfo();
            List<Model.TB_Quota> currentQuota = new List<TB_Quota>();
            string currentWhere = string.Empty;
            #endregion

            #region 组织查询条件

            //月定额
            if (queryQuota.QuotaType == 1)
            {
                currentWhere = " and OBJECTID=" + queryQuota.ObjectId;
                currentWhere += " and ItemCode='" + queryQuota.ItemCode + "' ";
                currentWhere += " and QuotaType=" + queryQuota.QuotaType;
                if (queryQuota.Date != null && !string.IsNullOrEmpty(queryQuota.Date.ToString()))
                {
                    currentWhere += string.Format(" and ( DATEPART(year,QUOTATIME)={0} and DATEPART(month,QUOTATIME) ={1})", queryQuota.Date.Year, queryQuota.Date.Month);
                }
            }
            else if (queryQuota.QuotaType == 2)
            {
                currentWhere = " and OBJECTID=" + queryQuota.ObjectId;
                currentWhere += " and ItemCode='" + queryQuota.ItemCode + "' ";
                currentWhere += " and QuotaType=" + queryQuota.QuotaType;
                if (queryQuota.Date != null && !string.IsNullOrEmpty(queryQuota.Date.ToString()))
                {
                    currentWhere += string.Format(" and DATEPART(year,QUOTATIME)={0}", queryQuota.Date.Year);
                }
            }

            #endregion

            #region 组织数据
            currentQuota = quotaOperator.GetQuota(currentWhere);
            if (currentQuota != null && currentQuota.Count > 0)
            {
                QueryQuotaLogContract queryQuotaLog = new QueryQuotaLogContract { QuotaId = currentQuota.First().QuotaId, PageCurrent = 1, PageSize = queryQuota.PageSize };
                var quotaLogList = GetQuotaLogs(queryQuotaLog);
                quotaInfo.QuotaLogList = quotaLogList.QuotaLogList;
                quotaInfo.Page = quotaLogList.Page;
                quotaInfo.QuotaData = currentQuota.Select(p => new Quota
                {
                    ObjectDesc = p.ObjectDesc,
                    ObjectId = p.ObjectId,
                    ObjectType = p.ObjectType,
                    QuotaId = p.QuotaId,
                    QuotaTime = p.QuotaTime,
                    QuotaType = (QuotaType)p.QuotaType,
                    QuotaValue = (float)p.QuotaValue,
                    ItemCode = p.ItemCode,
                    Reserved = p.Reserved
                }).First();
            }


            #endregion

            return quotaInfo;
        }


        public List<NTS.WEB.Model.Itemcode> GetItemcodeList()
        {
            return new NTS.WEB.BLL.Itemcode().GetItemcodeList(string.Format(" and parentid=0"), "");
        }

        public List<NTS.WEB.Model.Itemcode> GetAllItemcodeList()
        {
            var lastResult = new List<WEB.Model.Itemcode>();
            var result = GetItemcodeList();
            foreach (var itemcode in result)
            {
                lastResult.Add(itemcode);
                var child =
                    new WEB.BLL.Itemcode().GetItemcodeList(string.Format(" and parentid={0}", itemcode.ItemcodeID),
                        string.Empty);
                lastResult.AddRange(child);
            }
            return lastResult;
        }

        /// <summary>
        /// 定额日志查询
        /// </summary>
        /// <param name="queryQuotaLog"></param>
        /// <returns></returns>
        public ResultQuotaLogs GetQuotaLogs(QueryQuotaLogContract queryQuotaLog)
        {
            #region 定义变量

            ResultQuotaLogs resultQuotaLogs = new ResultQuotaLogs();
            string whereStr = string.Empty;

            #endregion

            #region 组织条件

            whereStr += " and QUOTAID=" + queryQuotaLog.QuotaId;
            if (queryQuotaLog.StartTime != null && queryQuotaLog.StartTime.ToShortDateString() != "1900-1-1" && !queryQuotaLog.StartTime.Equals(DateTime.MaxValue) && !queryQuotaLog.StartTime.Equals(DateTime.MinValue))
            {
                whereStr += " and LOGTIME >='" + queryQuotaLog.StartTime + "'";
            }
            if (queryQuotaLog.EndTime != null && queryQuotaLog.EndTime.ToShortDateString() != "1900-1-1" && !queryQuotaLog.EndTime.Equals(DateTime.MaxValue) && !queryQuotaLog.EndTime.Equals(DateTime.MinValue))
            {
                whereStr += " and LOGTIME <='" + queryQuotaLog.EndTime.AddDays(1) + "'";
            }

            #endregion

            #region 组织数据
            var currentQuota = quotaOperator.GetQuota(" and QUOTAID=" + queryQuotaLog.QuotaId);
            resultQuotaLogs.QuotaLogList = quotaOperator.GetQuotaLogList(whereStr).Select(p =>
                new QuotaLog
                {
                    LogTime = p.LogTime,
                    QuotaId = p.QuotaId,
                    QuotaValue = (float)p.QuotaValue,
                    Reserved = p.Reserved,
                    UserName = p.UserName,
                    QuotaTimeStr = currentQuota[0].QuotaType == 1 ? string.Format("{0}-{1}", currentQuota[0].QuotaTime.Year, currentQuota[0].QuotaTime.Month) : currentQuota[0].QuotaTime.Year.ToString(),
                    ObjectDesc = currentQuota.First().ObjectDesc
                }).Skip((queryQuotaLog.PageCurrent - 1) * queryQuotaLog.PageSize).Take(queryQuotaLog.PageSize).ToList();

            resultQuotaLogs.Page = new Padding();
            resultQuotaLogs.Page.Current = queryQuotaLog.PageCurrent;
            resultQuotaLogs.Page.Total = quotaOperator.GetQuotaLogList(whereStr).Count();
            return resultQuotaLogs;
            #endregion
        }

        /// <summary>
        /// 获取单个的Quota
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultQuotaInfo GetQuotaInfoById(int id)
        {
            ResultQuotaInfo quotaInfo = new ResultQuotaInfo();
            quotaInfo.QuotaData = new Quota();
            quotaInfo.QuotaLogList = new List<QuotaLog>();
            quotaInfo.Page = new Padding();
            List<Model.TB_Quota> currentQuota = new List<TB_Quota>();
            currentQuota = quotaOperator.GetQuota(" and QUOTAID=" + id);
            if (currentQuota != null && currentQuota.Count > 0)
            {
                QueryQuotaLogContract queryQuotaLog = new QueryQuotaLogContract { QuotaId = currentQuota.First().QuotaId, PageCurrent = 1, PageSize = 100 };
                var quotaLogList = GetQuotaLogs(queryQuotaLog);
                quotaInfo.QuotaLogList = quotaLogList.QuotaLogList;
                quotaInfo.Page = quotaLogList.Page;
                quotaInfo.QuotaData = currentQuota.Select(p => new Quota
                {
                    ObjectDesc = p.ObjectDesc,
                    ObjectId = p.ObjectId,
                    ObjectType = p.ObjectType,
                    QuotaId = p.QuotaId,
                    QuotaTime = p.QuotaTime,
                    QuotaType = (QuotaType)p.QuotaType,
                    QuotaValue = (float)p.QuotaValue,
                    ItemCode = p.ItemCode,
                    Reserved = p.Reserved
                }).First();
            }
            return quotaInfo;
        }
        #endregion

    }

}
