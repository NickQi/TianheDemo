using System;
namespace NTS.WEB.Common
{
    public class TimeParser
    {
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        #region 返回某年某月最后一天
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        #endregion

        #region 返回时间差
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion

        /// <summary>
        /// 返回当前月的第一天
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetCuttMonthFirstDate()
        {
            DateTime monthFirstDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
            //DateTime dt = System.DateTime.Now;
            return monthFirstDate.ToString("yyyy-MM-dd");
        }

        [AjaxSessionMethod]
        public int MaxIntervals()
        {
            var startTime = Convert.ToDateTime(System.Web.HttpContext.Current.Request["StartTime"]);
            var endTime = Convert.ToDateTime(System.Web.HttpContext.Current.Request["Endtime"]);
            var maxDays = Convert.ToInt32(System.Web.HttpContext.Current.Request["MaxDays"]);
            if (endTime.Subtract(startTime).Days > maxDays)
            {
                return 0;
            }
            return 1;
        }

        [AjaxSessionMethod]
        public string getCuttMonth()
        {
            DateTime mydate = Convert.ToDateTime(System.Web.HttpContext.Current.Request["mydate"]);
            return mydate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            // DateTime monthFirstDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
            //DateTime dt = System.DateTime.Now;
            // return monthFirstDate.ToString("yyyy-MM-dd");
        }


        [AjaxSessionMethod]
        public string GetLastYearMyDate()
        {
            int cyear = int.Parse(System.Web.HttpContext.Current.Request["mydate"]);
            DateTime nowyear = System.DateTime.Now;
            if (nowyear.Month.Equals(2))
            {
                if (!DateTime.IsLeapYear(cyear))
                {
                    return cyear.ToString() + nowyear.ToString("-MM-28");
                }
                return cyear.ToString() + nowyear.ToString("-MM-dd");
            }
            return cyear.ToString() + nowyear.ToString("-MM-dd");
        }

        /// <summary>
        /// 返回当前日期
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetToday()
        {
            DateTime dt = System.DateTime.Now;
            return dt.Date.ToString("yyyy-MM-dd");
        }

        //为了调整整体的显示日期和下面获取日期错误问题 --created by o07csy @2013.8.26
        /// <summary>
        /// 返回昨天日期，也可查询数据最后的一天
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetYestoday()
        {
            DateTime dt = System.DateTime.Now;
            return dt.Date.AddDays(-1).ToString("yyyy-MM-dd");
        }

        [AjaxSessionMethod]
        public string GetTodayZeroHour()
        {
            DateTime dt = System.DateTime.Now;
            return dt.ToString("yyyy-MM-dd 00:00");
        }
        [AjaxSessionMethod]
        public string GetTodayNowHour()
        {
            DateTime dt = System.DateTime.Now;
            return dt.ToString("yyyy-MM-dd HH:00:00");
        }
        [AjaxSessionMethod]
        public string GetNow()
        {
            DateTime dt = System.DateTime.Now;
            return dt.ToString("yyyy-MM-dd HH:00:00");
        }

        /// <summary>
        /// 返回周的首日
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetWeekFirstDay()
        {
            int dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek);
            int daydiff = (-1) * dayOfWeek;
            int dayadd = 7 - dayOfWeek;

            DateTime weekStartDate = DateTime.Now;
            if (daydiff != 0)
            {
                weekStartDate = weekStartDate.AddDays(daydiff + 1);
            }
            else
            {
                weekStartDate = weekStartDate.AddDays(-6);
            }
            return weekStartDate.ToString("yyyy-MM-dd");
            // 
        }

        /// <summary>
        /// 返回周的第几天
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetWeekDay()
        {
            return Convert.ToInt32(DateTime.Now.DayOfWeek).ToString();
        }

        /// <summary>
        /// 返回周的末日
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetWeekLastDay()
        {
            int dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek);
            int daydiff = (-1) * dayOfWeek;
            int dayadd = 7 - dayOfWeek;
            DateTime weekEndDate = DateTime.Now;
            if (daydiff != 0)
            {
                weekEndDate = weekEndDate.AddDays(dayadd);
            }
            return weekEndDate.ToString("yyyy-MM-dd");
        }


        /// <summary>
        /// 获取当月的最后一天
        /// </summary>
        /// <returns></returns>
        [AjaxSessionMethod]
        public string GetMonthLast()
        {
            DateTime now = DateTime.Now;
            DateTime monthFirstDate = new DateTime(now.Year, now.Month, 1);
            DateTime monthLastDate = monthFirstDate.AddMonths(1).AddDays(-1);
            return monthLastDate.ToString("yyyy-MM-dd");
        }


        [AjaxSessionMethod]
        public string GetMyNextMonth()
        {
            DateTime mydate = Convert.ToDateTime(System.Web.HttpContext.Current.Request["mydate"]);
            return mydate.AddMonths(1).AddDays(-1).ToString();
        }

        [AjaxSessionMethod]
        public string getCuttTimeDate()
        {
            DateTime mydate = Convert.ToDateTime(System.Web.HttpContext.Current.Request["mydate"]);
            return mydate.Date.AddDays(1).AddHours(-1).ToString("yyyy-MM-dd HH:00:00");
        }


        [AjaxSessionMethod]
        public string GetCuttYear()
        {
            DateTime now = DateTime.Now;
            return now.ToString("yyyy");
        }

        [AjaxSessionMethod]
        public string GetMyMonth()
        {
            DateTime mydate = Convert.ToDateTime(System.Web.HttpContext.Current.Request["mydate"] + "-1");
            return mydate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }

        [AjaxSessionMethod]
        public string GetMyDate()
        {
            int Hour = DateTime.Now.Hour;
            DateTime mydate = Convert.ToDateTime(System.Web.HttpContext.Current.Request["mydate"]);
            return mydate.AddHours(Hour).ToString();
        }
    }
}
