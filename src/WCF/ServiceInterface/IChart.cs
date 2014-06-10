using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IChart
    {
        [OperationContract]
        ResultIndexPieChart IndexElectricityPieChart();

        [OperationContract]
        ResultIndexLineChart IndexElectricityLineChart();

        [OperationContract]
        ResultQueryPie GetQueryPieChart(BasicQuery query);

        [OperationContract]
        ResultItemCode GetQueryLineChart(BasicQuery query);

       
       

        [OperationContract]
        ResultCompare GetCompareChart(QueryCompare query);

        [OperationContract]
        ResultCompare GetAreaCompareChart(QueryCompare query);

        [OperationContract]
        ResultItemCode RealChart(RealQuery query);

        [OperationContract]
        ResultRealLine DeviceRealChart(RealQuery query);

        [OperationContract]
        ResultItemCode GetTwoQueryLineChart(BasicQuery query, BasicQuery query2, int tabId);

        [OperationContract]
        ResultItemCode IndexAvgElectricityLineChart();

        [OperationContract]
        ItemList IndexItems(QueryEnergyIterm query);

        [OperationContract]
        ResultRealLine IndexElectricityRealLineChart();

        [OperationContract]
        ResultDevice GetDeviceList(QueryDevice2 query);
        [OperationContract]
        ResultEnergyAnalyse GetEnergyAnalyseLineChart(QueryAnalyse query);

        [OperationContract]
        EnergyAnalyseCompare GetEnergyAnalyseCompare(QueryAnalyse query);
        [OperationContract]
        ResultEnergyAnalysePie GetEnergyAnalysePie(QueryAnalyse query);
         [OperationContract]
        ResultQuota GetQuotaAnalyseChart(QueryQuota query);
    }
}
