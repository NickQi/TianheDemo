using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NTS.WEB.Base.Data;
using Framework.Data;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;

namespace NTS.WEB.DAL
{
    public class LoadForecast : ILoadForecast
    {
        public BaseResult GetLoadForecastChart(BaseQueryModel loadCast)
       {
           return EnergyContrastCommon.GetBaseEneryDataListNew(loadCast);
       }
    }
}
