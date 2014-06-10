using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultAlarmType
    {
        public ExecuteProcess ActionInfo { get; set; }

        public List<AlarmType> ItemLst;
    }

    public class AlarmType
    {
        public string ItemCode;
        public string ItemName;
    }

}
