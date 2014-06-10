using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NTS.EMS.Config.Controler
{
   public class Index
    {
       public Hashtable GetKeyHash()
       {
           var pageData = new Hashtable();
           pageData.Add("itemcodeList", new NTS.EMS.Config.BLL.OperateQuotaBll().GetItemcodeList());
           return pageData;
       }
    }
}
