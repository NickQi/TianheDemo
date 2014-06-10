using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.BLL
{
    public class BalanceAnalysis
    {
        private static string tempItemCodeId;
        private static int? tempareaid;
        private static DateTime? tempmonth;
        private static DataTable tempdt;

        private readonly IBalanceAnalysis dal = DataSwitchConfig.CreateBalanceAnalysis();
        public int GetChildAreaCount(int pageSize, int parentid)
        {
            var count = dal.GetChildAreaCount(parentid);
            return count % pageSize == 0 ? count / pageSize : Convert.ToInt32(count / pageSize) + 1;
        }

        public DataTable GetBalanaceValueByMonth(int pageIndex, int pageSize, string itemcodeid, int areaid, DateTime month, string orderby)
        {


            if (tempdt != null)
            {
                if ((itemcodeid != tempItemCodeId) || (tempareaid != areaid) || (tempmonth != month))
                {//仅分页及排序
                    tempItemCodeId = itemcodeid;
                    tempareaid = areaid;
                    tempmonth = month;
                    tempdt = GetTotalBalanaceValue(itemcodeid, areaid, month);
                }

            }
            else
            {
                tempItemCodeId = itemcodeid;
                tempareaid = areaid;
                tempmonth = month;
                tempdt = GetTotalBalanaceValue(itemcodeid, areaid, month);
            }
            DataTable dtCopy = tempdt.Copy();
            DataView dv = tempdt.DefaultView;
            dv.Sort = orderby;
            dtCopy = dv.ToTable();

            return SetPage(dtCopy, pageIndex, pageSize);
        }

        public DataTable GetTotalBalanaceValue(string itemcodeid, int areaid, DateTime month)
        {
            DataTable dt = new DataTable();
            dt.TableName = "balance";
            dt.Columns.Add("Objectname");
            dt.Columns.Add("Location");
            dt.Columns.Add("Mastervalue", typeof(double));
            dt.Columns.Add("Secondarytotalvalue", typeof(double));
            dt.Columns.Add("Dvalue", typeof(double));
            dt.Columns.Add("Percent");
            dt.Columns.Add("Unit");
            dt.Columns.Add("PercentsOrderBy", typeof(double));
            //DataTable dt_areaid = dal.GetChildAreaList(pageIndex, pageSize, areaid);
            DataTable dt_areaid = dal.GetChildAreaList(areaid);
            if (dt_areaid.Rows.Count > 0)
            {
                for (int i = 0; i <= dt_areaid.Rows.Count - 1; i++)
                {
                    int childareaid = Convert.ToInt32(dt_areaid.Rows[i]["layerobjectid"]);
                    string areaname = dt_areaid.Rows[i]["layerobjectname"].ToString();
                    string location = dt_areaid.Rows[i]["location"].ToString();
                    DataTable dt_value = dal.GetBalanaceValueByMonth(itemcodeid, childareaid, month);
                    if (dt_value.Rows.Count > 0)
                    {
                        dt.Rows.Add(
                            areaname, location,
                            dt_value.Rows[0]["Mastervalue"].ToString().Trim(),
                            dt_value.Rows[0]["Secondarytotalvalue"].ToString().Trim(),
                            dt_value.Rows[0]["Dvalue"].ToString().Trim(),
                            dt_value.Rows[0]["Percents"],
                             dt_value.Rows[0]["unit"],
                             Convert.ToDouble(dt_value.Rows[0]["Percents"].ToString().Replace("%", "")));
                    }
                    else
                    {
                        dt.Rows.Add(areaname, location, 0, 0, 0, "0.00%", "", 0.00);
                    }
                }
            }


            return dt;
        }




        /// <summary>
        /// DateTable的分页操作
        /// </summary>
        /// <param name="dt">要进行分页的DataTable</param>
        /// <param name="currentPageIndex">当前页数</param>
        /// <param name="pageSize">一页显示的条数</param>
        /// <returns>第pageIndex页的数据</returns>
        public static DataTable SetPage(DataTable dt, int currentPageIndex, int pageSize)
        {

            if (currentPageIndex == 0)
            {
                return dt;
            }

            DataTable newdt = dt.Clone();

            int rowbegin = (currentPageIndex - 1) * pageSize;//当前页的第一条数据在dt中的位置
            int rowend = currentPageIndex * pageSize;//当前页的最后一条数据在dt中的位置

            if (rowbegin >= dt.Rows.Count)
            {
                return newdt;
            }

            if (rowend > dt.Rows.Count)
            {
                rowend = dt.Rows.Count;
            }

            DataView dv = dt.DefaultView;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                newdt.ImportRow(dv[i].Row);
            }

            return newdt;
        }
    }
}
