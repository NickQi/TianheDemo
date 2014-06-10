using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.EMS.Config.Model;
using System.Data;
using System.IO;
using NTS.WEB.Common;

namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxSysLog
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;
        /// <summary>
        /// 日志查询
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultSysLog GetSysLog()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QuerySysLogContact>(inputValue);
            var result = new NTS.EMS.Config.BLL.QuerySysLogBll().GetSysLog(query);
            return result;
        }
        /// <summary>
        /// 日志导出
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public string ExportSysLogExcel()
        {
            try
            {
                #region 获取数据

                var inputValue = _ntsPage.Request.Form["Inputs"];
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QuerySysLogContact>(inputValue);
                query.PageCurrent = 1;
                query.PageSize = 100000000;
                var result = new NTS.EMS.Config.BLL.QuerySysLogBll().GetSysLog(query);

                #endregion

                #region 组织数据

                #endregion

                if (result.SysLogList.Count > 0)
                {
                    DataTable dt = TableView.CreateSysLogDataTable();
                    for (var i = 0; i < result.SysLogList.Count; i++)
                    {


                        DataRow dr = dt.NewRow();
                        dr[1] = result.SysLogList[i].SysNo;
                        dr[2] = result.SysLogList[i].ModelName;
                        dr[3] = result.SysLogList[i].LogContent;
                        dr[4] = result.SysLogList[i].LogTime;
                        dr[5] = result.SysLogList[i].OpType == OpType.Operate ? "操作" : "配置";
                        dr[6] = result.SysLogList[i].UserName;
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
                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\日志查询表.xls";

                    string suTitle = "";
                    string startTime = query.StartTime.ToString("yyyy-MM-dd");
                    string endTime = query.EndTime.ToString("yyyy-MM-dd");
                    if (query.StartTime.ToShortDateString() == "1900-1-1")
                    {
                        startTime = result.SysLogList.Min(p => p.LogTime).ToString("yyyy-MM-dd");
                    }
                    if (query.EndTime.ToShortDateString() == "1900-1-1")
                    {
                        endTime = result.SysLogList.Max(p => p.LogTime).ToString("yyyy-MM-dd");
                    }
                    suTitle = startTime + "~" + endTime;
                    TemplateParam param = new TemplateParam("日志查询", new CellParam(0, 0), suTitle, new CellParam(3, 0), false, new CellParam(4, 0));
                    param.DataColumn = new[] { 0, 6, 4, 5, 2, 3 };

                    param.SortColumn = 0;
                    dt.TableName = "日志查询";

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
