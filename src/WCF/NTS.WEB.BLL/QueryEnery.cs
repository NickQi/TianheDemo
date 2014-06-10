using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.Common;
using NTS.WEB.DataContact;

namespace NTS.WEB.BLL
{
    public class QueryEnery
    {
        NTS.WEB.ProductInteface.IReportBase reportBll = NTS.WEB.ProductInteface.DataSwitchConfig.CreateReportBase();

        public BaseResult GetDeviceQuerySingleItemCodeEneryTotal(NTS.WEB.DataContact.BasicQuery query)
        {
            var ObjectList = new BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectNum), " order by deviceid");
            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 1;
            model.ObjectList = (from p in ObjectList select p.DeviceID).ToList<int>();
            model.ItemCode = query.ItemCode;
            model.Unit = BaseTool.GetChartUnit(query.Unit);
            model.Starttime = query.StartTime;
            model.Endtime = query.EndTime;
            var ResList = reportBll.GetBaseEneryDataList(model);
            return ResList;
        }
        public QueryEneryTotal GetDeviceQueryEneryTotal(NTS.WEB.DataContact.BasicQuery query)
        {
            BaseResult baseResult = new BaseResult();
            baseResult.BaseLayerObjectResults = new Dictionary<string, BaseData>();
            var Object = new BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectNum), " order by deviceid")[0];
            baseResult.BaseLayerObjectResults = new Dictionary<string, BaseData>();
            baseResult.BaseLayerObjectResults.Add(Object.DeviceID.ToString(), new BaseData());
            baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList = new System.Collections.Hashtable();
            baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].Datas = new List<DataItems>();
            baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList.Add("ITEMCOAL", 0);
            baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList.Add("ITEMCO2", 0);


            if (query.ItemCode != "00000")
            {
                query.ItemCode = Object.ItemCodeID;
                var resList = GetQueryDeviceSingleItemCodeEneryTotal(query);
                return resList != null ? new QueryEneryTotal { TotalEnergy = resList.BaseLayerObjectResults[Object.DeviceID.ToString()].Total } : null;
            }
            else
            {
                query.ItemCode = Object.ItemCodeID;
                query.ItemCode = Object.ItemCodeID;
                var resList = GetQueryDeviceSingleItemCodeEneryTotal(query);
                return resList != null ? new QueryEneryTotal { TotalEnergy = decimal.Parse(resList.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString()) } : null;
                //var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
                //List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();

                //Dictionary<string, decimal> DataValue = new Dictionary<string, decimal>();
                //Dictionary<string, decimal> CoalDataValue = new Dictionary<string, decimal>();
                //Dictionary<string, decimal> Co2DataValue = new Dictionary<string, decimal>();
                //for (var item = 0; item < ItemCodeStr.Count; item++)
                //{

                //    query.ItemCode = ItemCodeStr[item];
                //    var ResList = GetQueryDeviceSingleItemCodeEneryTotal(query);
                //    if (ResList != null)
                //    {

                //        baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].Total += ResList.BaseLayerObjectResults[Object.DeviceID.ToString()].Total;
                //        baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCOAL"] = decimal.Parse(baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString()) + decimal.Parse(ResList.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString());
                //        baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCO2"] = decimal.Parse(baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCO2"].ToString()) + decimal.Parse(ResList.BaseLayerObjectResults[Object.DeviceID.ToString()].ConvertDataValueList["ITEMCO2"].ToString());
                //        foreach (var li in ResList.BaseLayerObjectResults[Object.DeviceID.ToString()].Datas)
                //        {
                //            #region 累加赋值
                //            if (DataValue.ContainsKey(li.DatePick))
                //            {
                //                DataValue[li.DatePick] += li.DataValue;
                //            }
                //            else
                //            {
                //                DataValue.Add(li.DatePick, li.DataValue);
                //            }
                //            if (CoalDataValue.ContainsKey(li.DatePick))
                //            {
                //                CoalDataValue[li.DatePick] += li.CoalDataValue;
                //            }
                //            else
                //            {
                //                CoalDataValue.Add(li.DatePick, li.CoalDataValue);
                //            }
                //            if (Co2DataValue.ContainsKey(li.DatePick))
                //            {
                //                Co2DataValue[li.DatePick] += li.Co2DataValue;
                //            }
                //            else
                //            {
                //                Co2DataValue.Add(li.DatePick, li.Co2DataValue);
                //            }
                //            #endregion

                //            if (item == ItemCodeStr.Count - 1)
                //            {
                //                li.DataValue = DataValue[li.DatePick];
                //                li.CoalDataValue = CoalDataValue[li.DatePick];
                //                li.Co2DataValue = Co2DataValue[li.DatePick];
                //                baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].Datas.Add(li);
                //            }
                //        }

                //    }
                //}
                // if (baseResult != null) { return new QueryEneryTotal { TotalEnergy = baseResult.BaseLayerObjectResults[Object.DeviceID.ToString()].Total }; }
                //return null;
            }
        }



        public BaseResult GetQuerySingleItemCodeEneryTotal(NTS.WEB.DataContact.BasicQuery query)
        {
            var ObjectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectID={0}", query.ObjectNum), " order by LayerObjectID");
            // var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=-1", " order by ItemcodeID");
            //List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = (from p in ObjectList select p.LayerObjectID).ToList<int>();
            model.ItemCode = query.ItemCode;
            model.Unit = BaseTool.GetChartUnit(query.Unit);
            model.Starttime = query.StartTime;
            model.Endtime = query.EndTime;
            var ResList = reportBll.GetBaseEneryDataList(model);
            return ResList;
        }

        public BaseResult GetQueryDeviceSingleItemCodeEneryTotal(NTS.WEB.DataContact.BasicQuery query)
        {
            var ObjectList = new BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectNum), " order by deviceid");
            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();
            BaseQueryModel model = new BaseQueryModel();
            model.IsDevice = 1;
            model.ObjectList = (from p in ObjectList select p.DeviceID).ToList<int>();
            model.ItemCode = query.ItemCode;
            model.Unit = BaseTool.GetChartUnit(query.Unit);
            model.Starttime = query.StartTime;
            model.Endtime = query.EndTime;
            var ResList = reportBll.GetBaseEneryDataList(model);
            return ResList;
        }

        public ResultReal GetRealTime(NTS.WEB.DataContact.RealQuery query)
        {
            ResultReal result = new ResultReal();

            var objectList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
            var itemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + objectList[0].ItemCodeID + "'", " order by ItemcodeID")[0];

            result.Info = new DeviceInfo();
            result.Data = new RealData();
            result.Info.Affiliations = objectList[0].Organization;
            result.Info.DevType = objectList[0].DeviceType.ToString(CultureInfo.InvariantCulture);
            result.Info.Category = (itemList.ItemCodeNumber == "01000" || itemList.ItemCodeNumber == "01A00" ||
                                  itemList.ItemCodeNumber == "01B00" || itemList.ItemCodeNumber == "01C00" ||
                                  itemList.ItemCodeNumber == "01D00")
                                     ? "电表"
                                     : itemList.ItemCodeName + "表";
            result.Info.Nature = objectList[0].DEVMODE;
            result.Info.Number = objectList[0].DeviceNumber;
            result.Info.Status = objectList[0].Status;
            result.Info.Location = objectList[0].Location;
            result.Info.Rating = objectList[0].Rating.ToString();
            if (query.IsDetail == 1)
            {
                for (int i = 1; i < 4; i++)
                {
                    BaseListModel model = new BaseListModel();
                    model.Page = 1;
                    model.PageSize = 10000;
                    model.ObjectId = objectList[0].DeviceID;
                    model.CategoryId = i;
                    DataTable dt = new Real().GetRealTimeData(model);
                    if (dt.Rows.Count > 0)
                    {

                        if (i == 1)
                        {
                            result.Data.Pulse = new List<dataUnit>();
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                result.Data.Pulse.Add(new dataUnit()
                                    {
                                        Id = j + 1,
                                        DataName = dt.Rows[j]["DataPoint_Name"].ToString(),
                                        Unit = itemList.Unit,
                                        Value = decimal.Parse(dt.Rows[j]["Result"].ToString())
                                    });
                            }
                        }
                        else if (i == 2)
                        {
                            result.Data.Analog = new List<dataUnit>();
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {

                                result.Data.Analog.Add(new dataUnit()
                                    {
                                        Id = j + 1,
                                        DataName = dt.Rows[j]["DataPoint_Name"].ToString(),
                                        Unit = itemList.Unit,
                                        Value = decimal.Parse(dt.Rows[j]["Result"].ToString())
                                    });
                            }
                        }
                        else
                        {
                            result.Data.Switch = new List<dataUnit>();
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                result.Data.Switch.Add(new dataUnit()
                                    {
                                        Id = j + 1,
                                        DataName = dt.Rows[j]["DataPoint_Name"].ToString(),
                                        Unit = itemList.Unit,
                                        Value = decimal.Parse(dt.Rows[j]["Result"].ToString())
                                    });
                            }
                        }
                    }
                }
            }

            //result.Data.Analog = new List<dataUnit>();
            //result.Data.Analog.Add(new dataUnit() { Id = 1, DataName = "29189H92-00", Unit = "kwh", Value = 900.88m });
            //result.Data.Pulse = new List<dataUnit>();
            //result.Data.Pulse.Add(new dataUnit() { Id = 1, DataName = "29189H92-10", Unit = "kwh", Value = 400.88m });
            //result.Data.Switch = new List<dataUnit>();
            //result.Data.Switch.Add(new dataUnit() { Id = 1, DataName = "29189H92-20", Unit = "kwh", Value = 200.88m });
            return result;
        }

        public QueryEneryTotal GetQueryEneryTotal(NTS.WEB.DataContact.BasicQuery query)
        {
            BaseResult baseResult = new BaseResult();
            baseResult.BaseLayerObjectResults = new Dictionary<string, BaseData>();
            var Object = new BLL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectID={0}", query.ObjectNum), " order by LayerObjectID")[0];
            baseResult.BaseLayerObjectResults = new Dictionary<string, BaseData>();
            baseResult.BaseLayerObjectResults.Add(Object.LayerObjectID.ToString(), new BaseData());
            baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList = new System.Collections.Hashtable();
            baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].Datas = new List<DataItems>();
            baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList.Add("ITEMCOAL", 0);
            baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList.Add("ITEMCO2", 0);
            if (query.ItemCode != "00000")
            {
                var ResList = GetQuerySingleItemCodeEneryTotal(query);
                if (ResList != null) { return new QueryEneryTotal { TotalEnergy = decimal.Parse(ResList.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString()) }; }//TotalEnergy = ResList.BaseLayerObjectResults[Object.LayerObjectID.ToString()].Total
                return null;
            }
            else
            {
                var ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
                List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();

                Dictionary<string, decimal> DataValue = new Dictionary<string, decimal>();
                Dictionary<string, decimal> CoalDataValue = new Dictionary<string, decimal>();
                Dictionary<string, decimal> Co2DataValue = new Dictionary<string, decimal>();
                for (var item = 0; item < ItemCodeStr.Count; item++)
                {

                    query.ItemCode = ItemCodeStr[item];
                    var ResList = GetQuerySingleItemCodeEneryTotal(query);
                    if (ResList != null)
                    {

                        baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].Total += ResList.BaseLayerObjectResults[Object.LayerObjectID.ToString()].Total;
                        baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCOAL"] = decimal.Parse(baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString()) + decimal.Parse(ResList.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString());
                        baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCO2"] = decimal.Parse(baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCO2"].ToString()) + decimal.Parse(ResList.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCO2"].ToString());
                        foreach (var li in ResList.BaseLayerObjectResults[Object.LayerObjectID.ToString()].Datas)
                        {
                            #region 累加赋值
                            if (DataValue.ContainsKey(li.DatePick))
                            {
                                DataValue[li.DatePick] += li.DataValue;
                            }
                            else
                            {
                                DataValue.Add(li.DatePick, li.DataValue);
                            }
                            if (CoalDataValue.ContainsKey(li.DatePick))
                            {
                                CoalDataValue[li.DatePick] += li.CoalDataValue;
                            }
                            else
                            {
                                CoalDataValue.Add(li.DatePick, li.CoalDataValue);
                            }
                            if (Co2DataValue.ContainsKey(li.DatePick))
                            {
                                Co2DataValue[li.DatePick] += li.Co2DataValue;
                            }
                            else
                            {
                                Co2DataValue.Add(li.DatePick, li.Co2DataValue);
                            }
                            #endregion

                            if (item == ItemCodeStr.Count - 1)
                            {
                                li.DataValue = DataValue[li.DatePick];
                                li.CoalDataValue = CoalDataValue[li.DatePick];
                                li.Co2DataValue = Co2DataValue[li.DatePick];
                                baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].Datas.Add(li);
                            }
                        }

                    }
                }
                if (baseResult != null) { return new QueryEneryTotal { TotalEnergy = decimal.Parse(baseResult.BaseLayerObjectResults[Object.LayerObjectID.ToString()].ConvertDataValueList["ITEMCOAL"].ToString()) }; }
                return null;
            }
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

        public NTS.WEB.ResultView.ShopOrderResult GetShopOrder(NTS.WEB.DataContact.QueryOrder query)
        {
            string keyCatch = string.Empty;
            foreach (var q in query.ObjectNum)
            {
                keyCatch += q.ToString() + "_";
            }
            keyCatch += query.ItemCode + query.OrderWay + query.StartTime + query.EndTime + query.Particle;
            if (CacheHelper.GetCache(keyCatch) != null)
            {
                return PaddingList((ShopOrderResult)CacheHelper.GetCache(keyCatch), query);
            }

            #region 定义区
            ShopOrderResult shopOrderResult = new ShopOrderResult();
            shopOrderResult.OrderList = new List<BaseOrder>();
            shopOrderResult.page = new Padding();
            #endregion

            var ItemList = new List<Model.Itemcode>();
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
            // var deepth = int.Parse(ConfigurationManager.AppSettings["ShopLevel"]);
            if (query.ItemCode == "00000")
            {
                ItemList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0", " order by ItemcodeID");
            }
            else
            {
                ItemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
            }
            List<string> ItemCodeStr = (from item in ItemList select item.ItemCodeNumber).ToList<string>();
            var model = new BaseQueryModel();
            model.IsDevice = 0;
            model.ObjectList = query.ObjectNum;
            Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
            Dictionary<string, decimal> tempAreaConvert = new Dictionary<string, decimal>();
            // decimal[] tempConvert = new decimal[objectList.Count];
            //  decimal[] tempAreaConvert = new decimal[objectList.Count];
            foreach (var item in ItemCodeStr)
            {
                model.ItemCode = item;
                model.Unit = GetUnit(query.StartTime, query.EndTime);
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
                var resList = reportBll.GetBaseEneryDataList(model);
                const int order = 1;
                foreach (var r in resList.BaseLayerObjectResults)
                {
                    if (tempConvert.ContainsKey(r.Value.baseLayerObject.LayerObjectName))
                    {
                        if (query.ItemCode == "00000")
                        {

                            tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                            tempAreaConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()) /
                                decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());
                        }
                        else
                        {
                            tempConvert[r.Value.baseLayerObject.LayerObjectName] += r.Value.Total;
                            tempAreaConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                r.Value.Total /
                                decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());
                        }
                    }
                    else
                    {
                        if (query.ItemCode == "00000")
                        {
                            tempConvert.Add(r.Value.baseLayerObject.LayerObjectName,
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()));
                            tempAreaConvert.Add(r.Value.baseLayerObject.LayerObjectName,
                                                decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()) /
                                                decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString()));
                        }
                        else
                        {
                            tempConvert.Add(r.Value.baseLayerObject.LayerObjectName,
                                            r.Value.Total);
                            tempAreaConvert.Add(r.Value.baseLayerObject.LayerObjectName,
                                                r.Value.Total /
                                                decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString()));
                        }
                    }

                    shopOrder.TotalEneryOrderList.Add(new EneryOrder()
                    {
                        BuildingName = r.Value.baseLayerObject.LayerObjectName,
                        EneryValue = double.Parse(tempConvert[r.Value.baseLayerObject.LayerObjectName].ToString("f2")),
                        OrderNum = order
                    });


                    shopOrder.AreaEneryOrderList.Add(new EneryOrder()
                    {
                        BuildingName = r.Value.baseLayerObject.LayerObjectName,
                        EneryValue = double.Parse(tempAreaConvert[r.Value.baseLayerObject.LayerObjectName].ToString("f2")),
                        OrderNum = order
                    });
                }
            }

            var res1 = from p in shopOrder.TotalEneryOrderList group p by p.BuildingName into g select new { g.Key, BuildingName = g.Max(p => p.BuildingName), OrderNum = g.Max(p => p.OrderNum), EneryValue = g.Max(p => p.EneryValue) };
            var res2 = from p in shopOrder.AreaEneryOrderList group p by p.BuildingName into g select new { g.Key, BuildingName = g.Max(p => p.BuildingName), OrderNum = g.Max(p => p.OrderNum), EneryValue = g.Max(p => p.EneryValue) };

            foreach (var c in res1)
            {
                shopOrderLast.TotalEneryOrderList.Add(new EneryOrder { BuildingName = c.BuildingName, EneryValue = c.EneryValue, OrderNum = c.OrderNum });
            }

            foreach (var c in res2)
            {
                shopOrderLast.AreaEneryOrderList.Add(new EneryOrder { BuildingName = c.BuildingName, EneryValue = c.EneryValue, OrderNum = c.OrderNum });
            }
            int record = shopOrderLast.TotalEneryOrderList.Count;
            shopOrderResult.page = new Padding()
            {
                Current = query.PageCurrent,
                Total = (record % query.PageSize == 0) ? record / query.PageSize : record / query.PageSize + 1
            };
            int orders = 1;
            if (query.Particle == "total")
            {
                foreach (var r in shopOrderLast.TotalEneryOrderList)
                {
                    shopOrderResult.OrderList.Add(new BaseOrder()
                        {
                            Energy = decimal.Parse(r.EneryValue.ToString("f2")),
                            Id = orders,
                            Title = r.BuildingName,
                            Units = query.ItemCode == "00000" ? "T" : ItemList[0].Unit
                        });
                    orders++;
                }
            }
            else
            {
                foreach (var r in shopOrderLast.AreaEneryOrderList)
                {
                    shopOrderResult.OrderList.Add(new BaseOrder()
                    {
                        Energy = decimal.Parse(r.EneryValue.ToString("f2")),
                        Id = r.OrderNum,
                        Title = r.BuildingName,
                        Units = query.ItemCode == "00000" ? "T" : ItemList[0].Unit
                    });
                }
            }

            if (CacheHelper.GetCache(keyCatch) == null)
            {
                CacheHelper.SetCache(keyCatch, shopOrderResult);
            }
            return PaddingList(shopOrderResult, query);
        }

        private void VerifyPersonOrAreaExist(NTS.WEB.DataContact.QueryOrderObjects query)
        {
            switch (query.QueryType)
            {
                case EnergyAnalyseQueryType.UnitArea:
                case EnergyAnalyseQueryType.UnitPerson:
                    string where = "";
                    List<Model.BaseLayerObject> LayerObjectList;
                    foreach (var areaid in query.AreaIdLst)
                    {
                        where = string.Format(@"and layerobjectid={0} ", areaid);
                        if (query.ObjType == AreaType.Liquid)
                        {//取液态
                            LayerObjectList = new DAL.BaseLayerObject().GetBaseFuncLayerObjectList(where, string.Format(" order by LayerObjectID"));
                        }
                        else
                        {
                            LayerObjectList = new DAL.BaseLayerObject().GetBaseLayerObjectList(where, string.Format(" order by LayerObjectID"));
                        }
                        if (LayerObjectList.Count > 0)
                        {
                            switch (query.QueryType)
                            {
                                case EnergyAnalyseQueryType.UnitArea:
                                    if (!(LayerObjectList[0].AreaNum > 0))
                                    {
                                        throw new Exception(string.Format("位置：【{0}】 单位面积尚未配置", LayerObjectList[0].LayerObjectName));
                                    }
                                    break;
                                case EnergyAnalyseQueryType.UnitPerson:
                                    if (!(LayerObjectList[0].PersonNum > 0))
                                    {
                                        throw new Exception(string.Format("位置：【{0}】 人均尚未配置", LayerObjectList[0].LayerObjectName));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        public ResultOrder GetShopOrderNew(NTS.WEB.DataContact.QueryOrderObjects query)
        {
            try
            {
                string keyCatch = string.Empty;
                foreach (var q in query.AreaIdLst)
                {
                    keyCatch += q.ToString() + "_";
                }
                keyCatch += query.ItemCode + query.StartTime + query.EndTime + query.QueryType + query.ObjType;
                if (CacheHelper.GetCache(keyCatch) != null)
                {
                    return (ResultOrder)CacheHelper.GetCache(keyCatch);
                }

                VerifyPersonOrAreaExist(query);


                #region 返回对象定义
                ResultOrder resultOrder = new ResultOrder();
                resultOrder.lineHighChart = new LineHighChart()
                {
                    series =
                        new List<Series>()
                };
                resultOrder.lineHighChart.series.Add(new Series() { data = new List<EneryHighChart>() });
                resultOrder.pieHighChart = new PieHighChart()
                {
                    series =
                        new List<Series>()
                };
                resultOrder.pieHighChart.series.Add(new Series() { data = new List<EneryHighChart>() });

                resultOrder.OrderLst = new List<EnergyOrder>();
                #endregion

                List<Model.Itemcode> itemCodeList = new List<Model.Itemcode>();
                if (query.ItemCode == "00000")
                {//总能耗
                    itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ParentID=0 ", " order by ItemcodeID");
                    resultOrder.Unit = "T";//标准煤单位
                }
                else
                {
                    itemCodeList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                    if (itemCodeList.Count > 0)
                    {
                        resultOrder.Unit = itemCodeList[0].Unit;//单个分类分项单位
                    }
                }
                switch(query.QueryType)
                {
                    case EnergyAnalyseQueryType.Convert2Co2:
                    case EnergyAnalyseQueryType.Convert2Coal:
                         resultOrder.Unit = "T";//标准煤单位
                        break;
                    case EnergyAnalyseQueryType.Convert2Money:
                        resultOrder.Unit = "元";//标准煤单位
                        break;
                }
                Dictionary<string, decimal> tempConvert = new Dictionary<string, decimal>();
                Dictionary<string, int> tempObjectConvert = new Dictionary<string, int>();
                var model = new BaseQueryModel();
                model.IsDevice = 0;
                model.ObjectList = query.AreaIdLst;
                //model.Unit = GetUnit(query.StartTime, query.EndTime);
                model.Unit = BaseTool.GetChartUnit(query.Particle);

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
                foreach (var item in itemCodeList)
                {
                    model.ItemCode = item.ItemCodeNumber;
                    BaseResult resList = reportBll.GetBaseEneryDataList(model, query.ObjType == AreaType.Liquid ? true : false);
                    if (resList == null)
                    {
                        continue;
                    }
                    foreach (var r in resList.BaseLayerObjectResults)
                    {
                        if (!tempConvert.ContainsKey(r.Value.baseLayerObject.LayerObjectName))
                        {
                            tempConvert.Add(r.Value.baseLayerObject.LayerObjectName, 0);
                        }
                        if (!tempObjectConvert.ContainsKey(r.Value.baseLayerObject.LayerObjectName))
                        {
                            tempObjectConvert.Add(r.Value.baseLayerObject.LayerObjectName, r.Value.baseLayerObject.LayerObjectID);
                        }
                        if (tempConvert.ContainsKey(r.Value.baseLayerObject.LayerObjectName))
                        {
                            if (query.ItemCode == "00000")
                            {//选择总能耗后把其他分类分项的能耗转化成标准煤
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default: //默认总能耗
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                  decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());//
                                        break;
                                    case EnergyAnalyseQueryType.UnitArea://单位面积能耗
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                    decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()) /
                                    decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());
                                        break;
                                    case EnergyAnalyseQueryType.UnitPerson://人均能耗
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                 decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString()) /
                                 decimal.Parse(r.Value.baseLayerObject.PersonNum.ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Coal://标准煤
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Co2://二氧化碳
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMCO2"].ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Money://人民币
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMMONEY"].ToString());
                                        break;
                                    default:
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());//
                                        break;
                                }
                            }
                            else
                            {
                                switch (query.QueryType)
                                {
                                    case EnergyAnalyseQueryType.Default: //默认总能耗
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] += r.Value.Total;
                                        break;
                                    case EnergyAnalyseQueryType.UnitArea://单位面积能耗
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                     r.Value.Total /
                                     decimal.Parse(r.Value.baseLayerObject.AreaNum.ToString());
                                        break;
                                    case EnergyAnalyseQueryType.UnitPerson://人均能耗
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                     r.Value.Total /
                                     decimal.Parse(r.Value.baseLayerObject.PersonNum.ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Coal://标准煤
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMCOAL"].ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Co2://二氧化碳
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMCO2"].ToString());
                                        break;
                                    case EnergyAnalyseQueryType.Convert2Money://人民币
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] +=
                                            decimal.Parse(r.Value.ConvertDataValueList["ITEMMONEY"].ToString());
                                        break;
                                    default:
                                        tempConvert[r.Value.baseLayerObject.LayerObjectName] += r.Value.Total;
                                        break;
                                }


                            }
                        }

                    }
                }
                var tempAsc = from tt in tempConvert orderby tt.Value ascending select tt;//LineChart按照能耗值从低到高排序

                foreach (var tempConvertItem in tempAsc)
                {
                    resultOrder.lineHighChart.series[0].data.Add(new EneryHighChart()
                    {
                        name = tempConvertItem.Key,
                        id = tempObjectConvert[tempConvertItem.Key].ToString(),
                        y = decimal.Parse(tempConvertItem.Value.ToString("f2"))

                    });
                }

                var tempDesc = from tt in tempConvert orderby tt.Value descending select tt;//PieChart按照能耗值从高到低排序
                int temppieindex = 1;
                decimal temppiecount = 0;
                foreach (var tempConvertItem in tempDesc)
                {
                    if (temppieindex < 6)
                    {
                        resultOrder.pieHighChart.series[0].data.Add(new EneryHighChart()
                        {
                            name = tempConvertItem.Key,
                            y = decimal.Parse(tempConvertItem.Value.ToString("f2"))
                        });
                    }
                    else
                    {
                        temppiecount += decimal.Parse(tempConvertItem.Value.ToString("f2"));

                    }
                    temppieindex++;
                }
                if (temppieindex > 6)
                {
                    resultOrder.pieHighChart.series[0].data.Add(new EneryHighChart()
                    {
                        name = "其他",
                        y = temppiecount

                    });
                }


                int order = 1;
                foreach (var tt in resultOrder.lineHighChart.series[0].data)
                {
                    resultOrder.OrderLst.Add(new EnergyOrder()
                                                 {
                                                     Order = order,
                                                     Tm = model.Unit == ChartUnit.unit_hour ? query.StartTime.ToString("yyyy-MM-dd") : query.StartTime.ToString("yyyy-MM-dd") + "~" + query.EndTime.ToString("yyyy-MM-dd"),
                                                     Obj = tt.name,
                                                     //ObjID=tempObjectConvert[tt.name],
                                                     Val = tt.y,
                                                     EneType = itemCodeList.Count > 1 ? "总能耗" : itemCodeList[0].ItemCodeName

                                                 });
                    order++;
                }

                if (CacheHelper.GetCache(keyCatch) == null)
                {
                    CacheHelper.SetCache(keyCatch, resultOrder, 30);
                }
                return resultOrder;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }





        private ShopOrderResult PaddingList(ShopOrderResult shopOrderResult, NTS.WEB.DataContact.QueryOrder query)
        {
            var padding = new ShopOrderResult();
            padding.OrderList = new List<BaseOrder>();
            int min = query.PageSize * (query.PageCurrent - 1);
            int max = min + query.PageSize;
            if (query.OrderWay != "asc")
            {
                var orderRes =
                from p in shopOrderResult.OrderList
                orderby p.Energy descending
                select p;
                shopOrderResult.OrderList = orderRes.ToList();
            }
            else
            {
                var orderRes =
                    from p in shopOrderResult.OrderList
                    orderby p.Energy ascending
                    select p;
                shopOrderResult.OrderList = orderRes.ToList();
            }
            for (int sr = 0; sr < shopOrderResult.OrderList.Count; sr++)
            {
                if (sr >= min && sr < max)
                {
                    shopOrderResult.OrderList[sr].Id = sr + 1;
                    padding.OrderList.Add(shopOrderResult.OrderList[sr]);
                }
            }
            padding.page = shopOrderResult.page;
            padding.page.Current = query.PageCurrent;
            return padding;
        }


    }
}
