using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AlarmService : IAlarmService
    {
        [CustomException]
        public ResultAlarmType GetAlarmType(string strWhere)
        {
            return new NTS.WEB.BLL.Alarm().GetAlarmType(strWhere);
        }

        [CustomException]
        public ResultAlarmNewList GetAlarmList(QueryAlarmNew ParamAlarm, string groupId)
        {
            return new NTS.WEB.BLL.Alarm().GetAlarmList(ParamAlarm, groupId);
        }

        [CustomException]
        public ResultAlarmIndex GetAlarmIndexCount()
        {
            return new NTS.WEB.BLL.Alarm().GetAlarmIndexCount();
        }

        [CustomException]
        public ResultAlarmIndex ExportAlarm()
        {
            //return new NTS.WEB.BLL.Alarm().
            return null;
        }
    }
}
