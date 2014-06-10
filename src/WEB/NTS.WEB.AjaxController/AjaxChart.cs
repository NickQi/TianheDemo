using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NTS.WEB.Common;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.WEB.AjaxController
{
    public class AjaxChart
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;


        [Framework.Common.CustomAjaxMethod]
        public string RealChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<RealQuery>(inputValue);
            var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                  .RealChart(query);
            //result.Dept.ToArray()
            return JsonForHightCharts.ShowColorLineChart("设备实时曲线", result.ObjectName.ToArray(), result.Dept.ToArray(),
                                                        result.Enery[result.Enery.Keys.First<string>()].ToList<decimal>());
        }

        /// <summary>
        /// 展示首页当日用电趋势
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public ResultRealLine IndexElectricRealLineChart()
        {
            ResultView.ResultRealLine result
                = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").IndexElectricityRealLineChart();
            return result;
        }

        /// <summary>
        /// 展示设备当日用电趋势
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public ResultRealLine DeviceRealChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<RealQuery>(inputValue);
            var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                  .DeviceRealChart(query);

            return result;
        }

        /// <summary>
        /// 展示首页电能耗占比图
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public string IndexElectricityPieChart()
        {
            ResultIndexPieChart result
                = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").IndexElectricityPieChart();
            return JsonForHightCharts.ShowColorPieChart("电分类占比", result.ItemCode.Select(p => p.ItemCodeName).ToArray(),
                                                        result.ItemCode.Select(p => p.Unit).ToArray(),
                                                        result.ItemCodeEnery);
        }

        [Framework.Common.CustomAjaxMethod]
        public string IndexElectricityLineChart()
        {
            ResultView.ResultItemCode result
                = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").IndexAvgElectricityLineChart();
            return JsonForHightCharts.ShowMoreColorLineChart("24小时实时监控", result.ObjectName.Select(p => (Convert.ToDateTime(p).ToString("HH").Trim())).ToArray(), result.Dept.ToArray(),
                                                        result.Enery);
        }

        /// <summary>
        /// 分类分项联动
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public ResultView.ItemList IndexItem()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryEnergyIterm>(inputValue);
            ResultView.ItemList result
                = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").IndexItems(query);
            return result;
        }

        [Framework.Common.CustomAjaxMethod]
        public string QueryPieChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicQuery>(inputValue);
            var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                  .GetQueryPieChart(query);
            //result.Dept.ToArray()
            return JsonForHightCharts.ShowColorPieChart("区域占比", result.ObjectName.ToArray(), result.Dept.ToArray(),
                                                        result.Enery);
        }
        [Framework.Common.CustomAjaxMethod]
        public string ShowQueryLineChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var tabId = int.Parse(_ntsPage.Request.Form["TabId"]);
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicQuery>(inputValue);
            var query2 = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicQuery>(inputValue);
            switch (tabId)
            {
                case 0:
                    try
                    {
                        var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                              .GetQueryLineChart(query);
                        return JsonForHightCharts.ShowMoreColorLineChart("分类分项曲线", result.ObjectName.ToArray(),
                                                                         result.Dept.ToArray(),
                                                                         result.Enery);
                    }
                    catch (Exception e)
                    {
                        return JsonForHightCharts.ShowMoreColorLineChart("分类分项曲线", null,
                                                                         null,
                                                                         null);
                    }
                    break;
                case 1:
                    query2.StartTime = query.StartTime.AddYears(-1);
                    query2.EndTime = query.EndTime.AddYears(-1);
                    try
                    {
                        var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                              .GetTwoQueryLineChart(query, query2, tabId);
                        return JsonForHightCharts.ShowMoreColorLineChart("分类分项曲线", result.ObjectName.ToArray(),
                                                                         result.Dept.ToArray(),
                                                                         result.Enery);
                    }
                    catch (Exception e)
                    {
                        return JsonForHightCharts.ShowMoreColorLineChart("分类分项曲线", null,
                                                                         null,
                                                                         null);
                    }
                    break;

                case 2:
                    query2.StartTime = query.StartTime.AddMonths(-1);
                    query2.EndTime = query.EndTime.AddMonths(-1);
                    try
                    {
                        var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                              .GetTwoQueryLineChart(query, query2, tabId);
                        return JsonForHightCharts.ShowMoreColorLineChart("分类分项曲线", result.ObjectName.ToArray(),
                                                                         result.Dept.ToArray(),
                                                                         result.Enery);
                    }
                    catch (Exception e)
                    {
                        return JsonForHightCharts.ShowMoreColorLineChart("分类分项曲线", null,
                                                                         null,
                                                                         null);
                    }
                    break;
            }
            return string.Empty;

        }




        [Framework.Common.CustomAjaxMethod]
        public ResultQuota GetQuotaAnalyseChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var Param = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryQuota>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").GetQuotaAnalyseChart(Param);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultEnergyAnalyse GetEnergyAnalyseLineChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var Param = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAnalyse>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").GetEnergyAnalyseLineChart(Param);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public EnergyAnalyseCompare GetEnergyAnalyseCompare()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var Param = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAnalyse>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").GetEnergyAnalyseCompare(Param);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultEnergyAnalysePie GetEnergyAnalysePie()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var Param = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAnalyse>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").GetEnergyAnalysePie(Param);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public string ShowCompareLineChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var tabId = int.Parse(_ntsPage.Request.Form["TabId"]);
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryCompare>(inputValue);
            var result = new ResultCompare();
            switch (tabId)
            {
                case 0:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                  .GetCompareChart(query);
                    break;

                case 1:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                 .GetAreaCompareChart(query); ;
                    break;
            }

            return JsonForHightCharts.ShowColorColumnChart("对比柱状图", result.ObjectName.ToArray(), result.Dept.ToArray(),
              result.Enery);
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultDevice IndexDeviceList()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryDevice2>(inputValue);
            ResultView.ResultDevice result
             = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").GetDeviceList(query);
            return result;
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportQueryLineChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];

            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<BasicQuery>(inputValue);

            var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                  .GetQueryLineChart(query);

            try
            {
                string name = new BLL.BaseLayerObject().GetBaseLayerObjectList(
                    string.Format(" and layerobjectid={0}", query.ObjectNum), " order by LayerObjectID")[0].LayerObjectName;
                string itemUnit = result.Dept[0];
                var dept = string.Empty;
                var itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                if (itList.Count > 0)
                {
                    dept = itList[0].Unit;
                }

                string itemName = query.ItemCode == "00000" ? "总能耗" : dept;

                var ItemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                string ItemName = query.ItemCode == "00000" ? "总能耗" : ItemList[0].ItemCodeName;
                DataTable dt = TableView.CreateBaseDataTable();
                if (query.ItemCode != "00000")
                {
                    for (var r = 0; r < result.Enery[itList[0].ItemCodeName].Count; r++)
                    {

                        DataRow dr = dt.NewRow();
                        dr[1] = result.ObjectName[r];
                        dr[2] = query.ObjectNum;
                        dr[3] = name;
                        dr[4] = result.ObjectName[r];
                        dr[5] = result.ObjectName[r];
                        dr[6] = itemName;
                        dr[7] = decimal.Round(decimal.Parse(result.Enery[itList[0].ItemCodeName][r].ToString()), 2).ToString();

                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    foreach (var i in result.Enery)
                    {
                        DataRow drs = dt.NewRow();
                        drs[1] = i.Key;
                        drs[2] = "-";
                        drs[3] = "-";
                        drs[4] = "-";
                        drs[5] = "-";
                        drs[6] = "-";
                        drs[7] = "-";
                        dt.Rows.Add(drs);
                        for (var r = 0; r < result.Enery[i.Key].Count; r++)
                        {

                            DataRow dr = dt.NewRow();
                            dr[1] = result.ObjectName[r];
                            dr[2] = query.ObjectNum;
                            dr[3] = name;
                            dr[4] = result.ObjectName[r];
                            dr[5] = result.ObjectName[r];
                            dr[6] = itemName;
                            dr[7] = decimal.Round(decimal.Parse(result.Enery[i.Key][r].ToString()), 2);

                            dt.Rows.Add(dr);
                        }
                    }
                }

                if (dt != null)
                {
                    string temp_path = AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                    if (!Directory.Exists(temp_path))
                    {
                        Directory.CreateDirectory(temp_path);
                        string[] files = Directory.GetFiles(temp_path);
                        foreach (string fn in files)
                        {
                            File.Delete(temp_path + fn);
                        }
                    }
                    string save_path = DateTime.Now.Ticks + ".xls";

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\能耗查询.xls";

                    TemplateParam param = new TemplateParam(name + " " + ItemName + " ", new CellParam(0, 0), query.StartTime.ToString("yyyy-MM-dd") + "~" + query.EndTime.ToString("yyyy-MM-dd"), new CellParam(3, 0), false, new CellParam(4, 0));
                    param.DataColumn = new[] { 0, 3, 1, 7 };
                    param.ItemUnit = "（单位：" + itemUnit + "）";
                    param.ItemUnitCell = new CellParam(3, 4);
                    dt.TableName = "能耗查询统计";

                    ExportHelper.ExportExcel(dt, temp_path + save_path, templatePath, param);

                    return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                }

            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败：由于当前无数据或其他原因导致" + ex.Message + "\"}";
            }

        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportExcelDataRanking()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];

            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<NTS.WEB.DataContact.QueryOrder>(inputValue);
            query.PageCurrent = 1;
            query.PageSize = 10000;

            string icode = query.ItemCode;
            var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery")
                                  .GetShopOrder(query);
            query.Particle = "area";
            var result2 = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery")
                                  .GetShopOrder(query);
            var dept = string.Empty;
            var itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
            if (itList.Count > 0)
            {
                dept = itList[0].Unit;
            }

            string itemName = icode == "00000" ? "T" : dept;

            var ItemList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
            string ItemName = query.ItemCode == "00000" ? "总能耗" : ItemList[0].ItemCodeName;


            try
            {
                string unittype = string.IsNullOrEmpty(HttpContext.Current.Request["unittype"]) ? "" : HttpContext.Current.Request["unittype"];//

                string type = "0";
                string name = "";
                switch (type)
                {
                    case "0":
                        name = "总能耗";
                        break;
                    case "1":
                        name = "面积能耗";
                        break;
                    case "2":
                        name = "人均能耗";
                        break;
                }



                if (query.ObjectNum.Count > 0)
                {
                    DataTable dt = TableView.CreateOrderBaseDataTable();
                    for (var r = 0; r < result.OrderList.Count; r++)
                    {


                        DataRow dr = dt.NewRow();
                        dr[1] = result.OrderList[r].Title;
                        dr[2] = query.ObjectNum;
                        dr[3] = result.OrderList[r].Title;
                        dr[4] = result.OrderList[r].Title;
                        dr[5] = result.OrderList[r].Title;
                        dr[6] = itemName;
                        dr[7] = decimal.Round(decimal.Parse(result.OrderList[r].Energy.ToString()), 2).ToString();
                        dr[8] = decimal.Round(decimal.Parse(result2.OrderList[r].Energy.ToString()), 2).ToString();

                        dt.Rows.Add(dr);

                    }
                    string temp_path = AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                    if (!Directory.Exists(temp_path))
                    {
                        Directory.CreateDirectory(temp_path);
                        string[] files = Directory.GetFiles(temp_path);
                        foreach (string fn in files)
                        {
                            File.Delete(temp_path + fn);
                        }
                    }
                    string save_path = DateTime.Now.Ticks + ".xls";

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\能耗排名表.xls";

                    TemplateParam param = new TemplateParam(ItemName + " ", new CellParam(0, 0), query.StartTime.ToString("yyyy-MM-dd") + "~" + query.EndTime.ToString("yyyy-MM-dd"), new CellParam(3, 0), false, new CellParam(4, 0));
                    param.DataColumn = new[] { 0, 3, 7, 8 };
                    param.ItemUnit = "（单位：" + dept + "）";
                    param.ItemUnitCell = new CellParam(3, 4);

                    param.SortColumn = 0;
                    dt.TableName = "能耗排名表";

                    ExportHelper.ExportExcel(dt, temp_path + save_path, templatePath, param);

                    return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                }

            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败：由于当前无数据或其他原因导致" + ex.Message + "\"}";
            }
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportExcelDataRankingNew()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];


            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryOrderObjects>(inputValue);
            string icode = query.ItemCode;
            query.QueryType = EnergyAnalyseQueryType.Default;
            ResultOrder resultAll = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery")
                                  .GetShopOrderNew(query);

            query.QueryType = EnergyAnalyseQueryType.UnitArea;

            ResultOrder resultArea = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery")
                            .GetShopOrderNew(query);


            query.QueryType = EnergyAnalyseQueryType.UnitPerson;

            ResultOrder resultPerson = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IQueryEnery>("EneryQuery")
                             .GetShopOrderNew(query);


            var dept = string.Empty;
            var itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
            if (itList.Count > 0)
            {
                dept = itList[0].Unit;
            }

            string unit = icode == "00000" ? "T" : dept;
            //switch (query.QueryType)
            //{
            //    case EnergyAnalyseQueryType.Convert2Co2:
            //    case EnergyAnalyseQueryType.Convert2Coal:
            //        unit = "T";//标准煤单位
            //        break;
            //    case EnergyAnalyseQueryType.Convert2Money:
            //        unit = "元";//标准煤单位
            //        break;
            //}
            string itemCodeName = itList.Count == 0 ? "总能耗" : itList[0].ItemCodeName;


            try
            {
                if (query.AreaIdLst.Count > 0)
                {
                    DataTable dt = TableView.CreateOrderBaseDataTable();
                    for (var r = 0; r < resultAll.OrderLst.Count; r++)
                    {


                        DataRow dr = dt.NewRow();
                        dr[1] = resultAll.OrderLst[r].Obj;
                        dr[2] = query.AreaIdLst;
                        dr[3] = resultAll.OrderLst[r].Obj;
                        dr[4] = resultAll.OrderLst[r].Obj;
                        dr[5] = resultAll.OrderLst[r].Obj;
                        dr[6] = itemCodeName;
                        dr[7] = decimal.Round(decimal.Parse(resultAll.OrderLst[r].Val.ToString()), 2).ToString();
                        dr[8] = resultArea.OrderLst == null ? "0" : decimal.Round(decimal.Parse(resultArea.OrderLst[r].Val.ToString()), 2).ToString();
                        dr[9] = resultPerson.OrderLst == null ? "0" : decimal.Round(decimal.Parse(resultPerson.OrderLst[r].Val.ToString()), 2).ToString();

                        dt.Rows.Add(dr);

                    }
                    string temp_path = AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                    if (!Directory.Exists(temp_path))
                    {
                        Directory.CreateDirectory(temp_path);
                        string[] files = Directory.GetFiles(temp_path);
                        foreach (string fn in files)
                        {
                            File.Delete(temp_path + fn);
                        }
                    }
                    string save_path = DateTime.Now.Ticks + ".xls";



                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\新能耗排名表.xls";
                    if (!File.Exists(templatePath))
                    {
                        return "{\"status\":\"error\",\"msg\":\"未发现Excel模板文件\"}";
                    }
                    TemplateParam param = new TemplateParam(itemCodeName + " ", new CellParam(0, 0), query.StartTime.ToString("yyyy-MM-dd") + "~" + query.EndTime.ToString("yyyy-MM-dd"), new CellParam(3, 0), false, new CellParam(4, 0));
                    param.DataColumn = new[] { 0, 3, 7, 8, 9 };
                    param.ItemUnit = "（单位：" + unit + "）";
                    param.ItemUnitCell = new CellParam(3, 4);

                    param.SortColumn = 0;
                    dt.TableName = "能耗排名表";

                    ExportHelper.ExportExcel(dt, temp_path + save_path, templatePath, param);

                    return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                }

            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败：由于当前无数据或其他原因导致" + ex.Message + "\"}";
            }
        }


        [Framework.Common.CustomAjaxMethod]
        public string ExportExcelEnergyAnalyse()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAnalyse>(inputValue);
            string icode = query.ItemCode;
            EnergyAnalyseQueryType tempQueryType = query.QueryType;

            query.QueryType = EnergyAnalyseQueryType.Default;//总能耗
            ResultEnergyAnalyse resultAll =
                Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart").GetEnergyAnalyseLineChart(query);
            ResultEnergyAnalyse resultArea = null;
            ResultEnergyAnalyse resultPerson = null;
            if (query.IsDevice == 0)
            {//
                query.QueryType = EnergyAnalyseQueryType.UnitArea;//单位面积
                resultArea = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                     .GetEnergyAnalyseLineChart(query);
                query.QueryType = EnergyAnalyseQueryType.UnitPerson;//人均
                resultPerson = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IChart>("Chart")
                                     .GetEnergyAnalyseLineChart(query);
            }

            var dept = string.Empty;
            List<Model.Itemcode> itList = null;
            string unit = "";
            string itemCodeName = "";

            if (query.IsDevice == 1)
            {
                var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(string.Format(" and deviceid={0}", query.ObjectId), " order by deviceid");
                itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + deviceList[0].ItemCodeID + "'", " order by ItemcodeID");
                if (itList.Count > 0)
                {
                    unit = itList[0].Unit;//单个分类分项单位
                    itemCodeName = itList[0].ItemCodeName;
                }
                if (query.ItemCode == "00000")
                {//总能耗

                    unit = "T";//标准煤单位
                    itemCodeName = "总能耗";
                }
            }
            else
            {
                itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                if (itList.Count > 0)
                {
                    dept = itList[0].Unit;
                }

                unit = icode == "00000" ? "T" : dept;
                itemCodeName = itList.Count == 0 ? "总能耗" : itList[0].ItemCodeName;
            }
            //query.QueryType = tempQueryType;
            //switch (query.QueryType)
            //{
            //    case EnergyAnalyseQueryType.Convert2Co2:
            //    case EnergyAnalyseQueryType.Convert2Coal:
            //        unit = "T";//标准煤单位
            //        break;
            //    case EnergyAnalyseQueryType.Convert2Money:
            //        unit = "元";//标准煤单位
            //        break;
            //}
            try
            {
                if (resultAll.OrderLst.Count > 0)
                {
                    DataTable dt = TableView.CreateOrderBaseDataTable();
                    for (var r = 0; r < resultAll.OrderLst.Count; r++)
                    {

                        DataRow dr = dt.NewRow();
                        dr[1] = resultAll.OrderLst[r].Tm;
                        dr[2] = query.ObjectId;
                        dr[3] = resultAll.OrderLst[r].Obj;
                        dr[4] = resultAll.OrderLst[r].Tm;
                        dr[5] = resultAll.OrderLst[r].Tm;
                        dr[6] = itemCodeName;
                        dr[7] = decimal.Round(decimal.Parse(resultAll.OrderLst[r].Val.ToString()), 2).ToString();
                        if (query.IsDevice == 0)
                        {
                            dr[8] = resultArea.OrderLst == null ? "0" : decimal.Round(decimal.Parse(resultArea.OrderLst[r].Val.ToString()), 2).ToString();
                            dr[9] = resultPerson.OrderLst == null ? "0" : decimal.Round(decimal.Parse(resultPerson.OrderLst[r].Val.ToString()), 2).ToString();
                        }


                        dt.Rows.Add(dr);

                    }
                    string temp_path = AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                    if (!Directory.Exists(temp_path))
                    {
                        Directory.CreateDirectory(temp_path);
                        string[] files = Directory.GetFiles(temp_path);
                        foreach (string fn in files)
                        {
                            File.Delete(temp_path + fn);
                        }
                    }
                    string save_path = DateTime.Now.Ticks + ".xls";

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\能耗分析表.xls";
                    if (query.IsDevice == 1)
                    {
                        templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\能耗分析表_设备.xls";
                    }
                    TemplateParam param = new TemplateParam(resultAll.OrderLst[0].Obj + " " + itemCodeName + " ", new CellParam(0, 0), query.StartTime.ToString("yyyy-MM-dd") + "~" + query.EndTime.ToString("yyyy-MM-dd"), new CellParam(3, 0), false, new CellParam(4, 0));

                    param.DataColumn = new[] { 0, 3, 1, 7, 8, 9 };
                    if (query.IsDevice == 1)
                    {
                        param.DataColumn = new[] { 0, 3, 1, 7 };
                    }
                    param.ItemUnit = "（单位：" + unit + "）";
                    param.ItemUnitCell = new CellParam(3, 5);

                    param.SortColumn = 0;
                    dt.TableName = "能耗分析表";

                    ExportHelper.ExportExcel(dt, temp_path + save_path, templatePath, param);

                    return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                }

            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败：由于当前无数据或其他原因导致" + ex.Message + "\"}";
            }
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportExcelCostQuery()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryCost>(inputValue);
            var result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.ICostQueryService>("CostQueryService").GetCostQuery(query);
            string objectName = "";
            switch(query.ObjType)
            {
                case AreaType.Area:
                    objectName = new BLL.BaseLayerObject().GetBaseLayerObjectList(
                    string.Format(" and layerobjectid={0}", query.ObjectId), " order by LayerObjectID")[0].LayerObjectName;
                    break;
                case AreaType.Liquid:
                    objectName = new BLL.BaseLayerObject().GetBaseFuncLayerObjectList(
                    string.Format(" and layerobjectid={0}", query.ObjectId), " order by LayerObjectID")[0].LayerObjectName;
                    break;
            }
          
            var dept = string.Empty;
            List<Model.Itemcode> itList = null;
            string unit = "";
            string itemCodeName = "";

            
          
                itList = new BLL.Itemcode().GetItemcodeList(" and ItemCodeNumber='" + query.ItemCode + "'", " order by ItemcodeID");
                if (itList.Count > 0)
                {
                    dept = itList[0].Unit;
                }

                unit = query.ItemCode == "00000" ? "T" : dept;
                itemCodeName = itList.Count == 0 ? "总能耗" : itList[0].ItemCodeName;
            
            
            try
            {
                if (result.FeeTbl != null)
                {

                    if (result.FeeTbl.FeeList.Count > 0)
                    {
                        DataTable dt = TableView.CreateCostQueryDataTable(result);
                        for (var r = 0; r < result.FeeTbl.FeeList.Count; r++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int j = 1; j <= dt.Columns.Count - 1; j++)
                            {
                                dr[j] = result.FeeTbl.FeeList[r][j - 1];
                            }

                            dt.Rows.Add(dr);

                        }
                        string temp_path = AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                        if (!Directory.Exists(temp_path))
                        {
                            Directory.CreateDirectory(temp_path);
                            string[] files = Directory.GetFiles(temp_path);
                            foreach (string fn in files)
                            {
                                File.Delete(temp_path + fn);
                            }
                        }
                        string save_path = DateTime.Now.Ticks + ".xls";

                        string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\费用查询.xls";
                        string time = "";
                        switch (query.Particle)
                        {
                            case Particle.Month:
                                time = query.StartTime.Year + "-" + query.StartTime.Month + "月";
                                break;
                            case Particle.Year:
                                time = query.StartTime.Year + "年";
                                break;

                        }
                        TemplateParam param = new TemplateParam(objectName + " 费用查询---" + itemCodeName + "能耗",
                                                                new CellParam(0, 0), time, new CellParam(3, 0), true,
                                                                new CellParam(4, 0));

                      
                        List<int> columnIndex = new List<int>();
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            columnIndex.Add(i);

                        }
                        param.DataColumn = columnIndex.ToArray();
                        param.ItemUnit = "（单位：" + unit + "）";
                        param.ItemUnitCell = new CellParam(3, 5);

                        param.SortColumn = 0;
                        dt.TableName = "费用查询";

                        ExportHelper.ExportExcel(dt, temp_path + save_path, templatePath, param);

                        return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                    }
                    else
                    {
                        return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                    }
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                }

            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败：由于当前无数据或其他原因导致" + ex.Message + "\"}";
            }
        }



    }
}
