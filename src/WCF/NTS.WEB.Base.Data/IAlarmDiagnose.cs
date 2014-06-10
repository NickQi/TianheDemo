using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IAlarmDiagnose
    {
        List<AlarmDiagnoseModel> GetAlarmDiagnose(QueryAlarm query);
    }
}
