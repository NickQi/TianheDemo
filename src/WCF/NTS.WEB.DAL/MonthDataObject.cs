using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.ProductInteface;
using System.Data;

namespace NTS.WEB.DAL
{
    public class MonthDataObject : IMonthDataObject
    {
         

        public List<Model.MonthSumData> GetMonthDataObjectList(string tableName,string date)
        {
            var cmd = new DataCommand("getMonthData", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#TableName#", tableName);
          
            return cmd.ExecuteEntityList<Model.MonthSumData>();
        }
    }
}
