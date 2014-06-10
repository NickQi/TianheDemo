using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.BLL
{
    public class AlarmDiagnose
    {
        private readonly IAlarmDiagnose dal = DataSwitchConfig.CreateAlarmDiagnose();
     
        public List<AlarmDiagnoseModel> GetAlarmDiagnose(QueryAlarm query)
        {
            return dal.GetAlarmDiagnose(query);
        }
    }
}
