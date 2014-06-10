using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
namespace NTS.WEB.AjaxController
{

    public class AjaxAlarm
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public ResultAlarm GetAlarmDiagnose()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarm>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAlarmDiagnoseService>("AlarmDiagnoseService").GetAlarmDiagnose(query);
            return res;
        }


        [Framework.Common.CustomAjaxMethod]
        public ResultAlarmType GetAlarmType()
        {
            //var inputValue = _ntsPage.Request.Form["Inputs"];
            //var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarm>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAlarmService>("AlarmService").GetAlarmType("");
            return res;
        }


        [Framework.Common.CustomAjaxMethod]
        public ResultAlarmNewList GetAlarmList()
        {
            string username = Utils.GetCookie("userid");
            var loginResult =
                Framework.Common.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").GetUserGroupID(
                    username);

            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmNew>(inputValue);
            var res =
                Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAlarmService>("AlarmService").GetAlarmList(
                    query, loginResult.ToString());
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ResultAlarmIndex GetAlarmIndexCount()
        {
            var res =
                Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAlarmService>("AlarmService").GetAlarmIndexCount();
            return res;
        }


        [Framework.Common.CustomAjaxMethod]
        public string ExportAlarm()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryAlarmNew>(inputValue);
            var res =
                Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAlarmService>("AlarmService").GetAlarmList(query, "");
            List<NTS.WEB.ResultView.AlarmNewList> dtRef = res.data;
            try
            {
                if ((dtRef != null) && (dtRef.Count > 0))
                {
                   // DataTable dtReport = TableView.CreateAlarmDataTable();
                    DataTable dtReport = new DataTable();
                    for (var r = 0; r < dtRef.Count; r++)
                    {
                        DataRow dr = dtReport.NewRow();
                        dr[1] = dtRef[r].Time.ToString();
                        dr[2] = dtRef[r].Object.ToString();
                        dr[3] = dtRef[r].Position;
                        dr[4] = dtRef[r].Info;
                        dr[5] = dtRef[r].AlarmItem;
                        dr[6] = dtRef[r].Class;
                        dr[7] = dtRef[r].AlarmStatus;
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

                    string templatePath = AppDomain.CurrentDomain.BaseDirectory + "template\\告警管理表.xls";

                    TemplateParam param = new TemplateParam("告警管理表", new CellParam(0, 0), "", new CellParam(3, 0), false, new CellParam(4, 0));
                    //TemplateParam param = new TemplateParam("itemCodeName", new CellParam(1, 1),"",null, false, new CellParam(5, 0));
                    param.DataColumn = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
                    //param.ItemUnit = "（单位：元";
                    //param.ItemUnitCell = new CellParam(3, 5);

                    dtReport.TableName = "告警管理表";

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
