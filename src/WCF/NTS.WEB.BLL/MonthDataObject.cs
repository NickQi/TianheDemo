using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.BLL
{
    public class MonthDataObject
    {
        readonly NTS.WEB.ProductInteface.IMonthDataObject _idal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateMonthData();

        public List<Model.MonthSumData> GetMonthDataObjectList(string tableName,string date)
        {
            return _idal.GetMonthDataObjectList(tableName, date);
        }
    }
}
