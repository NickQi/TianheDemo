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

    public class AjaxLoadForecast
    {
        private readonly HttpContext _ntsPage = HttpContext.Current; 

        [Framework.Common.CustomAjaxMethod]
        public ResultView.ResultLoadForecastMap GetLoadForecastChart()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryLoadForecast>(inputValue);
            ResultLoadForecastMap res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.ILoadForecastService>("LoadForecastService").GetLoadForecastChart(query);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportLoadForecast()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryLoadForecast>(inputValue);
            ResultLoadForecastMap dtRef = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.ILoadForecastService>("LoadForecastService").GetLoadForecastChart(query);
            try
            {
                if ((dtRef != null) && (dtRef.LoadForecast.Count > 0))
                {
                    DataTable dtReport = TableView.CreateFee_ForecastDataTable();
                    List<ResultLoadForecastList> listNew = dtRef.LoadForecast;
                    for (var r = 0; r < listNew.Count; r++)
                    {
                        DataRow dr = dtReport.NewRow();
                        dr[1] = listNew[r].TimeArea.ToString();
                        dr[2] = listNew[r].ForeCast.ToString();
                        if (listNew[r].History==-9999)
                        {
                            dr[3] = "--";
                            dr[4] = "--";
                            dr[5] = "--";
                        }
                        else
                        {
                            dr[3] = listNew[r].History.ToString();
                            dr[4] = listNew[r].Deviation.ToString();
                            dr[5] = listNew[r].Pecent.ToString(); 
                        }

                        dtReport.Rows.Add(dr);
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

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\负荷预测表.xls";

                    TemplateParam param = new TemplateParam("负荷预测表", new CellParam(0, 0), "", new CellParam(3, 0), false, new CellParam(4, 0));
                    //TemplateParam param = new TemplateParam("itemCodeName", new CellParam(1, 1),"",null, false, new CellParam(5, 0));
                    param.DataColumn = new[] { 0, 1, 2, 3, 4, 5 };
                    //param.ItemUnit = "（单位：元";
                    //param.ItemUnitCell = new CellParam(3, 5);

                    dtReport.TableName = "负荷预测表";

                    ExportHelper.ExportExcel(dtReport, temp_path + save_path, templatePath, param);

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
