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
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IAlarmService
    {
        [OperationContract]
        ResultAlarmType GetAlarmType(string strWhere);

        [OperationContract]
        ResultAlarmNewList GetAlarmList(QueryAlarmNew ParamAlarm, string groupId);

        [OperationContract]
        ResultAlarmIndex GetAlarmIndexCount();
    }
}
