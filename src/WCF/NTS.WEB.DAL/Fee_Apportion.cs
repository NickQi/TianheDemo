using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class Fee_Apportion : IFee_Apportion
    {


        public List<CostQueryModel> GetCostQuery(Queryfeeapportion query, DateTime dtBegin, DateTime dtEnd)
        {
            try
            {
                var cmd = new DataCommand("getCostQuery2", new SqlCustomDbCommand());
                string table = "TS_FEE_DAY_" + query.StartTime.Year;
                cmd.ReplaceParameterValue("#TableName#", table);
                cmd.SetParameterValue("@OBJECTID", query.ObjectId.ToString());
                cmd.SetParameterValue("@ITEMCODE", query.ItemCode);
                cmd.SetParameterValue("@StartTime", dtBegin);
                cmd.SetParameterValue("@EndTime", dtEnd);

                return cmd.ExecuteEntityList<CostQueryModel>();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        public List<TB_Alloction_Config> GetAlloctionConfig(Queryfeeapportion feeApport)
        {
            try
            {
                var cmd = new DataCommand("GetAlloctionConfig", new SqlCustomDbCommand());
                cmd.SetParameterValue("@OBJECTID", feeApport.ObjectId);
                cmd.SetParameterValue("@ITEMCODE", feeApport.ItemCode);
                cmd.SetParameterValue("@YEAR", feeApport.StartTime.Year);
                cmd.SetParameterValue("@MONTH", feeApport.StartTime.Month);
                return cmd.ExecuteEntityList<TB_Alloction_Config>();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
    }
}
