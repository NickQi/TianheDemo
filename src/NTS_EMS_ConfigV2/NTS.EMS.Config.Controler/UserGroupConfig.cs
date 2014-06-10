using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Collections;

namespace NTS.EMS.Config.Controler
{
    public class UserGroupConfig
    {
       public Hashtable GetKeyHash()
       {
           var pageData = new Hashtable();
           pageData.Add("MenuTree", new NTS.EMS.Config.BLL.OperateUserGroupBll().GetMenuTree());
           return pageData;
       }
    }
}


