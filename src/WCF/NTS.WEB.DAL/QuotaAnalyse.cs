using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using System.Data;
namespace NTS.WEB.DAL
{
    public class QuotaAnalyse : IQuotaAnalyse
    {
        public QuotaAnalyseModel GetQuotaAnalyse(string whereStr)
        {
            try
            {
                var cmd = new DataCommand("getQuotaValue", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                //return  cmd.ExecuteEntity<QuotaAnalyseModel>();
                var list = cmd.ExecuteEntityList<QuotaAnalyseModel>();
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            catch(Exception ee)
            {
               
            }
            return null;
           
        }
    }
}
