using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;
using System.Data;

namespace NTS.WEB.BLL
{
    public class Alarm
    {

        readonly NTS.WEB.ProductInteface.IAlarmAccess _Alarm = NTS.WEB.ProductInteface.DataSwitchConfig.CreateAlarmAccess();

        public ResultAlarmType GetAlarmType(string strWhere)
        {
            ExecuteProcess process = new ExecuteProcess();
            process.ActionName = "";
            process.ActionTime = System.DateTime.Now;
            process.Success = true;
            process.ExceptionMsg = "";

            ResultAlarmType alarm = new ResultAlarmType();
            DataTable dttype = _Alarm.GetAlarmType(strWhere);
            List<AlarmType> types = new List<AlarmType>();
            foreach (DataRow row in dttype.Rows)
            {
                AlarmType type = new AlarmType();
                type.ItemCode = row["TYPE"].ToString();
                type.ItemName = row["NAME"].ToString();
                types.Add(type);
            }
            alarm.ActionInfo = process;
            alarm.ItemLst = types;

            return alarm;
        }

        /// <summary>
        /// 获取告警记录数据。
        /// </summary>
        /// <param name="alarmList"></param>
        /// <returns></returns>
        public ResultAlarmNewList GetAlarmList(QueryAlarmNew ParamAlarm, string groupId)
        {
            ResultAlarmNewList alarmList = new ResultAlarmNewList();

            ExecuteProcess process = new ExecuteProcess();
            process.ActionName = "";
            process.ActionTime = System.DateTime.Now;
            process.Success = true;
            process.ExceptionMsg = "";

            alarmList.ActionInfo = process;

            List<AlarmNewList> lstAlarm = new List<AlarmNewList>();

            DataTable dttype = new DataTable();
            int total = 0;
            if (ParamAlarm.AllAlarm == false)
            {
                dttype = _Alarm.GetAlarmList(ParamAlarm, groupId);
                total = _Alarm.GetAlarmListCount(ParamAlarm);
            }
            else
            {
                dttype = _Alarm.GetAlarmListIndex(ParamAlarm);
                total = _Alarm.GetAlarmIndexCount(ParamAlarm);
            }
            int pageCount = 0;
            if (total > 0)
            {
                pageCount = (total - 1) / ParamAlarm.PageSize + 1;
            }
            foreach (DataRow row in dttype.Rows)
            {
                AlarmNewList type = new AlarmNewList();
                type.Time = row["ALARMTIME"].ToString();
                type.Object = row["ALARMOBJNAME"].ToString();
                type.Position = ""; //row["LOCATION"].ToString();
                if (row["LOCATION"] != DBNull.Value)
                {
                    type.Position = row["LOCATION"].ToString();
                }
                type.Info = row["ALARMCONTENT"] == DBNull.Value ? "" : row["ALARMCONTENT"].ToString();
                type.AlarmItem = row["ALARMTYPENAME"] == DBNull.Value ? "" : row["ALARMTYPENAME"].ToString(); //告警类型名称
                type.Class = row["ALARMLEVEL"] == DBNull.Value ? "" : row["ALARMLEVEL"].ToString(); //告警等级
                type.AlarmStatus = "未知"; //告警类型
                if (row["STATUS"] != DBNull.Value)
                {
                    switch (row["STATUS"].ToString())
                    {
                        case "0":
                            type.AlarmStatus = "正在告警";
                            break;
                        case "1":
                            type.AlarmStatus = "已确认";
                            break;
                        case "2":
                            type.AlarmStatus = "已恢复";
                            break;
                        case "3":
                            type.AlarmStatus = "已处理";
                            break;
                        case "4":
                            type.AlarmStatus = "已取消";
                            break;
                        default:
                            type.AlarmStatus = "未知";
                            break;
                    }
                }
                lstAlarm.Add(type);
            }
            alarmList.data = lstAlarm;
            int[] intPageCount = new int[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                intPageCount[i] = i + 1;
            }
            alarmList.pages = intPageCount;
            alarmList.total = pageCount;
            alarmList.current = ParamAlarm.PageIndex;
            return alarmList;
        }


        public ResultAlarmIndex GetAlarmIndexCount()
        {
            ExecuteProcess process = new ExecuteProcess();
            process.ActionName = "";
            process.ActionTime = System.DateTime.Now;
            process.Success = true;
            process.ExceptionMsg = "";

            ResultAlarmIndex alarmIndex = new ResultAlarmIndex();

            AlarmDayYestoDayComp allToday = new AlarmDayYestoDayComp();
            string strWhere = " and ALARMTIME between '" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "'";
            int AllTodayCount = _Alarm.GetAlarmCount(strWhere);

            allToday.Value = AllTodayCount;
            strWhere = " and ALARMTIME between '" + System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") + "' and '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59") + "'";
            int YesTodayCount = _Alarm.GetAlarmCount(strWhere);
            allToday.YesterdayValue = YesTodayCount;
            if (YesTodayCount != 0)
            {
                allToday.CompareValue = ((AllTodayCount - YesTodayCount) * 100 / YesTodayCount).ToString();
            }
            else
            {
                allToday.CompareValue = "-";
            }
            alarmIndex.AllAlarm = allToday;

            AlarmDayYestoDayComp UndoAlarm = new AlarmDayYestoDayComp();
            strWhere = " and STATUS=0 and ALARMTIME between '" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "'";
            int UndoTodayCount = _Alarm.GetAlarmCount(strWhere);
            UndoAlarm.Value = UndoTodayCount;
            strWhere = " and STATUS=0 and ALARMTIME between '" + System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") + "' and '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59") + "'";
            int UndoYesDayCount = _Alarm.GetAlarmCount(strWhere);
            UndoAlarm.YesterdayValue = UndoYesDayCount;
            if (YesTodayCount != 0)
            {
                UndoAlarm.CompareValue = ((UndoTodayCount - UndoYesDayCount) * 100 / UndoYesDayCount).ToString();
            }
            else
            {
                UndoAlarm.CompareValue = "-";
            }
            alarmIndex.UndoAlarm = UndoAlarm;

            AlarmDayYestoDayComp ProcessedAlarm = new AlarmDayYestoDayComp();
            strWhere = " and STATUS<>0 and ALARMTIME between '" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "'";
            int ProcessedTodayCount = _Alarm.GetAlarmCount(strWhere);
            ProcessedAlarm.Value = ProcessedTodayCount;
            strWhere = " and STATUS<>0 and ALARMTIME between '" + System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") + "' and '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59") + "'";
            int ProcessedYesDayCount = _Alarm.GetAlarmCount(strWhere);
            ProcessedAlarm.YesterdayValue = ProcessedYesDayCount;
            if (ProcessedYesDayCount != 0)
            {
                ProcessedAlarm.CompareValue =
                    ((ProcessedTodayCount - ProcessedYesDayCount) * 100 / ProcessedYesDayCount).ToString();
            }
            else
            {
                ProcessedAlarm.CompareValue = "-";
            }
            alarmIndex.ProcessedAlarm = ProcessedAlarm;

            alarmIndex.ActionInfo = process;
            //alarmIndex.AllAlarm = alarmIndex;

            return alarmIndex;
        }


    }
}
