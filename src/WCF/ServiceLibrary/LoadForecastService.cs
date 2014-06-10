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
    public class LoadForecastService : ILoadForecastService
    {
        NTS.WEB.BLL.LoadForecast bllForeCast = new NTS.WEB.BLL.LoadForecast();

       // [Log(ModelName = "负荷预测")]
        [CustomException]
        public ResultLoadForecastMap GetLoadForecastChart(QueryLoadForecast loadCast)
        {
            return bllForeCast.GetLoadForecastChart(loadCast);
        }
    }
}
