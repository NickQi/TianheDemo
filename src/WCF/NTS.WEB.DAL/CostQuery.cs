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
    public class CostQuery : ICostQuery
    {
     

        public List<CostQueryModel> GetCostQuery(QueryCost query)
        {
            try
            {
                var cmd = new DataCommand("getCostQuery", new SqlCustomDbCommand());
                string table = "";
                switch (query.Particle)
                {
                    case Particle.Month:
                        table = string.Format("TS_FEE_DAY_{0}", query.StartTime.Year);
                        break;
                    case Particle.Year:
                        table = string.Format("TS_FEE_MONTH_{0}", query.StartTime.Year);
                        break;
                    default:
                        table = string.Format("TS_FEE_DAY_{0}", query.StartTime.Year);
                        break;
                }
                cmd.ReplaceParameterValue("#TableName#", table);
                cmd.SetParameterValue("@OBJECTID", query.ObjectId.ToString());
                cmd.SetParameterValue("@ITEMCODE", query.ItemCode);
                cmd.SetParameterValue("@StartTime", query.StartTime);
                cmd.SetParameterValue("@EndTime", query.EndTime);

                return cmd.ExecuteEntityList<CostQueryModel>();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        public List<StepSettingModel> GetStepSetting(string itemcode)
        {
            try
            {
                var cmd = new DataCommand("getStepSetting", new SqlCustomDbCommand());
                cmd.SetParameterValue("@ITEMCODE", itemcode);
                return cmd.ExecuteEntityList<StepSettingModel>();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
    }
}
