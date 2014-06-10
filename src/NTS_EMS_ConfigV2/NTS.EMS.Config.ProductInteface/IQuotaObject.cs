using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IQuotaObject
    {
        List<Model.TS_Quota_Log> GetQuotaLogList(string whereStr);

        List<Model.TB_Quota> GetQuota(string whereStr);

        int InsertQuota(Model.TB_Quota quotaData, Model.TS_Quota_Log quotaLogData);

        int UpdateQuota(Model.TB_Quota quotaData, Model.TS_Quota_Log quotaLogData);

        int InsertQuotaLog(Model.TS_Quota_Log quotaLogData);

        List<Model.TS_DataCenter_Area_Month> GetTsDataCenterAreaMonth(string whereStr, string Year);

        int GetMaxQuotaId();
    }
}
