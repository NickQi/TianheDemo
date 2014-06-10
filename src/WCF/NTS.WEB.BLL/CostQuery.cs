using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;

namespace NTS.WEB.BLL
{
    public class CostQuery
    {
        readonly IReportBase _reportBll = DataSwitchConfig.CreateReportBase();
        readonly ICostQuery _idal = DataSwitchConfig.CreateCostQuery();
        readonly IAccessCommon _accssCommon = DataSwitchConfig.CreateAccessCommon();
        public ResultCostQuery GetCostQuery4Demo(QueryCost query)
        {
            ResultCostQuery result = new ResultCostQuery()
            {
                FeePie = new PieHighChart() { series = new List<Series>() },
                FeeQueryCharts = new QuotaHighChart() { series = new List<EneryAnalyseSeries>() },
                FeeAnalyses = new FeeAnalyses(),
                FeeTbl = new FeeTbl() { FeeList = new List<List<string>>() }
            };

            result.FeePie.series.Add(new Series() { data = new List<EneryHighChart>() });

            #region itemCodeList
            List<Model.Itemcode> itemCodeList = new List<Model.Itemcode>();

            if (query.ItemCode == "00000")
            {//总能耗
                itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0 ", " order by ItemcodeID");
                result.Unit = "T";//标准煤单位
            }
            else
            {
                itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                if (itemCodeList.Count > 0)
                {
                    result.Unit = itemCodeList[0].Unit;//单个分类分项单位
                    result.FeeTbl.Unit = itemCodeList[0].Unit;
                    result.FeeTbl.EneType = itemCodeList[0].ItemCodeName;
                    result.FeeAnalyses.EnergyUnit = result.Unit;
                }
            }
            #endregion

            #region 生成EndTime

            switch (query.Particle)
            {
                case Particle.Month://月 query.StartTime格式为yyyy-MM-01
                    query.EndTime = query.StartTime.AddMonths(1).AddDays(-1);
                    int day = DateTime.Now.Day;
                    if (query.StartTime.Year == DateTime.Now.Year && query.StartTime.Month == DateTime.Now.Month)//月、年第一天不算进去
                    {//当月
                        query.EndTime = DateTime.Now.AddDays(-1);//精确到前一天的能耗值
                    }
                    break;
                case Particle.Year://年  query.StartTime格式为yyyy-01-01
                    query.EndTime = query.StartTime.AddYears(1).AddMonths(-1);
                    if (query.StartTime.Year == DateTime.Now.Year)//月、年第一天不算进去
                    {
                        query.EndTime = DateTime.Now.AddDays(-1);
                    }
                    break;
                default:
                    query.EndTime = query.StartTime.AddMonths(1).AddDays(-1);
                    if (query.StartTime.Year == DateTime.Now.Year && query.StartTime.Month == DateTime.Now.Month)
                    {
                        query.EndTime = DateTime.Now.AddDays(-1);
                    }
                    break;
            }
            #endregion

            var cost = GenerateCost(query);


            if (cost.Count > 0)
            {
                result.FeeType = ((FeeType)cost[0].PAYMENT_TYPE).ToString();

                #region 绑定Highchart
                switch (((FeeType)cost[0].PAYMENT_TYPE))
                {
                    case FeeType.分时计费:
                        #region 绑定Highchart
                        result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                        {
                            name = "尖时费用",
                            data = (from item in cost select decimal.Round(decimal.Parse(item.SHARP_COST.ToString()), 2)).ToList()
                        });
                        result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                        {
                            name = "峰时费用",
                            data = (from item in cost select decimal.Round(decimal.Parse(item.HIGH_COST.ToString()), 2)).ToList()
                        });
                        result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                        {
                            name = "平时费用",
                            data = (from item in cost select decimal.Round(decimal.Parse(item.MID_COST.ToString()), 2)).ToList()
                        });
                        result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                        {
                            name = "谷时费用",
                            data = (from item in cost select decimal.Round(decimal.Parse(item.LOW_COST.ToString()), 2)).ToList()
                        });
                        #endregion

                        #region 绑定饼图

