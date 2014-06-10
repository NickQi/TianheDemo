using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model.QueryFile
{
    [DataContract]
    public class QueryAlarmSetting
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }

    [DataContract]
    public class QueryAlarmEventUpdate
    {
        [DataMember]
        public int AlarmTypeId { get; set; }
        [DataMember]
        public int? AlarmLevelId { get; set; }
        [DataMember]
        public List<QueryAlarmEvent> Update { get; set; }
        [DataMember]
        public List<int> Del { get; set; }
    }

    [DataContract]
    public class QueryAlarmEvent
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int PROJECTID { get; set; }

        [DataMember]
        public int SYSTEMID { get; set; }

        [DataMember]
        public int AlarmTypeId { get; set; }
        [DataMember]
        public string PlugIn { get; set; }
        [DataMember]
        public int TrigMode { get; set; }
        [DataMember]
        public int RunMode { get; set; }
        [DataMember]
        public int RunCount { get; set; }
        [DataMember]
        public int RunTime { get; set; }
        [DataMember]
        public string Options { get; set; }
    }

    [DataContract]
    public class QueryAlarmScaleSetting
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int AlarmType { get; set; }

        [DataMember]
        public int Scale { get; set; }
    }
}
