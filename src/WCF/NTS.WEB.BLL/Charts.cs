using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.BLL
{
    public class Charts
    {
        readonly NTS.WEB.ProductInteface.IReportBase _reportBll = NTS.WEB.ProductInteface.DataSwitchConfig.CreateReportBase();

        #region 返回首页的电分类占比图
        /// <summary>
        /// 返回首页的电分类占比图
        /// </summary>
        /// <returns></returns>
        public List<decimal> GetIndexPieChart()
        {
            var objectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectparentid={0}", 0), " order by LayerObjectID");
            var itemList = new NTS.WEB.BLL.Itemcode().GetItemcodeList(string.Format(" and ParentID=(select itemcodeid from Becm_ItemCode where ItemCodeNumber='{0}')", "01000"), " order by ItemcodeID");
            var list = (from item in itemList
                        select new BaseQueryModel
                            {
                                IsDevice = 0,
                                ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>(),
                                ItemCode = item.ItemCodeNumber,
                                Unit = ChartUnit.unit_month,
                                Starttime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-1")),
                                Endtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-1"))
                            }
                            into model
                            select _reportBll.GetBaseEneryDataList(model)
                                into resList
                                select objectList.Sum(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString()].Total)).ToList();
            return list;
        }
        #endregion

        #region 电能耗走势图
        public ResultIndexLineChart IndexElectricityLineChart(DateTime startTime, DateTime endTime)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
                var eneryDataList = new Dictionary<string, decimal>();
                var model = new BaseQueryModel();
                var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectParentID={0}", 0), " order by LayerObjectID");
                model.IsDevice = 0;
                model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
                model.ItemCode = "01000";
                model.Unit = ChartUnit.unit_hour;
                model.Starttime = startTime;
                model.Endtime = endTime;
                var resList = _reportBll.GetBaseEneryDataList(model);
                foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
                {
                    if (!eneryDataList.ContainsKey(d.DatePick))
                    {
                        eneryDataList.Add(d.DatePick, d.DataValue);
                    }
                    else
                    {
                        eneryDataList[d.DatePick] += d.DataValue;
                    }
                }

                foreach (var e in eneryDataList)
                {
                    result.DatePick.Add(e.Key);
                    result.DatePickEnery.Add(e.Value);
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception ex)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = ex.Message;
                return new ResultIndexLineChart() { ActionInfo = pAction };
            }
        }


        public ResultRealLine IndexElectricityRealLine(DateTime startTime, DateTime endTime)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new ResultRealLine { series = new List<EneryAnalyseSeries>(), Unit = "kwh" };
                var eneryDataList = new Dictionary<string, decimal>();
                var model = new BaseQueryModel();
                var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and LayerObjectParentID={0}", 0), " order by LayerObjectID");
                model.IsDevice = 0;
                model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
                model.ItemCode = "01000";
                model.Unit = ChartUnit.unit_hour;
                model.Starttime = startTime;
                model.Endtime = endTime;
                var resList = _reportBll.GetBaseEneryDataList(model,true);
                foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
                {
                    if (!eneryDataList.ContainsKey(d.DatePick))
                    {
                        eneryDataList.Add(d.DatePick, d.DataValue);
                    }
                    else
                    {
                        eneryDataList[d.DatePick] += d.DataValue;
                    }
                }

                List<decimal> dat = new List<decimal>(24);//modified by wxy
                for (int i = 0; i < 24; i++)
                    dat.Add(0);
                foreach (var e in eneryDataList)
                {
                    DateTime dt = DateTime.Parse(e.Key);//dat.Add(e.Value);
                    dat[dt.Hour] = e.Value;

                }
                List<decimal> dat1 = new List<decimal>();
                EneryAnalyseSeries eas = new EneryAnalyseSeries();
                decimal rightValue = 0;//modified by wxy
                decimal leftValue = 0;
                for (int i = 0; i <= DateTime.Now.Hour; i++)
                {
                    if (dat[i].CompareTo(-1) == 0)
                    {
                        int tempIndex = i;
                        while (--i >= 0)
                        {
                            if (dat[i].CompareTo(-1) >= 0)
                            {
                                rightValue = dat[i];
                                break;
                            }
                        }
                        i = tempIndex;
                        while (++i < 24)
                        {
                            if (dat[i].CompareTo(-1) >= 0)
                            {
                                leftValue = dat[i];
                                break;
                            }
                        }
                        i = tempIndex;
                        decimal meanValue = (rightValue + leftValue)/2;
                        dat1.Add( Math.Round(meanValue, 2) );
                    }
                    else
                        dat1.Add(dat[i]);
                }
                eas.data = dat1;

                result.series.Add(eas);
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception ex)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = ex.Message;
                return new ResultRealLine() { ActionInfo = pAction };
            }
        }

        public ResultItemCode IndexAvgElectricityLineChart(DateTime startTime, DateTime endTime)
        {
            var realLine = IndexElectricityLineChart(startTime, startTime);
            var resultList = new ResultItemCode();
            resultList.Dept = new List<string>();
            resultList.ObjectName = new List<string>();
            resultList.Enery = new Dictionary<string, List<decimal>>();
            var oldStartTime = Convert.ToDateTime(startTime.AddMonths(-1).ToString("yyyy-MM-1"));
            var oldEndTime = oldStartTime.AddMonths(1);
            var dayLine = new decimal[24];
            while (oldStartTime < oldEndTime)
            {
                var result = IndexElectricityLineChart(oldStartTime, oldStartTime);
                for (var i = 0; i < result.DatePickEnery.Count; i++)
                {
                    dayLine[i] += result.DatePickEnery[i];
                }
                oldStartTime = oldStartTime.AddDays(1);
            }
            int Count = oldStartTime.AddDays(-1).Subtract(Convert.ToDateTime(oldStartTime.AddDays(-1).ToString("yyyy-MM-1"))).Days + 1;
            for (var l = 0; l < dayLine.Length; l++)
            {
                dayLine[l] = dayLine[l] / Count;
                resultList.Dept.Add("kwh");
            }
            foreach (var real in realLine.DatePick)
            {
                resultList.ObjectName.Add(real);
            }
            resultList.Enery.Add("上月平均", dayLine.ToList());
            resultList.Enery.Add("实时消耗", realLine.DatePickEnery);
            return resultList;
        }
        #endregion


        public ResultItemCode RealChart(RealQuery query)
        {
            ResultItemCode result = new ResultItemCode();
            result.Dept = new List<string>();
            result.Enery = new Dictionary<string, List<decimal>>();
            result.ObjectName = new List<string>();
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();
            var objectList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
            model.IsDevice = 1;
            model.ObjectList = (from p in objectList select p.DeviceID).ToList<int>();
            model.ItemCode = objectList[0].ItemCodeID;
            model.Unit = ChartUnit.unit_hour;
            model.Starttime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd"));
            model.Endtime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd"));
            var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + model.ItemCode + "'", " order by ItemcodeID")[0];

            var resList = _reportBll.GetBaseEneryDataList(model);
            foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.DeviceID.ToString(CultureInfo.InvariantCulture)].Datas))
            {
                if (!eneryDataList.ContainsKey(d.DatePick))
                {
                    eneryDataList.Add(d.DatePick, d.DataValue);
                }
                else
                {
                    eneryDataList[d.DatePick] += d.DataValue;
                }
            }
            result.Enery.Add(objectList[0].DeviceID.ToString(CultureInfo.InvariantCulture), eneryDataList.Values.ToList());
            foreach (var e in eneryDataList)
            {
                result.Dept.Add(itemList.Unit);
                result.ObjectName.Add(e.Key);

            }
            return result;
        }


        public ItemList IndexItems(QueryEnergyIterm qery)
        {
            var pAction = new ExecuteProcess();
            try
            {
                Itemcode itcode = new NTS.WEB.BLL.Itemcode();


                List<Model.Itemcode> itemList = new List<Model.Itemcode>();
                if (qery.ItemCode != "00000")
                {
                    var itemParent = itcode.GetItemcodeList(
                      string.Format(" and  ItemCodeNumber='{0}'", qery.ItemCode),
                      " order by ItemcodeID");
                    if (itemParent.Count > 0)
                    {
                        itemList = itcode.GetItemcodeList(
                            string.Format("and  PARENTID= {0} ", itemParent[0].ItemcodeID.ToString()),
                            " order by ItemcodeID");

                    }
                }
                else
                {

                    itemList = itcode.GetItemcodeList(
                           string.Format("and  PARENTID= {0} ", 0),
                           " order by ItemcodeID");

                }
                ItemList result = new ItemList();
                result.ItemLst = new List<ItemSet>();
                foreach (Model.Itemcode ic in itemList)
                {
                    ItemSet its = new ItemSet();
                    its.ItemCode = ic.ItemCodeNumber;
                    its.ItemName = ic.ItemCodeName;
                    result.ItemLst.Add(its);

                }
                pAction.Success = true;
                result.ActionInfo = pAction;

                return result;
            }
            catch (Exception ex)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = ex.Message;
                return new ItemList() { ActionInfo = pAction };
            }

        }

        public ResultRealLine DeviceRealChart(RealQuery query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new ResultRealLine { series = new List<EneryAnalyseSeries>(), Unit = "kwh" };
                //ResultItemCode result = new ResultItemCode();
                //result.Dept = new List<string>();
                //result.Enery = new Dictionary<string, List<decimal>>();
                //result.ObjectName = new List<string>();
                // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
                var eneryDataList = new Dictionary<string, decimal>();
                var model = new BaseQueryModel();
                var objectList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
                model.IsDevice = 1;
                model.ObjectList = (from p in objectList select p.DeviceID).ToList<int>();
                model.ItemCode = objectList[0].ItemCodeID;
                model.Unit = ChartUnit.unit_hour;
                if (query.QueryType == EnergyAnalyseQueryType.Default)
                {
                    model.Starttime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    model.Endtime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (query.QueryType == EnergyAnalyseQueryType.MonthCompare)
                {
                    model.Starttime = Convert.ToDateTime(System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
                    model.Endtime = Convert.ToDateTime(System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
                }
                else if (query.QueryType == EnergyAnalyseQueryType.YearCompare)
                {
                    model.Starttime = Convert.ToDateTime(System.DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"));
                    model.Endtime = Convert.ToDateTime(System.DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"));
                }
                var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + model.ItemCode + "'", " order by ItemcodeID")[0];
                result.Unit = itemList.Unit;//by added wxy 
                var resList = _reportBll.GetBaseEneryDataList(model);
                foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.DeviceID.ToString(CultureInfo.InvariantCulture)].Datas))
                {
                    if (!eneryDataList.ContainsKey(d.DatePick))
                    {
                        eneryDataList.Add(d.DatePick, d.DataValue);
                    }
                    else
                    {
                        eneryDataList[d.DatePick] += d.DataValue;
                    }
                }

                List<decimal> dat = new List<decimal>();
                foreach (var e in eneryDataList)
                {
                    dat.Add(e.Value);

                }
                EneryAnalyseSeries eas = new EneryAnalyseSeries();
                eas.data = dat;

                result.series.Add(eas);

                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;


            }
            catch (Exception ex)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = ex.Message;
                return new ResultRealLine() { ActionInfo = pAction };

            }
        }

        public ResultCompare GetAreaCompareChart(QueryCompare query)
        {
            #region 定义区
            query.ClassId = 2;  // 给区域分类

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            var olist = query.ObjectNum.Aggregate(string.Empty, (current, q) => current + ("," + q.ToString()));

            if (olist.Length > 0)
            {
                List<Model.BaseLayerObject> objectList;
                if (query.ClassId == 1)
                {
                    objectList =
                      new BLL.BaseLayerObject().GetBaseLayerObjectList(
                           string.Format(" and layerobjectid in ({0})", olist.Substring(1)), " order by LayerObjectID");
                }
                else
                {
                    objectList =
  new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(
       string.Format(" and layerobjectid in ({0})", olist.Substring(1)), " order by LayerObjectID");
                }



                foreach (var o in objectList)
                {
                    var basicQuery = new BasicQuery
                    {
                        EndTime = query.EndTime,
                        StartTime = query.StartTime,
                        ItemCode = query.ItemCode,
                        Unit = query.Unit
                    };
                    basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetQueryLineChart(basicQuery);
                    //resultList.Dept = result.Dept;
                    resultList.ObjectName = result.ObjectName;
                    if (query.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
                        //resultList.Enery.Add(o.LayerObjectName, result.Enery[itemList.ItemCodeName]);
                        for (int i = 0; i < result.Dept.Count; i++)
                        {
                            resultList.Dept.Add(itemList.Unit);
                        }

                        resultList.Enery.Add(o.LayerObjectName, result.Enery[itemList.ItemCodeName].Select(p => p / decimal.Parse(o.AreaNum.ToString())).ToList());
                    }
                    else
                    {
                        var tempValue = new decimal[result.Enery[result.Enery.Keys.First<string>()].Count];
                        foreach (var re in result.Enery)
                        {
                            for (var i = 0; i < re.Value.Count; i++)
                            {
                                tempValue[i] += re.Value[i];
                            }
                        }
                        for (int i = 0; i < result.Dept.Count; i++)
                        {
                            resultList.Dept.Add("T");
                        }
                        resultList.Enery.Add(o.LayerObjectName, tempValue.ToList().Select(p => p / decimal.Parse(o.AreaNum.ToString())).ToList());
                    }
                }
                return resultList;
            }
            return null;
        }

        public ResultCompare GetCompareChart(QueryCompare query)
        {
            #region 定义区

            query.ClassId = 1;  // 给区域分类

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            var olist = query.ObjectNum.Aggregate(string.Empty, (current, q) => current + ("," + q.ToString()));
            if (olist.Length > 0)
            {
                List<Model.BaseLayerObject> objectList;
                if (query.ClassId == 1)
                {
                    objectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(
                           string.Format(" and layerobjectid in ({0})", olist.Substring(1)), " order by LayerObjectID");
                }
                else
                {
                    objectList = new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(
                    string.Format(" and layerobjectid in ({0})", olist.Substring(1)), " order by LayerObjectID");
                }


                foreach (var o in objectList)
                {
                    var basicQuery = new BasicQuery
                    {
                        EndTime = query.EndTime,
                        StartTime = query.StartTime,
                        ItemCode = query.ItemCode,
                        Unit = query.Unit
                    };
                    basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetQueryLineChart(basicQuery);
                    resultList.ObjectName = result.ObjectName;
                    if (query.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
                        resultList.Enery.Add(o.LayerObjectName, result.Enery[itemList.ItemCodeName]);
                        for (int i = 0; i < result.Dept.Count; i++)
                        {
                            resultList.Dept.Add(itemList.Unit);
                        }
                    }
                    else
                    {
                        var tempValue = new decimal[result.Enery[result.Enery.Keys.First<string>()].Count];
                        foreach (var re in result.Enery)
                        {
                            for (var i = 0; i < re.Value.Count; i++)
                            {
                                tempValue[i] += re.Value[i];
                            }
                        }
                        for (int i = 0; i < result.Dept.Count; i++)
                        {
                            resultList.Dept.Add("T");
                        }
                        resultList.Enery.Add(o.LayerObjectName, tempValue.ToList());
                    }
                }


                return resultList;
            }
            return null;

        }

        public ResultCompare GetCompareChartNew(QueryOrderObjects query)
        {
            #region 定义区

            query.ObjType = AreaType.Area;  // 给区域分类

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion



            //var olist = query.ObjectNum.Aggregate(string.Empty, (current, q) => current + ("," + q.ToString()));
            //if (olist.Length > 0)
            //{
            //    List<Model.BaseLayerObject> objectList;
            //    if (query.ClassId == 1)
            //    {
            //        objectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(
            //               string.Format(" and layerobjectid in ({0})", olist.Substring(1)), " order by LayerObjectID");
            //    }
            //    else
            //    {
            //        objectList = new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(
            //        string.Format(" and layerobjectid in ({0})", olist.Substring(1)), " order by LayerObjectID");
            //    }


            //    foreach (var o in objectList)
            //    {
            //        var basicQuery = new BasicQuery
            //        {
            //            EndTime = query.EndTime,
            //            StartTime = query.StartTime,
            //            ItemCode = query.ItemCode,
            //            Unit = query.Unit
            //        };
            //        basicQuery.ObjectNum = o.LayerObjectID;
            //        var result = GetQueryLineChart(basicQuery);
            //        resultList.ObjectName = result.ObjectName;
            //        if (query.ItemCode != "00000")
            //        {
            //            var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
            //            resultList.Enery.Add(o.LayerObjectName, result.Enery[itemList.ItemCodeName]);
            //            for (int i = 0; i < result.Dept.Count; i++)
            //            {
            //                resultList.Dept.Add(itemList.Unit);
            //            }
            //        }
            //        else
            //        {
            //            var tempValue = new decimal[result.Enery[result.Enery.Keys.First<string>()].Count];
            //            foreach (var re in result.Enery)
            //            {
            //                for (var i = 0; i < re.Value.Count; i++)
            //                {
            //                    tempValue[i] += re.Value[i];
            //                }
            //            }
            //            for (int i = 0; i < result.Dept.Count; i++)
            //            {
            //                resultList.Dept.Add("T");
            //            }
            //            resultList.Enery.Add(o.LayerObjectName, tempValue.ToList());
            //        }
            //    }
            //    return resultList;
            //}
            return null;

        }

        public ResultItemCode GetQueryLineChart(BasicQuery query)
        {
            #region 定义区

            var resultList = new ResultView.ResultItemCode
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion

            var dept = string.Empty;
            var objectList =
                new BLL.BaseLayerObject().GetBaseLayerObjectList(
                    string.Format(" and layerobjectid={0}", query.ObjectNum), " order by LayerObjectID");
            var itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
            if (itList.Count > 0)
            {
                dept = itList[0].Unit;
            }

            dept = query.ItemCode == "00000" ? "T" : dept;
            if (query.ItemCode == "00000")
            {
                // resultList.Dept = new List<string>() { "T" };
                var itemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
                var eneryDataListTotal = new Dictionary<string, Dictionary<string, decimal>>();
                var maxcode = "01000";
                var count = 0;
                foreach (var itemcode in itemList)
                {
                    query.ItemCode = itemcode.ItemCodeNumber;
                    var lineChart = GetSingleItemCodeByObject(query, 1);

                    if (count < lineChart.Count)
                    {
                        count = lineChart.Count;
                        maxcode = itemcode.ItemCodeNumber;
                    }
                    eneryDataListTotal.Add(itemcode.ItemCodeNumber, lineChart);

                    //  resultList.Enery.Add(itemcode.ItemCodeName, lineChart.DatePickEnery);
                }

                // 重新赋值
                foreach (var itemcode in itemList)
                {
                    var itemcode1 = itemcode;
                    foreach (var max in eneryDataListTotal[maxcode].Where(max => !eneryDataListTotal[itemcode1.ItemCodeNumber].ContainsKey(max.Key)))
                    {
                        eneryDataListTotal[itemcode.ItemCodeNumber].Add(max.Key, 0);
                    }
                    var temp = eneryDataListTotal[itemcode.ItemCodeNumber].OrderBy(p => Convert.ToDateTime(p.Key));
                    var tempOrder = temp.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
                    eneryDataListTotal[itemcode.ItemCodeNumber] = tempOrder;
                    resultList.Enery.Add(itemcode.ItemCodeName, eneryDataListTotal[itemcode.ItemCodeNumber].Values.ToList());
                }
                resultList.ObjectName = eneryDataListTotal[maxcode].Select(p => p.Key.ToString(CultureInfo.InvariantCulture)).ToList();
            }
            else
            {
                var item = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
                var lineChart = GetSingleItemCodeByObject(query, 0);
                // resultList.Dept = new List<string>() {item.Unit};
                resultList.ObjectName = lineChart.Select(p => p.Key.ToString()).ToList();
                resultList.Enery.Add(item.ItemCodeName, lineChart.Select(p => decimal.Parse(p.Value.ToString())).ToList());
            }
            foreach (var baseLayerObject in resultList.ObjectName)
            {
                resultList.Dept.Add(dept);
            }
            return resultList;

        }
        private ChartUnit GetUnit(DateTime sTime, DateTime eTime)
        {
            if (sTime == eTime)
            {
                return ChartUnit.unit_hour;
            }
            else if (eTime <= sTime.AddMonths(1))
            {
                return ChartUnit.unit_day;
            }
            else if (eTime > sTime.AddHours(1))
            {
                return ChartUnit.unit_month;
            }
            else
            {
                return ChartUnit.unit_day;
            }
        }

        private void VerifyPersonOrAreaExist(NTS.WEB.DataContact.QueryAnalyse query)
        {
            if (query.IsDevice == 0)
            {

                switch (query.QueryType)
                {
                    case EnergyAnalyseQueryType.UnitArea:
                    case EnergyAnalyseQueryType.UnitPerson:
                        string where = "";
                        List<Model.BaseLayerObject> LayerObjectList;
                        where = string.Format(@"and layerobjectid={0} ", query.ObjectId);
                        if (query.ObjType == AreaType.Liquid)
                        {//取液态
                            LayerObjectList = new DAL.BaseLayerObject().GetBaseFuncLayerObjectList(where,
                                                                                                   string.Format(
                                                                                                       " order by LayerObjectID"));
                        }
                        else
                        {
                            LayerObjectList = new DAL.BaseLayerObject().GetBaseLayerObjectList(where,
                                                                                               string.Format(
                                                                                                   " order by LayerObjectID"));
                        }
                        if (LayerObjectList.Count > 0)
                        {
                            switch (query.QueryType)
                            {
                                case EnergyAnalyseQueryType.UnitArea:
                                    if (!(LayerObjectList[0].AreaNum > 0))
                                    {
                                        throw new Exception(string.Format("位置：【{0}】 单位面积尚未配置",
                                                                          LayerObjectList[0].LayerObjectName));
                                    }
                                    break;
                                case EnergyAnalyseQueryType.UnitPerson:
                                    if (!(LayerObjectList[0].PersonNum > 0))
                                    {
                                        throw new Exception(string.Format("位置：【{0}】 人均尚未配置",
                                                                          LayerObjectList[0].LayerObjectName));
                                    }
                                    break;
                            }
                        }
                        break;
                }


            }
        }
        public ResultEnergyAnalyse GetEnergyAnalyseLineChart_OLD(QueryAnalyse query)
        {
            try
            {


                VerifyPersonOrAreaExist(query);
                #region 返回对象定义
                ResultEnergyAnalyse result = new ResultEnergyAnalyse()
                {
                    OrderLst = new List<EnergyOrder>(),
                    series = new List<EneryAnalyseSeries>()

                };
                result.series.Add(new EneryAnalyseSeries() { data = new List<decimal>() });
                #endregion

                List<Model.Itemcode> itemCodeList = new List<Model.Itemcode>();
                if (query.IsDevice == 0)
                {
                    if (query.ItemCode == "00000")
                    {//总能耗
                        itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0 ", " order by ItemcodeID");
                        result.Unit = "T";//标准煤单位
                        result.series[0].name = "总能耗";
                    }
                    else
                    {
                        itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                        if (itemCodeList.Count > 0)
                        {
                            result.Unit = itemCodeList[0].Unit;//单个分类分项单位
                            result.series[0].name = itemCodeList[0].ItemCodeName;

                            //var itemchildCodeList = new BLL.Itemcode().GetItemcodeList(string.Format(" and ParentID={0} ", itemCodeList[0].ItemcodeID), " order by ItemcodeID");

                            //if (itemchildCodeList.Count > 0)
                            //{
                            //    itemCodeList = itemchildCodeList;
                            //}
                        }

                    }
                }
                else
                {
                    var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
                    itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + deviceList[0].ItemCodeID + "'", " order by ItemcodeID");
                    if (itemCodeList.Count > 0)
                    {
                        result.Unit = itemCodeList[0].Unit;//单个分类分项单位
                        result.series[0].name = itemCodeList[0].ItemCodeName;
                    }
                    if (query.ItemCode == "00000")
                    {//总能耗
                    
                        result.Unit = "T";//标准煤单位
                        result.series[0].name = "总能耗";
                    }

                }

                switch (query.QueryType)
                {
                    case EnergyAnalyseQueryType.Convert2Co2:
                    case EnergyAnalyseQueryType.Convert2Coal:
                        result.Unit = "T";//标准煤单位
                        break;
                    case EnergyAnalyseQueryType.Convert2Money:
                        result.Unit = "元";//标准煤单位
                        break;
                }

                DateTime tempStartTime = query.StartTime;
                DateTime tempEndTime = query.EndTime;
                switch (query.QueryType)
                {
                    case EnergyAnalyseQueryType.YearCompare://同比值
                        tempStartTime = tempStartTime.AddYears(-1);
                        tempEndTime = tempEndTime.AddYears(-1);
                        break;
                    case EnergyAnalyseQueryType.MonthCompare://环比值
                        tempStartTime = tempStartTime.AddMonths(-1);
                        tempEndTime = tempEndTime.AddMonths(-1);

                        break;
                }
                Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
                Dictionary<string, decimal> tempConvertHour = new Dictionary<string, decimal>();
                var model = new BaseQueryModel();
                model.IsDevice = query.IsDevice;
                model.ObjectList = new List<int>();

                model.ObjectList = new List<int>() { query.ObjectId };

                //if (model.IsDevice == 0)
                //{//区域
                //    model.ObjectList = new List<int>() { query.ObjectId };
                //}
                //else
                //{
                //    var device = new BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
                //    if (device.Count > 0)
                //    {
                //        model.ObjectList = (from p in device select p.DeviceID).ToList<int>();
                //    }
                //}
                //model.Unit = GetUnit(query.StartTime, query.EndTime);
                model.Unit = BaseTool.GetChartUnit(query.particle);
                if (model.Unit == ChartUnit.unit_month)
                {
                    model.Starttime = Convert.ToDateTime(tempStartTime.ToString("yyyy-MM-1"));
                    model.Endtime = Convert.ToDateTime(tempEndTime.ToString("yyyy-MM-1"));
                }
                else
                {
                    model.Starttime = tempStartTime;
                    model.Endtime = tempEndTime;
                }
                int templastmonthdays = 0;
                switch (model.Unit)
                {
                    case ChartUnit.unit_month:
                        for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddMonths(1))
                        {
                            tempConvert.Add(i.ToString("yyyy-MM-dd"), 0);
                        }
                        break;
                    case ChartUnit.unit_hour:
                        for (DateTime i = model.Starttime; i < model.Endtime.AddDays(1); i = i.AddHours(1))
                        {
                            tempConvert.Add(i.ToString("yyyy-MM-dd HH:00:00"), 0);
                        }
                        break;
                    case ChartUnit.unit_day:
                        if ((DateTime.Now- model.Endtime).Days>0)
                        {
                            for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddDays(1))
                            {
                                tempConvert.Add(i.ToString(("yyyy-MM-dd")), 0);
                            }
                        }
                        else
                        {
                            for (DateTime i = model.Starttime; i <= model.Endtime.AddDays(-1); i = i.AddDays(1))
                            {
                                tempConvert.Add(i.ToString(("yyyy-MM-dd")), 0);
                            }
                            for (DateTime i = model.Endtime; i <= DateTime.Now; i = i.AddHours(1))
                            {
                                tempConvertHour.Add(i.ToString("yyyy-MM-dd HH:00:00"), 0);
                            }

                        }

                        templastmonthdays = (model.Endtime - model.Starttime).Days;
                        break;
                }
                string objName = "";
                foreach (var item in itemCodeList)
                {
                    model.ItemCode = item.ItemCodeNumber;

                    BaseResult resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);

                    foreach (var r in resList.BaseLayerObjectResults)
                    {
                        if (model.IsDevice == 0)
                        {
                            objName = r.Value.baseLayerObject.LayerObjectName;//取区域名称
                        }
                        else
                        {
                            objName = r.Value.device.DeviceName;
                        }
                        foreach (var rr in r.Value.Datas)
                        {
                            if (query.ItemCode == "00000")
                            {
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default://默认
                                    case EnergyAnalyseQueryType.YearCompare://默认
                                    case EnergyAnalyseQueryType.MonthCompare://默认
                                        tempConvert[rr.DatePick] += rr.CoalDataValue;
                                        break;
                                    case EnergyAnalyseQueryType.UnitArea://单位面积
                                        tempConvert[rr.DatePick] += rr.CoalDataValue / decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());

                                        break;
                                    case EnergyAnalyseQueryType.UnitPerson://人均
                                        tempConvert[rr.DatePick] += rr.CoalDataValue / decimal.Parse(r.Value.baseLayerObject.PersonNum.ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Coal://
                                        tempConvert[rr.DatePick] += rr.CoalDataValue;
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Co2://
                                        tempConvert[rr.DatePick] += rr.Co2DataValue;
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Money://
                                        tempConvert[rr.DatePick] += rr.MoneyDataValue;
                                        break;
                                }
                            }
                            else
                            {
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default://默认
                                    case EnergyAnalyseQueryType.YearCompare://默认
                                    case EnergyAnalyseQueryType.MonthCompare://默认
                                        tempConvert[rr.DatePick] += rr.DataValue;
                                        break;
                                    case EnergyAnalyseQueryType.UnitArea://单位面积
                                        tempConvert[rr.DatePick] += rr.DataValue / decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());

                                        break;
                                    case EnergyAnalyseQueryType.UnitPerson://人均
                                        tempConvert[rr.DatePick] += rr.DataValue / decimal.Parse(r.Value.baseLayerObject.PersonNum.ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Coal://
                                        tempConvert[rr.DatePick] += rr.CoalDataValue;
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Co2://
                                        tempConvert[rr.DatePick] += rr.Co2DataValue;
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Money://
                                        tempConvert[rr.DatePick] += rr.MoneyDataValue;
                                        break;
                                }

                            }
                        }
                    }
                }
                int order = 1;
                foreach (var item in tempConvert)
                {

                    var time = item.Key;
                    if (model.Unit == ChartUnit.unit_month)
                    {
                        time = Convert.ToDateTime(time).ToString("yyyy-MM");
                    }

                    decimal value = decimal.Round(item.Value, 2);
                    result.series[0].data.Add(value);
                    result.OrderLst.Add(new EnergyOrder()
                    {
                        Order = order,
                        Tm = time,
                        Obj = objName,
                        Val = value,
                        EneType = result.series[0].name
                        // EneType =itemCodeList.Count > 1 ? "总能耗" : itemCodeList[0].ItemCodeName

                    });
                    order++;
                }
                if (query.QueryType == EnergyAnalyseQueryType.MonthCompare)
                {
                    int tempcurrentdays = (query.EndTime - query.StartTime).Days;
                    //int index = (28 - query.StartTime.Day) < 0 ? 0 : (28 - query.StartTime.Day);

                    if (model.Unit == ChartUnit.unit_day)
                    {
                        if (tempcurrentdays > templastmonthdays)
                        {//当前时间范围大于上月环比范围
                            int index = System.DateTime.DaysInMonth(query.StartTime.AddMonths(-1).Year, query.StartTime.AddMonths(-1).Month) - query.StartTime.Day;
                            if (index < 0)
                            {
                                index = 0;
                            }
                            for (int i = templastmonthdays; i < tempcurrentdays; i++)
                            {
                                result.series[0].data.Insert(index, result.series[0].data[index]);
                            }

                        }
                        else
                        {
                            if (tempcurrentdays < templastmonthdays)
                            {
                                int index = System.DateTime.DaysInMonth(query.StartTime.AddMonths(-1).Year, query.StartTime.AddMonths(-1).Month) - query.StartTime.Day;
                                for (int i = tempcurrentdays; i < templastmonthdays; i++)
                                {
                                    result.series[0].data.RemoveAt(index);
                                    index--;
                                }
                            }
                        }

                        //for (int i = 0; i < ((query.EndTime - query.StartTime).Days - tempdaycount); i++)
                        //{
                        //    result.series[0].data.Insert(index, result.series[0].data[index]);

                        //}

                    }
                }


                return result;
            }
            catch (Exception ee)
            {
                throw ee;
            }

        }

        public ResultEnergyAnalyse GetEnergyAnalyseLineChart(QueryAnalyse query)
        {
            try
            {
                VerifyPersonOrAreaExist(query);
                #region 返回对象定义
                ResultEnergyAnalyse result = new ResultEnergyAnalyse()
                {
                    OrderLst = new List<EnergyOrder>(),
                    series = new List<EneryAnalyseSeries>()

                };
                result.series.Add(new EneryAnalyseSeries() { data = new List<decimal>() });
                #endregion

                List<Model.Itemcode> itemCodeList = new List<Model.Itemcode>();
                if (query.IsDevice == 0)
                {
                    if (query.ItemCode == "00000")
                    {//总能耗
                        itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0 ", " order by ItemcodeID");
                        result.Unit = "T";//标准煤单位
                        result.series[0].name = "总能耗";
                    }
                    else
                    {
                        itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                        if (itemCodeList.Count > 0)
                        {
                            result.Unit = itemCodeList[0].Unit;//单个分类分项单位
                            result.series[0].name = itemCodeList[0].ItemCodeName;
                        }

                    }
                }
                else
                {
                    var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
                    itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + deviceList[0].ItemCodeID + "'", " order by ItemcodeID");
                    if (itemCodeList.Count > 0)
                    {
                        result.Unit = itemCodeList[0].Unit;//单个分类分项单位
                        result.series[0].name = itemCodeList[0].ItemCodeName;
                    }
                    if (query.ItemCode == "00000")
                    {//总能耗

                        result.Unit = "T";//标准煤单位
                        result.series[0].name = "总能耗";
                    }

                }

                switch (query.QueryType)
                {
                    case EnergyAnalyseQueryType.Convert2Co2:
                    case EnergyAnalyseQueryType.Convert2Coal:
                        result.Unit = "T";//标准煤单位
                        break;
                    case EnergyAnalyseQueryType.Convert2Money:
                        result.Unit = "元";//标准煤单位
                        break;
                }

                DateTime tempStartTime = query.StartTime;
                DateTime tempEndTime = query.EndTime;
                if ((DateTime.Now - tempEndTime).Days == 0)
                {
                    tempEndTime = DateTime.Now;
                }
                switch (query.QueryType)
                {
                    case EnergyAnalyseQueryType.YearCompare://同比值
                        tempStartTime = tempStartTime.AddYears(-1);
                        tempEndTime = tempEndTime.AddYears(-1);
                        break;
                    case EnergyAnalyseQueryType.MonthCompare://环比值
                        
                        tempStartTime = tempStartTime.AddMonths(-1);
                        tempEndTime = tempEndTime.AddMonths(-1);

                        break;
                }
                Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
                Dictionary<string, decimal> tempConvertHour = new Dictionary<string, decimal>();
                var model = new BaseQueryModel();
                model.IsDevice = query.IsDevice;
                model.ObjectList = new List<int>();

                model.ObjectList = new List<int>() { query.ObjectId };

              
                model.Unit = BaseTool.GetChartUnit(query.particle);
                if (model.Unit == ChartUnit.unit_month)
                {
                    model.Starttime = Convert.ToDateTime(tempStartTime.ToString("yyyy-MM-1"));
                    model.Endtime = Convert.ToDateTime(tempEndTime.ToString("yyyy-MM-1"));
                }
                else
                {
                    model.Starttime = tempStartTime;
                    model.Endtime = tempEndTime;
                }
                int templastmonthdays = 0;
                switch (model.Unit)
                {
                    case ChartUnit.unit_month:
                        for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddMonths(1))
                        {
                            tempConvert.Add(i.ToString("yyyy-MM-dd"), 0);
                        }
                        break;
                    case ChartUnit.unit_hour:
                        for (DateTime i = model.Starttime; i < model.Endtime.AddDays(1); i = i.AddHours(1))
                        {
                            tempConvert.Add(i.ToString("yyyy-MM-dd HH:00:00"), 0);
                        }
                        break;
                    case ChartUnit.unit_day:
                        templastmonthdays = (model.Endtime - model.Starttime).Days;
                        if (model.Endtime.Hour== 0)
                        {
                            for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddDays(1))
                            {
                                tempConvert.Add(i.ToString(("yyyy-MM-dd")), 0);
                            }
                        }
                        else
                        {
                           
                            for (DateTime i = model.Starttime; i <= model.Endtime.AddDays(-1); i = i.AddDays(1))
                            {
                                tempConvert.Add(i.ToString(("yyyy-MM-dd")), 0);
                            }

                            for (DateTime i = DateTime.Parse(model.Endtime.ToString("yyyy-MM-dd 00:00:00")); i <= model.Endtime; i = i.AddHours(1))
                            {
                                tempConvertHour.Add(i.ToString("yyyy-MM-dd HH:00:00"), 0);
                            }
                            model.Endtime = DateTime.Parse(tempEndTime.AddDays(-1).ToString("yyyy-MM-dd"));
                        }

                       
                        break;
                }
                string objName = "";

                decimal temphourValue = 0;
                foreach (var item in itemCodeList)
                {
                    model.ItemCode = item.ItemCodeNumber;
                    GenerateEnergyDicValue(model, query, tempConvert,ref objName);
                    if (tempConvertHour.Count>0)
                    {
                        model.Unit = ChartUnit.unit_hour;
                        model.Starttime = DateTime.Parse(tempEndTime.ToString("yyyy-MM-dd 00:00:00"));
                        model.Endtime = DateTime.Parse(tempEndTime.ToString("yyyy-MM-dd HH:00:00"));
                        GenerateEnergyDicValue(model, query, tempConvertHour, ref objName);
                        temphourValue = tempConvertHour.Sum(i => i.Value);
                     
                        
                        model.Starttime = tempStartTime;
                        model.Endtime = DateTime.Parse(tempEndTime.AddDays(-1).ToString("yyyy-MM-dd"));
                        model.Unit = ChartUnit.unit_day;
                    }

                }
                if (tempConvertHour.Count > 0)
                {
                    tempConvert.Add(DateTime.Now.ToString("yyyy-MM-dd"), temphourValue);
                }
                int order = 1;
                foreach (var item in tempConvert)
                {

                    var time = item.Key;
                    if (model.Unit == ChartUnit.unit_month)
                    {
                        time = Convert.ToDateTime(time).ToString("yyyy-MM");
                    }

                    decimal value = decimal.Round(item.Value, 2);
                    result.series[0].data.Add(value);
                    result.OrderLst.Add(new EnergyOrder()
                    {
                        Order = order,
                        Tm = time,
                        Obj = objName,
                        Val = value,
                        EneType = result.series[0].name
                        // EneType =itemCodeList.Count > 1 ? "总能耗" : itemCodeList[0].ItemCodeName

                    });
                    order++;
                }
                if (query.QueryType == EnergyAnalyseQueryType.MonthCompare)
                {
                    int tempcurrentdays = (query.EndTime - query.StartTime).Days;
                    //int index = (28 - query.StartTime.Day) < 0 ? 0 : (28 - query.StartTime.Day);

                    if (model.Unit == ChartUnit.unit_day)
                    {
                        if (tempcurrentdays > templastmonthdays)
                        {//当前时间范围大于上月环比范围
                            int index = System.DateTime.DaysInMonth(query.StartTime.AddMonths(-1).Year, query.StartTime.AddMonths(-1).Month) - query.StartTime.Day;
                            if (index < 0)
                            {
                                index = 0;
                            }
                            for (int i = templastmonthdays; i < tempcurrentdays; i++)
                            {
                                result.series[0].data.Insert(index, result.series[0].data[index]);
                            }

                        }
                        else
                        {
                            if (tempcurrentdays < templastmonthdays)
                            {
                                int index = System.DateTime.DaysInMonth(query.StartTime.AddMonths(-1).Year, query.StartTime.AddMonths(-1).Month) - query.StartTime.Day;
                                for (int i = tempcurrentdays; i < templastmonthdays; i++)
                                {
                                    result.series[0].data.RemoveAt(index);
                                    index--;
                                }
                            }
                        }

                        //for (int i = 0; i < ((query.EndTime - query.StartTime).Days - tempdaycount); i++)
                        //{
                        //    result.series[0].data.Insert(index, result.series[0].data[index]);

                        //}

                    }
                }


                return result;
            }
            catch (Exception ee)
            {
                throw ee;
            }

        }

        private Dictionary<string, decimal> GenerateEnergyDicValue(BaseQueryModel model, QueryAnalyse query, Dictionary<string, decimal> tempConvert,ref string objName)
        {
          

             BaseResult resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);

             foreach (var r in resList.BaseLayerObjectResults)
             {
                 if (model.IsDevice == 0)
                 {
                     objName = r.Value.baseLayerObject.LayerObjectName; //取区域名称
                 }
                 else
                 {
                     objName = r.Value.device.DeviceName;
                 }
                 foreach (var rr in r.Value.Datas)
                 {
                     if (query.ItemCode == "00000")
                     {
                         switch (query.QueryType)
                         {
                             case EnergyAnalyseQueryType.Default: //默认
                             case EnergyAnalyseQueryType.YearCompare: //默认
                             case EnergyAnalyseQueryType.MonthCompare: //默认
                                 tempConvert[rr.DatePick] += rr.CoalDataValue;
                                 break;
                             case EnergyAnalyseQueryType.UnitArea: //单位面积
                                 tempConvert[rr.DatePick] += rr.CoalDataValue/
                                                             decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());

                                 break;
                             case EnergyAnalyseQueryType.UnitPerson: //人均
                                 tempConvert[rr.DatePick] += rr.CoalDataValue/
                                                             decimal.Parse(r.Value.baseLayerObject.PersonNum.ToString());
                                 break;
                             case EnergyAnalyseQueryType.Convert2Coal: //
                                 tempConvert[rr.DatePick] += rr.CoalDataValue;
                                 break;
                             case EnergyAnalyseQueryType.Convert2Co2: //
                                 tempConvert[rr.DatePick] += rr.Co2DataValue;
                                 break;
                             case EnergyAnalyseQueryType.Convert2Money: //
                                 tempConvert[rr.DatePick] += rr.MoneyDataValue;
                                 break;
                         }
                     }
                     else
                     {
                         switch (query.QueryType)
                         {
                             case EnergyAnalyseQueryType.Default: //默认
                             case EnergyAnalyseQueryType.YearCompare: //默认
                             case EnergyAnalyseQueryType.MonthCompare: //默认
                                 tempConvert[rr.DatePick] += rr.DataValue;
                                 break;
                             case EnergyAnalyseQueryType.UnitArea: //单位面积
                                 tempConvert[rr.DatePick] += rr.DataValue/
                                                             decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());

                                 break;
                             case EnergyAnalyseQueryType.UnitPerson: //人均
                                 tempConvert[rr.DatePick] += rr.DataValue/
                                                             decimal.Parse(r.Value.baseLayerObject.PersonNum.ToString());
                                 break;
                             case EnergyAnalyseQueryType.Convert2Coal: //
                                 tempConvert[rr.DatePick] += rr.CoalDataValue;
                                 break;
                             case EnergyAnalyseQueryType.Convert2Co2: //
                                 tempConvert[rr.DatePick] += rr.Co2DataValue;
                                 break;
                             case EnergyAnalyseQueryType.Convert2Money: //
                                 tempConvert[rr.DatePick] += rr.MoneyDataValue;
                                 break;
                         }

                     }
                 }
             }
            return tempConvert;
        }

        public EnergyAnalyseCompare GetEnergyAnalyseCompare(QueryAnalyse query)
        {
            try
            {

                EnergyAnalyseCompare result = new EnergyAnalyseCompare();
                query.QueryType = EnergyAnalyseQueryType.Default;

                ResultEnergyAnalyse temp = GetEnergyAnalyseLineChart(query);
                if (temp != null)
                {
                    result.Unit = temp.Unit;
                    result.MaxValue = decimal.Round(temp.OrderLst.Select(t => t.Val).Max(), 2);
                    result.MinValue = decimal.Round(temp.OrderLst.Select(t => t.Val).Min(), 2);
                    result.AverageValue = decimal.Round(temp.OrderLst.Select(t => t.Val).Average(), 2);
                    result.TotalValue = decimal.Round(temp.OrderLst.Select(t => t.Val).Sum(), 2);
                }
                query.QueryType = EnergyAnalyseQueryType.YearCompare;
                temp = GetEnergyAnalyseLineChart(query);
                if (temp != null)
                {
                    result.LastYearTotalValue = temp.OrderLst.Select(t => t.Val).Sum();
                    if (result.LastYearTotalValue > 0)
                    {
                        result.LastYearCompare = decimal.Round(100 * (result.TotalValue - result.LastYearTotalValue) / result.LastYearTotalValue, 2)
                                 .ToString(CultureInfo.InvariantCulture);
                    }
                }

                query.QueryType = EnergyAnalyseQueryType.MonthCompare;
                temp = GetEnergyAnalyseLineChart(query);
                if (temp != null)
                {
                    result.LastMonthTotalValue = temp.OrderLst.Select(t => t.Val).Sum();
                    if (result.LastMonthTotalValue > 0)
                    {
                        result.LastMonthCompare =
                            decimal.Round(100 * (result.TotalValue - result.LastMonthTotalValue) / result.LastMonthTotalValue,
                                          2).ToString(CultureInfo.InvariantCulture);

                    }
                }

                return result;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public ResultEnergyAnalysePie GetEnergyAnalysePie(QueryAnalyse query)
        {
            try
            {

                VerifyPersonOrAreaExist(query);

                ResultEnergyAnalysePie result = new ResultEnergyAnalysePie()
                {
                    LayerPie = new PieHighChart() { series = new List<Series>() },
                    ItemCodePie = new PieHighChart() { series = new List<Series>() }
                };
                result.LayerPie.series.Add(new Series() { data = new List<EneryHighChart>() });
                result.ItemCodePie.series.Add(new Series() { data = new List<EneryHighChart>() });
                query.QueryType = EnergyAnalyseQueryType.Default;

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

                        var itemchildCodeList = new BLL.Itemcode().GetItemcodeList(string.Format(" and ParentID={0} ", itemCodeList[0].ItemcodeID), " order by ItemcodeID");

                        if (itemchildCodeList.Count > 0)
                        {
                            itemCodeList = itemchildCodeList;
                        }
                    }
                }
                switch (query.QueryType)
                {
                    case EnergyAnalyseQueryType.Convert2Co2:
                    case EnergyAnalyseQueryType.Convert2Coal:
                        result.Unit = "T";//标准煤单位
                        break;
                    case EnergyAnalyseQueryType.Convert2Money:
                        result.Unit = "元";//标准煤单位
                        break;
                }
                var model = new BaseQueryModel();
                model.IsDevice = query.IsDevice;
                model.ObjectList = new List<int>() { query.ObjectId };
                model.IsDevice = 0;//只获取区域的
                if (model.IsDevice == 0)
                {//区域
                    //model.ObjectList = (from p in LayerObjectList select p.LayerObjectID).ToList<int>();
                    model.ObjectList = query.ObjectChildren;
                    if (model.ObjectList.Count == 0)
                    {//如果选择的区域对象下没有子集则取当前对象能耗，否则取各子集能耗
                        model.ObjectList = new List<int>() { query.ObjectId };
                    }
                }
                else
                {//设备只取当前对象能耗

                    //var device = new BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
                    //if (device.Count > 0)
                    //{
                    //    model.ObjectList = (from p in device select p.DeviceID).ToList<int>();
                    //}
                }

                //model.Unit = GetUnit(query.StartTime, query.EndTime);
                model.Unit = BaseTool.GetChartUnit(query.particle);
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

                Dictionary<string, decimal> tempLayerConvert = new Dictionary<string, decimal>();
                Dictionary<string, decimal> tempItemcodeConvert = new Dictionary<string, decimal>();
                if (query.ObjectChildren.Count == 0)
                {//当前选项没有子节点，则layer能耗和分类分项能耗就取当前选中节点的能耗值

                    foreach (var item in itemCodeList)
                    {
                        tempItemcodeConvert.Add(item.ItemCodeName, 0);

                        model.ItemCode = item.ItemCodeNumber;
                        BaseResult resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);
                        foreach (var r in resList.BaseLayerObjectResults)
                        {
                            var key = "";
                            if (model.IsDevice == 0)
                            {
                                key = r.Value.baseLayerObject.LayerObjectName;
                            }
                            else
                            {
                                key = r.Value.device.DeviceName;
                            }
                            if (!tempLayerConvert.ContainsKey(key))
                            {
                                tempLayerConvert.Add(key, 0);
                            }
                            if (query.ItemCode == "00000")
                            {//选择总能耗后把其他分类分项的能耗转化成标准煤
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default: //默认总能耗
                                        tempLayerConvert[key] +=
                                  decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());//
                                        tempItemcodeConvert[item.ItemCodeName] += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                                        break;
                                    default:
                                        tempLayerConvert[key] +=
                                decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                                        tempItemcodeConvert[item.ItemCodeName] += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                                        break;
                                }
                            }
                            else
                            {
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default: //默认总能耗
                                        tempLayerConvert[key] += r.Value.Total;
                                        tempItemcodeConvert[item.ItemCodeName] += r.Value.Total;
                                        break;
                                    default:
                                        tempLayerConvert[key] += r.Value.Total;
                                        tempItemcodeConvert[item.ItemCodeName] += r.Value.Total;
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    model.ObjectList = query.ObjectChildren;//先获取layer子节点能耗值
                    foreach (var item in itemCodeList)
                    {

                        model.ItemCode = item.ItemCodeNumber;
                        BaseResult resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);
                        foreach (var r in resList.BaseLayerObjectResults)
                        {
                            var key = "";
                            if (model.IsDevice == 0)
                            {
                                key = r.Value.baseLayerObject.LayerObjectName;
                            }
                            else
                            {
                                key = r.Value.device.DeviceName;
                            }
                            if (!tempLayerConvert.ContainsKey(key))
                            {
                                tempLayerConvert.Add(key, 0);
                            }
                            if (query.ItemCode == "00000")
                            {//选择总能耗后把其他分类分项的能耗转化成标准煤
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default: //默认总能耗
                                        tempLayerConvert[key] +=
                                  decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());//

                                        break;
                                    default:
                                        tempLayerConvert[key] +=
                                decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());

                                        break;
                                }
                            }
                            else
                            {
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default: //默认总能耗
                                        tempLayerConvert[key] += r.Value.Total;

                                        break;
                                    default:
                                        tempLayerConvert[key] += r.Value.Total;

                                        break;
                                }


                            }


                        }

                    }


                    model.ObjectList = new List<int>() { query.ObjectId };//获取分类分项能耗值
                    foreach (var item in itemCodeList)
                    {
                        tempItemcodeConvert.Add(item.ItemCodeName, 0);
                        model.ItemCode = item.ItemCodeNumber;
                        BaseResult resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);

                        foreach (var r in resList.BaseLayerObjectResults)
                        {
                            if (query.ItemCode == "00000")
                            {//选择总能耗后把其他分类分项的能耗转化成标准煤
                                tempItemcodeConvert[item.ItemCodeName] += decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                            }
                            else
                            {
                                tempItemcodeConvert[item.ItemCodeName] += r.Value.Total;
                            }
                        }

                    }
                }
                var tempDesc = from tt in tempLayerConvert orderby tt.Value descending select tt;//PieChart按照能耗值从高到低排序
                int tempindex = 1;
                decimal tempcount = 0;
                foreach (var item in tempDesc)
                {
                    decimal value = decimal.Round(item.Value, 2);
                    if (tempindex < 11)
                    {
                        result.LayerPie.series[0].data.Add(new EneryHighChart()
                        {
                            name = item.Key,
                            y = value
                        });
                    }
                    else
                    {
                        tempcount += value;
                    }

                    tempindex++;
                }
                if (tempindex > 11)
                {
                    result.LayerPie.series[0].data.Add(new EneryHighChart()
                    {
                        name = "其他",
                        y = tempcount
                    });
                }
                tempDesc = from tt in tempItemcodeConvert orderby tt.Value descending select tt;//PieChart按照能耗值从高到低排序
                tempindex = 1;
                tempcount = 0;
                foreach (var item in tempDesc)
                {
                    decimal value = decimal.Round(item.Value, 2);
                    if (tempindex < 11)
                    {
                        result.ItemCodePie.series[0].data.Add(new EneryHighChart()
                        {
                            name = item.Key,
                            y = value
                        });
                    }
                    else
                    {
                        tempcount += value;
                    }
                    tempindex++;
                }
                if (tempindex > 10)
                {
                    result.ItemCodePie.series[0].data.Add(new EneryHighChart()
                    {
                        name = "其他",
                        y = tempcount
                    });
                }
                return result;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }




        /// <summary>
        /// 获取定额分析
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultQuota GetQuotaAnalyseChart(QueryQuota query)
        {
            try
            {

                #region 返回对象定义
                ResultQuota result = new ResultQuota()
                {
                    Pie = new QuotaAnalysePie(),
                    BalanceHighChart = new QuotaHighChart() { series = new List<EneryAnalyseSeries>() },
                    TrendHighChart = new QuotaHighChart() { series = new List<EneryAnalyseSeries>() }
                };
                #endregion

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
                    }
                }
                #endregion

                #region 生成EndTime

                switch (query.Particle)
                {
                    case Particle.Month://月 query.StartTime格式为yyyy-MM-01
                        query.EndTime = query.StartTime.AddMonths(1).AddDays(-1);
                        int day = DateTime.Now.Day;
                        if (query.StartTime.Year == DateTime.Now.Year && query.StartTime.Month == DateTime.Now.Month )//月、年第一天不算进去
                        {//当月

                            query.EndTime = DateTime.Now;//精确到前一天的能耗值
                           
                        }
                        break;
                    case Particle.Year://年  query.StartTime格式为yyyy-01-01
                        query.EndTime = query.StartTime.AddYears(1).AddMonths(-1);
                        if (query.StartTime.Year == DateTime.Now.Year)//月、年第一天不算进去
                        {
                            query.EndTime = DateTime.Now;
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

                #region 获取定额值
                Model.QuotaAnalyseModel quotaAnalyseModel = GetQuotaAnalyseModel(query);
                if (quotaAnalyseModel != null)
                {
                    result.Pie.QuotaValue = decimal.Round(quotaAnalyseModel.QuotaValue, 2); 
                    if (!(quotaAnalyseModel.QuotaValue > 0))
                    {
                        throw new Exception("无效的定额值");
                    }
                }
                else
                {
                     throw new Exception("当前未设置定额值");
                    //result.Pie.QuotaValue = 10000;
                }
                #endregion


                var model = new BaseQueryModel();
                model.IsDevice = 0;
                model.ObjectList = new List<int>() { query.ObjectId };
                model.Unit = BaseTool.GetChartUnit((int)query.Particle - 1);//查询颗粒度比方法中的颗粒度大于一
                decimal averagequotavalue = 0;
                int TotalCount = 1;//月天数或者12月
                if (model.Unit == ChartUnit.unit_month)
                {//年定额分析 精确到天
                    model.Starttime = Convert.ToDateTime(query.StartTime.ToString("yyyy-MM-01"));
                    model.Endtime = Convert.ToDateTime(query.EndTime.ToString("yyyy-MM-01"));
                    //定额月平均值
                    averagequotavalue = decimal.Round(result.Pie.QuotaValue / 12, 2);
                    TotalCount = 12;
                }
                else
                {//月定额分析 精确到小时
                    model.Starttime = query.StartTime;
                    model.Endtime = query.EndTime;
                    //定额日平均值
                    TotalCount = DateTime.DaysInMonth(query.StartTime.Year, query.StartTime.Month);
                    averagequotavalue = decimal.Round(result.Pie.QuotaValue / TotalCount, 2);
                   
                }


                Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
                if(query.EndTime>=query.StartTime)
                {
                    tempConvert = GetSingleItemCodeValue(model, query, itemCodeList);
                }
               

                result.Pie.ActualValue = decimal.Round(tempConvert.Select(t => t.Value).Sum(), 2);
                result.Pie.OverPlusValue = decimal.Round(result.Pie.QuotaValue - result.Pie.ActualValue, 2);
                result.Pie.OverPlusPercent = (result.Pie.OverPlusValue * 100 / result.Pie.QuotaValue).ToString("0.00") + "%";

                var balanceQuotaValue = new List<decimal>();
                for (int i = 0; i <= TotalCount - 1;i++ )
                {
                    balanceQuotaValue.Add(result.Pie.QuotaValue);
                }
                //趋势分析能耗值
                var TrendActualValue = new List<decimal>();
                TrendActualValue = (from item in tempConvert select decimal.Round(item.Value, 2)).ToList();
                for (int i = 0; i <= TrendActualValue.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        TrendActualValue[i] = TrendActualValue[i] + TrendActualValue[i - 1];
                    }

                }
                //差额分析能耗值
                result.BalanceHighChart.series.Add(new EneryAnalyseSeries()
                {
                    name = "ActualLine",
                    //data = (from item in tempConvert select decimal.Round(item.Value, 2)).ToList()
                    data = TrendActualValue
                });
                //差额分析定额值
               
                result.BalanceHighChart.series.Add(new EneryAnalyseSeries()
                {
                    name = "QuotaLine",
                    data = balanceQuotaValue
                });
                //差额分析能耗差值
                result.BalanceHighChart.series.Add(new EneryAnalyseSeries()
                {
                    name = "BalanceLine",
                    data = ((from item in TrendActualValue select result.Pie.QuotaValue - decimal.Round(item, 2))).ToList()
                });
                //趋势分析定额值
                var TrendQuotaValue = new List<decimal>();
               
                //趋势分析预测值
                var TrendForeCastValue = new List<decimal>();
              
                result.TrendHighChart.series.Add(new EneryAnalyseSeries()
                {
                    name = "ActualLine",
                    data = TrendActualValue
                });
                int[] x = new int[TotalCount];
                int count = 1;
                //TrendQuotaValue.Add(0);//以0坐标开始
                for (int i = 0; i <= x.Length - 1; i++)
                {
                    x[i] = i;
                    if (count == x.Length)
                    {
                        TrendQuotaValue.Add(result.Pie.QuotaValue);
                    }
                    else
                    {
                        TrendQuotaValue.Add(averagequotavalue * count);
                    }
                    count++;
                }
                result.TrendHighChart.series.Add(new EneryAnalyseSeries()
                {
                    name = "QuotaLine",
                    data = TrendQuotaValue
                });
                TrendForeCastValue = SPT(x, TrendActualValue.ToArray());
                //预测节能率：定额值-线性回归公式值（最后一天或者12月）/定额值
                if (TrendForeCastValue.Count>0)
                {
                    result.Pie.ForecastSavingPercent = ((result.Pie.QuotaValue - TrendForeCastValue[TrendForeCastValue.Count - 1]) * 100 / result.Pie.QuotaValue).ToString("0.00") + "%";
                }
                else
                {
                    result.Pie.ForecastSavingPercent = "-";
                }

                #region 暂时禁用

                //decimal lastvalue = 0;
                //decimal lastsecondvalue = 0;
                //if (TrendActualValue.Count > 0)
                //{
                //    lastvalue = TrendActualValue[TrendActualValue.Count - 1];
                //    if (TrendActualValue.Count > 1)
                //    {
                //        lastsecondvalue = TrendActualValue[TrendActualValue.Count - 2];
                //    }
                //}
                //if (query.Particle == Particle.Year)
                //{
                //    if (query.StartTime.Year < DateTime.Now.Year)
                //    {
                //        result.Pie.ForecastSavingPercent = ((result.Pie.QuotaValue - result.Pie.ActualValue) * 100 / result.Pie.QuotaValue).ToString("0.00") + "%";

                //    }
                //    else
                //    {
                //        //当前理论计划值= 年定额值/12*（结束时间月-1）+年定额值/12/结束月的总天数*（结束时间天-1）
                //        decimal theoryPlanvalue = result.Pie.QuotaValue / 12 * (query.EndTime.Month - 1) + result.Pie.QuotaValue / 12 / DateTime.DaysInMonth(query.EndTime.Year, query.EndTime.Month) * (query.EndTime.Day - 1);
                //        //预测节能率=（当前理论计划值-实时能耗值）/年定额值，
                //        result.Pie.ForecastSavingPercent = ((theoryPlanvalue - result.Pie.ActualValue) * 100 / result.Pie.QuotaValue).ToString("0.00") + "%";
                //        if (query.EndTime.Month < 12)
                //        {//添加趋势预测线
                //            TrendForeCastValue.Add(lastvalue);

                //            for (int i = 1; i <= (12 - query.EndTime.Month); i++)
                //            {
                //                TrendForeCastValue.Add(lastvalue + (lastvalue - lastsecondvalue) * i);
                //            }
                //        }
                //    }

                //}
                //else
                //{
                //    if (query.StartTime.Month < DateTime.Now.Month)
                //    {
                //        result.Pie.ForecastSavingPercent = ((result.Pie.QuotaValue - result.Pie.ActualValue) * 100 / result.Pie.QuotaValue).ToString("0.00") + "%";
                //    }
                //    else
                //    {
                //        //当前理论计划值= 月定额值/当月天数*（当前天-1）+月定额值/当月天数/24*（当前小时数）
                //        decimal theoryPlanvalue = result.Pie.QuotaValue / DateTime.DaysInMonth(query.EndTime.Year, query.EndTime.Month) * (query.EndTime.Day - 1) + result.Pie.QuotaValue / DateTime.DaysInMonth(query.EndTime.Year, query.EndTime.Month) / 24 * (query.EndTime.Hour);
                //        //预测节能率=（当前理论计划值-实时能耗值）/年定额值，
                //        result.Pie.ForecastSavingPercent = ((theoryPlanvalue - result.Pie.ActualValue) * 100 / result.Pie.QuotaValue).ToString("0.00") + "%";
                //    }
                //    if (query.EndTime.Day < DateTime.DaysInMonth(query.StartTime.Year, query.StartTime.Month))
                //    {//添加趋势预测线
                //        TrendForeCastValue.Add(lastvalue);

                //        for (int i = 1; i <= (DateTime.DaysInMonth(query.StartTime.Year, query.StartTime.Month) - query.EndTime.Day); i++)
                //        {
                //            TrendForeCastValue.Add(lastvalue + (lastvalue - lastsecondvalue) * i);
                //        }
                //    }
                //}
                #endregion
                result.TrendHighChart.series.Add(new EneryAnalyseSeries()
                {
                    name = "ForeCastLine",
                    data = TrendForeCastValue
                });
                //同比
                query.StartTime = query.StartTime.AddYears(-1);
                query.EndTime = query.EndTime.AddYears(-1);
                quotaAnalyseModel = GetQuotaAnalyseModel(query);
                if (quotaAnalyseModel != null)
                {
                    result.Pie.LastYearQuotaValue = decimal.Round(quotaAnalyseModel.QuotaValue, 2); ;
                    if (!(quotaAnalyseModel.QuotaValue > 0))
                    {
                        throw new Exception("无效的定额值");
                    }
                }
                else
                {
                    //throw new Exception("当前未设置定额值");
                    result.Pie.LastYearQuotaValue = 0;
                }

                model.Unit = BaseTool.GetChartUnit((int)query.Particle - 1);
                if (model.Unit == ChartUnit.unit_month)
                {//年定额分析 精确到天
                    model.Starttime = Convert.ToDateTime(query.StartTime.ToString("yyyy-MM-01"));
                    model.Endtime = Convert.ToDateTime(query.EndTime.ToString("yyyy-12-01"));
                    
                }
                else
                {//月定额分析 精确到小时

                    model.Starttime = query.StartTime;
                    model.Endtime = model.Starttime.AddMonths(1);

                }
                //model.Unit = ChartUnit.unit_month;
                //model.Starttime = Convert.ToDateTime(query.StartTime.ToString("yyyy-MM-01"));
                //model.Endtime = Convert.ToDateTime(query.EndTime.ToString("yyyy-MM-01"));
                //if(query.Particle==Particle.Year)
                //{
                //    model.Endtime = Convert.ToDateTime(query.EndTime.ToString("yyyy-12-01"));
                //}
               
                tempConvert = new Dictionary<string, decimal>();
                if (query.EndTime >= query.StartTime)
                {
                    tempConvert = GetSingleItemCodeValue(model, query, itemCodeList);
                }
          
                result.Pie.LastYearActualValue = decimal.Round(tempConvert.Select(t => t.Value).Sum(), 2);
                if (result.Pie.LastYearQuotaValue>0)
                {
                    result.Pie.LastYearSavingPercent =
                   ((result.Pie.LastYearActualValue - result.Pie.LastYearQuotaValue) * 100 / result.Pie.LastYearQuotaValue)
                       .ToString("0.00") + "%";
                }
                else
                {
                    result.Pie.LastYearSavingPercent = "-";
                }

                return result;
            }
            catch (Exception ee)
            {
                throw ee;
            }

        }
        /// <summary>
        ///  一元线性回归分析
        /// </summary>
        /// <param name="x"> 存放自变量x的n个取值</param>
        /// <param name="y">存放与自变量x的n个取值相对应的随机变量y的观察值</param>
        /// <param name="para"> a(0) 返回回归系数b ,a(1)返回回归系数a</param>
        public List<decimal> SPT(int[] x, decimal[] y)
        {
            List<decimal> list = new List<decimal>();
            if (y.Length==1)
            {
                return list;
            }
            decimal ave_xlist = 0;
            decimal ave_ylist = 0;//平均值
            decimal max_xlist = 0;
            decimal max_ylist = 0;//最大数
            decimal j = 0;
            decimal k = 0;
            decimal a = 0;
            decimal b = 0;

            for (int i = 0; i < y.Length; i++)
            {
                ave_xlist += x[i];
                ave_ylist += y[i];
                if (x[i] > max_xlist)
                {
                    max_xlist = x[i];
                }
                if (y[i] > max_ylist)
                {
                    max_ylist = y[i];
                }
            }

            for (int i = 0; i < y.Length; i++)
            {
                j += (x[i] - ave_xlist) * (y[i] - ave_ylist);
                k += (x[i] - ave_xlist) * (x[i] - ave_xlist);
            }
            b = j / k;
            a = ave_ylist - b * ave_xlist;

            // Y'=a+bx 
            for (int i = 0; i < y.Length; i++)
            {
                decimal f = a + b * x[i];
                list.Add(decimal.Round(f, 2));
            }
            if(x.Length>y.Length)
            {
                if (list.Count > 1)
                {
                    decimal lastvalue = 0;
                    decimal lastsecondvalue = 0;
                    lastvalue = list[list.Count - 1];
                    lastsecondvalue = list[list.Count - 2];

                    for (int i = 1; i <= (x.Length - y.Length);i++ )
                    {
                        list.Add(decimal.Round(lastvalue + (lastvalue - lastsecondvalue) * i));
                    }

                }
            }
           
            return list;
        }

        public List<decimal> SPT2(int[] x, float[] y)
        {
            List<decimal> list = new List<decimal>();
            float ave_xlist = 0;
            float ave_ylist = 0;//平均值
            float max_xlist = 0;
            float max_ylist = 0;//最大数
            float j = 0;
            float k = 0;
            float a = 0;
            float b = 0;

            for (int i = 0; i < x.Length; i++)
            {
                ave_xlist += x[i];
                ave_ylist += y[i];
                if (x[i] > max_xlist)
                {
                    max_xlist = x[i];
                }
                if (y[i] > max_ylist)
                {
                    max_ylist = y[i];
                }
            }

            for (int i = 0; i < x.Length; i++)
            {
                j += (x[i] - ave_xlist) * (y[i] - ave_ylist);
                k += (x[i] - ave_xlist) * (x[i] - ave_xlist);
            }
            b = j / k;
            a = ave_ylist - b * ave_xlist;

            // Y'=a+bx 
            for (int i = 0; i < x.Length; i++)
            {
                float f = a + b * x[i];
                list.Add(decimal.Parse(f.ToString("0.00")));
            }
            return list;
        }
        private Model.QuotaAnalyseModel GetQuotaAnalyseModel(QueryQuota query)
        {
            string strwhere = "";
            switch (query.Particle)
            {
                case Particle.Year://年定额分析
                    strwhere = string.Format(@"and objectid={0}
                                               and quotatype={1}
                                               and itemcode='{2}'
                                               and quotatime='{3}'", query.ObjectId, (int)query.Particle - 1, query.ItemCode, query.StartTime.ToString("yyyy-01-01"));

                    break;
                case Particle.Month://月定额分析
                    strwhere = string.Format(@"and objectid={0}
                                               and quotatype={1}
                                               and itemcode='{2}'
                                               and quotatime='{3}'", query.ObjectId, (int)query.Particle - 1, query.ItemCode, query.StartTime.ToString("yyyy-MM-01"));
                    break;
                default:
                    break;
            }

            return new QuotaAnalyse().GetQuotaAnalyse(strwhere);
        }

        public Dictionary<string, decimal> GetSingleItemCodeValue(BaseQueryModel model, QueryQuota query, List<Model.Itemcode> itemCodeList)
        {

            Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
            //if (model.Endtime.Day==1)
            //{
            //    return tempConvert;
            //}
           // Dictionary<string, decimal> tempHourConvert = new Dictionary<string, decimal>();//暂时禁用小时
            switch (model.Unit)
            {
                case ChartUnit.unit_month:
                    for (DateTime i = model.Starttime; i <= model.Endtime; i = i.AddMonths(1))
                    {
                        tempConvert.Add(i.ToString("yyyy-MM-dd"), 0);
                    }
                    break;
                case ChartUnit.unit_hour:
                    for (DateTime i = model.Starttime; i < model.Endtime.AddDays(1); i = i.AddHours(1))
                    {
                        tempConvert.Add(i.ToString("yyyy-MM-dd HH:mm:ss"), 0);
                    }
                    break;
                case ChartUnit.unit_day:
                    if (query.EndTime.Hour > 0)
                    {
                        for (DateTime i = model.Starttime; i <=model.Endtime.AddDays(-1); i = i.AddDays(1))
                        {
                            tempConvert.Add(i.ToString(("yyyy-MM-dd")), 0);
                        }
                        //暂时禁用小时
                        //for (DateTime i = DateTime.Parse(model.Endtime.ToString("yyyy-MM-dd 00:00:00")); i < model.Endtime; i = i.AddHours(1))
                        //{
                        //    tempHourConvert.Add(i.ToString("yyyy-MM-dd HH:mm:ss"), 0);
                        //}
                    }
                    else
                    {
                        for (DateTime i = model.Starttime; i <= model.Endtime; i = i = i.AddDays(1))
                        {
                            tempConvert.Add(i.ToString(("yyyy-MM-dd")), 0);
                        }
                    }

                    break;
            }
            foreach (var item in itemCodeList)
            {
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
                                tempConvert[rr.DatePick] += rr.CoalDataValue;
                            }
                            else
                            {
                                tempConvert[rr.DatePick] += rr.DataValue;
                            }
                        }
                    }
                }

                //if (tempHourConvert.Count > 0)
                //{
                //    model.Starttime = DateTime.Parse(query.EndTime.ToString("yyyy-MM-dd 01:mm:ss"));
                //    model.Unit = ChartUnit.unit_hour;
                //    resList = _reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);
                //    foreach (var r in resList.BaseLayerObjectResults)
                //    {
                //        foreach (var rr in r.Value.Datas)
                //        {
                //            if (tempHourConvert.ContainsKey(rr.DatePick))
                //            {
                //                if (query.ItemCode == "00000")
                //                {
                //                    tempHourConvert[rr.DatePick] += rr.CoalDataValue;
                //                }
                //                else
                //                {
                //                    tempHourConvert[rr.DatePick] += rr.DataValue;
                //                }
                //            }

                //        }
                //    }
                //    decimal currentdayvalue = tempHourConvert.Select(t => t.Value).Sum();
                //    tempConvert.Add(model.Endtime.ToString(("yyyy-MM-dd")), currentdayvalue);
                //}
            }
            return tempConvert;
        }


        public Dictionary<string, decimal> GetSingleItemCodeByObject(BasicQuery query, int isCoal)
        {
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();
            var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.ObjectNum), " order by LayerObjectID");
            model.IsDevice = 0;
            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = query.ItemCode;
            model.Unit = BaseTool.GetChartUnit(query.Unit);
            model.Starttime = query.StartTime;
            model.Endtime = query.EndTime;
            var resList = _reportBll.GetBaseEneryDataList(model);
            foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
            {
                if (!eneryDataList.ContainsKey(d.DatePick))
                {

                    eneryDataList.Add(d.DatePick, isCoal == 0 ? d.DataValue : d.CoalDataValue);
                }
                else
                {
                    eneryDataList[d.DatePick] += isCoal == 0 ? d.DataValue : d.CoalDataValue;
                }
            }

            //foreach (var e in eneryDataList)
            //{
            //    result.DatePick.Add(e.Key);
            //    result.DatePickEnery.Add(e.Value);
            //}
            return eneryDataList;
        }




        public Dictionary<string, decimal> GetSingleItemCodeByObject22(QueryContrastPeriods query2, int isCoal)
        {
            QueryContrastPeriods query = new QueryContrastPeriods();
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();
            var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.ItemCode), " order by LayerObjectID");
            model.IsDevice = 0;
            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = query.ItemCode;
            //model.Unit = BaseTool.GetChartUnit(query);
            //model.Starttime = query.StartTime;
            //model.Endtime = query.EndTime;
            foreach (var perTime in query.PeriodLst)
            {
                model.Starttime = perTime.StartTime;
                model.Endtime = perTime.EndTime;
                var resList = _reportBll.GetBaseEneryDataList(model);
                //eneryDataList.Add(d.DatePick, isCoal == 0 ? d.DataValue : d.CoalDataValue);
            }

            // var resList = _reportBll.GetBaseEneryDataList(model);
            //foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
            //{
            //    if (!eneryDataList.ContainsKey(d.DatePick))
            //    {

            //        eneryDataList.Add(d.DatePick, isCoal == 0 ? d.DataValue : d.CoalDataValue);
            //    }
            //    else
            //    {
            //        eneryDataList[d.DatePick] += isCoal == 0 ? d.DataValue : d.CoalDataValue;
            //    }
            //}

            //foreach (var e in eneryDataList)
            //{
            //    result.DatePick.Add(e.Key);
            //    result.DatePickEnery.Add(e.Value);
            //}
            return eneryDataList;
        }

        public ResultQueryPie GetQueryPieChart(DataContact.BasicQuery query)
        {
            #region 定义区

            var resultList = new ResultView.ResultQueryPie
                {
                    ObjectName = new List<string>(),
                    Enery = new List<decimal>(),
                    Dept = new List<string>()
                };

            #endregion

            var objectList =
                new BLL.BaseLayerObject().GetBaseLayerObjectList(
                    string.Format(" and layerobjectparentid={0}", query.ObjectNum), " order by LayerObjectID");
            if (objectList.Count > 0)
            {
                decimal eTotal = 0;
                for (var i = 0; i < objectList.Count; i++)
                {
                    var querynew = new BasicQuery
                        {
                            StartTime = query.StartTime,
                            EndTime = query.EndTime,
                            ItemCode = query.ItemCode,
                            ObjectNum = objectList[i].LayerObjectID,
                            ObjectType = query.ObjectType,
                            Unit = query.Unit
                        };
                    //if (i < 5)
                    //{
                    if (query.ItemCode != "00000")
                    {
                        var item =
                            new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'",
                                                               " order by ItemcodeID")[0];

                        resultList.Dept.Add(item.Unit);
                    }
                    else
                    {
                        resultList.Dept.Add("T");
                    }
                    resultList.ObjectName.Add(objectList[i].LayerObjectName);

                    resultList.Enery.Add(new QueryEnery().GetQueryEneryTotal(querynew).TotalEnergy);
                    //}
                    //else
                    //{
                    //    eTotal += new QueryEnery().GetQueryEneryTotal(querynew).TotalEnergy;
                    //    if (i == objectList.Count - 1)
                    //    {
                    //        if (query.ItemCode != "00000")
                    //        {
                    //            var item =
                    //                new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'",
                    //                                                   " order by ItemcodeID")[0];
                    //            resultList.Dept.Add(item.Unit);
                    //        }
                    //        else
                    //        {
                    //            resultList.Dept.Add("T");
                    //        }
                    //        resultList.ObjectName.Add("其他");
                    //        resultList.Enery.Add(eTotal);
                    //    }
                    //}
                }
                if (resultList.Enery.Count > 5)
                {
                    //resultList.ObjectName[5] = "其他";
                    List<EnergyRank> lstEnergy = new List<EnergyRank>();

                    for (var i = 0; i < resultList.Enery.Count; i++)
                    {
                        EnergyRank er = new EnergyRank();
                        er.ObjName = resultList.ObjectName[i];
                        er.Energy = resultList.Enery[i];
                        lstEnergy.Add(er);

                    }
                    lstEnergy = lstEnergy.OrderByDescending(p => p.Energy).ToList();
                    resultList.Dept.Clear();
                    resultList.ObjectName.Clear();
                    resultList.Enery.Clear();

                    if (lstEnergy.Count < 6)
                    {
                        foreach (EnergyRank er in lstEnergy)
                        {
                            resultList.Dept.Add("T");
                            resultList.ObjectName.Add(er.ObjName);
                            resultList.Enery.Add(er.Energy);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            resultList.Dept.Add("T");
                            resultList.ObjectName.Add(lstEnergy[i].ObjName);
                            resultList.Enery.Add(lstEnergy[i].Energy);
                        }
                        resultList.Dept.Add("T");
                        resultList.ObjectName.Add("其他");
                        //统计排名第五以后的能耗总和。
                        resultList.Enery.Add(lstEnergy.GetRange(5, lstEnergy.Count - 5).Sum(p => p.Energy));
                    }
                }
                return resultList;
            }
            else
            {
                var deviceList = new List<Device>();
                if (query.ItemCode == "00000")
                {
                    deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + query.ObjectNum, " order by deviceid");
                }
                else
                {
                    string itemCodeAll = query.ItemCode;
                    string tempCode = string.Empty;
                    var itemcodeList = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ItemCodeNumber='" + query.ItemCode + "'", " order by itemcodeid")[0];
                    var itemcodeListChild = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ParentID=" + itemcodeList.ParentID, " order by itemcodeid");
                    foreach (Model.Itemcode itemcode in itemcodeListChild)
                        tempCode += ",'" + itemcode.ItemCodeNumber + "'";
                    itemCodeAll = itemCodeAll.Length > 0 ? "'" + itemCodeAll + "'" : itemCodeAll.Substring(1);
                    deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and ItemCodeID in (" + itemCodeAll + ") and areaid=" + query.ObjectNum, " order by deviceid");
                }
                if (deviceList.Count > 0)
                {
                    decimal eTotal = 0;
                    for (var i = 0; i < deviceList.Count; i++)
                    {
                        var querynew = new BasicQuery
                        {
                            StartTime = query.StartTime,
                            EndTime = query.EndTime,
                            ItemCode = query.ItemCode,
                            ObjectNum = deviceList[i].DeviceID,
                            ObjectType = query.ObjectType,
                            Unit = query.Unit
                        };
                        if (i < 5)
                        {
                            if (query.ItemCode != "00000")
                            {
                                var item =
                                    new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'",
                                                                       " order by ItemcodeID")[0];

                                resultList.Dept.Add(item.Unit);
                            }
                            else
                            {
                                resultList.Dept.Add("T");
                            }
                            resultList.ObjectName.Add(deviceList[i].DeviceName);
                            resultList.Enery.Add(new QueryEnery().GetDeviceQueryEneryTotal(querynew).TotalEnergy);
                        }
                        else
                        {
                            eTotal += new QueryEnery().GetDeviceQueryEneryTotal(querynew).TotalEnergy;
                            if (i == deviceList.Count - 1)
                            {
                                if (query.ItemCode != "00000")
                                {
                                    var item =
                                        new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'",
                                                                           " order by ItemcodeID")[0];
                                    resultList.Dept.Add(item.Unit);
                                }
                                else
                                {
                                    resultList.Dept.Add("T");
                                }
                                resultList.ObjectName.Add("其他");
                                resultList.Enery.Add(eTotal);
                            }
                        }
                    }
                    return resultList;
                }
            }
            return null;
        }



        public ResultItemCode GetTwoQueryLineChart(BasicQuery query, BasicQuery query2, int tabId)
        {
            #region 定义区

            var queryCode = query.ItemCode;
            var resultList = new ResultView.ResultItemCode
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion
            ResultItemCode result = GetQueryLineChart(query);
            ResultItemCode result2 = GetQueryLineChart(query2);

            resultList.Dept = result.Dept;
            foreach (var oname in result.ObjectName)
            {
                resultList.ObjectName.Add("");
            }
            // resultList.ObjectName = result.ObjectName;
            decimal[] Arr = new decimal[result.Enery[result.Enery.Keys.First<string>()].Count];
            decimal[] ArrNew = new decimal[result2.Enery[result2.Enery.Keys.First<string>()].Count];
            if (queryCode == "00000")
            {
                foreach (var re in result.Enery)
                {
                    for (int i = 0; i < re.Value.Count; i++)
                    {
                        Arr[i] += re.Value[i];
                    }
                }
                foreach (var re2 in result2.Enery)
                {
                    for (int i = 0; i < re2.Value.Count; i++)
                    {
                        ArrNew[i] += re2.Value[i];
                    }
                }
                if (tabId == 1)
                {
                    resultList.Enery.Add(query.StartTime.ToString("yyyy") + "总能耗", Arr.ToList());
                    resultList.Enery.Add(query2.StartTime.ToString("yyyy") + "总能耗", ArrNew.ToList());
                }
                else
                {
                    resultList.Enery.Add(query.StartTime.ToString("yyyy-MM") + "总能耗", Arr.ToList());
                    resultList.Enery.Add(query2.StartTime.ToString("yyyy-MM") + "总能耗", ArrNew.ToList());
                }


            }
            else
            {
                var itemcodeList = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ItemCodeNumber='" + queryCode + "'", " order by itemcodeid")[0];

                foreach (var r1 in result.Enery)
                {
                    if (tabId == 1)
                    {
                        resultList.Enery.Add(query.StartTime.ToString("yyyy") + r1.Key, r1.Value);
                    }
                    else
                    {
                        resultList.Enery.Add(query.StartTime.ToString("yyyy-MM") + r1.Key, r1.Value);
                    }
                }
                foreach (var r2 in result2.Enery)
                {
                    if (tabId == 1)
                    {
                        resultList.Enery.Add(query2.StartTime.ToString("yyyy") + r2.Key, r2.Value);
                    }
                    else
                    {
                        resultList.Enery.Add(query2.StartTime.ToString("yyyy-MM") + r2.Key, r2.Value);
                    }
                }
            }

            return resultList;
        }

        public ResultDevice GetDeviceList(QueryDevice2 query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                StringBuilder sbTree = new StringBuilder();

                List<Device> deviceList;
                if (query.ItemCode == "00000")
                {
                    if (query.ObjType == AreaType.Area)
                    {
                        deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + query.ObjectId, " order by deviceid");
                    }
                    else
                    {
                        deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid2=" + query.ObjectId, " order by deviceid");

                    }
                }
                else
                {
                    string itemCodeAll = query.ItemCode;
                    Itemcode objItem = new Itemcode();
                    var itemcodeList = objItem.GetItemcodeList("  and ItemCodeNumber='" + query.ItemCode + "'", " order by itemcodeid")[0];
                    var itemcodeListChild = objItem.GetItemcodeList("  and ParentID=" + itemcodeList.ItemcodeID, " order by itemcodeid");
                    itemCodeAll = itemcodeListChild.Aggregate("'" + itemCodeAll + "'", (current, itemcode) => current + ("," + "'" + itemcode.ItemCodeNumber + "'"));
                    if (query.ObjType == AreaType.Area)
                    {
                        deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and ItemCodeID in (" + itemCodeAll + ") and areaid=" + query.ObjectId, " order by deviceid");
                    }
                    else
                    {
                        deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and ItemCodeID in (" + itemCodeAll + ") and areaid2=" + query.ObjectId, " order by deviceid");

                    }
                }
                //sbTree.Append("[");
                //for (var device = 0; device < deviceList.Count; device++)
                //{
                //    sbTree.Append("{\"text\": \"" + deviceList[device].DeviceName + "\",\"id\": " + deviceList[device].DeviceID + ",\"classes\": \"equip\"}");
                //    sbTree.Append(device == deviceList.Count - 1 ? "" : ",");
                //}
                //sbTree.Append("]");

                List<DeviceUnit> lstDvc = new List<DeviceUnit>();

                foreach (var device in deviceList)
                {
                    DeviceUnit du = new DeviceUnit();
                    du.DeviceID = device.DeviceID;
                    du.DeviceName = device.DeviceName;
                    lstDvc.Add(du);
                }

                ResultDevice result = new ResultDevice();
                result.DeviceUnitList = lstDvc;
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception ex)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = ex.Message;
                return new ResultDevice() { ActionInfo = pAction };
            }

        }


    }

    struct EnergyRank
    {
        public string ObjName;
        public decimal Energy;
    }

}
