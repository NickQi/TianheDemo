using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultLoadForecastMap
    {
        public ExecuteProcess ActionInfo;

        public List<SerialData> series;

        public string Unit;

        public List<ResultLoadForecastList> LoadForecast;

        public decimal ForeCastTotal;

        public decimal HistoryTotal;
    }

    public class SerialData
    {
        public string name;

        public List<decimal> data;
    }


    /// <summary>
    /// 能耗分析比较结果
    /// </summary>
    public class ResultLoadForecastList
    {
        public int Id;
        public string TimeArea { get; set; }
        public decimal ForeCast { get; set; }
        public decimal History { get; set; }
        public decimal Deviation { get; set; }
        public string Pecent { get; set; }
    }
}
