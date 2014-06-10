using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultRealLine
    {
        public ExecuteProcess ActionInfo;

        public string Unit { get; set; }
        /// <summary>
        /// 绑定LineHighChart
        /// </summary>
        public List<EneryAnalyseSeries> series;
    }
}
