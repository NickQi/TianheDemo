using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class QuotaObject : IQuotaObject
    {
        public List<Model.TS_Quota_Log> GetQuotaLogList(string whereStr)
        {
            var cmd = new DataCommand("GetQuotaLog", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TS_Quota_Log>();
        }

        public List<Model.TB_Quota> GetQuota(string whereStr)
        {
            var cmd = new DataCommand("GetQuota", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_Quota>();
        }

        public int InsertQuota(Model.TB_Quota quotaData, Model.TS_Quota_Log quotaLogData)
        {
            var cmd = new DataCommand("InsertQuota", new SqlCustomDbCommand());
            cmd.SetParameterValue("@QuotaId", quotaData.QuotaId);
            cmd.SetParameterValue("@ObjectDesc", quotaData.ObjectDesc);
            cmd.SetParameterValue("@ObjectType", quotaData.ObjectType);
            cmd.SetParameterValue("@ObjectId", quotaData.ObjectId);
            cmd.SetParameterValue("@QuotaTime", quotaData.QuotaTime);
            cmd.SetParameterValue("@QuotaValue", quotaData.QuotaValue);
            cmd.SetParameterValue("@Reserved", quotaData.Reserved);
            cmd.SetParameterValue("@QuotaType", quotaData.QuotaType);
            cmd.SetParameterValue("@ItemCode", quotaData.ItemCode);

           
            cmd.SetParameterValue("@LogTime", quotaLogData.LogTime);
            cmd.SetParameterValue("@UserName", quotaLogData.UserName);
           return cmd.ExecuteNonQuery();
        }

        public int UpdateQuota(Model.TB_Quota quotaData, Model.TS_Quota_Log quotaLogData)
        {
            var cmd = new DataCommand("UpdateQuota", new SqlCustomDbCommand());
            cmd.SetParameterValue("@QuotaId", quotaData.QuotaId);
            cmd.SetParameterValue("@QuotaTime", quotaData.QuotaTime);
            cmd.SetParameterValue("@QuotaValue", quotaData.QuotaValue);
            cmd.SetParameterValue("@Reserved", quotaData.Reserved);
            cmd.SetParameterValue("@QuotaType", quotaData.QuotaType);

            
            cmd.SetParameterValue("@LogTime", quotaLogData.LogTime);
            cmd.SetParameterValue("@UserName", quotaLogData.UserName);
            return cmd.ExecuteNonQuery();
        }

        public int InsertQuotaLog(Model.TS_Quota_Log quotaLogData)
        {
            var cmd = new DataCommand("InsertQuotaLog", new SqlCustomDbCommand());
            cmd.SetParameterValue("@QuotaId", quotaLogData.QuotaId);
            cmd.SetParameterValue("@LogTime", quotaLogData.LogTime);
            cmd.SetParameterValue("@UserName", quotaLogData.UserName);
            cmd.SetParameterValue("@QuotaValue", quotaLogData.QuotaValue);
            cmd.SetParameterValue("@Reserved", quotaLogData.Reserved);
            return cmd.ExecuteNonQuery();
        }

        public List<Model.TS_DataCenter_Area_Month> GetTsDataCenterAreaMonth(string whereStr, string Year)
        {
            var cmd = new DataCommand("GetTsDataCenterAreaMonth", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            cmd.ReplaceParameterValue("#Year#", Year);
            return cmd.ExecuteEntityList<Model.TS_DataCenter_Area_Month>();
        }

        public int GetMaxQuotaId()
        {
            try
            {
                var cmd = new DataCommand("GetMaxQuotaId", new SqlCustomDbCommand());
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}
