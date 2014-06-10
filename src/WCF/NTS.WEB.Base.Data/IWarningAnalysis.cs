using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IWarningAnalysis
    {
        int GetWarningPageCount(int pageSize, string startTime, string endTime, string warningTypeId, int areaId);

        DataTable GetWarningListByPage(int pageIndex, int pageSize, string startTime, string endTime,
                                       string warningTypeId, int areaId);
        int GetWarningPageCount(WarningAnalysisModel model);

        DataTable GetWarningListByPage(WarningAnalysisModel model);


        DataSet GetAreaList();

        DataSet GetAreaTreeList();
        string GetWaringContentPref(string devicename);

        List<WarningTypeModel> GetWarningTypeList();
    }
}
