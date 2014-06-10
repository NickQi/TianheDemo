using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{

   
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AlarmDiagnoseService : IAlarmDiagnoseService
    {

        [Log(ModelName = "管理诊断")]
        [CustomException]
        public ResultAlarm GetAlarmDiagnose(QueryAlarm query)
        {
            ResultAlarm result = new ResultAlarm();
            var pAction = new ExecuteProcess();
            try
            {
                var alarmdiagnoselist = new NTS.WEB.BLL.AlarmDiagnose().GetAlarmDiagnose(query);
                if (alarmdiagnoselist.Count > 0)
                {
                    pAction.Success = true;
                    result.Rows = alarmdiagnoselist;
                    result.ActionInfo = pAction;
                    return result;
                }
                else
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    result.ActionInfo = pAction;
                    return result;
                }
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                result.ActionInfo = pAction;
                return result;
            }
        }

      
    }
}
