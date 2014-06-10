using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.EMS.Config.BLL;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.Model.ResultViewFile;
using NTS.WEB.Model;

namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxImport
    {
        #region 公共对象
        private readonly WEB.ProductInteface.IReportBase _dalReportBase =
            WEB.ProductInteface.DataSwitchConfig.CreateReportBase();
        private readonly HttpContext _ntsPage = HttpContext.Current;
        private readonly ImportBll _bll = new ImportBll();
        #endregion

        #region main
        /// <summary>
        /// 根据区域id获取设备的列表
        /// </summary>
        /// <returns></returns>
        [Framework.Ajax.CustomAjaxMethod]
        [Framework.LogAndException.CustomException]
        public List<Device> GetDeviceListByAreaId()
        {
            LayerObjectBll bllObjectBll;
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryDevice>(inputValue);

            // 递归树
            if (query.treeInfo == 1)
            {
                bllObjectBll = new LayerFunObject(query.areaid);
            }
            else
            {
                bllObjectBll = new LayerBaseObject(query.areaid);
            }
             
            var itemcodelist = new StringBuilder();
            var list =
                new NTS.WEB.BLL.Itemcode().GetItemcodeList(string.Format(" and (parentid=(select ItemcodeID from Becm_ItemCode where ItemCodeNumber='{0}')) or ItemCodeNumber='{0}' ", query.itemCode), string.Empty);
            foreach (var l in list)
            {
                itemcodelist.Append(string.Format(",'{0}'", l.ItemCodeNumber));
            }
            var whereStr = query.treeInfo == 1
                ? string.Format(" and areaid2 in({0}) and itemCodeID in({1})", bllObjectBll.GetTreeObjects(), itemcodelist.ToString().Substring(1))
                : string.Format(" and areaid in({0}) and itemCodeID in({1})", bllObjectBll.GetTreeObjects(), itemcodelist.ToString().Substring(1));
            var devicelist = new WEB.BLL.BaseLayerObject().GetDeviceObjectList(whereStr,
                string.Empty);
            return devicelist;
        }


        /// <summary>
        /// 获取对象的能耗值
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultEnery GetObjectEnery()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryEnery>(inputValue);
            var objectId = 0;
            objectId = query.IsArea == 1 ? query.ObjectId : query.DeviceId;
            var model = new BaseQueryModel
            {
                IsDevice = 1 - query.IsArea,
                ObjectList = new List<int> {objectId},
                ItemCode = query.ItemCode,
                Unit = ConvertUnit(query.DateUnit),
                Starttime = query.Starttime,
                Endtime = query.Starttime
            };
            var list = query.IsAreaTree == 1 ? _dalReportBase.GetBaseEneryDataList(model, true) : _dalReportBase.GetBaseEneryDataList(model);
            return list.BaseLayerObjectResults.Count > 0
                ? new ResultEnery
                {
                    Total = list.BaseLayerObjectResults[objectId.ToString(CultureInfo.InvariantCulture)].Total
                }
                : new ResultEnery {Total = 0};
        }


        /// <summary>
        /// 保存手工录入的能耗值
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [AjaxAopBussinessLog(ModelName = "人工导入", LogType = OperatorType.Operator)]
        [Framework.Ajax.CustomAjaxMethod]
        public SaveResult SaveObjectEnery()
        {
            decimal temp;
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryEnery>(inputValue);
            query.ObjectId = query.IsArea == 1 ? query.ObjectId : query.DeviceId;
            if (!decimal.TryParse(query.ObjectValue.ToString(), out temp))
            {
                return new SaveResult {IsOK = false, MessageContent = "录入的能耗值的格式不正确。"};
            }
            var import = new ImportTemp()
            {
                ImportValue = temp,
                ExcelId = -1,
                StartTime = query.Starttime,
                EndTime = query.Starttime,
                IsArea = query.IsArea,
                ItemCode = query.ItemCode,
                ObjectId = query.ObjectId,
                MonthType = query.DateUnit
            };
            return _bll.SaveImportEneryValue(import) > 0
                ? new SaveResult {IsOK = true, MessageContent = "人工导入成功，等待后台服务进行处理。"}
                : new SaveResult {IsOK = false, MessageContent = "人工导入失败，请联系管理员。"};
        }

        /// <summary>
        /// 查询历史的人工导入的值列表
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public HistoryImport ShowImportDataList()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<HistoryQuery>(inputValue);
            return _bll.GetResultImportList(query);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultExcelImport ExcelImport()
        {
            return Uploadfile(_ntsPage);
        }

        #region 上传处理===================================
        private ResultExcelImport Uploadfile(HttpContext context)
        {
            var excelDateUnit = context.Request["ExcelDateUnit"];
            var excelImportIsArea = context.Request["ExcelImportIsArea"];
            var upfile = context.Request.Files["UpFilePath"];
            if (upfile == null)
            {
                return new ResultExcelImport { Success = false, MsgContent = "请选择要上传文件！" };
            }
            var upload=FileSaveAs(upfile);
            if (upload.Success)
            {
                return _bll.SaveImportExcel(int.Parse(excelDateUnit), int.Parse(excelImportIsArea), upload.MsgContent);
            }
            return upload;
        }
        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private static bool CheckFileExt(string fileExt)
        {
            string[] allowExt = {"xls"};
            return allowExt.Any(t => String.Equals(t, fileExt, StringComparison.CurrentCultureIgnoreCase));
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isWater">是否返回文件原名称</param>
        /// <returns>服务器文件路径</returns>
        public ResultExcelImport FileSaveAs(HttpPostedFile postedFile)
        {
            try
            {
                string fileExt = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(".", System.StringComparison.Ordinal) + 1); //文件扩展名，不含“.”
                string originalFileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1); //取得文件原名
                string fileName = Utils.GetRamCode() + "." + fileExt; //随机文件名
                string dirPath = ConfigurationManager.AppSettings["UploadExcel"]; //上传目录相对路径

                //检查文件扩展名是否合法
                if (!CheckFileExt(fileExt))
                {
                    return new ResultExcelImport {Success = false, MsgContent = "不允许上传" + fileExt + "类型的文件！"};
                }

                //获得要保存的文件路径
                string serverFileName = dirPath + fileName;
                string returnFileName = serverFileName;
                //物理完整路径                    
               // string toFileFullPath = Utils.GetMapPath(dirPath);
                //检查有该路径是否就创建
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                //保存文件
                postedFile.SaveAs(dirPath + fileName);
                return new ResultExcelImport { Success = true, MsgContent = returnFileName };
               
            }
            catch
            {
                return new ResultExcelImport { Success = false, MsgContent = "上传过程中发生意外错误！" };
            }
        }
        #endregion

        #endregion

        #region  时间颗粒对象转化
        private static ChartUnit ConvertUnit(int unit)
        {
            switch (unit)
            {
                case 1:
                    return ChartUnit.unit_month;
                case 2:
                    return ChartUnit.unit_day;
                default:
                    return ChartUnit.unit_hour;
            }
        }
        #endregion
    }
}
