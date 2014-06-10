using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultDevice
    {
        public ExecuteProcess ActionInfo { get; set; }
        public List<DeviceUnit> DeviceUnitList { get; set; }
    }



    public class DeviceUnit
    {
        public int DeviceID { get; set; }

        public string DeviceName { get; set; }
    }
}
