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
    public class AjaxFee_Apportion
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public ResultFeeapportion GetFeeApportion()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<Queryfeeapportion>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IFee_ApportionService>("Fee_ApportionService").GetFeeApportionData(query);
            return res;
        }


        [Framework.Common.CustomAjaxMethod]
        public string ExportFeeApportion()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<Queryfeeapportion>(inputValue);
            List<FeeApportionListClass> dtRef = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IFee_ApportionService>("Fee_ApportionService").GetFeeApportDataList(query);
            try
            {
                // 
                if ((dtRef != null) && (dtRef.Count > 0))
                {
                    DataTable dtReport = TableView.CreateFee_ApportionDataTable();
                    for (var r = 0; r < dtRef.Count; r++)
                    {
                        DataRow dr = dtReport.NewRow();
                        dr[1] = dtRef[r].Tm.ToString();
                        dr[2] = dtRef[r].Obj.ToString();
                        dr[3] = Math.Round(dtRef[r].BeforeVal, 2);
                        dr[4] = Math.Round(dtRef[r].ApportionVal, 2);
                        dr[5] = Math.Round(dtRef[r].TotalVal, 2);
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

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\费用分摊表.xls";

                    TemplateParam param = new TemplateParam("费用分摊表", new CellParam(0, 0), "", new CellParam(3, 0), false, new CellParam(4, 0));
                    //TemplateParam param = new TemplateParam("itemCodeName", new CellParam(1, 1),"",null, false, new CellParam(5, 0));
                    param.DataColumn = new[] { 0, 1, 2, 3, 4, 5 };
                    param.ItemUnit = "（单位：元 ）";
                    param.ItemUnitCell = new CellParam(3, 5);

                    dtReport.TableName = "费用分摊表";

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
