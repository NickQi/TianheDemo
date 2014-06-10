using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.Model.ResultViewFile;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IImport
    {
        int SaveImportEneryValue(ImportTemp import);
        HistoryImport GetResultImportList(HistoryQuery query,int action);
        ResultExcelImport SaveImportExcel(int monthType, int isArea, string excelPath);
    }
}
