using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
   public class ExecuteProcess
    {
       public bool Success { get; set; }
       public string ExceptionMsg { get; set; }
       public string ActionName{get; set;}
       public string ActionUser { get; set; }
       public DateTime ActionTime { get; set; }
    }
}
