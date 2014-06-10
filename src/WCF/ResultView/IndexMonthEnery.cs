using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class IndexMonthEnery
    {
        public ExecuteProcess ActionInfo { get; set; }

        /// <summary>
        /// 本月电
        /// </summary>
        public decimal MonthElectricity { get; set; }
        /// <summary>
        /// 本月水
        /// </summary>
        public decimal MonthWater { get; set; }
        /// <summary>
        /// 本月气
        /// </summary>
        public decimal MonthGas { get; set; }
        /// <summary>
        /// 本月冷暖
        /// </summary>
        public decimal MonthWarm { get; set; }
    }
}
