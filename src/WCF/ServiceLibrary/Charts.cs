using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Charts : IChart
    {
        /// <summary>
        /// 首页电分类占比图
        /// </summary>
        /// <returns></returns>
        public ResultIndexPieChart IndexElectricityPieChart()
        {
            var result = new ResultIndexPieChart {ItemCode = new List<Itemcode>(), ItemCodeEnery = new List<decimal>()};

            var itemList = new NTS.WEB.BLL.Itemcode().GetItemcodeList(string.Format(" and ParentID=(select itemcodeid from Becm_ItemCode where ItemCodeNumber='{0}')", "01000"), " order by ItemcodeID");
            result.ItemCode = itemList;

            var eneryList = new NTS.WEB.BLL.Charts().GetIndexPieChart();
            result.ItemCodeEnery = eneryList;
            return result;
        }
        [Log(ModelName = "首页24小时曲线")]
        [CustomException]
        public ResultIndexLineChart IndexElectricityLineChart()
        {
            var nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            var indexLine = new NTS.WEB.BLL.Charts().IndexElectricityLineChart(nowDate, nowDate.AddDays(1).AddHours(-1));
            return indexLine;
        }

        [Log(ModelName = "首页平均线曲线")]
        [CustomException]
        public ResultItemCode IndexAvgElectricityLineChart()
        {
            var nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            var indexLine = new NTS.WEB.BLL.Charts().IndexAvgElectricityLineChart(nowDate, nowDate.AddDays(1).AddHours(-1));
            return indexLine;
        }

        [Log(ModelName = "首页当日用电趋势")]
        [CustomException]
        public ResultRealLine IndexElectricityRealLineChart()
        {
            var nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            var realLine = new NTS.WEB.BLL.Charts().IndexElectricityRealLine(nowDate, nowDate.AddDays(1).AddHours(-1));

            return realLine;
        }



        [Log(ModelName = "能耗查询占比图")]
        [CustomException]
        public ResultQueryPie GetQueryPieChart(BasicQuery query)
        {
            return new NTS.WEB.BLL.Charts().GetQueryPieChart(query);
        }
        [Log(ModelName = "能耗查询曲线")]
        [CustomException]
        public ResultItemCode GetQueryLineChart(BasicQuery query)
        {
            return new NTS.WEB.BLL.Charts().GetQueryLineChart(query);
        }

        [Log(ModelName = "能耗对比")]
        [CustomException]
        public ResultCompare GetCompareChart(QueryCompare query)
        {
            return new NTS.WEB.BLL.Charts().GetCompareChart(query);
        }


        [Log(ModelName = "面积对比")]
        [CustomException]
        public ResultCompare GetAreaCompareChart(QueryCompare query)
        {
            return new NTS.WEB.BLL.Charts().GetAreaCompareChart(query);
        }
        [Log(ModelName = "设备监测曲线")]
        [CustomException]
        public ResultItemCode RealChart(RealQuery query)
        {
            return new NTS.WEB.BLL.Charts().RealChart(query);
        }

        [Log(ModelName = "设备监测曲线")]
        [CustomException]
        public ResultRealLine DeviceRealChart(RealQuery query)
        {
            return new NTS.WEB.BLL.Charts().DeviceRealChart(query);
        }

        [CustomException]
        public ItemList IndexItems(QueryEnergyIterm query)
        {

            return new NTS.WEB.BLL.Charts().IndexItems(query);
        }
        public ResultItemCode GetTwoQueryLineChart(BasicQuery query, BasicQuery query2, int tabId)
        {
            return new NTS.WEB.BLL.Charts().GetTwoQueryLineChart(query, query2, tabId);
        }

        public ResultDevice GetDeviceList(QueryDevice2 query)
        {
            return new NTS.WEB.BLL.Charts().GetDeviceList(query);
        }

        

       

        [Log(ModelName = "能耗分析查询曲线")]
        [CustomException]
        public ResultEnergyAnalyse GetEnergyAnalyseLineChart(QueryAnalyse query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.Charts().GetEnergyAnalyseLineChart(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ResultEnergyAnalyse() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ResultEnergyAnalyse() { ActionInfo = pAction };
            }
           
        }

        [Log(ModelName = "能耗分析比较")]
        [CustomException]
        public EnergyAnalyseCompare GetEnergyAnalyseCompare(QueryAnalyse query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.Charts().GetEnergyAnalyseCompare(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new EnergyAnalyseCompare() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new EnergyAnalyseCompare() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "能耗分析环状图")]
        [CustomException]
        public ResultEnergyAnalysePie GetEnergyAnalysePie(QueryAnalyse query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.Charts().GetEnergyAnalysePie(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ResultEnergyAnalysePie() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ResultEnergyAnalysePie() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "定额分析")]
        [CustomException]
        public ResultQuota GetQuotaAnalyseChart(QueryQuota query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.Charts().GetQuotaAnalyseChart(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ResultQuota() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ResultQuota() { ActionInfo = pAction };
            }
        }
    }
}
