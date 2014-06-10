using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
     [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IEnergyContrastService
    {
        [OperationContract]
        ResultCompare GetCompareChart(QueryOrderObjects query);

        [OperationContract]
        ResultCompare GetPersonNumCompareChart(QueryOrderObjects query);

        [OperationContract]
        ResultCompare GetAreaCompareChart(QueryOrderObjects query);

        [OperationContract]
        List<NTS.WEB.Model.Itemcode> GetItemCodeList(string whereStr, string sortStr);

        [OperationContract]
        ResultCompare GetPeriodsCompareChart(QueryContrastPeriods query);

        [OperationContract]
        ResultCompare GetPersonNumPeriodsCompareChart(QueryContrastPeriods query);

        [OperationContract]
        ResultCompare GetAreaPeriodsCompareChart(QueryContrastPeriods query);
    }
}
