using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ProductInteface
{
    public interface IMonthDataObject
    {

        List<Model.MonthSumData> GetMonthDataObjectList(string tableName, string date);
    }
}
