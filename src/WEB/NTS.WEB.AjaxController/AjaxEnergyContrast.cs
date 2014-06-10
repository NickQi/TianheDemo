using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using NTS.WEB.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.AjaxController
{
    public class AjaxEnergyContrast
    {
        private NTS.WEB.ProductInteface.IBaseLayerObject dal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        private readonly HttpContext _ntsPage = HttpContext.Current;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public ResultContrast IndexContrastChart()
        {
            ResultContrast refResutl = new ResultContrast();
            try
            {
                int inputValue = int.Parse(_ntsPage.Request["cType"]);
                switch (inputValue)
                {
                    case 1:
                        refResutl = IndexContrastObjsChart();
                        break;
                    case 2:
                        refResutl = IndexContrastPeriodsChart();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ResultContrast con = new ResultContrast();
                ExecuteProcess process = new ExecuteProcess();
                process.ExceptionMsg = ex.Message;
                process.Success = false;
                con.ActionInfo = process;
                return con;

            }
            return refResutl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        public ResultObjLst IndexContrastList()
        {
            try
            {
                ResultObjLst refResutl = new ResultObjLst();
                try
                {
                    int inputValue = int.Parse(_ntsPage.Request["cType"]);
                    switch (inputValue)
                    {
                        case 1:
                            refResutl = IndexContrastObjsLst();
                            break;
                        case 2:
                            refResutl = IndexContrastPeriodssLst();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    string strEx = ex.ToString();
                }
                return refResutl;
            }
            catch (Exception ex)
            {
                ResultObjLst lst = new ResultObjLst();
                ExecuteProcess process = new ExecuteProcess();
                process.ExceptionMsg = ex.Message;
                process.Success = false;
                lst.ActionInfo = process;
                return lst;
            }
        }


        [Framework.Common.CustomAjaxMethod]
        public ResultContrast IndexContrastObjsChart()
        {
            try
            {
                var inputValue = _ntsPage.Request["inputs"]; ;
                var oderObject = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryOrderObjects>(inputValue);


                QueryCompare query = new QueryCompare();
                query.StartTime = oderObject.StartTime;
                query.EndTime = oderObject.EndTime;
                query.ObjectNum = oderObject.AreaIdLst;
                query.ItemCode = oderObject.ItemCode;
                query.ObjType = oderObject.ObjType;
                query.Unit = oderObject.Particle;

                if (oderObject.AreaIdLst.Count <= 1)
                {
                    throw new Exception("请选择多个对象进行比较！");
                }

                EnergyAnalyseQueryType typeItemCode = oderObject.QueryType;
                var result = new ResultCompare();
                switch (oderObject.QueryType)
                {
                    case EnergyAnalyseQueryType.Default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetCompareChart(oderObject);
                        break;

                    case EnergyAnalyseQueryType.UnitArea:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetAreaCompareChart(oderObject);
                        break;

                    case EnergyAnalyseQueryType.UnitPerson:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                     .GetPersonNumCompareChart(oderObject); ;
                        break;
                    default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                     .GetCompareChart(oderObject);
                        break;
                }
                string strDepName = "";
                if (result.Dept.Count > 0)
                {
                    strDepName = result.Dept[0].ToString();
                }
                else
                {
                    throw new Exception("没有数据信息");
                }

                switch (oderObject.QueryType)
                {
                    case EnergyAnalyseQueryType.Convert2Co2:
                        strDepName = "T";
                        break;
                    case EnergyAnalyseQueryType.Convert2Coal:
                        strDepName = "T";
                        break;
                    case EnergyAnalyseQueryType.Convert2Money:
                        strDepName = "元";
                        break;
                }

                return JsonForHightChartsNew.IndexContrastObjsChart(query, "对比柱状图", result.ObjectName.ToArray(), strDepName,
                  result.Enery);
            }
            catch (Exception ex)
            {
                ResultContrast con = new ResultContrast();
                ExecuteProcess process = new ExecuteProcess();
                process.ExceptionMsg = ex.Message;
                process.Success = false;
                con.ActionInfo = process;
                return con;
            }
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultObjLst IndexContrastObjsLst()
        {
            try
            {
                var inputValue = _ntsPage.Request["inputs"]; ;
                var oderObject = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryOrderObjects>(inputValue);

                QueryCompare query = new QueryCompare();
                query.StartTime = oderObject.StartTime;
                query.EndTime = oderObject.EndTime;
                query.ObjectNum = oderObject.AreaIdLst;
                query.ItemCode = oderObject.ItemCode;
                query.ObjType = oderObject.ObjType;
                query.Unit = oderObject.Particle;

                if (oderObject.AreaIdLst.Count <= 1)
                {
                    throw new Exception("请选择多个对象进行比较！");
                }

                var result = new ResultCompare();
                switch (oderObject.QueryType)
                {
                    case EnergyAnalyseQueryType.Default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetCompareChart(oderObject);
                        break;

                    case EnergyAnalyseQueryType.UnitArea:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetAreaCompareChart(oderObject);
                        break;

                    case EnergyAnalyseQueryType.UnitPerson:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                     .GetPersonNumCompareChart(oderObject); ;
                        break;
                    default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                                  .GetCompareChart(oderObject);
                        break;
                }

                string strDepName = "";
                if (result.Dept.Count > 0)
                {
                    strDepName = result.Dept[0].ToString();
                }
                else
                {
                    throw new Exception("没有数据信息");
                }

                switch (oderObject.QueryType)
                {
                    case EnergyAnalyseQueryType.Convert2Co2:
                        strDepName = "T";
                        break;
                    case EnergyAnalyseQueryType.Convert2Coal:
                        strDepName = "T";
                        break;
                    case EnergyAnalyseQueryType.Convert2Money:
                        strDepName = "元";
                        break;
                }

                return JsonForHightChartsNew.IndexContrastObjsLst(query, result.ObjectName.ToArray(), strDepName,
                   result.Enery);
            }
            catch (Exception ex)
            {
                ResultObjLst lst = new ResultObjLst();
                ExecuteProcess process = new ExecuteProcess();
                process.ExceptionMsg = ex.Message;
                process.Success = false;
                lst.ActionInfo = process;
                return lst;
            }

        }


        [Framework.Common.CustomAjaxMethod]
        public ResultContrast IndexContrastPeriodsChart()
        {
            try
            {
                var inputValue = _ntsPage.Request["inputs"]; ;
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryContrastPeriods>(inputValue);

                var result = new ResultCompare();
                switch (query.QueryType)
                {
                    case QueryOrderType.Default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetPeriodsCompareChart(query);
                        break;
                    case QueryOrderType.UnitArea:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetAreaPeriodsCompareChart(query);
                        break;

                    case QueryOrderType.UnitPerson:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                     .GetPersonNumPeriodsCompareChart(query); ;
                        break;
                    default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetPeriodsCompareChart(query);
                        break;

                }

                string strAreaName = "";
                var listObject = dal.GetBaseLayerObjectList(" and layerobjectid = " + query.AreaId, "");
                if (listObject.Count > 0)
                {
                    strAreaName = listObject[0].LayerObjectName;
                }
                string strDepName = "";
                if (result.Dept.Count > 0)
                {
                    strDepName = result.Dept[0].ToString();
                }
                else
                {
                    throw new Exception("没有数据信息");
                }

                switch (query.QueryType)
                {
                    case QueryOrderType.CarbanOut:
                        strDepName = "T";
                        break;
                    case QueryOrderType.ConvCoal:
                        strDepName = "T";
                        break;
                    case QueryOrderType.Renminbi:
                        strDepName = "元";
                        break;
                }
                return JsonForHightChartsNew.IndexPeriodsContrastObjsChart(query, strAreaName, result.ObjectName.ToArray(), strDepName,
                  result.Enery);
            }
            catch (Exception ex)
            {
                ResultContrast con = new ResultContrast();
                ExecuteProcess process = new ExecuteProcess();
                process.ExceptionMsg = ex.Message;
                process.Success = false;
                con.ActionInfo = process;
                return con;
            }

        }

        [Framework.Common.CustomAjaxMethod]
        public ResultObjLst IndexContrastPeriodssLst()
        {
            try
            {
                var inputValue = _ntsPage.Request["inputs"]; ;
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryContrastPeriods>(inputValue);

                var result = new ResultCompare();
                string strDepName = "";
                switch (query.QueryType)
                {
                    case QueryOrderType.Default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetPeriodsCompareChart(query);
                        strDepName = result.Dept[0].ToString();
                        break;

                    case QueryOrderType.UnitArea:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetPeriodsCompareChart(query);
                        strDepName = result.Dept[0].ToString();
                        break;

                    case QueryOrderType.UnitPerson:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                     .GetPersonNumPeriodsCompareChart(query);
                        strDepName = result.Dept[0].ToString(); ;
                        break;
                    default:
                        result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                      .GetPeriodsCompareChart(query);
                        strDepName = result.Dept[0].ToString(); ;
                        break;
                }
                switch (query.QueryType)
                {
                    case QueryOrderType.CarbanOut:
                        strDepName = "T";
                        break;
                    case QueryOrderType.ConvCoal:
                        strDepName = "T";
                        break;
                    case QueryOrderType.Renminbi:
                        strDepName = "元";
                        break;
                }

                return JsonForHightChartsNew.IndexContrastObjsLst(query, result.ObjectName.ToArray(), strDepName, result.Enery);
            }
            catch (Exception ex)
            {
                ResultObjLst lst = new ResultObjLst();
                ExecuteProcess process = new ExecuteProcess();
                process.ExceptionMsg = ex.Message;
                process.Success = false;
                lst.ActionInfo = process;
                return lst;
            }
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportContrast()
        {
            string strReturn = "";
            try
            {
                int inputValue = int.Parse(_ntsPage.Request["cType"]);
                switch (inputValue)
                {
                    case 1:
                        strReturn = ExportQueryObjs();
                        break;
                    case 2:
                        strReturn = ExportQueryPeriod();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            return strReturn;
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportQueryObjs()
        {
            var inputValue = _ntsPage.Request["inputs"]; ;
            var oderObject = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryOrderObjects>(inputValue);

            //var inputValue = _ntsPage.Request.Form["Inputs"];
            //var tabId = int.Parse(_ntsPage.Request.Form["TabId"]);
            //var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryCompare>(inputValue);
            //var result = new ResultCompare();

            QueryCompare query = new QueryCompare();
            query.StartTime = oderObject.StartTime;
            query.EndTime = oderObject.EndTime;
            query.ObjectNum = oderObject.AreaIdLst;
            query.ItemCode = oderObject.ItemCode;
            query.ObjType = oderObject.ObjType;
            query.Unit = oderObject.Particle;

            var result = new ResultCompare();
            switch (oderObject.QueryType)
            {
                case EnergyAnalyseQueryType.Default:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                  .GetCompareChart(oderObject);
                    break;

                case EnergyAnalyseQueryType.UnitArea:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                  .GetAreaCompareChart(oderObject);
                    break;

                case EnergyAnalyseQueryType.UnitPerson:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                 .GetPersonNumCompareChart(oderObject); ;
                    break;
                default:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                .GetCompareChart(oderObject);
                    break;
            }

            string strDepName = "";
            if (result.Dept.Count > 0)
            {
                strDepName = result.Dept[0].ToString();
            }
            else
            {
                throw new Exception("没有数据信息");
            }

            switch (oderObject.QueryType)
            {
                case EnergyAnalyseQueryType.Convert2Co2:
                    strDepName = "T";
                    break;
                case EnergyAnalyseQueryType.Convert2Coal:
                    strDepName = "T";
                    break;
                case EnergyAnalyseQueryType.Convert2Money:
                    strDepName = "元";
                    break;
            }

            ResultContrast resultCon = JsonForHightChartsNew.IndexContrastObjsChart(query, "对比柱状图", result.ObjectName.ToArray(), strDepName,
              result.Enery);
            try
            {
                if (resultCon.ContrastLst.Count > 0)
                {
                    DataTable dt = TableView.CreateContrastDataTable();
                    for (var r = 0; r < resultCon.ContrastLst.Count; r++)
                    {
                        DataRow dr = dt.NewRow();
                        dr[1] = resultCon.ContrastLst[r].Tm.ToString();
                        dr[2] = resultCon.ContrastLst[r].Obj;
                        dr[3] = resultCon.ContrastLst[r].EneType;
                        dr[4] = resultCon.ContrastLst[r].Val;
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

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\能耗对比表.xls";

                    TemplateParam param = new TemplateParam("能耗对比表", new CellParam(0, 0), "", new CellParam(3, 0), false, new CellParam(4, 0));
                    //TemplateParam param = new TemplateParam("itemCodeName", new CellParam(1, 1),"",null, false, new CellParam(5, 0));
                    param.DataColumn = new[] { 0, 1, 2, 3, 4 };
                    param.ItemUnit = "（单位：" + result.Dept[0].ToString() + "）";
                    param.ItemUnitCell = new CellParam(3, 4);

                    param.SortColumn = 0;
                    dt.TableName = "能耗对比表";

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
        public string ExportQueryPeriod()
        {
            var inputValue = _ntsPage.Request["inputs"]; ;
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryContrastPeriods>(inputValue);

            var result = new ResultCompare();
            switch (query.QueryType)
            {
                case QueryOrderType.Default:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                  .GetPeriodsCompareChart(query);
                    break;

                case QueryOrderType.UnitArea:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                  .GetPeriodsCompareChart(query);
                    break;

                case QueryOrderType.UnitPerson:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                 .GetPersonNumPeriodsCompareChart(query); ;
                    break;
                default:
                    result = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                .GetPeriodsCompareChart(query);
                    break;
            }
            string strAreaName = "";
            var listObject = dal.GetBaseLayerObjectList(" and layerobjectid = " + query.AreaId, "");
            if (listObject.Count > 0)
            {
                strAreaName = listObject[0].LayerObjectName;
            }
            string strDepName = "";
            if (result.Dept.Count > 0)
            {
                strDepName = result.Dept[0].ToString();
            }
            else
            {
                throw new Exception("没有数据信息");
            }

            switch (query.QueryType)
            {
                case QueryOrderType.CarbanOut:
                    strDepName = "T";
                    break;
                case QueryOrderType.ConvCoal:
                    strDepName = "T";
                    break;
                case QueryOrderType.Renminbi:
                    strDepName = "元";
                    break;
            }

            ResultContrast resultCon = JsonForHightChartsNew.IndexPeriodsContrastObjsChart(query, strAreaName, result.ObjectName.ToArray(), strDepName,
              result.Enery);
            try
            {
                if (resultCon.ContrastLst.Count > 0)
                {
                    DataTable dt = TableView.CreateContrastDataTable();
                    for (var r = 0; r < resultCon.ContrastLst.Count; r++)
                    {
                        DataRow dr = dt.NewRow();
                        dr[1] = resultCon.ContrastLst[r].Tm.ToString();
                        dr[2] = resultCon.ContrastLst[r].Obj;
                        dr[3] = resultCon.ContrastLst[r].EneType;
                        dr[4] = resultCon.ContrastLst[r].Val;
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

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\能耗对比表.xls";

                    TemplateParam param = new TemplateParam("能耗对比表", new CellParam(0, 0), "", new CellParam(3, 0), false, new CellParam(4, 0));
                    //TemplateParam param = new TemplateParam("itemCodeName", new CellParam(1, 1),"",null, false, new CellParam(5, 0));
                    param.DataColumn = new[] { 0, 1, 2, 3, 4 };
                    param.ItemUnit = "（单位：" + result.Dept[0].ToString() + "）";
                    param.ItemUnitCell = new CellParam(3, 4);

                    param.SortColumn = 0;
                    dt.TableName = "能耗对比表";

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
    }
}
