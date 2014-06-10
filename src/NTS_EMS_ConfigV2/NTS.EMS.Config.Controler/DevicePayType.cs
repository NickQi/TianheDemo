using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Collections;

namespace NTS.EMS.Config.Controler
{
    public class DevicePayType
    {
       public Hashtable GetKeyHash()
       {
           var pageData = new Hashtable();
           pageData.Add("PayType", new NTS.EMS.Config.BLL.OperateDevicePayTypeBll().GetPayTypeList(""));
           return pageData;
       }
    }
}


