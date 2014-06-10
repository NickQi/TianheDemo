using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.Model;

namespace NTS.WEB.BLL
{
    public class LoadForecast
    {
        readonly NTS.WEB.ProductInteface.ILoadForecast _energCon = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLoadForecast();

        public ResultLoadForecastMap GetLoadForecastChart(QueryLoadForecast loadCast)
        {
            try
            {
                ResultLoadForecastMap mapNew = new ResultLoadForecastMap();
                ExecuteProcess proc = new ExecuteProcess();
                proc.ActionName = "";
                proc.ActionTime = DateTime.Now;
                proc.ActionUser = "";
                proc.ExceptionMsg = "";
                proc.Success = true;
                var basicQuery = new BasicQuery
                {
                    EndTime = loadCast.EndTime,
                    StartTime = loadCast.StartTime,
                    ItemCode = loadCast.ItemCode,
                    Unit = loadCast.Particle,
                    AreaType = loadCast.ObjType,
                    ObjectNum = loadCast.ObjectId
                };

                // 获取数据
                var resultList = GetDateTongJiData(basicQuery);

                List<decimal> lstDecAvg = GetYuceData(basicQuery, loadCast);

                List<SerialData> serData = new List<SerialData>();

                foreach (var d in resultList.Enery)
                {
                    SerialData ser1 = new SerialData();
                    ser1.name = "负荷预测值";
                    ser1.data = lstDecAvg;
                    serData.Add(ser1);
                    ser1 = new SerialData();
                    ser1.name = "能耗实际值";
                    TimeSpan ts = DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-dd")) - loadCast.StartTime;
                    int DayChas = ts.Days;
                    List<decimal> lstNew = d.Value;
                    if (DayChas == 0)
                    {
                        for (int iCount = 0; iCount < lstNew.Count; iCount++)
                        {
                            lstNew[iCount] = Math.Round(lstNew[iCount], 2);
                        }
                    }
                    else if (DayChas > 0)
                    {
                        for (int iCount = 0; iCount < lstNew.Count; iCount++)
                        {
                            if (iCount > DayChas)
                            {
                                lstNew[iCount] = 0;
                            }
                            else
                            {
                                lstNew[iCount] = Math.Round(lstNew[iCount], 2);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lstNew.Count; i++)
                        {
                            lstNew[i] = -9999;
                        }
                    }

                    ser1.data = lstNew;

                    serData.Add(ser1);

                }
                mapNew.ActionInfo = proc;
                // 判断是否是将来当天数据。
                if (loadCast.StartTime.CompareTo(DateTime.Now) > 0)
                {
                    List<SerialData> serDataNew = new List<SerialData>();
                    serDataNew.Add(serData[0]);
                    mapNew.series = serDataNew;
                }
                else
                {
                    mapNew.series = serData;
                }

                if (resultList.Dept.Count > 0)
                {
                    mapNew.Unit = resultList.Dept[0].ToString();
                }

                List<ResultLoadForecastList> listFore = new List<ResultLoadForecastList>();
                for (int i = 0; i < serData[0].data.Count; i++)
                {
                    ResultLoadForecastList list = new ResultLoadForecastList();
                    list.Id = i + 1;
                    if (loadCast.StartTime.Date == loadCast.EndTime.Date)
                    {
                        string hour = "";
                        if (i < 10)
                        {
                            hour = "0" + i.ToString();
                        }
                        else
                        {
                            hour = i.ToString();
                        }
                        list.TimeArea = loadCast.StartTime.ToString("yyyy-MM-dd" + " " + hour + ":00:00");
                    }
                    else
                    {
                        list.TimeArea = loadCast.StartTime.AddDays(i).ToString("yyyy-MM-dd");
                    }
                    if (i < serData[1].data.Count)
                    {
                        list.History = Math.Round(serData[1].data[i], 2);
                    }
                    else
                    {
                        list.History = -9999;
                    }

                    list.ForeCast = Math.Round(serData[0].data[i], 2);
                    if (list.History == -9999)
                    {
                        list.Deviation = -9999;
                    }
                    else
                    {
                        list.Deviation = Math.Round((serData[0].data[i] - serData[1].data[i]), 2);
                    }

                    if (list.History == -9999)
                    {
                        list.Pecent = "--";
                        listFore.Add(list);
                    }
                    else
                    {
                        decimal dec1 = (serData[0].data[i] - serData[1].data[i]) * 100;
                        decimal dec2 = serData[1].data[i];
                        if (dec1 != 0 && dec2 != 0)
                        {
                            decimal pec = (serData[0].data[i] - serData[1].data[i]) * 100 / serData[1].data[i];
                            list.Pecent = Math.Round(pec, 2).ToString() + "%";
                            listFore.Add(list);
                        }
                        else
                        {
                            list.Pecent = "--";
                            listFore.Add(list);
                        }
                    }
                }
                mapNew.LoadForecast = listFore;

                mapNew.HistoryTotal = 0;
                Math.Round(serData[1].data.Sum(), 2);
                for (int rCount = 0; rCount < serData[1].data.Count; rCount++)
                {
                    if (serData[1].data[rCount] != -9999)
                    {
                        mapNew.HistoryTotal = mapNew.HistoryTotal + serData[1].data[rCount];
                    }
                }
                mapNew.ForeCastTotal = Math.Round(lstDecAvg.Sum(), 2);
                return mapNew;
            }
            catch (Exception ex)
            {
                ResultLoadForecastMap map2 = new ResultLoadForecastMap();
                ExecuteProcess proc = new ExecuteProcess();
                proc.ActionName = "";
                proc.ActionTime = DateTime.Now;
                proc.ActionUser = "";
                proc.ExceptionMsg = ex.Message;
                proc.Success = false;

                map2.ActionInfo = proc;
                return map2;
            }
        }


        private List<decimal> GetYuceData(BasicQuery basicQuery, QueryLoadForecast queryLoad)
        {
            List<ResultCompare> compReult = new List<ResultCompare>();
            DateTime deBegin = new DateTime();
            DateTime deEnd = new DateTime();
            TimeSpan ts = queryLoad.EndTime - queryLoad.StartTime;
            int Days = ts.Days;
            #region  获取35%数据

            if (Days == 0)
            {
                deBegin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")).AddDays(-10);
                deEnd = queryLoad.StartTime;
                // 循环前10天
                for (DateTime begTime = deBegin; begTime < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")); begTime = begTime.AddDays(1))
                {
                    BasicQuery baseQueryItem = basicQuery;
                    baseQueryItem.StartTime = begTime;
                    baseQueryItem.EndTime = begTime;
                    ResultCompare reultCom = GetDateTongJiData(baseQueryItem);
                    compReult.Add(reultCom);
                }
            }
            else
            {
                if (queryLoad.StartTime.CompareTo(DateTime.Now) > 0)
                {
                    BasicQuery baseQueryItem = basicQuery;
                    baseQueryItem.StartTime = DateTime.Now.AddDays(-(Days + 1)); ;
                    baseQueryItem.EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-1);
                    ResultCompare reultCom = GetDateTongJiData(baseQueryItem);
                    compReult.Add(reultCom);
                }
                else
                {
                    BasicQuery baseQueryItem = basicQuery;
                    baseQueryItem.StartTime = queryLoad.StartTime.AddDays(-(Days + 1)); ;
                    baseQueryItem.EndTime = queryLoad.StartTime.AddDays(-1);
                    ResultCompare reultCom = GetDateTongJiData(baseQueryItem);
                    compReult.Add(reultCom);
                }
            }
            #endregion

            #region 获取65%数据。
            //获取去年当天的预测值
            BasicQuery baseQuery2 = basicQuery;
            baseQuery2.StartTime = queryLoad.StartTime.AddYears(-1);
            baseQuery2.EndTime = queryLoad.EndTime.AddYears(-1);
            var resultList2 = GetDateTongJiData(baseQuery2);
            List<decimal> lstAvg2 = new List<decimal>();

            List<decimal> lstAvgMonth = GetAvgYuce(compReult, queryLoad);
            if (Days == 0)
            {
                foreach (var d in resultList2.Enery)
                {
                    List<decimal> lstDec = d.Value;
                    for (int i = 0; i < d.Value.Count; i++)
                    {
                        if (d.Value[i] > 0)
                        {
                            lstAvg2.Add(Math.Round((d.Value[i]), 2));
                        }
                    }
                }

                // 最终值。
                List<decimal> lstAvgEnd = new List<decimal>();
                for (int i = 0; i < lstAvgMonth.Count; i++)
                {
                    if (lstAvg2.Count == 0)
                    {
                        lstAvgEnd.Add(lstAvgMonth[i]);
                    }
                    else
                    {
                        lstAvgEnd.Add(lstAvgMonth[i] * 35 / 100 + lstAvg2[i] * 65 / 100);
                    }
                }

                return lstAvgEnd;
            }
            else
            {
                foreach (var d in resultList2.Enery)
                {
                    List<decimal> lstDec = d.Value;
                    for (int i = 0; i < d.Value.Count; i++)
                    {
                        lstAvg2.Add(Math.Round((d.Value[i]), 2));
                    }
                }

                // 最终值。
                List<decimal> lstAvgEnd = new List<decimal>();
                for (int i = 0; i < lstAvgMonth.Count; i++)
                {
                    if (lstAvgMonth.Count == Days + 1)
                    {
                        if (lstAvg2.Count == 0)
                        {
                            decimal avgDec = lstAvgMonth[i];
                            lstAvgEnd.Add(Math.Round(avgDec, 2));
                        }
                        else
                        {
                            decimal avgDec = lstAvgMonth[i] * 35 / 100 + lstAvg2[i] * 65 / 100;
                            lstAvgEnd.Add(Math.Round(avgDec, 2));
                        }
                    }
                    else
                    {
                        throw new Exception("没有历史数据，无法进行预测");
                    }
                }

                return lstAvgEnd;

            }

            #endregion
        }


        private List<decimal> GetAvgYuce(List<ResultCompare> compReult, QueryLoadForecast queryLoad)
        {
            TimeSpan ts = queryLoad.EndTime - queryLoad.StartTime;
            int days = ts.Days;
            if (days == 0)
            {
                #region
                // 数据循环次数
                List<int> intTest = new List<int>();
                List<decimal> decTest = new List<decimal>();
                for (int i = 0; i < compReult.Count; i++)
                {
                    foreach (var d in compReult[i].Enery)
                    {
                        int iCount = 1;
                        decimal avgDec = 0;
                        if (intTest.Count == 0)
                        {
                            for (int j = 0; j < d.Value.Count; j++)
                            {
                                intTest.Add(0);
                                decTest.Add(0);
                            }
                        }
                        for (int j = 0; j < d.Value.Count; j++)
                        {
                            if (d.Value[j] > 0)
                            {
                                intTest[j] = intTest[j] + 1;
                                decTest[j] = decTest[j] + d.Value[j];
                            }
                        }
                    }
                }
                List<decimal> lstAvgMonth = new List<decimal>();
                for (int i = 0; i < decTest.Count; i++)
                {
                    decimal decAvg = Math.Round((decTest[i] / intTest[i]), 2);
                    lstAvgMonth.Add(decAvg);
                }
                return lstAvgMonth;
                #endregion
            }
            else
            {
                #region
                List<decimal> lstValue = new List<decimal>();
                // 数据循环次数
                List<decimal> decTest = new List<decimal>();
                for (int i = 0; i < compReult.Count; i++)
                {
                    foreach (var d in compReult[i].Enery)
                    {
                        for (int j = 0; j < d.Value.Count; j++)
                        {
                            lstValue.Add(d.Value[j]);
                        }
                    }
                }
                return lstValue;
                #endregion
            }
        }

        /// <summary>
        /// 根据查询条件获取数据。
        /// </summary>
        /// <param name="basicQuery"></param>
        /// <returns></returns>
        public ResultCompare GetDateTongJiData(BasicQuery basicQuery)
        {
            var resultList = new ResultView.ResultCompare
            {
                ObjectName = new List<string>(),
                Enery = new Dictionary<string, List<decimal>>(),
                Dept = new List<string>()
            };

            List<Model.BaseLayerObject> objectList;
            if (basicQuery.AreaType == AreaType.Area)
            {
                objectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(
                       string.Format(" and layerobjectid = {0}", basicQuery.ObjectNum), "");
            }
            else
            {
                objectList = new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(
                string.Format(" and layerobjectid = {0}", basicQuery.ObjectNum), "");
            }

            EnergyContrast contst = new EnergyContrast();
            var result = contst.GetQueryLineChart(basicQuery);
            resultList.ObjectName = result.ObjectName;
            if (basicQuery.ItemCode != "00000")
            {
                var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + basicQuery.ItemCode + "'", " order by ItemcodeID")[0];
                resultList.Enery.Add(objectList[0].LayerObjectName, result.Enery[itemList.ItemCodeName]);
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
                resultList.Enery.Add(objectList[0].LayerObjectName, tempValue.ToList());
            }

            return resultList;
        }
    }
}
