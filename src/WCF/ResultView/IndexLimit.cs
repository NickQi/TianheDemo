using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class IndexLimit
    {
        public ExecuteProcess ActionInfo { get; set; }
        public decimal ElectricityHigh { get; set; }
        public string ElectricityHighTime { get; set; }
        public decimal ElectricityLow { get; set; }
        public string ElectricityLowTime { get; set; }
    }
}
