using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 定额告警
    /// </summary>
    public class ResultQuotaAlarm
    {
        public ExecuteResult ResultInfo { get; set; }
        public QuotaAlarmData QuotaAlarm { get; set; }
    }

    /// <summary>
    /// 定额告警列表
    /// </summary>
    public class ResultQuotaAlarmList
    {
        public ExecuteResult ResultInfo { get; set; }
        public List<QuotaAlarmData> QuotaAlarmList { get; set; }
        public Padding Page { get; set; }
    }

    public class QuotaAlarmData
    {
        public int AlarmType { get; set; }//告警类型
        public string AlarmName { get; set; }//告警类型名称
        public int QuotaType { get; set; }//定额类型
        public string QuotaTypeStr { get { return QuotaType == 1 ? "月" : "年"; } }
        public string ItemCode { get; set; }//分类分项
        public string ItemName { get; set; }//分类分项名称
        public int ObjectId { get; set; }//对象Id
        public int ObjectType { get; set; }//对象类型
        public string ObjectDesc { get; set; }//对象名称
        public double Percent { get; set; }//百分比
        public string PercentS { get { return string.Format("{0:F2}", (Percent * 100)); } }
        public int Id { get; set; }//Id
    }
}
