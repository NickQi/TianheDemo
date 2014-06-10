using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.Base.Data
{
    public class CommDataTool
    {
        /// <summary>
        /// 格式化日期显示的格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="style">显示格式</param>
        /// <returns></returns>
        public  static  string FormatDate(string date, ReportStyle style)
        {
            DateTime Dates = Convert.ToDateTime(date);
            switch (ConvertBaseCountType(style))
            {
                case 0:
                    return Dates.ToString("HH 点");
                case 1:
                    return Dates.ToString("MM-dd");
                default:
                    return Dates.ToString("yyyy-MM 月");
            }
        }

        /// <summary>
        /// 根据前台查询的类型转化为后台对应的实际统计单元
        /// </summary>
        /// <param name="style">前台的统计风格</param>
        /// <returns></returns>
        public static int ConvertBaseCountType(ReportStyle style)
        {
            switch (style)
            {
                case ReportStyle.DayStyle:
                    return 0;
                case ReportStyle.WeekStyle:
                    return 1;
                case ReportStyle.MonthStyle:
                    return 1;
                case ReportStyle.YearStyle:
                    return 4;
                default:
                    return 1;
            }
        }



        #region 基础方法

        public StringBuilder GetTrueCountID(string[] ItemcodeArr, string[] ObjectArr, ReportQueryModel model)
        {
            StringBuilder CountList = new StringBuilder();

            for (int i = 0; i < ObjectArr.Length; i++)
            {
                for (int j = 0; j < ItemcodeArr.Length; j++)
                {
                    int tempid = 0;
                    ReportQueryModel newmodel = model;
                    model.objectid = ObjectArr[i];
                    model.itemcode = int.Parse(ItemcodeArr[j]);
                    if (ObjectIsConfig(newmodel, out tempid))
                    {
                        CountList.AppendFormat(",{0}", tempid.ToString());
                    }
                }
            }
            return CountList;
        }

        /// <summary>
        /// 判断是否配置数据统计
        /// </summary>
        /// <returns></returns>
        public bool ObjectIsConfig(ReportQueryModel model, out int objectid)
        {
            objectid = 0;
            IObjectConfig dal = DataSwitchConfig.CreateObjectConfig();
            SqlParameter[] parameter = {
                                            new SqlParameter("@objectid", SqlDbType.NVarChar),
                                            new SqlParameter("@itemcodeid", SqlDbType.Int),
                                            new SqlParameter("@unit", SqlDbType.Int),
                                            new SqlParameter("@objecttype", SqlDbType.Int)
                                        };
            parameter[0].Value = model.objectid;
            parameter[1].Value = model.itemcode;
            parameter[2].Value = CommDataTool.ConvertBaseCountType(model.unit);
            parameter[3].Value = model.objecttype;
            DataTable becmcountdt = dal.GetList(" objectid=@objectid and objecttype=@objecttype and itemcodeid=@itemcodeid and unit=@unit", "objectid", parameter);

            if (becmcountdt.Rows.Count.Equals(0))
            {
                return false;
            }
            objectid = int.Parse(becmcountdt.Rows[0][0].ToString());
            return true;
        }

      
        #endregion



    }
}
