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
    public class AlarmDiagnose : IAlarmDiagnose
    {
        public List<AlarmDiagnoseModel> GetAlarmDiagnose(QueryAlarm query)
        {
            try
            {
                var cmd = new DataCommand("getAlarmDiagnose", new SqlCustomDbCommand());
                IList<string> objectIds = new AlarmAccess().GetAlarmListAreaId(query.ObjectId, query.ObjType);
                StringBuilder sb = new StringBuilder();
                foreach (string s in objectIds)
                {
                    sb.Append(s);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                cmd.ReplaceParameterValue("#ObjectId#", sb.ToString());
                cmd.SetParameterValue("@StartTime", query.StartTime);
                cmd.SetParameterValue("@EndTime", query.EndTime);

                return cmd.ExecuteEntityList<AlarmDiagnoseModel>();
            }
            catch(Exception ee)
            {
                throw ee;
                //return null;
            }
            
        }
    }
}
