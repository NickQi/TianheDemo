using System.Collections.Generic;
using System.Data;
using System;
using NTS.WEB.Common;
using NTS.WEB.Model;
namespace NTS.WEB.Common
{
    public class CreateTable
    {
        /// <summary>
        /// 基础的统计对象的虚拟表
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateBaseDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("ObjectID", ColType.NTSString);
            Dir.Add("ObjectName", ColType.NTSString);
            Dir.Add("Starttime", ColType.NTSString);
            Dir.Add("Endtime", ColType.NTSString);
            Dir.Add("ItemCode", ColType.NTSString);
            Dir.Add("EneryValue", ColType.NTSDecimal);
            return TableTool.CreateTable(Dir);
        }


        /// <summary>
        /// 基础的统计对象的虚拟表
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateBaseDataTableAll()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("ObjectID", ColType.NTSString);
            Dir.Add("ObjectName", ColType.NTSString);
            Dir.Add("Starttime", ColType.NTSString);
            Dir.Add("Endtime", ColType.NTSString);
            for (int i = 0; i < 5; i++) // 转换对象个数
            {
                Dir.Add("E" + i.ToString(), ColType.NTSDecimal);
            }
            return TableTool.CreateTable(Dir);
        }




        /// <summary>
        /// 格式化日期显示的格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="style">显示格式</param>
        /// <returns></returns>
        public static string FormatDate(string date, ReportStyleNew style)
        {
            DateTime Dates = Convert.ToDateTime(date);
            switch (ConvertBaseCountType(style))
            {
                case 1:
                    ////Dates = Dates.AddHours(1);
                    return Dates.ToString("HH 点");
                case 2:
                    return Dates.ToString("MM-dd");
                case 3:
                    return Dates.ToString("MM-dd");
                //return Dates.ToString("yyyy-MM 月");
                default:
                    return Dates.ToString("yyyy-MM 月");
            }
        }


        /// <summary>
        /// 格式化日期显示的格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="style">显示格式</param>
        /// <returns></returns>
        public static string ReportFormatDate(string date, ReportStyleNew style)
        {
            DateTime Dates = Convert.ToDateTime(date);
            switch (ConvertBaseCountType(style))
            {
                case 1:
                    ////Dates = Dates.AddHours(1);
                    return Dates.ToString("HH 点");
                case 2:
                    return Dates.ToString("yyyy-MM-dd");
                case 3:
                    return Dates.ToString("yyyy-MM-dd");
                //return Dates.ToString("yyyy-MM 月");
                default:
                    return Dates.ToString("yyyy-MM 月");
            }
        }

        /// <summary>
        /// 根据前台查询的类型转化为后台对应的实际统计单元
        /// </summary>
        /// <param name="style">前台的统计风格</param>
        /// <returns></returns>
        public static int ConvertBaseCountType(ReportStyleNew style)
        {
            switch (style)
            {
                case ReportStyleNew.DayStyle:
                    return 1;
                case ReportStyleNew.MonthStyle:
                    return 2;
                case ReportStyleNew.YearStyle:
                    return 3;
                default:
                    return 1;
            }
        }


        ///// <summary>
        ///// 统计的时间表现形式
        ///// </summary>
        //public enum ReportStyle
        //{
        //    DayStyle = 1, // 天
        //    MonthStyle, // 月
        //    YearStyle, // 年
        //    SeasonStyle, // 季度报表
        //    DiyStyle // 自定义查询
        //}



    }
}
