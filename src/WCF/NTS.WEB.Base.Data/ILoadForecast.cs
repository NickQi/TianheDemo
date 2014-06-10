using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.ProductInteface
{
    public interface ILoadForecast
    {
        BaseResult GetLoadForecastChart(BaseQueryModel loadCast);
    }
}
