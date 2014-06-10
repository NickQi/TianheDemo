using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NTS.WEB.ProductInteface
{
    public interface IBalanceAnalysis
    {
        int GetChildAreaCount(int parentid);
        DataTable GetChildAreaList(int pageIndex, int pageSize, int parentid);
        DataTable GetChildAreaList(int parentid);
        DataTable GetBalanaceValueByMonth(string itemcodeid, int areaid, DateTime month);
    }
}
