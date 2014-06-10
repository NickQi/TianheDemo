using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.Model.ResultViewFile;

namespace NTS.EMS.Config.DAL
{
    public class Import:ProductInteface.IImport
    {
        public int SaveImportEneryValue(ImportTemp import)
        {
            var cmd = new DataCommand("ImportData", new SqlCustomDbCommand());
            cmd.SetParameterValue("@ObjectID", import.ObjectId);
            cmd.SetParameterValue("@IsArea", import.IsArea);
            cmd.SetParameterValue("@StartTime", import.StartTime);
            cmd.SetParameterValue("@EndTime", import.EndTime);
            cmd.SetParameterValue("@ItemCode", import.ItemCode);
            cmd.SetParameterValue("@ImportValue", import.ImportValue);
            cmd.SetParameterValue("@MonthType", import.MonthType);
            cmd.SetParameterValue("@ExcelId", import.ExcelId);
            return cmd.ExecuteNonQuery();
        }

        public HistoryImport GetResultImportList(HistoryQuery import, int action)
        {
            var result = new HistoryImport { HistoryItem = new List<ResultImport>(), PageInfo = new PageInfo() };
            var cmd = new DataCommand("SearchImportData", new SqlCustomDbCommand());
            var whereStr = new StringBuilder();
            if (import.DeviceId == null || import.DeviceId==0)
            {
                if (!string.IsNullOrEmpty(import.ObjectId.ToString()))
                {
                    whereStr.Append(string.Format(" and import.ObjectID={0}", import.ObjectId));
                }
            }
            else
            {
                whereStr.Append(string.Format(" and import.ObjectID={0}", import.DeviceId));
            }
            if (!string.IsNullOrEmpty(import.Starttime.ToString()))
            {
                whereStr.Append(string.Format(" and import.Starttime>='{0}'", import.Starttime));
            }
            if (!string.IsNullOrEmpty(import.Endtime.ToString()))
            {
                whereStr.Append(string.Format(" and import.Starttime<='{0}'", import.Endtime));
            }
            if (!string.IsNullOrEmpty(import.ItemCode))
            {
                whereStr.Append(string.Format(" and import.ItemCode='{0}'", import.ItemCode));
            }
            if (!string.IsNullOrEmpty(import.DateUnit.ToString(CultureInfo.InvariantCulture)))
            {
                whereStr.Append(string.Format(" and import.MonthType={0}", import.DateUnit));
            }
            cmd.SetParameterValue("@action", action);
            cmd.ReplaceParameterValue("#whereStr#", whereStr.ToString());
            cmd.ReplaceParameterValue("#pagesize#",import.PaddingInfo.PageSize.ToString(CultureInfo.InvariantCulture));
            cmd.ReplaceParameterValue("#pagenums#", ((import.PaddingInfo.Page - 1) * import.PaddingInfo.PageSize).ToString(CultureInfo.InvariantCulture));
            result.HistoryItem = cmd.ExecuteEntityList<ResultImport>();
            foreach (var item in result.HistoryItem)
            {
                item.ObjectName = GetObjectName(item.IsArea, item.ObjectId);
            }
            result.PageInfo.Total = (int)cmd.GetParameterValue("@Count");
            return result;
        }

        private string GetObjectName(int isarea, int id)
        {
            var cmd = new DataCommand("getImportObject", new SqlCustomDbCommand());
            cmd.SetParameterValue("@isarea", isarea);
            cmd.SetParameterValue("@id", id);
            var dt = cmd.ExecuteDataSet().Tables[0];
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : string.Empty;
        }

        public ResultExcelImport SaveImportExcel(int monthType, int isArea, string excelPath)
        {
            try
            {
                var cmd = new DataCommand("SaveImportExcel", new SqlCustomDbCommand());
                cmd.SetParameterValue("@monthType", monthType);
                cmd.SetParameterValue("@isArea", isArea);
                cmd.SetParameterValue("@excelPath", excelPath);
                cmd.ExecuteNonQuery();
                return new ResultExcelImport() {Success = true,MsgContent = "文件批量上传成功，后台系统在努力处理，根据您上传的数据量，可能会花费几分钟或几个小时。"};
            }
            catch (Exception e)
            {

                return new ResultExcelImport() {Success = false, MsgContent = e.Message};
            }
           
        }
    }
}
