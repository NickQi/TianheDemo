using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.BLL
{
    public class EnergyContrast
    {
        //readonly NTS.WEB.ProductInteface.IReportBase _reportBll = NTS.WEB.ProductInteface.DataSwitchConfig.CreateReportBase();


        readonly NTS.WEB.ProductInteface.IEnergyContrast _energCon = NTS.WEB.ProductInteface.DataSwitchConfig.CreateEnergyContrast();
        NTS.WEB.DAL.EnergyContrast contsDal = new DAL.EnergyContrast();

        #region 多对象
        public ResultCompare GetCompareChart(QueryOrderObjects queryObject)
        {
            #region 定义区
            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            var olist = queryObject.AreaIdLst.Aggregate(string.Empty, (current, q) => current + ("," + q.ToString()));
            if (olist.Length > 0)
            {
                List<Model.BaseLayerObject> objectList;
                if (queryObject.ObjType == AreaType.Area)
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
                        EndTime = queryObject.EndTime,
                        StartTime = queryObject.StartTime,
                        ItemCode = queryObject.ItemCode,
                        Unit = queryObject.Particle,
                        AreaType = queryObject.ObjType,
                        QueryType = queryObject.QueryType
                    };
                    basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetQueryLineChart(basicQuery);
                    resultList.ObjectName = result.ObjectName;
                    if (queryObject.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + queryObject.ItemCode + "'", " order by ItemcodeID")[0];
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

        public ResultCompare GetAreaCompareChart(QueryOrderObjects queryObject)
        {

            #region 定义区


            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            var olist = queryObject.AreaIdLst.Aggregate(string.Empty, (current, q) => current + ("," + q.ToString()));

            if (olist.Length > 0)
            {
                List<Model.BaseLayerObject> objectList;
                if (queryObject.ObjType == AreaType.Area)
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
                        EndTime = queryObject.EndTime,
                        StartTime = queryObject.StartTime,
                        ItemCode = queryObject.ItemCode,
                        Unit = queryObject.Particle,
                        AreaType = queryObject.ObjType,
                        QueryType = queryObject.QueryType
                    };
                    basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetQueryLineChart(basicQuery);
                    //resultList.Dept = result.Dept;
                    resultList.ObjectName = result.ObjectName;
                    if (queryObject.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + queryObject.ItemCode + "'", " order by ItemcodeID")[0];
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

        /// <summary>
        /// add by: jy
        /// add at: 2014-3-24
        /// note  : 人均能耗对比
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultCompare GetPersonNumCompareChart(QueryOrderObjects queryObject)
        {

            #region 定义区

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            var olist = queryObject.AreaIdLst.Aggregate(string.Empty, (current, q) => current + ("," + q.ToString()));

            if (olist.Length > 0)
            {
                List<Model.BaseLayerObject> objectList;
                if (queryObject.ObjType == AreaType.Area)
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
                        EndTime = queryObject.EndTime,
                        StartTime = queryObject.StartTime,
                        ItemCode = queryObject.ItemCode,
                        Unit = queryObject.Particle,
                        AreaType = queryObject.ObjType,
                        QueryType = queryObject.QueryType
                    };
                    basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetQueryLineChart(basicQuery);
                    //resultList.Dept = result.Dept;
                    resultList.ObjectName = result.ObjectName;
                    if (queryObject.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + queryObject.ItemCode + "'", " order by ItemcodeID")[0];
                        //resultList.Enery.Add(o.LayerObjectName, result.Enery[itemList.ItemCodeName]);
                        for (int i = 0; i < result.Dept.Count; i++)
                        {
                            resultList.Dept.Add(itemList.Unit);
                        }

                        resultList.Enery.Add(o.LayerObjectName, result.Enery[itemList.ItemCodeName].Select(p => p / decimal.Parse(o.PersonNum.ToString())).ToList());
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
                        resultList.Enery.Add(o.LayerObjectName, tempValue.ToList().Select(p => p / decimal.Parse(o.PersonNum.ToString())).ToList());
                    }
                }
                return resultList;
            }
            return null;
        }

        #endregion


        #region 多时间
        public ResultCompare GetPeriodsCompareChart(QueryContrastPeriods query)
        {

            #region 定义区

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            if (query.PeriodLst.Count > 0)
            {
                resultList.ObjectName.Clear();
                for (int iCount = 0; iCount < query.PeriodLst.Count; iCount++)
                {
                    //QueryContrastPeriods basicQuery = new QueryContrastPeriods();
                    //basicQuery.PeriodLst = query.PeriodLst;
                    //basicQuery.ItemCode = query.ItemCode;
                    //basicQuery.AreaId = query.AreaId;
                    //basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetPeriodsQueryLineChart(query, query.PeriodLst[iCount]);
                    resultList.ObjectName.Add(query.PeriodLst[iCount].StartTime.ToString("yyyy-MM-dd") + "~" + query.PeriodLst[iCount].EndTime.ToString("yyyy-MM-dd"));
                    if (query.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
                        resultList.Enery.Add((iCount + 1).ToString(), result.Enery[itemList.ItemCodeName]);
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
                        resultList.Enery.Add(iCount.ToString(), tempValue.ToList());
                    }
                }
                return resultList;
            }
            return null;

        }

        public ResultCompare GetPersonNumPeriodsCompareChart(QueryContrastPeriods query)
        {
            #region 定义区

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            if (query.PeriodLst.Count > 0)
            {
                resultList.ObjectName.Clear();
                for (int iCount = 0; iCount < query.PeriodLst.Count; iCount++)
                {
                    //QueryContrastPeriods basicQuery = new QueryContrastPeriods();
                    //basicQuery.PeriodLst = query.PeriodLst;
                    //basicQuery.ItemCode = query.ItemCode;
                    //basicQuery.AreaId = query.AreaId;
                    //basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetPeriodsQueryLineChart(query, query.PeriodLst[iCount]);
                    resultList.ObjectName.Add(query.PeriodLst[iCount].StartTime.ToString("yyyy-MM-dd") + "~" + query.PeriodLst[iCount].EndTime.ToString("yyyy-MM-dd"));
                    if (query.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
                        resultList.Enery.Add((iCount + 1).ToString(), result.Enery[itemList.ItemCodeName]);
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
                        resultList.Enery.Add(iCount.ToString(), tempValue.ToList());
                        //resultList.Enery.Add(o.LayerObjectName, tempValue.ToList().Select(p => p / decimal.Parse(o.PersonNum.ToString())).ToList());
                    }
                }
                return resultList;
            }
            return null;

        }

        public ResultCompare GetAreaPeriodsCompareChart(QueryContrastPeriods query)
        {
            #region 定义区

            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion


            if (query.PeriodLst.Count > 0)
            {
                resultList.ObjectName.Clear();
                for (int iCount = 0; iCount < query.PeriodLst.Count; iCount++)
                {
                    //QueryContrastPeriods basicQuery = new QueryContrastPeriods();
                    //basicQuery.PeriodLst = query.PeriodLst;
                    //basicQuery.ItemCode = query.ItemCode;
                    //basicQuery.AreaId = query.AreaId;
                    //basicQuery.ObjectNum = o.LayerObjectID;
                    var result = GetPeriodsQueryLineChart(query, query.PeriodLst[iCount]);
                    resultList.ObjectName.Add(query.PeriodLst[iCount].StartTime.ToString("yyyy-MM-dd") + "~" + query.PeriodLst[iCount].EndTime.ToString("yyyy-MM-dd"));
                    if (query.ItemCode != "00000")
                    {
                        var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID")[0];
                        resultList.Enery.Add((iCount + 1).ToString(), result.Enery[itemList.ItemCodeName]);
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
                        resultList.Enery.Add(iCount.ToString(), tempValue.ToList());
                        //resultList.Enery.Add(o.LayerObjectName, tempValue.ToList().Select(p => p / decimal.Parse(o.PersonNum.ToString())).ToList());
                    }
                }
                return resultList;
            }
            return null;

        }
        #endregion





        #region 整理数据
        public ResultItemCode GetPeriodsQueryLineChart(QueryContrastPeriods querOld, TimePeriod time)
        {
            #region 定义区
            QueryContrastPeriods query = new QueryContrastPeriods();
            query = querOld;
            var resultList = new ResultView.ResultItemCode
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            #endregion

            var dept = string.Empty;
            //var objectList =
            //    new BLL.BaseLayerObject().GetBaseLayerObjectList(
            //        string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
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
                    if (querOld.QueryType == QueryOrderType.Default)
                    {
                        var lineChart = GetSingleItemCodeByObjectNew(query, itemcode.ItemCodeNumber, time, 1);

                        if (count < lineChart.Count)
                        {
                            count = lineChart.Count;
                            maxcode = itemcode.ItemCodeNumber;
                        }
                        eneryDataListTotal.Add(itemcode.ItemCodeNumber, lineChart);
                    }
                    else if (querOld.QueryType == QueryOrderType.UnitArea)
                    {
                        var lineChart = this.GetAreaSingleItemCodeByObject(query, itemcode.ItemCodeNumber, time, 1);

                        if (count < lineChart.Count)
                        {
                            count = lineChart.Count;
                            maxcode = itemcode.ItemCodeNumber;
                        }
                        eneryDataListTotal.Add(itemcode.ItemCodeNumber, lineChart);
                    }
                    else if (querOld.QueryType == QueryOrderType.UnitPerson)
                    {
                        var lineChart = GetPersonNumSingleItemCodeByObject(query, itemcode.ItemCodeNumber, time, 1);

                        if (count < lineChart.Count)
                        {
                            count = lineChart.Count;
                            maxcode = itemcode.ItemCodeNumber;
                        }
                        eneryDataListTotal.Add(itemcode.ItemCodeNumber, lineChart);
                    }


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
                if (querOld.QueryType == QueryOrderType.Default)
                {
                    var lineChart = GetSingleItemCodeByObjectNew(query, query.ItemCode, time, 0);
                    // resultList.Dept = new List<string>() {item.Unit};
                    resultList.ObjectName = lineChart.Select(p => p.Key.ToString()).ToList();
                    resultList.Enery.Add(item.ItemCodeName, lineChart.Select(p => decimal.Parse(p.Value.ToString())).ToList());
                }
                else if (querOld.QueryType == QueryOrderType.UnitArea)
                {
                    var lineChart = GetAreaSingleItemCodeByObject(query, query.ItemCode, time, 0);
                    // resultList.Dept = new List<string>() {item.Unit};
                    resultList.ObjectName = lineChart.Select(p => p.Key.ToString()).ToList();
                    resultList.Enery.Add(item.ItemCodeName, lineChart.Select(p => decimal.Parse(p.Value.ToString())).ToList());
                }
                else if (querOld.QueryType == QueryOrderType.UnitPerson)
                {
                    var lineChart = GetPersonNumSingleItemCodeByObject(query, query.ItemCode, time, 0);
                    // resultList.Dept = new List<string>() {item.Unit};
                    resultList.ObjectName = lineChart.Select(p => p.Key.ToString()).ToList();
                    resultList.Enery.Add(item.ItemCodeName, lineChart.Select(p => decimal.Parse(p.Value.ToString())).ToList());
                }

            }
            foreach (var baseLayerObject in resultList.ObjectName)
            {
                resultList.Dept.Add(dept);
            }
            return resultList;

        }

        public ResultItemCode GetPersonNumPeriodsQueryLineChart(QueryContrastPeriods query, TimePeriod time)
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
                    string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
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
                    var lineChart = GetPersonNumSingleItemCodeByObject(query, itemcode.ItemCodeNumber, time, 1);

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
                var lineChart = GetSingleItemCodeByObjectNew(query, query.ItemCode, time, 0);
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
                    var lineChart = GetSingleItemCodeByObject(query, itemcode.ItemCodeNumber);

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
                var lineChart = GetSingleItemCodeByObject(query, query.ItemCode);
                // resultList.Dept = new List<string>() {item.Unit};
                if (lineChart != null)
                {
                    resultList.ObjectName = lineChart.Select(p => p.Key.ToString()).ToList();
                    resultList.Enery.Add(item.ItemCodeName, lineChart.Select(p => decimal.Parse(p.Value.ToString())).ToList());
                }

            }
            foreach (var baseLayerObject in resultList.ObjectName)
            {
                resultList.Dept.Add(dept);
            }
            return resultList;

        }

        //objectList[0]
        public Dictionary<string, decimal> GetSingleItemCodeByObjectNew(QueryContrastPeriods query, string itemcode, TimePeriod time, int isCoal)
        {
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();
            List<Model.BaseLayerObject> objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            model.IsDevice = 0;
            if (query.ObjType == AreaType.Area)
            {
                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            }
            else
            {
                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            }

            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = itemcode;
            model.Unit = BaseTool.GetChartUnit(query.particle);
            model.Starttime = time.StartTime;
            model.Endtime = time.EndTime;
            model.areaType = query.ObjType;


            var resList = contsDal.GetBaseEneryDataListNew(model);
            for (int iCount = 0; iCount < query.PeriodLst.Count; iCount++)
            {
                TimePeriod tpTime = query.PeriodLst[iCount];
                //if (!eneryDataList.ContainsKey(d.DatePick))
                //{

                //    eneryDataList.Add(d.DatePick, isCoal == 0 ? d.DataValue : d.CoalDataValue);
                //}
                //else
                //{
                //    eneryDataList[d.DatePick] += isCoal == 0 ? d.DataValue : d.CoalDataValue;
                //}

            }
            foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
            {
                if (!eneryDataList.ContainsKey(d.DatePick))
                {
                    switch (query.QueryType)
                    {
                        case QueryOrderType.CarbanOut:
                            eneryDataList.Add(d.DatePick, d.Co2DataValue);
                            break;
                        case QueryOrderType.ConvCoal:
                            eneryDataList.Add(d.DatePick, d.CoalDataValue);
                            break;
                        case QueryOrderType.Renminbi:
                            eneryDataList.Add(d.DatePick, d.MoneyDataValue);
                            break;
                        default:
                            eneryDataList.Add(d.DatePick, d.DataValue);
                            break;
                    }
                }
                else
                {
                    eneryDataList[d.DatePick] += isCoal == 0 ? d.DataValue : d.CoalDataValue;
                    switch (query.QueryType)
                    {
                        case QueryOrderType.CarbanOut:
                            eneryDataList[d.DatePick] += d.Co2DataValue;
                            break;
                        case QueryOrderType.ConvCoal:
                            eneryDataList[d.DatePick] += d.CoalDataValue;
                            break;
                        case QueryOrderType.Renminbi:
                            eneryDataList[d.DatePick] += d.MoneyDataValue;
                            break;
                        default:
                            eneryDataList[d.DatePick] += d.DataValue;
                            break;
                    }
                }
            }

            //foreach (var e in eneryDataList)
            //{
            //    result.DatePick.Add(e.Key);
            //    result.DatePickEnery.Add(e.Value);
            //}
            return eneryDataList;
        }

        /// <summary>
        /// 获得单位面积能耗值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="itemcode"></param>
        /// <param name="time"></param>
        /// <param name="isCoal"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetAreaSingleItemCodeByObject(QueryContrastPeriods query, string itemcode, TimePeriod time, int isCoal)
        {
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();
            var objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            if (query.ObjType == AreaType.Area)
            {
                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            }
            else
            {
                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            }
            model.IsDevice = 0;
            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = itemcode;
            model.Unit = BaseTool.GetChartUnit(query.particle);
            model.Starttime = time.StartTime;
            model.Endtime = time.EndTime;
            model.areaType = query.ObjType;

            var resList = _energCon.GetBaseEneryDataListNew(model);
            for (int iCount = 0; iCount < query.PeriodLst.Count; iCount++)
            {
                TimePeriod tpTime = query.PeriodLst[iCount];
            }
            foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
            {

                decimal areaNum = Convert.ToDecimal(objectList[0].AreaNum);
                if (!eneryDataList.ContainsKey(d.DatePick))
                {
                    switch (query.QueryType)
                    {
                        case QueryOrderType.CarbanOut:
                            eneryDataList.Add(d.DatePick, d.Co2DataValue);
                            break;
                        case QueryOrderType.ConvCoal:
                            eneryDataList.Add(d.DatePick, d.CoalDataValue);
                            break;
                        case QueryOrderType.Renminbi:
                            eneryDataList.Add(d.DatePick, d.MoneyDataValue);
                            break;
                        default:
                            eneryDataList.Add(d.DatePick, d.DataValue);
                            break;
                    }
                }
                else
                {
                    eneryDataList[d.DatePick] += isCoal == 0 ? d.DataValue : d.CoalDataValue;
                    switch (query.QueryType)
                    {
                        case QueryOrderType.CarbanOut:
                            eneryDataList[d.DatePick] += d.Co2DataValue;
                            break;
                        case QueryOrderType.ConvCoal:
                            eneryDataList[d.DatePick] += d.CoalDataValue;
                            break;
                        case QueryOrderType.Renminbi:
                            eneryDataList[d.DatePick] += d.MoneyDataValue;
                            break;
                        default:
                            eneryDataList[d.DatePick] += d.DataValue;
                            break;
                    }
                }
            }

            //foreach (var e in eneryDataList)
            //{
            //    result.DatePick.Add(e.Key);
            //    result.DatePickEnery.Add(e.Value);
            //}
            return eneryDataList;
        }

        /// <summary>
        /// 获得人均能耗值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="itemcode"></param>
        /// <param name="time"></param>
        /// <param name="isCoal"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetPersonNumSingleItemCodeByObject(QueryContrastPeriods query, string itemcode, TimePeriod time, int isCoal)
        {
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();

            List<Model.BaseLayerObject> objectList = new List<Model.BaseLayerObject>();
            if (query.ObjType == AreaType.Area)
            {
                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            }
            else
            {

                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and layerobjectid={0}", query.AreaId), " order by LayerObjectID");
            }
            model.IsDevice = 0;
            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = itemcode;
            model.Unit = BaseTool.GetChartUnit(query.particle);
            model.Starttime = time.StartTime;
            model.Endtime = time.EndTime;
            model.areaType = query.ObjType;

            var resList = _energCon.GetBaseEneryDataListNew(model);
            for (int iCount = 0; iCount < query.PeriodLst.Count; iCount++)
            {
                TimePeriod tpTime = query.PeriodLst[iCount];
            }
            foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
            {

                if (!eneryDataList.ContainsKey(d.DatePick))
                {
                    switch (query.QueryType)
                    {
                        case QueryOrderType.CarbanOut:
                            eneryDataList.Add(d.DatePick, d.Co2DataValue);
                            break;
                        case QueryOrderType.ConvCoal:
                            eneryDataList.Add(d.DatePick, d.CoalDataValue);
                            break;
                        case QueryOrderType.Renminbi:
                            eneryDataList.Add(d.DatePick, d.MoneyDataValue);
                            break;
                        default:
                            eneryDataList.Add(d.DatePick, d.DataValue);
                            break;
                    }
                }
                else
                {
                    eneryDataList[d.DatePick] += isCoal == 0 ? d.DataValue : d.CoalDataValue;
                    switch (query.QueryType)
                    {
                        case QueryOrderType.CarbanOut:
                            eneryDataList[d.DatePick] += d.Co2DataValue;
                            break;
                        case QueryOrderType.ConvCoal:
                            eneryDataList[d.DatePick] += d.CoalDataValue;
                            break;
                        case QueryOrderType.Renminbi:
                            eneryDataList[d.DatePick] += d.MoneyDataValue;
                            break;
                        default:
                            eneryDataList[d.DatePick] += d.DataValue;
                            break;
                    }
                }
            }

            //foreach (var e in eneryDataList)
            //{
            //    result.DatePick.Add(e.Key);
            //    result.DatePickEnery.Add(e.Value);
            //}
            return eneryDataList;
        }

        public Dictionary<string, decimal> GetSingleItemCodeByObject(BasicQuery query, string itemcode)
        {
            // var result = new ResultIndexLineChart { DatePick = new List<string>(), DatePickEnery = new List<decimal>() };
            var eneryDataList = new Dictionary<string, decimal>();
            var model = new BaseQueryModel();

            List<Model.BaseLayerObject> objectList = new List<Model.BaseLayerObject>();
            if (query.AreaType == AreaType.Area)
            {
                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and layerobjectid={0}", query.ObjectNum), " order by LayerObjectID");
            }
            else
            {

                objectList = new NTS.WEB.BLL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and layerobjectid={0}", query.ObjectNum), " order by LayerObjectID");
            }


            model.IsDevice = 0;
            model.ObjectList = (from p in objectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = itemcode;
            model.Unit = BaseTool.GetChartUnit(query.Unit);
            model.Starttime = query.StartTime;
            model.Endtime = query.EndTime;
            model.areaType = query.AreaType;

            var resList = _energCon.GetBaseEneryDataListNew(model);
            foreach (var d in objectList.SelectMany(o => resList.BaseLayerObjectResults[o.LayerObjectID.ToString(CultureInfo.InvariantCulture)].Datas))
            {
                if (!eneryDataList.ContainsKey(d.DatePick))
                {
                    switch (query.QueryType)
                    {
                        case EnergyAnalyseQueryType.Convert2Co2:
                            eneryDataList.Add(d.DatePick, d.Co2DataValue);
                            break;
                        case EnergyAnalyseQueryType.Convert2Coal:
                            eneryDataList.Add(d.DatePick, d.CoalDataValue);
                            break;
                        case EnergyAnalyseQueryType.Convert2Money:
                            eneryDataList.Add(d.DatePick, d.MoneyDataValue);
                            break;
                        default:
                            eneryDataList.Add(d.DatePick, d.DataValue);
                            break;
                    }
                }
                else
                {
                    switch (query.QueryType)
                    {
                        case EnergyAnalyseQueryType.Convert2Co2:
                            eneryDataList[d.DatePick] += d.Co2DataValue;
                            break;
                        case EnergyAnalyseQueryType.Convert2Coal:
                            eneryDataList[d.DatePick] += d.CoalDataValue;
                            break;
                        case EnergyAnalyseQueryType.Convert2Money:
                            eneryDataList[d.DatePick] += d.MoneyDataValue;
                            break;
                        default:
                            eneryDataList[d.DatePick] += d.DataValue;
                            break;
                    }
                }


            }

            //foreach (var e in eneryDataList)
            //{
            //    result.DatePick.Add(e.Key);
            //    result.DatePickEnery.Add(e.Value);
            //}
            return eneryDataList;
        }
        #endregion

    }
}
