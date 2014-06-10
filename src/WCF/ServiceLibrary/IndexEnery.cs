using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class IndexEnery : IIndexEnery
    {
        [Log(ModelName = "首页弹框")]
        [CustomException]
        public IndexWindowResult GetIndexWindowResult(QueryIndexWindow query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.IndexEnery().GetItemCodeListByObjectID(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new IndexWindowResult() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new IndexWindowResult() { ActionInfo = pAction };
            }
        }
        [Log(ModelName = "首页月统计")]
        [CustomException]
        public IndexMonthEnery GetIndexMonthEneryResult()
        {
            DateTime endTime;
            var startTime = endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-1"));
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.IndexEnery().GetMonthItemCodeList(startTime, endTime);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new IndexMonthEnery() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new IndexMonthEnery() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "首页同期对比")]
        [CustomException]
        public IndexCompareEnery GetIndexCompareEnery()
        {
            var indexCompany = new IndexCompareEnery
                {
                    SameCompare = "-",
                    ElectricitySameCompare = "-",
                    MonthCompare = "-",
                    ElectricityMonthCompare = "-"
                };
            var nowMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-1"));
            var monthMonth = nowMonth.AddMonths(-1);
            var companyMonth = nowMonth.AddYears(-1);
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.IndexEnery().GetIndexCompareEnery(nowMonth, nowMonth);
                var companyresult = new NTS.WEB.BLL.IndexEnery().GetIndexCompareEnery(companyMonth, companyMonth);
                var monthresult = new NTS.WEB.BLL.IndexEnery().GetIndexCompareEnery(monthMonth, monthMonth);

                if (result != null)
                {
                    indexCompany.Total = result[1];
                    indexCompany.Electricity = result[0];

                    if (companyresult != null)
                    {
                        if (companyresult[1] > 0)
                        {
                            indexCompany.SameCompare =
                                decimal.Round(100 * (result[1] - companyresult[1]) / companyresult[1], 2)
                                    .ToString(CultureInfo.InvariantCulture) + "%";
                        }
                        if (companyresult[0] > 0)
                        {
                            indexCompany.ElectricitySameCompare =
                                decimal.Round(100 * (result[0] - companyresult[0]) / companyresult[0], 2)
                                    .ToString(CultureInfo.InvariantCulture) + "%";
                        }
                    }

                    if (monthresult != null)
                    {
                        if (monthresult[1] > 0)
                        {
                            indexCompany.MonthCompare =
                                decimal.Round(100 * (result[1] - monthresult[1]) / monthresult[1], 2)
                                    .ToString(CultureInfo.InvariantCulture) + "%";
                        }
                        if (monthresult[0] > 0)
                        {
                            indexCompany.ElectricityMonthCompare =
                                decimal.Round(100 * (result[0] - monthresult[0]) / monthresult[0], 2)
                                    .ToString(CultureInfo.InvariantCulture) + "%";
                        }
                    }
                }

                pAction.Success = true;
                indexCompany.ActionInfo = pAction;
                return indexCompany;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new IndexCompareEnery() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "首页同期对比")]
        [CustomException]
        public MainInfo GetIndexCompareEneryNew()
        {
            //string username= HttpContext.Current.Session["userid"].ToString() ;
            MainInfo mainInfo = new MainInfo();
            var nowMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-1"));
            var endTime = DateTime.Now;
            var pAction = new ExecuteProcess();

            try
            {
                mainInfo = new NTS.WEB.BLL.IndexEnery().GetIndexCompareEneryNew(nowMonth, endTime);
                pAction.Success = true;
                mainInfo.ActionInfo = pAction;
                return mainInfo;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new MainInfo() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "首页排名")]
        [CustomException]
        public IndexShopOrder GetIndexShopOrder()
        {
            var pAction = new ExecuteProcess();
            var nowMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-1"));
            try
            {
                var shopOrder = new NTS.WEB.BLL.IndexEnery().GetIndexShopOrder(nowMonth, nowMonth);
                shopOrder.AreaEneryOrderList = (from o in shopOrder.AreaEneryOrderList orderby o.EneryValue select o).ToList();
                shopOrder.TotalEneryOrderList = (from o in shopOrder.TotalEneryOrderList orderby o.EneryValue select o).ToList();
                int order = 0;
                // 设置排序的序号
                foreach (var a in shopOrder.AreaEneryOrderList)
                {
                    order++;
                    a.OrderNum = order;
                }
                order = 0;
                foreach (var t in shopOrder.TotalEneryOrderList)
                {
                    order++;
                    t.OrderNum = order;
                }
                pAction.Success = true;
                shopOrder.ActionInfo = pAction;
                return shopOrder;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new IndexShopOrder() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "首页极值信息")]
        [CustomException]
        public IndexLimit GetIndexLimit()
        {
            var pAction = new ExecuteProcess();
            var nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            try
            {
                var indexLimit = new NTS.WEB.BLL.IndexEnery().GetIndexLimit(nowDate, nowDate.AddDays(1).AddHours(-1));
                pAction.Success = true;
                indexLimit.ActionInfo = pAction;
                return indexLimit;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new IndexLimit() { ActionInfo = pAction };
            }
        }
    }
}
