using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NTS.WEB.Model
{
    [Serializable]
    public class ReportQueryModelNew : ICloneable
    {
        private int _objecttype;
        public string objectid { get; set; } // 统计的对象
        public DateTime startime { get; set; } // 开始时间
        public DateTime endtime { get; set; } // 结束时间
        public ReportStyleNew timeunit { get; set; } // 统计的样式
        public string itemcode { get; set; } // 分类分项的id
        public int objecttype { get { return _objecttype; } set { _objecttype = value; } }//设备类型

        public object Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            object des = bf.Deserialize(ms);
            ms.Close();
            return des;
        }
    }

    public class ReportCompareQueryModelNew
    {
        private int _objecttype;
        public string objectid { get; set; } // 统计的对象
        public DateTime startime1 { get; set; } // 开始时间
        public DateTime endtime1 { get; set; } // 结束时间
        public DateTime startime2 { get; set; } // 开始时间
        public DateTime endtime2 { get; set; } // 结束时间
        public ReportStyle unit { get; set; } // 统计的样式
        public int itemcode { get; set; } // 分类分项的id
        public int objecttype { get { return _objecttype; } set { _objecttype = value; } }//设备类型
    }

    /// <summary>
    /// 统计的时间表现形式
    /// </summary>
    public enum ReportStyleNew
    {
        DayStyle = 1, // 天
        MonthStyle, // 月
        YearStyle, // 年
        SeasonStyle, // 季度报表
        DiyStyle // 自定义查询
    }
}
