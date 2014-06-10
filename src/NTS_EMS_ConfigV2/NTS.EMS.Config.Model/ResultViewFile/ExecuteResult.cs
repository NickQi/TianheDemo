using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class ExecuteResult
    {
        public bool Success { get; set; }
        public string ExceptionMsg { get; set; }
        public object ExtendContent { get; set; }
        public ExecuteResult()
        {
            ExtendContent = null;
            Success = true;
            ExceptionMsg = string.Empty;
        }
    }
}
