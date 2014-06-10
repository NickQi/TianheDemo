using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class ResultQuotaInfo
    {
       // public ExecuteProcess ActionInfo { get; set; }
        public Quota QuotaData { get; set; }
        public List<QuotaLog> QuotaLogList { get; set; }
        public Padding Page { get; set; }
    }

    public class ResultQuotaLogs
    {
        //public ExecuteProcess ActionInfo { get; set; }
        public List<QuotaLog> QuotaLogList { get; set; }
        public Padding Page { get; set; }
    }

    public class Quota
    {
        public int QuotaId { get; set; }
        public QuotaType QuotaType { get; set; }
        public DateTime QuotaTime { get; set; }
        public string QuotaTimeStr { get { return QuotaTime.ToShortDateString(); } }
        public float QuotaValue { get; set; }
        public string Reserved { get; set; }
        public int ObjectType { get; set; }
        public int ObjectId { get; set; }
        public string ItemCode { get; set; }
        public string ObjectDesc { get; set; }
    }

    public class QuotaLog
    {
        public int QuotaId { get; set; }
        public string UserName { get; set; }
        public DateTime LogTime { get; set; }
        public string LogTimeStr { get { return LogTime.ToString(); } }
        public string QuotaTimeStr { get; set; }
        public float QuotaValue { get; set; }
        public string Reserved { get; set; }
        public string ObjectDesc { get; set; }
    }

    public enum QuotaType
    {
        Month = 1,
        Year = 2
    }

    public class Padding
    {
        public int Current { get; set; }
        public int Total { get; set; }
    } 
}
