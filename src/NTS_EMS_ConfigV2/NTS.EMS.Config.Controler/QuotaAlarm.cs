using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Collections;

namespace NTS.EMS.Config.Controler
{
    public class QuotaAlarm
    {
       public Hashtable GetKeyHash()
       {
           var pageData = new Hashtable();
           pageData.Add("AlarmTypes", new NTS.EMS.Config.BLL.OperateQuotaAlarmBll().GetAlarmTypes("QoutaAlarm"));
           return pageData;
       }
    }
}


