using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class QuotaAlarmObject : IQuotaAlarmObject
    {

        public List<Model.TB_Ems_Quota_Percent> GetQuotaAlarmList(string whereStr)
        {
            var cmd = new DataCommand("GetQuotaAlarmList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_Ems_Quota_Percent>();
        }

        public Model.TB_Ems_Quota_Percent GetQuotaAlarm(string whereStr)
        {
            var cmd = new DataCommand("GetQuotaAlarmList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntity<Model.TB_Ems_Quota_Percent>();
        }

        public int SaveQuotaAlarm(Model.QuotaAlarmDataContact quotaAlarmData)
        {
            var cmd = new DataCommand("SaveQuotaAlarmData", new SqlCustomDbCommand());
            cmd.SetParameterValue("@AlarmType", quotaAlarmData.AlarmType);
            cmd.SetParameterValue("@ObjectDesc", quotaAlarmData.ObjectDesc);
            cmd.SetParameterValue("@ObjectType", quotaAlarmData.ObjectType);
            cmd.SetParameterValue("@ObjectId", quotaAlarmData.ObjectId);
            cmd.SetParameterValue("@QuotaType", quotaAlarmData.QuotaType);
            cmd.SetParameterValue("@ItemCode", quotaAlarmData.ItemCode);
            cmd.SetParameterValue("@Percent", quotaAlarmData.Percent);
            return cmd.ExecuteNonQuery();
        }

        public int DeleteQuotaAlarm(int id)
        {
            var cmd = new DataCommand("DeleteQuotaAlarm", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", id);
            return cmd.ExecuteNonQuery();
        }
    }
}
