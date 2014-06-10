using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.BLL;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EnergyContrastService : IEnergyContrastService
    {
        NTS.WEB.BLL.Itemcode itemcodeBll = new Itemcode();
        NTS.WEB.BLL.EnergyContrast entBll = new EnergyContrast();

        public  List<NTS.WEB.Model.Itemcode> GetItemCodeList(string whereStr, string sortStr)
        {
            return itemcodeBll.GetItemcodeList(whereStr, sortStr);
        }


        [Log(ModelName = "能耗对比")]
        [CustomException]
        public ResultCompare GetCompareChart(QueryOrderObjects query)
        {
            return entBll.GetCompareChart(query);
        }

        [Log(ModelName = "人均对比")]
        [CustomException]
        public ResultCompare GetPersonNumCompareChart(QueryOrderObjects query)
        {
            return entBll.GetPersonNumCompareChart(query);
        }

        [Log(ModelName = "人均对比")]
        [CustomException]
        public ResultCompare GetAreaCompareChart(QueryOrderObjects query)
        {
            return entBll.GetAreaCompareChart(query);
        }


        [Log(ModelName = "多时间能耗对比")]
        [CustomException]
        public ResultCompare GetPeriodsCompareChart(QueryContrastPeriods query)
        {
            return entBll.GetPeriodsCompareChart(query);
        }

        [Log(ModelName = "多时间人均能耗对比")]
        [CustomException]
        public ResultCompare GetPersonNumPeriodsCompareChart(QueryContrastPeriods query)
        {
            return entBll.GetPersonNumPeriodsCompareChart(query);
        }

        [Log(ModelName = "多时间人均能耗对比")]
        [CustomException]
        public ResultCompare GetAreaPeriodsCompareChart(QueryContrastPeriods query)
        {
            return entBll.GetAreaPeriodsCompareChart(query);
        }

        
    }
}
