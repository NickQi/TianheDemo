using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.BLL
{
    public class QuotaAnalyse
    {
        readonly NTS.WEB.ProductInteface.IQuotaAnalyse _idal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateQuotaAnalyse();
        public QuotaAnalyseModel GetQuotaAnalyse(string whereStr)
        {
            return _idal.GetQuotaAnalyse(whereStr);
        }
    }
}
