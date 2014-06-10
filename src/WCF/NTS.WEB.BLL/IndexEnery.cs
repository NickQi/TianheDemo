using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Common;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.DataContact;
namespace NTS.WEB.BLL
{
    public class IndexEnery
    {
        NTS.WEB.ProductInteface.IReportBase reportBll = NTS.WEB.ProductInteface.DataSwitchConfig.CreateReportBase();
        public ResultView.IndexWindowResult GetItemCodeListByObjectID(QueryIndexWindow query)
        {

            Dictionary<string, BaseResult> Res = new Dictionary<string, BaseResult>();
            IndexWindowResult Results = new IndexWindowResult();
            string[] ItemCodeStr = { "01000", "02000", "03000", "04000" };//daixy 该处需要修改
            DateTime[] Dates = { query.StatisticsDate, query.StatisticsDate.AddDays(-1) };
            List<Model.BaseLayerObject> queryObjectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectID={0}", query.BuildingNumber), " order by LayerObjectID");
            var queryObject = new Model.BaseLayerObject();
            if (queryObjectList.Count > 0)
            {
                queryObject = queryObjectList[0];
            }
            else
            {
                return null;
            }
            BaseQueryModel model1 = new BaseQueryModel();
            try
            {
                BaseQueryModel model = new BaseQueryModel();
                model.IsDevice = 0;
                model.ObjectList = new List<int>() { query.BuildingNumber };
                foreach (var i in ItemCodeStr)
                {
                    foreach (var d in Dates)
                    {
                        model.ItemCode = i;
                        model.Unit = ChartUnit.unit_day;
                        model.Starttime = d;
                        model.Endtime = d;
                        model1 = new BaseQueryModel();
                        model1.ItemCode = i;
                        model1.Unit = ChartUnit.unit_day;
                        model1.Starttime = d;
                        model1.Endtime = d;
                        Res.Add(i + "_" + d, reportBll.GetBaseEneryDataList(model));
                    }
                }
                if (Res.Count > 0)
                {
                    Results.Electricity = Res[ItemCodeStr[0] + "_" + Dates[0]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()].Total;
                    Results.Water = Res[ItemCodeStr[1] + "_" + Dates[0]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()].Total;
                    Results.Gas = Res[ItemCodeStr[2] + "_" + Dates[0]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()].Total;
                    Results.Warm = Res[ItemCodeStr[3] + "_" + Dates[0]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()].Total;
                    if (
                        Res[ItemCodeStr[0] + "_" + Dates[1]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()]
                            .Total == 0)
                    {
                        Results.ComparedElectricity = "-";
                    }
                    else
                    {
                        Results.ComparedElectricity =
                            decimal.Round(
                                100 * (Results.Electricity -
                                 Res[ItemCodeStr[0] + "_" + Dates[1]].BaseLayerObjectResults[
                                     queryObject.LayerObjectID.ToString()].Total) /
                                Res[ItemCodeStr[0] + "_" + Dates[1]].BaseLayerObjectResults[
                                    queryObject.LayerObjectID.ToString()].Total, 2).ToString() + "%";
                    }
                    if (Res[ItemCodeStr[1] + "_" + Dates[1]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()]
                            .Total == 0)
                    {
                        Results.ComparedWater = "-";
                    }
                    else
                    {
                        Results.ComparedWater =
                            decimal.Round(
                                100 * (Results.Water -
                                 Res[ItemCodeStr[1] + "_" + Dates[1]].BaseLayerObjectResults[
                                     queryObject.LayerObjectID.ToString()].Total) /
                                Res[ItemCodeStr[1] + "_" + Dates[1]].BaseLayerObjectResults[
                                    queryObject.LayerObjectID.ToString()].Total, 2).ToString() + "%";
                    }
                    if (Res[ItemCodeStr[2] + "_" + Dates[1]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()]
                            .Total == 0)
                    {
                        Results.ComparedGas = "-";
                    }
                    else
                    {
                        Results.ComparedGas =
                            decimal.Round(
                                100 * (Results.Gas -
                                 Res[ItemCodeStr[2] + "_" + Dates[1]].BaseLayerObjectResults[
                                     queryObject.LayerObjectID.ToString()].Total) /
                                Res[ItemCodeStr[2] + "_" + Dates[1]].BaseLayerObjectResults[
                                    queryObject.LayerObjectID.ToString()].Total, 2).ToString() + "%";
                    }
                    if (Res[ItemCodeStr[3] + "_" + Dates[1]].BaseLayerObjectResults[queryObject.LayerObjectID.ToString()]
                            .Total == 0)
                    {
                        Results.ComparedWarm = "-";
                    }
                    else
                    {
                        Results.ComparedWarm =
                            decimal.Round(
                                100 * (Results.Warm -
                                 Res[ItemCodeStr[3] + "_" + Dates[1]].BaseLayerObjectResults[
                                     queryObject.LayerObjectID.ToString()].Total) /
                                Res[ItemCodeStr[3] + "_" + Dates[1]].BaseLayerObjectResults[
                                    queryObject.LayerObjectID.ToString()].Total, 2).ToString() + "%";
                    }
                    return Results;
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            return null;
        }


        public ResultView.IndexMonthEnery GetMonthItemCodeList(DateTime startTime, DateTime endTime)
        {
            IndexMonthEnery MonthResult = new IndexMonthEnery();
            var ObjectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectParentID='{0}'", 0), " order by LayerObjectID");
            string[] ItemCodeStr = { "01000", "02000", "03000", "04000" };
            Dictionary<string, decimal> SumMonth = new Dictionary<string, decimal>();
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = (from p in ObjectList select p.LayerObjectID).ToList<int>();
            foreach (var item in ItemCodeStr)
            {
                decimal tempCount = 0;
                model.ItemCode = item;
                model.Unit = ChartUnit.unit_month;
                model.Starttime = startTime;
                model.Endtime = endTime;
                var ResList = reportBll.GetBaseEneryDataList(model);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    tempCount += r.Value.Total;
                }
                SumMonth.Add(item, tempCount);
            }
            if (SumMonth.Count > 0)
            {
                MonthResult.MonthElectricity = SumMonth[ItemCodeStr[0]];
                MonthResult.MonthWater = SumMonth[ItemCodeStr[1]];
                MonthResult.MonthGas = SumMonth[ItemCodeStr[2]];
                MonthResult.MonthWarm = SumMonth[ItemCodeStr[3]];
                return MonthResult;
            }
            return null;
        }


        public decimal[] GetIndexCompareEnery(DateTime startTime, DateTime endTime)
        {
            decimal[] MonthResult = new decimal[2];
            //            string userid = "";
            //            try
            //            {
            //                userid = Utils.GetCookie("userid");
            //            }
            //            catch
            //            {
            //                userid = "";
            //            }
            //            var ObjectList = new List<Model.BaseLayerObject>();
            //            string where = "";
            //            if (string.IsNullOrEmpty(userid))
            //            {
            //                where = string.Format(@"and LayerObjectParentID={0} ", 0);
            //            }
            //            else
            //            {
            //                where = string.Format(@"and LayerObjectParentID={0} AND layerobjectid in( SELECT  AREAID from TB_USERGROUPAREARIGHT 
            //WHERE USERGROUPID =(SELECT GROUPS FROM TB_USER WHERE ID={1} ))", 0, userid);
            //            }

            //            ObjectList = new DAL.BaseLayerObject().GetBaseLayerObjectList(where, string.Format(" order by LayerObjectID"));

            var ObjectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectParentID={0}", 0), " order by LayerObjectID");
            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = (from p in ObjectList select p.LayerObjectID).ToList<int>();
            decimal tempConvert = 0;
            foreach (var item in ItemCodeStr)
            {
                decimal tempCount = 0;
                decimal tempCountConvert = 0;
                model.ItemCode = item;
                model.Unit = ChartUnit.unit_month;
                model.Starttime = startTime;
                model.Endtime = endTime;
                var ResList = reportBll.GetBaseEneryDataList(model);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    tempCount += r.Value.Total;
                    tempCountConvert += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                }
                if (item == "01000")
                {
                    MonthResult[0] = tempCount;
                }
                tempConvert += tempCountConvert;
            }
            MonthResult[1] = tempConvert;
            return MonthResult;
        }

