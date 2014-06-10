using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IQuotaAlarmObject
    {
        /// <summary>
        /// 获取定额告警列表
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        List<Model.TB_Ems_Quota_Percent> GetQuotaAlarmList(string whereStr);

        /// <summary>
        /// 获取单条定额告警信息
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        Model.TB_Ems_Quota_Percent GetQuotaAlarm(string whereStr);

        /// <summary>
        /// 保存定额告警信息
        /// </summary>
        /// <param name="quotaAlarmData"></param>
        /// <returns></returns>
        int SaveQuotaAlarm(Model.QuotaAlarmDataContact quotaAlarmData);

        /// <summary>
        /// 删除定额告警
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteQuotaAlarm(int id);
    }
}
