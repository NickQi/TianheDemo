using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IAlarmSetting
    {
        /// <summary>
        /// 获取告警类型表所有数据
        /// </summary>
        /// <returns>List<Model.TB_AlarmType></returns>
        List<Model.TB_AlarmType> GetAlarmTypeList(string whereStr);

        /// <summary>
        /// 获取告警触发事件表数据
        /// </summary>
        /// <param name="alarmTypeId">告警类型表ID</param>
        /// <returns>List<Model.TB_AlarmEvent></returns>
        List<Model.TB_AlarmEvent> GetAlarmEventByAlarmTypeId(int alarmTypeId);


        /// <summary>
        /// 更新告警触发事件表
        /// </summary>
        /// <param name="alarmEvent"></param>
        /// <returns></returns>
        int UpdateAlarmEvent(Model.QueryFile.QueryAlarmEvent alarmEvent);

        int UpdateAlarmEventByID(Model.QueryFile.QueryAlarmEvent alarmEventUpdate);

        /// <summary>
        /// 获取告警触发事件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Model.TB_AlarmEvent GetAlarmEventById(int Id);

        /// <summary>
        /// 删除告警触发事件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int DeleteAlarmEventById(int Id);

        /// <summary>
        /// 更新告警类型等级
        /// </summary>
        /// <param name="levelId">告警等级Id</param>
        /// <param name="Id">告警类型Id</param>
        /// <returns></returns>
        int UpdateAlarmTypeLevelById(int levelId, int Id);

        /// <summary>
        /// 获取告警分值配置列表
        /// </summary>
        /// <returns></returns>
        List<Model.TE_Alarm_Scale> GetAlarmScaleList();

        /// <summary>
        /// 根据ID 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteAlarmScaleByID(int id);

        bool InsertAlarmScale(int alarmType, int scale);

        bool UpdateAlarmScaleByID(int id, int scale);
    }
}