        public MainInfo GetIndexCompareEneryNew_OLD(DateTime startTime, DateTime endTime)
        {
            MainInfo mainInfo = new MainInfo();
            mainInfo.IsOnlyElec = false;
            mainInfo.ItemValues = new List<EneryStatistic>();
            mainInfo.PeriodValues = new List<EnerySum>();
            var ObjectList = new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and LayerObjectParentID={0}", 0), " order by LayerObjectID");
            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            //var ItemList = new BLL.Itemcode().GetItemcodeList("", " order by ItemcodeID");

            EneryStatistic eneryStatistic4Elec = new EneryStatistic();
            if (ItemList.Count == 1)
            {//只有电
                mainInfo.IsOnlyElec = true;//只有电
                eneryStatistic4Elec.CName = ItemList[0].ItemCodeName;
                eneryStatistic4Elec.ItemCode = ItemList[0].ItemCodeNumber;
                eneryStatistic4Elec.Unit = ItemList[0].Unit;
                ItemList = new BLL.Itemcode().GetItemcodeList(string.Format(" and ParentID={0}", ItemList[0].ItemcodeID), " order by ItemcodeID");
            }
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = (from p in ObjectList select p.LayerObjectID).ToList<int>();

            foreach (var item in ItemList)
            {
                EneryStatistic eneryStatistic = new EneryStatistic();
                eneryStatistic.CName = item.ItemCodeName;
                eneryStatistic.ItemCode = item.ItemCodeNumber;
                eneryStatistic.Unit = item.Unit;

                model.ItemCode = item.ItemCodeNumber;

                model.Unit = ChartUnit.unit_month;
                model.Starttime = startTime;
                model.Endtime = endTime;
                var ResList = reportBll.GetBaseEneryDataList(model, true);
                if (ResList == null)
                {
                    continue;
                }
                decimal tempItemCoalEneryValue = 0;
                decimal tempLastMonthItemCoalEneryValue = 0;
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    eneryStatistic.EneryValue += r.Value.Total;
                    eneryStatistic.EnergyValue2Coal += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                    tempItemCoalEneryValue += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                }
                eneryStatistic.EneryValue = decimal.Round(eneryStatistic.EneryValue, 2);
                eneryStatistic.EnergyValue2Coal = decimal.Round(eneryStatistic.EnergyValue2Coal, 2);
                tempItemCoalEneryValue = decimal.Round(tempItemCoalEneryValue, 2);
                model.Starttime = startTime.AddMonths(-1);
                model.Endtime = endTime.AddMonths(-1);
                ResList = reportBll.GetBaseEneryDataList(model, true);
                if (ResList == null)
                {
                    continue;
                }
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    eneryStatistic.EnergyLastMonth += decimal.Round(r.Value.Total, 2);
                    tempLastMonthItemCoalEneryValue += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                }
                eneryStatistic.EnergyLastMonth = decimal.Round(eneryStatistic.EnergyLastMonth, 2);
                tempLastMonthItemCoalEneryValue = decimal.Round(tempLastMonthItemCoalEneryValue, 2);
                if (eneryStatistic.EnergyLastMonth > 0)
                {
                    eneryStatistic.MonthCompare = decimal.Round(100 * (eneryStatistic.EneryValue - eneryStatistic.EnergyLastMonth) / eneryStatistic.EnergyLastMonth, 2)
                                 .ToString(CultureInfo.InvariantCulture) + "%";
                }

