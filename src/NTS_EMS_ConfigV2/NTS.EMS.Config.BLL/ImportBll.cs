using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.Model.ResultViewFile;

namespace NTS.EMS.Config.BLL
{
    public class ImportBll
    {
        private readonly NTS.EMS.Config.ProductInteface.IImport _import = ProductInteface.DataSwitchConfig.CreateImport();
        public int SaveImportEneryValue(ImportTemp import)
        {
            return _import.SaveImportEneryValue(import);
        }

        /// <summary>
        /// 显示导入的历史记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public HistoryImport GetResultImportList(HistoryQuery query)
        {
            var action = 0;
            if (query.ObjectId != null)
            {
                action = 1;
            }
            var dataResult = _import.GetResultImportList(query,action);
            try
            {
                dataResult.PageInfo.CuttrentPage = query.PaddingInfo.Page;
                dataResult.PageInfo.Pages = dataResult.PageInfo.Total % query.PaddingInfo.PageSize == 0
                    ? dataResult.PageInfo.Total / query.PaddingInfo.PageSize
                    : dataResult.PageInfo.Total / query.PaddingInfo.PageSize + 1;
                dataResult.Success = true;
                dataResult.ErrorMsg = string.Empty;
            }
            catch (Exception e)
            {
                dataResult.Success = false;
                dataResult.ErrorMsg = e.Message;
            }
            return dataResult;
        }

        /// <summary>
        /// 将Excel信息录入数据库
        /// </summary>
        /// <param name="monthType"></param>
        /// <param name="isArea"></param>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        public ResultExcelImport SaveImportExcel(int monthType, int isArea, string excelPath)
        {
            return _import.SaveImportExcel(monthType, isArea, excelPath);
        }
    }
}
