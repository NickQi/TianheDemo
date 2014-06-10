using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.BLL
{
    public class BaseTool
    {
        #region 转化为查询的单元
        /// <summary>
        /// 转化为查询的单元
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public  static ChartUnit GetChartUnit(int v)
        {
            switch (v)
            {
                case 0:
                    return ChartUnit.unit_hour;
                case 1:
                    return ChartUnit.unit_day;
                case 2:
                    return ChartUnit.unit_month;
                case 3:
                    return ChartUnit.unit_year;
                default:
                    return ChartUnit.unit_day;
            }
        }
        #endregion
    }
}