                mainInfo.Total += Math.Round(tempItemCoalEneryValue, 2);
                mainInfo.TotalLastMon += Math.Round(tempLastMonthItemCoalEneryValue, 2);

                if (mainInfo.IsOnlyElec)
                {
                    eneryStatistic4Elec.EneryValue += eneryStatistic.EneryValue;
                    eneryStatistic4Elec.EnergyValue2Coal += eneryStatistic.EneryValue;
                    eneryStatistic4Elec.EnergyLastMonth += eneryStatistic.EnergyLastMonth;
                }
                mainInfo.ItemValues.Add(eneryStatistic);

                if (item.ItemCodeNumber == "01000")
                {
                    GeneratePeriodValues(model, eneryStatistic, mainInfo);
                }

            }

            if (mainInfo.TotalLastMon > 0)
            {
                mainInfo.TotalCompare = decimal.Round(100 * (mainInfo.Total - mainInfo.TotalLastMon) / mainInfo.TotalLastMon, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            if (mainInfo.IsOnlyElec)
            {
                eneryStatistic4Elec.MonthCompare = mainInfo.TotalCompare;
                mainInfo.ItemValues.Insert(0, eneryStatistic4Elec);
                model.ItemCode = "01000";
                GeneratePeriodValues(model, eneryStatistic4Elec, mainInfo);
            }
            return mainInfo;
        }

        public MainInfo GetIndexCompareEneryNew(DateTime startTime, DateTime endTime)
        {
            MainInfo mainInfo = new MainInfo();
            mainInfo.IsOnlyElec = false;
            mainInfo.ItemValues = new List<EneryStatistic>();
            mainInfo.PeriodValues = new List<EnerySum>();
            var ObjectList = new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and LayerObjectParentID={0}", 0), " order by LayerObjectID");
            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            //var ItemList = new BLL.Itemcode().GetItemcodeList("", " order by ItemcodeID");

            EneryStatistic eneryStatistic4Elec = new EneryStatistic();
            if (ItemList.Count == 1)
            {//只有电
                mainInfo.IsOnlyElec = true;//只有电
                eneryStatistic4Elec.CName = ItemList[0].ItemCodeName;
                eneryStatistic4Elec.ItemCode = ItemList[0].ItemCodeNumber;
                eneryStatistic4Elec.Unit = ItemList[0].Unit;
                ItemList = new BLL.Itemcode().GetItemcodeList(string.Format(" and ParentID={0}", ItemList[0].ItemcodeID), " order by ItemcodeID");
            }
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = (from p in ObjectList select p.LayerObjectID).ToList<int>();

            foreach (var item in ItemList)
            {
                EneryStatistic eneryStatistic = new EneryStatistic();
                eneryStatistic.CName = item.ItemCodeName;
                eneryStatistic.ItemCode = item.ItemCodeNumber;
                eneryStatistic.Unit = item.Unit;

                model.ItemCode = item.ItemCodeNumber;
                decimal tempItemCoalEneryValue = 0;
                decimal tempLastMonthItemCoalEneryValue = 0;
                BaseResult ResList = new BaseResult();
                if ((endTime - startTime).Days > 0)
                {
                    model.Unit = ChartUnit.unit_day;
                    model.Starttime = startTime;
                    model.Endtime = DateTime.Parse(endTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
                    ResList = reportBll.GetBaseEneryDataList(model, true);
                    if (ResList == null)
                    {
                        continue;
                    }
                    foreach (var r in ResList.BaseLayerObjectResults)
                    {
                        eneryStatistic.EneryValue += r.Value.Total;
                        eneryStatistic.EnergyValue2Coal += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                        tempItemCoalEneryValue += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                    }
                }
                if (endTime.Hour > 0)
                {
                    model.Unit = ChartUnit.unit_hour;
                    model.Starttime = DateTime.Parse(endTime.ToString("yyyy-MM-dd 00:00:00"));
                    model.Endtime = DateTime.Parse(endTime.ToString("yyyy-MM-dd HH:00:00"));
                    ResList = reportBll.GetBaseEneryDataList(model, true);
                    if (ResList == null)
                    {
                        continue;
                    }
                    foreach (var r in ResList.BaseLayerObjectResults)
                    {
                        eneryStatistic.EneryValue += r.Value.Total;
                        eneryStatistic.EnergyValue2Coal += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                        tempItemCoalEneryValue += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                    }
                }


                eneryStatistic.EneryValue = decimal.Round(eneryStatistic.EneryValue, 2);
                eneryStatistic.EnergyValue2Coal = decimal.Round(eneryStatistic.EnergyValue2Coal, 2);
                tempItemCoalEneryValue = decimal.Round(tempItemCoalEneryValue, 2);

                model.Starttime = startTime.AddMonths(-1);
                model.Endtime = endTime.AddMonths(-1);

                if ((endTime - startTime).Days > 0)
                {
                    model.Unit = ChartUnit.unit_day;
                    model.Endtime = DateTime.Parse(model.Endtime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
                    ResList = reportBll.GetBaseEneryDataList(model, true);
                    if (ResList == null)
                    {
                        continue;
                    }
                    foreach (var r in ResList.BaseLayerObjectResults)
                    {
                        eneryStatistic.EnergyLastMonth += decimal.Round(r.Value.Total, 2);
                        tempLastMonthItemCoalEneryValue += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                    }
                }
                if (endTime.Hour > 0)
                {
                    model.Unit = ChartUnit.unit_hour;
                    model.Starttime = DateTime.Parse(endTime.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00"));
                    model.Endtime = DateTime.Parse(endTime.AddMonths(-1).ToString("yyyy-MM-dd HH:00:00"));
                    ResList = reportBll.GetBaseEneryDataList(model, true);
                    if (ResList == null)
                    {
                        continue;
                    }
                    foreach (var r in ResList.BaseLayerObjectResults)
                    {
                        eneryStatistic.EnergyLastMonth += decimal.Round(r.Value.Total, 2);
                        tempLastMonthItemCoalEneryValue += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                    }
                }

                eneryStatistic.EnergyLastMonth = decimal.Round(eneryStatistic.EnergyLastMonth, 2);
                tempLastMonthItemCoalEneryValue = decimal.Round(tempLastMonthItemCoalEneryValue, 2);
                if (eneryStatistic.EnergyLastMonth > 0)
                {
                    eneryStatistic.MonthCompare = decimal.Round(100 * (eneryStatistic.EneryValue - eneryStatistic.EnergyLastMonth) / eneryStatistic.EnergyLastMonth, 2)
                                 .ToString(CultureInfo.InvariantCulture) + "%";
                }

                mainInfo.Total += Math.Round(tempItemCoalEneryValue, 2);
                mainInfo.TotalLastMon += Math.Round(tempLastMonthItemCoalEneryValue, 2);

                if (mainInfo.IsOnlyElec)
                {
                    eneryStatistic4Elec.EneryValue += eneryStatistic.EneryValue;
                    eneryStatistic4Elec.EnergyValue2Coal += eneryStatistic.EneryValue;
                    eneryStatistic4Elec.EnergyLastMonth += eneryStatistic.EnergyLastMonth;
                }
                mainInfo.ItemValues.Add(eneryStatistic);

                if (item.ItemCodeNumber == "01000")
                {

                    GeneratePeriodValues(model, mainInfo);
                }

            }

            if (mainInfo.TotalLastMon > 0)
            {
                mainInfo.TotalCompare = decimal.Round(100 * (mainInfo.Total - mainInfo.TotalLastMon) / mainInfo.TotalLastMon, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            if (mainInfo.IsOnlyElec)
            {
                eneryStatistic4Elec.MonthCompare = mainInfo.TotalCompare;
                mainInfo.ItemValues.Insert(0, eneryStatistic4Elec);
                model.ItemCode = "01000";
                GeneratePeriodValues(model, mainInfo);
            }
            return mainInfo;
        }

        private void GeneratePeriodValues(BaseQueryModel model, EneryStatistic eneryStatistic, MainInfo mainInfo)
        {

            decimal lastmonthEnergyValue = 0;
            model.Unit = ChartUnit.unit_month;
            var ResList = reportBll.GetBaseEneryDataList(model, true);
            if (ResList != null)
            {
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    lastmonthEnergyValue += decimal.Round(r.Value.Total, 2);
                }
            }


            #region 电能耗综合评价 上月日均用电量PK昨日用电量
            //取日的
            EnerySum enerySum = new EnerySum();
            enerySum.PeriodType = 1;//日
            //上月日均用电量:
            enerySum.Value1 =
                (eneryStatistic.EnergyLastMonth / System.DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            model.Unit = ChartUnit.unit_day;
            model.Starttime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));//昨天
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                //昨日用电量  
                enerySum.Value2 += r.Value.Total;
            }
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                //环比
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

            #region 电能耗综合评价 上月周均用电量PK上周用电量
            //取周的
            enerySum = new EnerySum();
            enerySum.PeriodType = 2;//周
            //上月周均用电量:
            enerySum.Value1 =
                (eneryStatistic.EnergyLastMonth * 7 / DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            model.Unit = ChartUnit.unit_day;
            //上周一:
            DateTime lastMonday = getWeekUpOfDate(DateTime.Now, DayOfWeek.Monday, -1);
            //循环获取上周一至周日的能耗值
            for (int i = 0; i <= 6; i++)
            {
                model.Starttime = DateTime.Parse(lastMonday.ToString("yyyy-MM-dd")); ;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    //上周用电量
                    enerySum.Value2 += r.Value.Total;
                }
                lastMonday = lastMonday.AddDays(1);

            }
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                //环比
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

            #region 电能耗综合评价 去年同月用电量PK本月用电量（精确到小时）
            //取月的，//本月用电量:取法为取昨天之前每天用电量之和加上今天当前时间之前每小时用电量
            enerySum = new EnerySum();
            enerySum.PeriodType = 3;//日
            int tempcount = DateTime.Now.Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //当前月的一号
            DateTime temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    //本月用电量:取法为取昨天之前每天用电量之和加上今天当前时间之前每小时用电量
                    enerySum.Value2 += r.Value.Total;//上周用电量  
                }
                temptime = temptime.AddDays(1);
            }

            model.Unit = ChartUnit.unit_hour;
            //当前时间零点
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            model.Starttime = temptime;
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                if (r.Value.Datas.Count > 0)
                {
                    foreach (var data in r.Value.Datas)
                    {
                        if (Convert.ToDateTime(data.DatePick).Hour < DateTime.Now.Hour)
                        {
                            //今天当前时间之前每小时用电量累加
                            enerySum.Value2 += data.DataValue;//
                        }
                    }
                }
            }

            //去年同月用电量

            tempcount = DateTime.Now.AddYears(-1).Day - 1;
            model.Unit = ChartUnit.unit_day;
            //去年当月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;
                }
                temptime = temptime.AddDays(1);
            }

            model.Unit = ChartUnit.unit_hour;
            //去年同时时间零点
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd 00:00:00"));
            model.Starttime = temptime;
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                if (r.Value.Datas.Count > 0)
                {
                    foreach (var data in r.Value.Datas)
                    {
                        if (Convert.ToDateTime(data.DatePick).Hour < DateTime.Now.Hour)
                        {
                            enerySum.Value1 += data.DataValue;
                        }
                    }
                }
            }
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);

            #endregion

            #region 电能耗综合评价 去年同期用电PK今年截止本日用电量（精确到天）
            //取年的，//今年截止本日用电量:取法为取这个月之前每月用电量之和加上当前时间之前每天用电量
            enerySum = new EnerySum();
            enerySum.PeriodType = 4;//年
            tempcount = DateTime.Now.Month - 1;//前几月
            model.Unit = ChartUnit.unit_month;
            //当前时间的一月一号
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value2 += r.Value.Total;
                }
                temptime = temptime.AddMonths(1);
            }

            tempcount = DateTime.Now.Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //当前月的一号
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value2 += r.Value.Total;
                }
                temptime = temptime.AddDays(1);
            }

            //去年同期用电量

            tempcount = DateTime.Now.AddYears(-1).Month - 1;
            model.Unit = ChartUnit.unit_month;
            //去年一月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;
                }
                temptime = temptime.AddMonths(1);
            }


            tempcount = DateTime.Now.AddYears(-1).Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //去年同期月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;//上周用电量  
                }
                temptime = temptime.AddDays(1);
            }
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

        }
        private void GeneratePeriodValues_OLD(BaseQueryModel model, MainInfo mainInfo)
        {

            decimal lastmonthEnergyValue = 0;
            model.Unit = ChartUnit.unit_month;
            model.Starttime = DateTime.Parse(model.Starttime.ToString("yyyy-MM-01"));
            model.Endtime = DateTime.Parse(model.Endtime.ToString("yyyy-MM-01"));
            var ResList = reportBll.GetBaseEneryDataList(model, true);
            if (ResList != null)
            {
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    lastmonthEnergyValue += decimal.Round(r.Value.Total, 2);
                }
            }


            #region 电能耗综合评价 上月日均用电量PK昨日用电量
            //取日的
            EnerySum enerySum = new EnerySum();
            enerySum.PeriodType = 1;//日
            //上月日均用电量:
            enerySum.Value1 =
                (lastmonthEnergyValue / System.DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            model.Unit = ChartUnit.unit_day;
            model.Starttime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));//昨天
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                //昨日用电量  
                enerySum.Value2 += r.Value.Total;
            }
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                //环比
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

            #region 电能耗综合评价 上月周均用电量PK上周用电量
            //取周的
            enerySum = new EnerySum();
            enerySum.PeriodType = 2;//周
            //上月周均用电量:
            enerySum.Value1 =
                (lastmonthEnergyValue * 7 / DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            model.Unit = ChartUnit.unit_day;
            //上周一:
            DateTime lastMonday = getWeekUpOfDate(DateTime.Now, DayOfWeek.Monday, -1);
            //循环获取上周一至周日的能耗值
            for (int i = 0; i <= 6; i++)
            {
                model.Starttime = DateTime.Parse(lastMonday.ToString("yyyy-MM-dd")); ;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    //上周用电量
                    enerySum.Value2 += r.Value.Total;
                }
                lastMonday = lastMonday.AddDays(1);

            }
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                //环比
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

            #region 电能耗综合评价 去年同月用电量PK本月用电量（精确到小时）
            //取月的，//本月用电量:取法为取昨天之前每天用电量之和加上今天当前时间之前每小时用电量
            enerySum = new EnerySum();
            enerySum.PeriodType = 3;//日
            int tempcount = DateTime.Now.Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //当前月的一号
            DateTime temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    //本月用电量:取法为取昨天之前每天用电量之和加上今天当前时间之前每小时用电量
                    enerySum.Value2 += r.Value.Total;//上周用电量  
                }
                temptime = temptime.AddDays(1);
            }

            model.Unit = ChartUnit.unit_hour;
            //当前时间零点
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            model.Starttime = temptime;
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                if (r.Value.Datas.Count > 0)
                {
                    foreach (var data in r.Value.Datas)
                    {
                        if (Convert.ToDateTime(data.DatePick).Hour < DateTime.Now.Hour)
                        {
                            //今天当前时间之前每小时用电量累加
                            enerySum.Value2 += data.DataValue;//
                        }
                    }
                }
            }

            //去年同月用电量

            tempcount = DateTime.Now.AddYears(-1).Day - 1;
            model.Unit = ChartUnit.unit_day;
            //去年当月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;
                }
                temptime = temptime.AddDays(1);
            }

            model.Unit = ChartUnit.unit_hour;
            //去年同时时间零点
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd 00:00:00"));
            model.Starttime = temptime;
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                if (r.Value.Datas.Count > 0)
                {
                    foreach (var data in r.Value.Datas)
                    {
                        if (Convert.ToDateTime(data.DatePick).Hour < DateTime.Now.Hour)
                        {
                            enerySum.Value1 += data.DataValue;
                        }
                    }
                }
            }
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);

            #endregion

            #region 电能耗综合评价 去年同期用电PK今年截止本日用电量（精确到天）
            //取年的，//今年截止本日用电量:取法为取这个月之前每月用电量之和加上当前时间之前每天用电量
            enerySum = new EnerySum();
            enerySum.PeriodType = 4;//年
            tempcount = DateTime.Now.Month - 1;//前几月
            model.Unit = ChartUnit.unit_month;
            //当前时间的一月一号
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value2 += r.Value.Total;
                }
                temptime = temptime.AddMonths(1);
            }

            tempcount = DateTime.Now.Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //当前月的一号
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value2 += r.Value.Total;
                }
                temptime = temptime.AddDays(1);
            }

            //去年同期用电量

            tempcount = DateTime.Now.AddYears(-1).Month - 1;
            model.Unit = ChartUnit.unit_month;
            //去年一月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;
                }
                temptime = temptime.AddMonths(1);
            }


            tempcount = DateTime.Now.AddYears(-1).Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //去年同期月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-01"));
            for (int i = 0; i <= tempcount - 1; i++)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime;
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;//上周用电量  
                }
                temptime = temptime.AddDays(1);
            }
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

        }

        private void GeneratePeriodValues(BaseQueryModel model, MainInfo mainInfo)
        {

            decimal lastmonthEnergyValue = 0;
            model.Unit = ChartUnit.unit_month;
            model.Starttime = DateTime.Parse(model.Starttime.ToString("yyyy-MM-01"));
            model.Endtime = DateTime.Parse(model.Endtime.ToString("yyyy-MM-01"));
            var ResList = reportBll.GetBaseEneryDataList(model, true);
            if (ResList != null)
            {
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    lastmonthEnergyValue += decimal.Round(r.Value.Total, 2);
                }
            }


            #region 电能耗综合评价 上月日均用电量PK昨日用电量
            //取日的
            EnerySum enerySum = new EnerySum();
            enerySum.PeriodType = 1;//日
            //上月日均用电量:
            enerySum.Value1 =
                (lastmonthEnergyValue / System.DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            model.Unit = ChartUnit.unit_day;
            model.Starttime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));//昨天
            model.Endtime = model.Starttime;
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                //昨日用电量  
                enerySum.Value2 += r.Value.Total;
            }
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                //环比
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

            #region 电能耗综合评价 上月周均用电量PK上周用电量
            //取周的
            enerySum = new EnerySum();
            enerySum.PeriodType = 2;//周
            //上月周均用电量:
            enerySum.Value1 =
                (lastmonthEnergyValue * 7 / DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            model.Unit = ChartUnit.unit_day;
            //上周一:
            DateTime lastMonday = getWeekUpOfDate(DateTime.Now, DayOfWeek.Monday, -1);
            model.Starttime = DateTime.Parse(lastMonday.ToString("yyyy-MM-dd"));
            model.Endtime = model.Starttime.AddDays(6);
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                //上周用电量
                enerySum.Value2 += r.Value.Total;
            }
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                //环比
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

            #region 电能耗综合评价 去年同月用电量PK本月用电量（精确到小时）
            //取月的，//本月用电量:取法为取昨天之前每天用电量之和加上今天当前时间之前每小时用电量
            enerySum = new EnerySum();
            enerySum.PeriodType = 3;//日
            int tempcount = DateTime.Now.Day - 1;//前几天
            //当前月的一号
            DateTime temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            if (tempcount > 0)
            {
                model.Unit = ChartUnit.unit_day;
                model.Starttime = temptime;
                model.Endtime = model.Starttime.AddDays(tempcount - 1);
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    //本月用电量:取法为取昨天之前每天用电量之和加上今天当前时间之前每小时用电量
                    enerySum.Value2 += r.Value.Total;//上周用电量  
                }
            }
            //-----------------------
            model.Unit = ChartUnit.unit_hour;
            //当前时间零点
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            model.Starttime = temptime;
            model.Endtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                enerySum.Value2 += r.Value.Total;
            }
            //-----------------------
            //去年同月用电量
            tempcount = DateTime.Now.AddYears(-1).Day - 1;
            if (tempcount > 0)
            {
                model.Unit = ChartUnit.unit_day;
                //去年当月一号
                temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-01"));
                model.Starttime = temptime;
                model.Endtime = model.Starttime.AddDays(tempcount - 1);

                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;
                }
            }
            //-----------------------

            model.Unit = ChartUnit.unit_hour;
            //去年同时时间零点
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd 00:00:00"));
            model.Starttime = temptime;
            model.Endtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
            ResList = reportBll.GetBaseEneryDataList(model, true);
            foreach (var r in ResList.BaseLayerObjectResults)
            {
                enerySum.Value1 = r.Value.Total;     
            }
            //-----------------------
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);

            #endregion

            #region 电能耗综合评价 去年同期用电PK今年截止本日用电量（精确到天）
            //取年的，//今年截止本日用电量:取法为取这个月之前每月用电量之和加上当前时间之前每天用电量
            enerySum = new EnerySum();
            enerySum.PeriodType = 4;//年
            tempcount = DateTime.Now.Month - 1;//前几月
            model.Unit = ChartUnit.unit_month;
            //当前时间的一月一号
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
            if (tempcount>0)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime.AddMonths(tempcount-1);
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value2 += r.Value.Total;
                }
            }
            //-----------------------
            tempcount = DateTime.Now.Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //当前月的一号
            temptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            if (tempcount>0)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime.AddDays(tempcount-1);
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value2 += r.Value.Total;
                }
            }
            //-----------------------

            //去年同期用电量

            tempcount = DateTime.Now.AddYears(-1).Month - 1;
            model.Unit = ChartUnit.unit_month;
            //去年一月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-01-01"));
            if (tempcount>0)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime.AddMonths(tempcount-1);
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;
                }
            }

            //-----------------------

            tempcount = DateTime.Now.AddYears(-1).Day - 1;//前几天
            model.Unit = ChartUnit.unit_day;
            //去年同期月一号
            temptime = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy-MM-01"));
            if (tempcount>0)
            {
                model.Starttime = temptime;
                model.Endtime = model.Starttime.AddDays(tempcount-1);
                ResList = reportBll.GetBaseEneryDataList(model, true);
                foreach (var r in ResList.BaseLayerObjectResults)
                {
                    enerySum.Value1 += r.Value.Total;//上周用电量  
                }
            }
            //-----------------------
            enerySum.Value1 = decimal.Round(enerySum.Value1, 2);
            enerySum.Value2 = decimal.Round(enerySum.Value2, 2);
            if (enerySum.Value1 > 0)
            {
                enerySum.MonthCompare = decimal.Round(100 * (enerySum.Value2 - enerySum.Value1) / enerySum.Value1, 2)
                             .ToString(CultureInfo.InvariantCulture) + "%";
            }
            mainInfo.PeriodValues.Add(enerySum);
            #endregion

        }
        private string GetShopLevel(int level)
        {
            switch (level)
            {
                case 2:
                    return "建筑";
                case 3:
                    return "楼层";
                case 4:
                    return "商铺";
                default:
                    return "商铺";
            }

        }
        private DateTime getWeekUpOfDate(DateTime dt, DayOfWeek weekday, int Number)
        {
            int wd1 = (int)weekday;
            int wd2 = (int)dt.DayOfWeek;
            if (wd2 == 0)
            {
                wd2 = 7;
            }
            return wd2 == wd1 ? dt.AddDays(7 * Number) : dt.AddDays(7 * Number - wd2 + wd1);
        }
        public IndexShopOrder GetIndexShopOrder(DateTime startTime, DateTime endTime)
        {
            var shopOrder = new IndexShopOrder
                {
                    TotalEneryOrderList = new List<EneryOrder>(),
                    AreaEneryOrderList = new List<EneryOrder>()
                };
            var shopOrderLast = new IndexShopOrder
            {
                TotalEneryOrderList = new List<EneryOrder>(),
                AreaEneryOrderList = new List<EneryOrder>()
            };


            Dictionary<int, string> cacheLayer = NTS.WEB.Common.CacheHelper.GetCache("object-Layer") as Dictionary<int, string>;
            if (cacheLayer == null)
            {
                var res = new LayerObjects().GetObjectLayers();
                NTS.WEB.Common.CacheHelper.SetCache("object-Layer", res);
                cacheLayer = res;
            }

            var deepth = cacheLayer.ContainsKey(int.Parse(ConfigurationManager.AppSettings["ShopLevel"])) ?
                int.Parse(ConfigurationManager.AppSettings["ShopLevel"]) : cacheLayer.Count;
            shopOrderLast.ShopLevel = GetShopLevel(deepth);

            //层级对象数据
            var listObject = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList
                (string.Format(" and layerobjectid in ({0})", cacheLayer[deepth]), " order by LayerObjectID");

            //拼接表名
            string tbName = "TS_COUNT_AREA_MONTH_" + startTime.Year.ToString();
            var monthDataList = new NTS.WEB.BLL.MonthDataObject().GetMonthDataObjectList(tbName, startTime.ToShortDateString());
            //分类分项
            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            //总能耗
            //List<EneryOrder> lstTotal = (from o in listObject
            //                             join b in monthDataList
            //                             on o.LayerObjectID equals int.Parse(b.CNAME)
            //                             select new EneryOrder
            //                             {
            //                                 BuildingName = o.LayerObjectName,
            //                                 EneryValue = b.COUNTVALUE * ItemList.Find(p => p.ItemCodeNumber == b.ITEMCODE).ItemCoal

            //                             }).ToList();
            List<EneryOrder> lstTotal = (from o in listObject
                                         join b in monthDataList
                                         on o.LayerObjectID equals int.Parse(b.CNAME)
                                         select new EneryOrder
                                         {
                                             BuildingName = o.LayerObjectName,
                                             // EneryValue=0
                                             EneryValue = b.COUNTVALUE * Convert.ToDouble(ItemList.Find(p => p.ItemCodeNumber.Substring(0, 2) == b.ITEMCODE.Substring(0, 2)).ItemCoal.ToString())

                                         }).ToList();
            ////单位面积能耗
            List<EneryOrder> lstArea = (from o in listObject
                                        join b in monthDataList
                                        on o.LayerObjectID equals int.Parse(b.CNAME)
                                        select new EneryOrder
                                        {
                                            BuildingName = o.LayerObjectName,
                                            //EneryValue=0
                                            EneryValue = (b.COUNTVALUE * Convert.ToDouble(ItemList.Find(p => p.ItemCodeNumber.Substring(0, 2) == b.ITEMCODE.Substring(0, 2)).ItemCoal.ToString())) / o.AreaNum

                                        }).ToList();
            List<EneryOrder> lstTtl = new List<EneryOrder>();
            List<EneryOrder> lstAr = new List<EneryOrder>();

            //按对象名称分组
            var lstGpTtl = from item in lstTotal
                           group item by item.BuildingName;

            string nm = string.Empty;
            double sum = 0;
            foreach (var group in lstGpTtl)
            {
                nm = string.Empty;
                sum = 0;
                foreach (var v in group)
                {

                    nm = v.BuildingName;
                    sum += v.EneryValue;
                }
                lstTtl.Add(new EneryOrder { BuildingName = nm, EneryValue = sum });
            }

            var lstGpAr = from item in lstArea
                          group item by item.BuildingName;

            foreach (var group in lstGpAr)
            {
                nm = string.Empty;
                sum = 0;
                foreach (var v in group)
                {

                    nm = v.BuildingName;
                    sum += v.EneryValue;
                }
                lstAr.Add(new EneryOrder { BuildingName = nm, EneryValue = sum });
            }


            // var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectDeepth={0}", deepth), " order by LayerObjectID");

            //List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();
            //var model = new BaseQueryModel();
            //model.IsDevice = 0;
            //model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            //Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
            //Dictionary<string, decimal> tempAreaConvert = new Dictionary<string, decimal>();
            //decimal[] tempConvert = new decimal[objectList.Count];
            //decimal[] tempAreaConvert = new decimal[objectList.Count];



            //foreach (var item in ItemCodeStr)
            //{
            //    model.ItemCode = item;
            //    model.Unit = ChartUnit.unit_month;
            //    model.Starttime = startTime;
            //    model.Endtime = endTime;
            //    var resList = reportBll.GetBaseEneryDataList(model);
            //    const int order = 1;
            //    foreach ( var r in resList.BaseLayerObjectResults)
            //    {
            //        if(tempConvert.ContainsKey(r.Value.baseLayerObject.LayerObjectName))
            //        {
            //            tempConvert[r.Value.baseLayerObject.LayerObjectName] += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
            //            tempAreaConvert[r.Value.baseLayerObject.LayerObjectName] += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()) /
            //                              decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());
            //        }
            //        else
            //        {
            //            tempConvert.Add(r.Value.baseLayerObject.LayerObjectName, decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()));
            //            tempAreaConvert.Add(r.Value.baseLayerObject.LayerObjectName, decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()) /
            //                               decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString()));
            //        }
            //        shopOrder.TotalEneryOrderList.Add(new EneryOrder()
            //            {
            //                BuildingName = r.Value.baseLayerObject.LayerObjectName,
            //EneryValue = double.Parse(tempConvert[r.Value.baseLayerObject.LayerObjectName].ToString("f2")),
            //                OrderNum = order
            //            });


            //        shopOrder.AreaEneryOrderList.Add(new EneryOrder()
            //        {
            //            BuildingName = r.Value.baseLayerObject.LayerObjectName,
            //            EneryValue = double.Parse(tempAreaConvert[r.Value.baseLayerObject.LayerObjectName].ToString("f2")),
            //            OrderNum = order
            //        });
            //    }
            //}

            //var res1 = from p in shopOrder.TotalEneryOrderList group p by p.BuildingName into g select new { g.Key, BuildingName = g.Max(p => p.BuildingName), OrderNum = g.Max(p => p.OrderNum), EneryValue = g.Max(p => p.EneryValue) };
            //var res2 = from p in shopOrder.AreaEneryOrderList group p by p.BuildingName into g select new { g.Key, BuildingName = g.Max(p => p.BuildingName), OrderNum = g.Max(p => p.OrderNum), EneryValue = g.Max(p => p.EneryValue) };

            foreach (var c in lstTtl)
            {
                shopOrderLast.TotalEneryOrderList.Add(new EneryOrder
                {
                    BuildingName = c.BuildingName,
                    EneryValue = double.Parse(c.EneryValue.ToString("f2")),
                    OrderNum = c.OrderNum
                });
            }

            foreach (var c in lstAr)
            {
                shopOrderLast.AreaEneryOrderList.Add(new EneryOrder
                {
                    BuildingName = c.BuildingName,
                    EneryValue = double.Parse(c.EneryValue.ToString("f2")),
                    OrderNum = c.OrderNum
                });
            }
            return shopOrderLast;
        }

        public IndexLimit GetIndexLimit(DateTime startTime, DateTime endTime)
        {
            var eneryDataList = new Dictionary<string, decimal>();
            var indexLimit = new IndexLimit()
                {
                    ElectricityHigh = 0,
                    ElectricityHighTime = "-",
                    ElectricityLow = 0,
                    ElectricityLowTime = "-"
                };
            var model = new BaseQueryModel();
            var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectParentID={0}", 0), " order by LayerObjectID");
            model.IsDevice = 0;
            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = "01000";
            model.Unit = ChartUnit.unit_hour;
            model.Starttime = startTime;
            model.Endtime = endTime;
            var resList = reportBll.GetBaseEneryDataList(model);
            foreach (var o in objectList)
            {
                decimal countValue = 0;
                foreach (var d in resList.BaseLayerObjectResults[o.LayerObjectID.ToString()].Datas)
                {
                    // countValue += d.DataValue;
                    if (!eneryDataList.ContainsKey(d.DatePick))
                    {
                        eneryDataList.Add(d.DatePick, d.DataValue);
                    }
                    else
                    {
                        eneryDataList[d.DatePick] += d.DataValue;
                    }
                }
            }

            var limitTemp = eneryDataList.Select(e => new TempList() { DatePick = e.Key, DataValue = e.Value }).ToList();
            limitTemp = (from l in limitTemp orderby l.DataValue select l).ToList();
            if (limitTemp.Count > 0)
            {
                indexLimit.ElectricityLow = limitTemp[0].DataValue;
                indexLimit.ElectricityLowTime = Convert.ToDateTime(limitTemp[0].DatePick).ToString("HH:mm");
                indexLimit.ElectricityHigh = limitTemp[limitTemp.Count - 1].DataValue;
                indexLimit.ElectricityHighTime = Convert.ToDateTime(limitTemp[limitTemp.Count - 1].DatePick).ToString("HH:mm");
            }
            return indexLimit;
        }
    }

    public class TempList
    {
        public string DatePick { get; set; }
        public decimal DataValue { get; set; }
    }
}
