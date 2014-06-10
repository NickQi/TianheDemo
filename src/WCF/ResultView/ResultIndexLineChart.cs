using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultIndexLineChart
    {
        public ExecuteProcess ActionInfo;
        /// <summary>
        /// 时刻点的集合
        /// </summary>
        public List<string> DatePick { get; set; }
        /// <summary>
        /// 时刻点的值集合
        /// </summary>
        public List<decimal> DatePickEnery { get; set; }
    }
}
