using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;

namespace NTS.WEB.ProductInteface
{
    public interface IAlarmAccess
    {
        DataTable GetAlarmType(string whereStr);

        /// <summary>
        /// 获取告警类型
        /// </summary>
        DataTable GetAlarmList(QueryAlarmNew alarmNew, string groupId);

        int GetAlarmListCount(QueryAlarmNew alarmNew);

        IList<string> GetAlarmListAreaId(int ObjectId, AreaType ObjType);

        /// <summary>
        /// 获取告警数量
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        int GetAlarmCount(string whereStr);


        DataTable GetAlarmListIndex(QueryAlarmNew alarmNew);

        int GetAlarmIndexCount(QueryAlarmNew alarmNew);
    }
}
