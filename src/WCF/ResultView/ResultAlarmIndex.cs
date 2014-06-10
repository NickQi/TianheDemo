using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultAlarmIndex
    {
        public ExecuteProcess ActionInfo;

        public AlarmDayYestoDayComp AllAlarm;

        public AlarmDayYestoDayComp UndoAlarm;

        public AlarmDayYestoDayComp ProcessedAlarm;
    }

    public class AlarmDayYestoDayComp
    {
        public int Value;

        public int YesterdayValue;

        public string CompareValue;

    }
}
