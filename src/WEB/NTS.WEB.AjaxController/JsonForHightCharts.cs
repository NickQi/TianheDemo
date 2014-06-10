using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.WEB.AjaxController
{
    public class JsonForHightCharts
    {
        /// <summary>
        /// 显示柱状图
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="chartObject"></param>
        /// <param name="chartDept"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string ShowColorColumnChart(string itemName, string[] chartObject, string[] chartDept, Dictionary<string, List<decimal>> dir)
        {
            int Index = 0;

            StringBuilder buildNew = new StringBuilder();

            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartObject.Length; i++)
                {
                    if (i == chartObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                chartJson.Append("\"ItemUnit\": [");
                for (int i = 0; i < chartDept.Length; i++)
                {
                    if (i == chartDept.Length - 1)
                    {
                        chartJson.Append("\"" + chartDept[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartDept[i] + "\",");
                    }
                }
                chartJson.Append("],");

                chartJson.Append("\"DataValue\": [");
                foreach (var d in dir)
                {
                    chartJson.Append("{");
                    chartJson.Append(string.Format("\"name\": '{0}',", d.Key));
                    chartJson.Append("data: [" + All(d.Value) + "]");
                    if (!Index.Equals(dir.Count - 1))
                    {

                        chartJson.Append("},");
                        //chartJson.Append(d + ",");
                    }
                    else
                    {
                        chartJson.Append("}");
                        //chartJson.Append(d);
                    }
                    Index++;
                }

                chartJson.Append("]}}");
                //chartJson.Append("{");
                //chartJson.Append("\"name\": \"" + itemName + "\",");
                //chartJson.Append("\"data\": [");
                //foreach (var d in dir)
                //{
                //    if (Index.Equals(dir.Count - 1))
                //    {
                //        chartJson.Append(d.Value);
                //    }
                //    else
                //    {
                //        chartJson.Append(d.Value + ",");
                //    }
                //    Index++;
                //}

                //chartJson.Append("]}]}}");

                return chartJson.ToString();
            }
        }

        public static string ShowColorPieChart(string itemName, string[] chartObject, string[] chartDept, List<decimal> dir)
        {
            int Index = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                if (chartDept != null)
                {
                    chartJson.Append("\"ItemUnit\": [");
                    for (int i = 0; i < chartDept.Length; i++)
                    {
                        if (i == chartDept.Length - 1)
                        {
                            chartJson.Append("\"" + chartDept[i] + "\"");
                        }
                        else
                        {
                            chartJson.Append("\"" + chartDept[i] + "\",");
                        }
                    }
                    chartJson.Append("],");
                }
                chartJson.Append("\"DataValue\": [");

                foreach (var d in dir)
                {
                    if (Index.Equals(dir.Count - 1))
                    {
                        chartJson.Append("{name:'" + chartObject[Index] + "',y:" + d + ",sliced: true,selected: true}");
                    }
                    else
                    {
                        chartJson.Append("{name:'" + chartObject[Index] + "',y:" + d + "},");
                    }
                    Index++;
                }

                chartJson.Append("]}}");

                return chartJson.ToString();
            }
        }

        public static string ShowMoreColorLineChart(string itemName, string[] chartObject, string[] chartDept, Dictionary<string, List<decimal>> dir)
        {
            int Index = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartObject.Length; i++)
                {
                    if (i == chartObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                if (chartDept != null)
                {
                    chartJson.Append("\"ItemUnit\": [");
                    for (int i = 0; i < chartDept.Length; i++)
                    {
                        if (i == chartDept.Length - 1)
                        {
                            chartJson.Append("\"" + chartDept[i] + "\"");
                        }
                        else
                        {
                            chartJson.Append("\"" + chartDept[i] + "\",");
                        }
                    }
                    chartJson.Append("],");
                }
                chartJson.Append("\"DataValue\": [");
               
                
                //chartJson.Append("{");
                //chartJson.Append("\"name\": \"" + itemName + "\",");
                //chartJson.Append("\"data\": [");
                foreach (var d in dir)
                {
                    chartJson.Append("{");
                    chartJson.Append(string.Format("name: '{0}',", d.Key));
                    chartJson.Append("data: [" + All(d.Value) + "]");
                    if (!Index.Equals(dir.Count - 1))
                    {
                        
                        chartJson.Append("},");
                        //chartJson.Append(d + ",");
                    }
                    else
                    {
                        chartJson.Append("}");
                        //chartJson.Append(d);
                    }
                    Index++;
                }

                chartJson.Append("]}}");

                return chartJson.ToString();
            }
        }


        public static ResultContrast ShowEnergyAnalyseLineChart(QueryAnalyse queryAnalyse, string[] chartObject, string[] chartDept, Dictionary<string, List<decimal>> dir)
        {
            //表格数据定义
            List<EnergyContrst> enerList = new List<EnergyContrst>();

            ResultContrast contrast = new ResultContrast();
            ExecuteProcess execu = new ExecuteProcess();
            if (chartObject.Length == 0)
            {
                execu.Success = false;
                execu.ExceptionMsg = "暂无数据信息";
                execu.ActionName = "";
                execu.ActionUser = "";
                execu.ActionTime = System.DateTime.Now;
            }
            else
            {
                execu.Success = true;
                execu.ExceptionMsg = "";
            }

            string sql = " and ItemCodeNumber = '" + queryAnalyse.ItemCode + "'";
            List<NTS.WEB.Model.Itemcode> itemList = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
                                 .GetItemCodeList(sql, "");

            contrast.ActionInfo = execu;

            #region 数据series
            LineJson lineJson = new LineJson();
            List<EneryCompare> compareList = new List<EneryCompare>();
            #endregion

            int Index = 0;
            StringBuilder chartNew = new StringBuilder();
            {
                foreach (var d in dir)
                {

                    EnergyContrst ener = new EnergyContrst();

                    EneryCompare compareItem = new EneryCompare();

                    if (itemList.Count == 0)
                    {
                        ener.EneType = "总能耗";
                    }
                    else
                    {
                        ener.EneType = itemList[0].ItemCodeName;
                    }


                    ener.Obj = d.Key;
                    ener.Tm = queryAnalyse.StartTime.ToString();
                    ener.Val = Math.Round(d.Value.Sum(), 2);

                    compareItem.name = d.Key;
                    compareItem.data = d.Value;
                    compareList.Add(compareItem);

                    enerList.Add(ener);
                    Index++;
                }
                if (chartDept.Length > 0)
                {
                    lineJson.Unit = chartDept[0];
                    contrast.Unit = chartDept[0];
                }
                lineJson.CompareType = "object";

                contrast.ContrastLst = enerList; // 表格数据。

                lineJson.series = compareList;
                contrast.lineJson = lineJson;

                //lineJson.series = 
                return contrast;
            }
        }

        //public static ResultEnergyAnalyse ShowEnergyAnalyseLineChart(QueryAnalyse queryAnalyse, ResultItemCode resultItemCode)
        //{
          
        //    ResultEnergyAnalyse result = new ResultEnergyAnalyse(){ };
        //    ExecuteProcess execu = new ExecuteProcess();
        //    if (resultItemCode.ObjectName.ToArray().Length == 0)
        //    {
        //        execu.Success = false;
        //        execu.ExceptionMsg = "暂无数据信息";
        //        execu.ActionName = "";
        //        execu.ActionUser = "";
        //        execu.ActionTime = System.DateTime.Now;
        //    }
        //    else
        //    {
        //        execu.Success = true;
        //        execu.ExceptionMsg = "";
        //    }

        //    string sql = " and ItemCodeNumber = '" + queryAnalyse.ItemCode + "'";
        //    List<NTS.WEB.Model.Itemcode> itemList = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IEnergyContrastService>("EnergyContrastService")
        //                         .GetItemCodeList(sql, "");

        //    result.ActionInfo = execu;

        //    #region 数据series

        //    result.OrderLst = new List<EnergyOrder>();
        //    result.series = new  List<EneryAnalyseSeries>();
          
        //    #endregion

           
        //        int Index = 1;
        //        foreach (var d in resultItemCode.Enery)
        //        {
        //            result.series.Add(new EneryAnalyseSeries()
        //                                  {
                                              
        //                                      name=d.Key,
        //                                      data=d.Value
        //                                  });

                  
                    
        //            result.OrderLst.Add(new EnergyOrder()
        //                                    {
        //                                         Order=Index,
        //                                          Obj=d.Key,
        //                                           Val=
                                                
                                                
        //                                    });
        //            Index++;

        //        }
        //        if (chartDept.Length > 0)
        //        {
        //            lineJson.Unit = chartDept[0];
        //            contrast.Unit = chartDept[0];
        //        }
        //        lineJson.CompareType = "object";

        //        contrast.ContrastLst = enerList; // 表格数据。

        //        lineJson.series = compareList;
        //        contrast.lineJson = lineJson;

        //        //lineJson.series = 
        //        return result;
            
        //}
        private static string All(IEnumerable<decimal> li)
        {
            var sb = new StringBuilder();
            foreach (var l in li)
            {
                sb.AppendFormat(",{0}", l.ToString(CultureInfo.InvariantCulture));
            }
            return sb.ToString().Length > 0 ? sb.ToString().Substring(1) : "";
        }

        public static string ShowColorLineChart(string itemName, string[] chartObject, string[] chartDept, List<decimal> dir)
        {
            int Index = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartObject.Length; i++)
                {
                    if (i == chartObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                if (chartDept != null)
                {
                    chartJson.Append("\"ItemUnit\": [");
                    for (int i = 0; i < chartDept.Length; i++)
                    {
                        if (i == chartDept.Length - 1)
                        {
                            chartJson.Append("\"" + chartDept[i] + "\"");
                        }
                        else
                        {
                            chartJson.Append("\"" + chartDept[i] + "\",");
                        }
                    }
                    chartJson.Append("],");
                }
                chartJson.Append("\"DataValue\": [");
                chartJson.Append("{");
                chartJson.Append("\"name\": \"" + itemName + "\",");
                chartJson.Append("\"data\": [");
                foreach (var d in dir)
                {
                    if (!Index.Equals(dir.Count - 1))
                    {
                        chartJson.Append(d + ",");
                    }
                    else
                    {
                        chartJson.Append(d);
                    }
                    Index++;
                }

                chartJson.Append("]}]}}");

                return chartJson.ToString();
            }
        }

        /// <summary>
        /// 拼接24小时数据
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ShowRealLineChart(ResultIndexLineChart result)
        {

            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"series\":[{");
                chartJson.Append("\"data\":[");

                for (int i = 0; i < result.DatePickEnery.Count; i++)
                {
                    if (i < result.DatePickEnery.Count - 1)
                    {
                        chartJson.Append(result.DatePickEnery[i] + ",");
                    }
                    else
                    {
                        chartJson.Append(result.DatePickEnery[i]);
                    }

                }
                chartJson.Append("]}], ");


                chartJson.Append("\"unit\": \"kwh\"");
                chartJson.Append("}");
                return chartJson.ToString();
            }
        }


        /// <summary>
        /// 显示Hightcharts的接力图
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="chartObject"></param>
        /// <param name="chartDept"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string ShowStackChart(string itemName, string[] chartObject, string[] chartDept, Dictionary<string, decimal[]> dir)
        {
            int Index = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartObject.Length; i++)
                {
                    if (i == chartObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                if (chartDept != null)
                {
                    chartJson.Append("\"ItemUnit\": [");
                    for (int i = 0; i < chartDept.Length; i++)
                    {
                        if (i == chartDept.Length - 1)
                        {
                            chartJson.Append("\"" + chartDept[i] + "\"");
                        }
                        else
                        {
                            chartJson.Append("\"" + chartDept[i] + "\",");
                        }
                    }
                    chartJson.Append("],");
                }
                // chartJson.Append("\"DataValue\": [");
                //  chartJson.Append("{");
                // chartJson.Append("\"name\": \"" + chartName + "\",");
                chartJson.Append("\"DataValue\": [");
                foreach (var d in dir)
                {
                    chartJson.Append("{");
                    chartJson.Append("name: '" + d.Key + "',");
                    chartJson.Append("data: [");
                    decimal[] dd = d.Value;
                    for (int k = 0; k < dd.Length; k++)
                    {
                        chartJson = (k == dd.Length - 1) ? chartJson.Append(dd[k]) : chartJson.Append(dd[k] + ",");
                    }

                    chartJson.Append("]");

                    chartJson.Append(!Index.Equals(dir.Count - 1) ? "}," : "}");
                    Index++;
                }

                chartJson.Append("]}}");

                return chartJson.ToString();
            }
        }


        /// <summary>
        /// 显示对比的柱状图
        /// </summary>
        /// <param name="itemName">内容项名称</param>
        /// <param name="chartXObject">x坐标轴对象数组</param>
        /// <param name="chartDept">对比对象的单位数组</param>
        /// <param name="dir">对象，数据值集合</param>
        /// <returns></returns>
        public static string ShowCompareColumnChart(string itemName, string[] chartXObject, string[] chartDept, Dictionary<string, decimal[]> dir)
        {
            int Index = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartXObject.Length; i++)
                {
                    if (i == chartXObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartXObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartXObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                if (chartDept != null)
                {
                    chartJson.Append("\"ItemUnit\": [");
                    for (int i = 0; i < chartDept.Length; i++)
                    {
                        if (i == chartDept.Length - 1)
                        {
                            chartJson.Append("\"" + chartDept[i] + "\"");
                        }
                        else
                        {
                            chartJson.Append("\"" + chartDept[i] + "\",");
                        }
                    }
                    chartJson.Append("],");
                }
                // chartJson.Append("\"DataValue\": [");
                //  chartJson.Append("{");
                // chartJson.Append("\"name\": \"" + chartName + "\",");
                chartJson.Append("\"DataValue\": [");
                foreach (var d in dir)
                {
                    chartJson.Append("{");
                    chartJson.Append("name: '" + d.Key + "',");
                    chartJson.Append("data: [");
                    decimal[] dd = d.Value;
                    for (int k = 0; k < dd.Length; k++)
                    {
                        chartJson = (k == dd.Length - 1) ? chartJson.Append(dd[k]) : chartJson.Append(dd[k] + ",");
                    }

                    chartJson.Append("]");

                    chartJson.Append(!Index.Equals(dir.Count - 1) ? "}," : "}");
                    Index++;
                }

                chartJson.Append("]}}");

                return chartJson.ToString();
            }
        }

        public static string ShowColumnPieChart(string itemName, string[] chartObject, string[] chartDept, Dictionary<string, decimal> dirColumn, string pieName, Dictionary<string, decimal> dirPie)
        {
            int Index1 = dirColumn.Count - dirPie.Count, Index2 = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartObject.Length; i++)
                {
                    if (i == chartObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                chartJson.Append("\"ItemUnit\": [");
                for (int i = 0; i < chartDept.Length; i++)
                {
                    if (i == chartDept.Length - 1)
                    {
                        chartJson.Append("\"" + chartDept[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartDept[i] + "\",");
                    }
                }
                chartJson.Append("],");

                chartJson.Append("\"DataValue\": [");
                chartJson.Append("{");
                chartJson.Append("\"name\": \"" + pieName + "\",");
                chartJson.Append("\"data\": [");
                foreach (var d in dirPie)
                {
                    if (Index1.Equals(dirColumn.Count - 1))
                    {
                        chartJson.Append("{\"name\":'" + chartObject[Index1] + "',y:" + d.Value + "," + d.Key + ",sliced: true,selected: true}");
                    }
                    else
                    {
                        chartJson.Append("{\"name\":'" + chartObject[Index1] + "',y:" + d.Value + "," + d.Key + "},");
                    }
                    Index1++;
                }
                chartJson.Append("]},{");
                chartJson.Append("\"name\": \"" + itemName + "\",");
                chartJson.Append("\"data\": [");
                foreach (var d in dirColumn)
                {
                    if (Index2.Equals(dirColumn.Count - 1))
                    {
                        chartJson.Append("{y:" + d.Value + "," + (d.Key.IndexOf("#", System.StringComparison.Ordinal) > -1 ? d.Key : new Model.EnumColor().Blue) + "}");
                    }
                    else
                    {
                        chartJson.Append("{y:" + d.Value + "," + (d.Key.IndexOf("#", System.StringComparison.Ordinal) > -1 ? d.Key : new Model.EnumColor().Blue) + "},");
                    }
                    Index2++;
                }

                chartJson.Append("]}]}}");

                return chartJson.ToString();
            }
        }

        /// <summary>
        /// 定额特殊图
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="chartXObject"></param>
        /// <param name="chartDept"></param>
        /// <param name="dir"></param>
        /// <param name="maxarr"> </param>
        /// <returns></returns>
        public static string ShowCombineChart(string itemName, string[] chartXObject, string[] chartDept, Dictionary<string, string[]> dir, string[] maxarr)
        {
            int Index = 0;
            StringBuilder chartJson = new StringBuilder();
            {
                chartJson.Append("{\"status\": \"success\",");
                chartJson.Append("\"msg\": \"图标加载完成\",");
                chartJson.Append("\"Charts\": {");
                chartJson.Append("\"xCategory\": [");
                for (int i = 0; i < chartXObject.Length; i++)
                {
                    if (i == chartXObject.Length - 1)
                    {
                        chartJson.Append("\"" + chartXObject[i] + "\"");
                    }
                    else
                    {
                        chartJson.Append("\"" + chartXObject[i] + "\",");
                    }
                }
                chartJson.Append("],");

                if (chartDept != null)
                {
                    chartJson.Append("\"ItemUnit\": [");
                    for (int i = 0; i < chartDept.Length; i++)
                    {
                        if (i == chartDept.Length - 1)
                        {
                            chartJson.Append("\"" + chartDept[i] + "\"");
                        }
                        else
                        {
                            chartJson.Append("\"" + chartDept[i] + "\",");
                        }
                    }
                    chartJson.Append("],");
                }
                chartJson.Append("\"DataValue\": [");
                // 定额线
                if (maxarr.Length > 0)
                {
                    chartJson.Append("{");
                    chartJson.Append("\"name\": \"" + maxarr[0] + "\",");
                    //[0, 400], [3, 400] ,[11, 400]
                    chartJson.Append("\"data\": [" + maxarr[1] + "]");
                    chartJson.Append("},");
                }
                foreach (var d in dir)
                {
                    chartJson.Append("{");
                    chartJson.Append("\"name\": \"" + d.Key + "\",");
                    chartJson.Append("\"data\": [");
                    string[] dd = d.Value;
                    for (int k = 0; k < dd.Length; k++)
                    {
                        chartJson = (k == dd.Length - 1) ? chartJson.Append(dd[k]) : chartJson.Append(dd[k] + ",");
                    }

                    chartJson.Append("]");

                    chartJson.Append(!Index.Equals(dir.Count - 1) ? "}," : "}");
                    Index++;
                }

                chartJson.Append("]}}");

                return chartJson.ToString();
            }
        }
    }
}
