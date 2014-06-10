using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
    [ServiceContract(SessionMode=SessionMode.Allowed)]
    public interface IAlarmDiagnoseService
    {
        [OperationContract]
        ResultAlarm GetAlarmDiagnose(QueryAlarm query);
        

        
    }
}
