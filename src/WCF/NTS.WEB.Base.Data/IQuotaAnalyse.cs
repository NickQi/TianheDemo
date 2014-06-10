using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
   public interface IQuotaAnalyse
   {
       QuotaAnalyseModel GetQuotaAnalyse(string whereStr);
   }
}
