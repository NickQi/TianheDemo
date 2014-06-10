using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model.QueryFile;

namespace NTS.EMS.Config.ProductInteface
{
    /// <summary>
    /// 分摊配置接口
    /// </summary>
    public interface IAlloction
    {
        bool SaveAlloctionAndLog(string sql);

        Model.TB_BECM_COUNTTYPE GetCountType(string energyId);

        List<Model.TB_ALLOCTION_CONFIG> GetAlloctionList(string whereStr);

        List<Model.TB_ALLOCTION_CONFIG_History> GetConfigLogList(string whereStr, string orderBy);

        List<Model.TB_AREA_Info> GetAreaInfoList(string whereStr);

        List<Model.TS_FEE_DAY> GetFeeDayList(int year, string whereStr);
    }
}
