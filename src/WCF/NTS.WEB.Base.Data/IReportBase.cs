using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IReportBase
    {
        #region
        //DataTable GetItemcodeData(ReportQueryModel model); // 根据分项的获取某个对象的能耗值
        //DataTable GetSmallItemcodeData(ReportQueryModel model, out int recordCount);// 获取某个分项的子分项的能耗值
        //DataTable GetPerDataJson(DataTable Report); // 获取分项的获取某个对象的单位能耗值
        //DataTable GetManayObjectAndItemcodeData(string[] ItemcodeArr, string[] ObjectArr, ReportQueryModel model); // 获取对象集合和能耗集合的总数据
        //DataTable GetItemcodeDataOnlyTotal(ReportQueryModel model);// 根据分项的获取某个对象的总能耗值
        #endregion

        BaseResult GetBaseEneryDataList(BaseQueryModel model);

        BaseResult GetBaseEneryDataList(BaseQueryModel model, bool IsLiquid);
    }
}
