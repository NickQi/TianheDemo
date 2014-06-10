using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class IndexWindowResult
    {
        public ExecuteProcess ActionInfo { get; set; }
        /// <summary>
        /// 今日电
        /// </summary>
        public decimal Electricity { get; set; }
        /// <summary>
        /// 今日水
        /// </summary>
        public decimal Water { get; set; }
        /// <summary>
        /// 今日气
        /// </summary>
        public decimal Gas { get; set; }
        /// <summary>
        /// 今日冷暖
        /// </summary>
        public decimal Warm { get; set; }
        /// <summary>
        /// 同比昨天电
        /// </summary>
        public string ComparedElectricity { get; set; }
        /// <summary>
        /// 同比昨天水
        /// </summary>
        public string ComparedWater { get; set; }
        /// <summary>
        /// 同比昨天气
        /// </summary>
        public string ComparedGas { get; set; }
        /// <summary>
        /// 同比昨天冷暖
        /// </summary>
        public string ComparedWarm { get; set; }
    }
}