                        result.FeePie.series[0].data.Add(new EneryHighChart()
                        {
                            name = "尖时费用",
                            y = decimal.Round(decimal.Parse(cost.Select(t => t.SHARP_COST).Sum().ToString()), 2)
                        });
                        result.FeePie.series[0].data.Add(new EneryHighChart()
                        {
                            name = "峰时费用",
                            y = decimal.Round(decimal.Parse(cost.Select(t => t.HIGH_COST).Sum().ToString()), 2)
                        });
                        result.FeePie.series[0].data.Add(new EneryHighChart()
                        {
                            name = "平时费用",
                            y = decimal.Round(decimal.Parse(cost.Select(t => t.MID_COST).Sum().ToString()), 2)
                        });
                        result.FeePie.series[0].data.Add(new EneryHighChart()
                        {
                            name = "谷时费用",
                            y = decimal.Round(decimal.Parse(cost.Select(t => t.LOW_COST).Sum().ToString()), 2)
                        });

                        #endregion
                        break;
                    case FeeType.平时计费:

                        #region 绑定Highchart
                        result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                        {
                            name = "平时费用",
                            data = (from item in cost select decimal.Round(decimal.Parse(item.TOTAL_COST.ToString()), 2)).ToList()
                        });
                        #endregion

                        #region 绑定饼图
                        result.FeePie.series[0].data.Add(new EneryHighChart()
                        {
                            name = "平时费用",
                            y = decimal.Round(decimal.Parse(cost.Select(t => t.MID_COST).Sum().ToString()), 2)
                        });


                        #endregion


                        break;
                    case FeeType.阶梯计费:

                        //1 峰 2 平 3 谷 4 尖
                        var stepsetting = _idal.GetStepSetting(query.ItemCode);

                        result.StepSettingID = (from i in stepsetting select i.GEARSID).ToList<int>();
                        if (result.StepSettingID.Contains(1))
                        {
                            result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                            {
                                name = "第一档费用",
                                data = (from item in cost select decimal.Round(decimal.Parse(item.HIGH_COST.ToString()), 2)).ToList()
                            });
                            result.FeePie.series[0].data.Add(new EneryHighChart()
                            {
                                name = "第一档费用",
                                y = decimal.Round(decimal.Parse(cost.Select(t => t.HIGH_COST).Sum().ToString()), 2)
                            });
                        }
                        if (result.StepSettingID.Contains(2))
                        {
                            result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                            {
                                name = "第二档费用",
                                data = (from item in cost select decimal.Round(decimal.Parse(item.MID_COST.ToString()), 2)).ToList()
                            });
                            result.FeePie.series[0].data.Add(new EneryHighChart()
                            {
                                name = "第二档费用",
                                y = decimal.Round(decimal.Parse(cost.Select(t => t.MID_COST).Sum().ToString()), 2)
                            });
                        }
                        if (result.StepSettingID.Contains(3))
                        {
                            result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                            {
                                name = "第三档费用",
                                data = (from item in cost select decimal.Round(decimal.Parse(item.LOW_COST.ToString()), 2)).ToList()
                            });
                            result.FeePie.series[0].data.Add(new EneryHighChart()
                            {
                                name = "第三档费用",
                                y = decimal.Round(decimal.Parse(cost.Select(t => t.LOW_COST).Sum().ToString()), 2)
                            });
                        }
                        if (result.StepSettingID.Contains(4))
                        {
                            result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                            {
                                name = "第四档费用",
                                data = (from item in cost select decimal.Round(decimal.Parse(item.SHARP_COST.ToString()), 2)).ToList()
                            });
                            result.FeePie.series[0].data.Add(new EneryHighChart()
                            {
                                name = "第四档费用",
                                y = decimal.Round(decimal.Parse(cost.Select(t => t.SHARP_COST).Sum().ToString()), 2)
                            });
                        }

                        break;
                }

                #endregion



                #region 绑定表格

                foreach (var item in cost)
                {
                    string time = "";
                    switch (query.Particle)
                    {
                        case Particle.Month:
                            time = item.TIMEID.ToString("yyyy-MM-dd");
                            break;
                        case Particle.Year:
                            time = item.TIMEID.Year + "-" + item.TIMEID.Month;
                            break;
                    }
                    List<string> fl = new List<string>();

                    fl.Add(time);
                    switch (((FeeType)cost[0].PAYMENT_TYPE))
                    {
                        case FeeType.分时计费:
                            fl.Add(item.TOTAL.ToString("0.00"));
                            fl.Add(item.TOTAL_COST.ToString("0.00"));
                            fl.Add(item.SHARP.ToString("0.00"));
                            fl.Add(item.SHARP_COST.ToString("0.00"));
                            fl.Add(item.HIGH.ToString("0.00"));
                            fl.Add(item.HIGH_COST.ToString("0.00"));
                            fl.Add(item.MID.ToString("0.00"));
                            fl.Add(item.MID_COST.ToString("0.00"));
                            fl.Add(item.LOW.ToString("0.00"));
                            fl.Add(item.LOW_COST.ToString("0.00"));
                            break;
                        case FeeType.平时计费:
                            fl.Add(item.TOTAL.ToString("0.00"));
                            fl.Add(item.TOTAL_COST.ToString("0.00"));

                            break;
                        case FeeType.阶梯计费:
                            fl.Add(item.TOTAL.ToString("0.00"));
                            fl.Add(item.TOTAL_COST.ToString("0.00"));

                            if (result.StepSettingID.Contains(1))
                            {
                                fl.Add(item.HIGH.ToString("0.00"));
                                fl.Add(item.HIGH_COST.ToString("0.00"));
                            }
                            if (result.StepSettingID.Contains(2))
                            {
                                fl.Add(item.MID.ToString("0.00"));
                                fl.Add(item.MID_COST.ToString("0.00"));
                            }
                            if (result.StepSettingID.Contains(3))
                            {
                                fl.Add(item.LOW.ToString("0.00"));
                                fl.Add(item.LOW_COST.ToString("0.00"));
                            }
                            if (result.StepSettingID.Contains(4))
                            {
                                fl.Add(item.SHARP.ToString("0.00"));
                                fl.Add(item.SHARP_COST.ToString("0.00"));
                            }

                            break;
                    }


                    result.FeeTbl.FeeList.Add(fl);
                }
                #endregion

                #region 绑定分析

                result.FeeAnalyses.TotalEnergy = decimal.Round(
                  decimal.Parse(cost.Select(t => t.TOTAL).Sum().ToString()), 2);
                result.FeeAnalyses.TotalVal = decimal.Round(
                   decimal.Parse(cost.Select(t => t.TOTAL_COST).Sum().ToString()), 2);
                result.FeeAnalyses.MaxVal = decimal.Round(
                  decimal.Parse(cost.Select(t => t.TOTAL_COST).Max().ToString()), 2);
                result.FeeAnalyses.MinVal = decimal.Round(
                   decimal.Parse(cost.Select(t => t.TOTAL_COST).Min().ToString()), 2);
                result.FeeAnalyses.AvgVal = decimal.Round(
                    decimal.Parse(cost.Select(t => t.TOTAL_COST).Average().ToString()), 2);
                #endregion


                query.StartTime = query.StartTime.AddMonths(-1);
                query.EndTime = query.EndTime.AddMonths(-1);
                cost = GenerateCost(query);
                result.FeeAnalyses.EnergyLastMonth = decimal.Round(
                      decimal.Parse(cost.Select(t => t.TOTAL_COST).Sum().ToString()), 2);
                if (result.FeeAnalyses.EnergyLastMonth > 0)
                {
                    result.FeeAnalyses.CompareLastMonth = decimal.Round(100 * (result.FeeAnalyses.TotalVal - result.FeeAnalyses.EnergyLastMonth) / result.FeeAnalyses.EnergyLastMonth, 2)
                                 .ToString(CultureInfo.InvariantCulture) + "%";
                }
            }
            else
            {
                return null;
            }



            return result;
        }

        public Dictionary<string, EnergyValueCost> GetSingleItemCodeValue_OLD(QueryCost query, List<Model.Itemcode> itemCodeList)
        {
            var model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = new List<int>() { query.ObjectId };
            model.Unit = BaseTool.GetChartUnit((int)query.Particle - 1);

            if (model.Unit == ChartUnit.unit_month)
            {
                model.Starttime = Convert.ToDateTime(query.StartTime.ToString("yyyy-MM-1"));
                model.Endtime = Convert.ToDateTime(query.EndTime.ToString("yyyy-MM-1"));
            }
            else
            {
                model.Starttime = query.StartTime;
                model.Endtime = query.EndTime;
            }

            Dictionary<string, EnergyValueCost> tempConvert = new Dictionary<string, EnergyValueCost>();
            Dictionary<string, EnergyValueCost> tempConvertDay = new Dictionary<string, EnergyValueCost>();
            switch (model.Unit)
            {
                case ChartUnit.unit_month:
                    for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddMonths(1))
                    {
                        tempConvert.Add(i.ToString("yyyy-MM-01"), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    break;
                case ChartUnit.unit_hour:
                    for (DateTime i = model.Starttime; i < model.Endtime.AddDays(1); i = i.AddHours(1))
                    {
                        tempConvert.Add(i.ToString("yyyy-MM-dd HH:00:00"), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    break;
                case ChartUnit.unit_day:
                    for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddDays(1))
                    {
                        tempConvert.Add(i.ToString(("yyyy-MM-dd")), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    break;
            }
            foreach (var item in itemCodeList)
            {
                decimal price = _accssCommon.GetFeePrice(item.ItemCodeNumber);
                model.ItemCode = item.ItemCodeNumber;
                BaseResult resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);
                foreach (var r in resList.BaseLayerObjectResults)
                {
                    foreach (var rr in r.Value.Datas)
                    {
                        if (tempConvert.ContainsKey(rr.DatePick))
                        {
                            if (query.ItemCode == "00000")
                            {
                                tempConvert[rr.DatePick].EnergyValue += rr.CoalDataValue;
                                tempConvert[rr.DatePick].EnergyCost += rr.CoalDataValue * price;
                            }
                            else
                            {
                                tempConvert[rr.DatePick].EnergyValue += rr.DataValue;
                                tempConvert[rr.DatePick].EnergyCost += rr.DataValue * price;
                            }
                        }
                    }
                }
            }
            return tempConvert;
        }

        public Dictionary<string, EnergyValueCost> GetSingleItemCodeValue(QueryCost query, List<Model.Itemcode> itemCodeList)
        {
            var model = new BaseQueryModel();
            model.IsDevice = 0;
            model.areaType = query.ObjType;
            model.ObjectList = new List<int>() { query.ObjectId };
            model.Unit = BaseTool.GetChartUnit((int)query.Particle - 1);

            if (model.Unit == ChartUnit.unit_month)
            {
                model.Starttime = Convert.ToDateTime(query.StartTime.ToString("yyyy-MM-1"));
                model.Endtime = Convert.ToDateTime(query.EndTime.ToString("yyyy-MM-1"));
            }
            else
            {
                model.Starttime = query.StartTime;
                model.Endtime = query.EndTime;
            }

            Dictionary<string, EnergyValueCost> tempConvert = new Dictionary<string, EnergyValueCost>();
            Dictionary<string, EnergyValueCost> tempConvert4Day = new Dictionary<string, EnergyValueCost>();
            switch (model.Unit)
            {
                case ChartUnit.unit_month:
                    for (DateTime i = model.Starttime; i <= model.Endtime.AddMonths(-1); i = i.AddMonths(1))
                    {
                        tempConvert.Add(i.ToString("yyyy-MM-01"), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    for (DateTime i = DateTime.Parse(query.EndTime.ToString("yyyy-MM-01")); i <= query.EndTime; i = i.AddDays(1))
                    {
                        tempConvert4Day.Add(i.ToString(("yyyy-MM-dd")), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    model.Endtime = query.EndTime.AddMonths(-1);
                    break;
                case ChartUnit.unit_hour:
                    for (DateTime i = model.Starttime; i < model.Endtime.AddDays(1); i = i.AddHours(1))
                    {
                        tempConvert.Add(i.ToString("yyyy-MM-dd HH:00:00"), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    break;
                case ChartUnit.unit_day:
                    for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddDays(1))
                    {
                        tempConvert.Add(i.ToString(("yyyy-MM-dd")), new EnergyValueCost { EnergyCost = 0, EnergyValue = 0 });
                    }
                    break;
            }
            foreach (var item in itemCodeList)
            {
                decimal price = _accssCommon.GetFeePrice(item.ItemCodeNumber);
                model.ItemCode = item.ItemCodeNumber;
                GenerateEnergyValue(model, tempConvert, price);
                if (tempConvert4Day.Count > 0)
                {
                    model.Starttime = DateTime.Parse(query.EndTime.ToString("yyyy-MM-01"));
                    model.Endtime = query.EndTime;
                    model.Unit = ChartUnit.unit_day;
                    GenerateEnergyValue(model, tempConvert4Day, price);
                    EnergyValueCost temp = new EnergyValueCost()
                                               {
                                                    EnergyCost=tempConvert4Day.Sum(i => i.Value.EnergyCost),
                                                    EnergyValue = tempConvert4Day.Sum(i => i.Value.EnergyValue),
                                                    
                                               };

                    tempConvert.Add(query.EndTime.ToString("yyyy-MM-01"), temp);
                }

            }
            
            return tempConvert;
        }
        private void GenerateEnergyValue(BaseQueryModel model, Dictionary<string, EnergyValueCost> tempConvert, decimal price)
        {

            BaseResult resList = _reportBll.GetBaseEneryDataList(model, model.areaType == AreaType.Liquid ? true : false);
            foreach (var r in resList.BaseLayerObjectResults)
            {
                foreach (var rr in r.Value.Datas)
                {
                    if (tempConvert.ContainsKey(rr.DatePick))
                    {
                        if (model.ItemCode == "00000")
                        {
                            tempConvert[rr.DatePick].EnergyValue += rr.CoalDataValue;
                            tempConvert[rr.DatePick].EnergyCost += rr.CoalDataValue * price;
                        }
                        else
                        {
                            tempConvert[rr.DatePick].EnergyValue += rr.DataValue;
                            tempConvert[rr.DatePick].EnergyCost += rr.DataValue * price;
                        }
                    }
                }
            }
        }

        public ResultCostQuery GetCostQuery(QueryCost query)
        {
            ResultCostQuery result = new ResultCostQuery()
            {
                FeePie = new PieHighChart() { series = new List<Series>() },
                FeeQueryCharts = new QuotaHighChart() { series = new List<EneryAnalyseSeries>() },
                FeeAnalyses = new FeeAnalyses(),
                FeeTbl = new FeeTbl() { FeeList = new List<List<string>>() }
            };

            result.FeePie.series.Add(new Series() { data = new List<EneryHighChart>() });

            #region itemCodeList
            List<Model.Itemcode> itemCodeList = new List<Model.Itemcode>();

            if (query.ItemCode == "00000")
            {//总能耗
                itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0 ", " order by ItemcodeID");
                result.Unit = "T";//标准煤单位
            }
            else
            {
                itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                if (itemCodeList.Count > 0)
                {
                    result.Unit = itemCodeList[0].Unit;//单个分类分项单位
                    result.FeeTbl.Unit = itemCodeList[0].Unit;
                    result.FeeTbl.EneType = itemCodeList[0].ItemCodeName;
                    result.FeeAnalyses.EnergyUnit = result.Unit;
                }
            }
            #endregion

            #region 生成EndTime

            switch (query.Particle)
            {
                case Particle.Month://月 query.StartTime格式为yyyy-MM-01
                    query.EndTime = query.StartTime.AddMonths(1).AddDays(-1);
                    
                    int day = DateTime.Now.Day;
                    if (query.StartTime.Year == DateTime.Now.Year && query.StartTime.Month == DateTime.Now.Month)//月、年第一天不算进去
                    {//当月
                        query.EndTime = DateTime.Now.AddDays(-1);//精确到前一天的能耗值
                    }
                    break;
                case Particle.Year://年  query.StartTime格式为yyyy-01-01
                    query.EndTime = query.StartTime.AddYears(1).AddMonths(-1).AddDays(-1);
                    if (query.StartTime.Year == DateTime.Now.Year)//月、年第一天不算进去
                    {
                        query.EndTime = DateTime.Now.AddDays(-1);
                    }
                    break;
                default:
                    query.EndTime = query.StartTime.AddMonths(1).AddDays(-1);
                    if (query.StartTime.Year == DateTime.Now.Year && query.StartTime.Month == DateTime.Now.Month)
                    {
                        query.EndTime = DateTime.Now.AddDays(-1);
                    }
                    break;
            }
            #endregion
            Dictionary<string, EnergyValueCost> tempConvert = new Dictionary<string, EnergyValueCost>();
            tempConvert = GetSingleItemCodeValue(query, itemCodeList);



            if (tempConvert.Count > 0)
            {
                result.FeeType = FeeType.平时计费.ToString();

                #region 绑定Highchart


                #region 绑定Highchart
                result.FeeQueryCharts.series.Add(new EneryAnalyseSeries()
                {
                    name = "平时费用",
                    data = (from item in tempConvert select decimal.Round(item.Value.EnergyCost, 2)).ToList()
                });
                #endregion

                #region 绑定饼图
                result.FeePie.series[0].data.Add(new EneryHighChart()
                {
                    name = "平时费用",
                    y = decimal.Round(decimal.Parse(tempConvert.Select(t => t.Value.EnergyCost).Sum().ToString()), 2)
                });


                #endregion

                #endregion



                #region 绑定表格

                foreach (var item in tempConvert)
                {
                    string time = "";
                    switch (query.Particle)
                    {
                        case Particle.Month:
                            time = item.Key;
                            break;
                        case Particle.Year:
                            time = item.Key.Substring(0, item.Key.LastIndexOf('-'));
                            break;
                    }
                    List<string> fl = new List<string>();

                    fl.Add(time);

                    fl.Add(item.Value.EnergyValue.ToString("0.00"));
                    fl.Add((item.Value.EnergyCost).ToString("0.00"));

                    result.FeeTbl.FeeList.Add(fl);
                }
                #endregion

                #region 绑定分析

                result.FeeAnalyses.TotalEnergy = decimal.Round(
                  decimal.Parse(tempConvert.Select(t => t.Value.EnergyValue).Sum().ToString()), 2);
                result.FeeAnalyses.TotalVal = decimal.Round(
                   decimal.Parse(tempConvert.Select(t => t.Value.EnergyCost).Sum().ToString()), 2);
                result.FeeAnalyses.MaxVal = decimal.Round(
                  decimal.Parse(tempConvert.Select(t => t.Value.EnergyCost).Max().ToString()), 2);
                result.FeeAnalyses.MinVal = decimal.Round(
                   decimal.Parse(tempConvert.Select(t => t.Value.EnergyCost).Min().ToString()), 2);
                result.FeeAnalyses.AvgVal = decimal.Round(
                    decimal.Parse(tempConvert.Select(t => t.Value.EnergyCost).Average().ToString()), 2);
                #endregion

                if (query.Particle==Particle.Month)
                {
                    query.StartTime = query.StartTime.AddMonths(-1);
                    query.EndTime = query.EndTime.AddMonths(-1);
                }
                else
                {
                    query.StartTime = query.StartTime.AddYears(-1);
                    query.EndTime = query.EndTime.AddYears(-1);
                }

                tempConvert = GetSingleItemCodeValue(query, itemCodeList);
                result.FeeAnalyses.EnergyLastMonth = decimal.Round(
                      decimal.Parse(tempConvert.Select(t => t.Value.EnergyCost).Sum().ToString()), 2);
                if (result.FeeAnalyses.EnergyLastMonth > 0)
                {
                    result.FeeAnalyses.CompareLastMonth = decimal.Round(100 * (result.FeeAnalyses.TotalVal - result.FeeAnalyses.EnergyLastMonth) / result.FeeAnalyses.EnergyLastMonth, 2)
                                 .ToString(CultureInfo.InvariantCulture) + "%";
                }
            }
            else
            {
                return null;
            }



            return result;
        }



        private List<CostQueryModel> GenerateCost(QueryCost query)
        {
            var cost = _idal.GetCostQuery(query);
            return cost;
            //Particle temp = query.Particle;

            //switch (query.Particle)
            //{
            //    case Particle.Year:
            //        if (query.EndTime.Year < DateTime.Now.Year)
            //        {
            //            var cost = _idal.GetCostQuery(query);
            //        }
            //        else
            //        {
            //            if (query.EndTime.Year==DateTime.Now.Year)
            //            {

            //            }
            //        }
            //        break;
            //    case Particle.Month:
            //        if (query.EndTime.Month < DateTime.Now.Month)
            //        {
            //            var cost = _idal.GetCostQuery(query);
            //        }
            //        else
            //        {
            //            var cost = _idal.GetCostQuery(query);
            //        }
            //        break;
            //}
            //return new List<CostQueryModel>();
        }
        public class EnergyValueCost
        {
            public decimal EnergyValue;
            public decimal EnergyCost;
        }

    }
}
