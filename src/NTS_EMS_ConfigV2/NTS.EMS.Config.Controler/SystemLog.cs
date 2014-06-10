using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Collections;

namespace NTS.EMS.Config.Controler
{
    public class SystemLog
    {
       public Hashtable GetKeyHash()
       {
           var pageData = new Hashtable();
           pageData.Add("MenuList", new NTS.EMS.Config.BLL.OperateUserGroupBll().GetTbMenu(""," order by menuname "));
           pageData.Add("UserList", new NTS.EMS.Config.BLL.OperateUserBll().GetUserListNotPage("order by cname "));
           return pageData;
       }
    }
}


