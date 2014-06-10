using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NTS.WEB.Model
{
    [Serializable]
    public class ReportQueryModel : ICloneable
    {
        public string Objectid { get; set; } // 统计的对象
        public DateTime Startime { get; set; } // 开始时间
        public DateTime Endtime { get; set; } // 结束时间
        public ReportStyle Unit { get; set; } // 统计的样式
        public int Itemcode { get; set; } // 分类分项的id
        public int Objecttype { get; set; }//设备类型

        public object Clone()
        {
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            var des = bf.Deserialize(ms);
            ms.Close();
            return des;
        }
    }

    public class ReportCompareQueryModel
    {
        public string Objectid { get; set; } // 统计的对象
        public DateTime Startime1 { get; set; } // 开始时间
        public DateTime Endtime1 { get; set; } // 结束时间
        public DateTime Startime2 { get; set; } // 开始时间
        public DateTime Endtime2 { get; set; } // 结束时间
        public ReportStyle Unit { get; set; } // 统计的样式
        public int Itemcode { get; set; } // 分类分项的id
        public int Objecttype { get; set; } // 设备类型
    }

    /// <summary>
    /// 统计的时间表现形式
    /// </summary>
    public enum ReportStyle
    {
        DayStyle = 1, // 天
        WeekStyle, // 周
        MonthStyle, // 月
        YearStyle, // 年
        DiyStyle // 自定义查询
    }
}
