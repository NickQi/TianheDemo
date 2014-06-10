using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultIndexPieChart
    {
        public ExecuteProcess ActionInfo;
        /// <summary>
        /// 分类分项的集合
        /// </summary>
        public List<Model.Itemcode> ItemCode { get; set; }
        /// <summary>
        /// 分项的值集合
        /// </summary>
        public List<decimal> ItemCodeEnery { get; set; }
    }
}
