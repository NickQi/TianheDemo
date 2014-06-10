using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.ProductInteface;
using NTS.EMS.Config.Model.ResultViewFile;
using NTS.EMS.Config.Model.QueryFile;

namespace NTS.EMS.Config.BLL
{
    public class AlarmSettingBLL
    {
        private readonly IAlarmSetting _dal = DataSwitchConfig.CreateAlarmSetting();

        /// <summary>
        /// 获取告警类型表所有数据
        /// </summary>
        /// <returns>List<Model.TB_AlarmType></returns>
        public ResultAlarmTypes GetAlarmTypeList(QueryAlarmSetting query)
        {
            ResultAlarmTypes resultAlarmTypes = new ResultAlarmTypes();
            var list = _dal.GetAlarmTypeList("");
            #region 组织数据
            var alarmTypeList = list.Select(p =>
                                    new AlarmType
                                    {
                                        ID = p.ID,
                                        ProjectId = p.ProjectId,
                                        SystemId = p.SystemId,
                                        Type = p.Type,
                                        Name = p.Name,
                                        AlarmLevel = p.AlarmLevel
                                    }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();

            foreach (AlarmType at in alarmTypeList)
            {
                var aeLst = _dal.GetAlarmEventByAlarmTypeId(at.Type).Select(p =>
                                new AlarmEvent
                                {
                                    ID = p.ID,
                                    ProjectId = p.ProjectId,
                                    SystemId = p.SystemId,
                                    AlarmTypeId = p.AlarmTypeId,
                                    PlugIn = p.PlugIn,
                                    TrigMode = p.TrigMode,
                                    RunMode = p.RunMode,
                                    RunCount = p.RunCount,
                                    RunTime = p.RunTime,
                                    Options = p.Options
                                }).ToList();
                at.PlugIns = aeLst;
            }
            resultAlarmTypes.AlarmTypeList = alarmTypeList;

            resultAlarmTypes.Page = new Padding();
            resultAlarmTypes.Page.Current = query.PageCurrent;
            resultAlarmTypes.Page.Total = list.Count();
            return resultAlarmTypes;
            #endregion
        }

        /// <summary>
        /// 更新告警触发事件表
        /// </summary>
        /// <param name="alarmEvent"></param>
        /// <returns></returns>
        public ResultAlarmEvent UpdateAlarmEvent(QueryAlarmEventUpdate alarmEventUpdate)
        {
            try
            {
                TB_AlarmType modelAlarm = new TB_AlarmType();
                // 获取当前修改的告警信息
                List<TB_AlarmType> listType = _dal.GetAlarmTypeList(" and TYPE='" + alarmEventUpdate.AlarmTypeId + "'");
                if (listType.Count > 0)
                {
                    modelAlarm = listType[0];
                }

                //update alarmLeven
                if (alarmEventUpdate.AlarmLevelId.HasValue)
                {
                    _dal.UpdateAlarmTypeLevelById(alarmEventUpdate.AlarmLevelId.Value, alarmEventUpdate.AlarmTypeId);
                }

                //update alarmEvent
                if (alarmEventUpdate.Update != null)
                    foreach (QueryAlarmEvent qae in alarmEventUpdate.Update)
                    {
                        qae.PROJECTID = modelAlarm.ProjectId;
                        qae.SYSTEMID = modelAlarm.SystemId;
                        _dal.UpdateAlarmEvent(qae);
                    }
                //del alarmEvent
                if (alarmEventUpdate.Del != null)
                    foreach (int Id in alarmEventUpdate.Del)
                    {
                        _dal.DeleteAlarmEventById(Id);
                    }

                return new ResultAlarmEvent { IsSucess = true };
            }
            catch (Exception ex)
            {
                return new ResultAlarmEvent { IsSucess = false };
            }
        }

        /// <summary>
        /// 更新告警触发事件表
        /// </summary>
        /// <param name="alarmEvent"></param>
        /// <returns></returns>
        public ResultAlarmEvent UpdateAlarmEventByID(QueryAlarmEvent alarmEventUpdate)
        {
            try
            {
                _dal.UpdateAlarmEventByID(alarmEventUpdate);

                return new ResultAlarmEvent { IsSucess = true };
            }
            catch (Exception ex)
            {
                return new ResultAlarmEvent { IsSucess = false };
            }
        }

        /// <summary>
        /// 根据告警类型，获取触发事件列表
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ResultAlarmEvents GetAlarmEventList(int typeId)
        {
            ResultAlarmEvents result = new ResultAlarmEvents();
            List<TB_AlarmEvent> list = _dal.GetAlarmEventByAlarmTypeId(typeId);
            result.ListAlarmEvent = list;
            return result;
        }

        #region 告警分值
        public ResultAlarmScaleTypes GetAlarmScaleList(QueryAlarmSetting query)
        {
            ResultAlarmScaleTypes resultAlarmTypes = new ResultAlarmScaleTypes();
            var list = _dal.GetAlarmScaleList();
            #region 组织数据

            var alarmScaleList = list.Select(p =>
                                    new AlarmScale
                                    {
                                        ID = p.ID,
                                        AlarmType = p.AlarmType,
                                        AlarmName = p.AlarmName,
                                        Scale = p.Scale
                                    }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();


            resultAlarmTypes.AlarmScaleList = alarmScaleList;

            resultAlarmTypes.Page = new Padding();
            resultAlarmTypes.Page.Current = query.PageCurrent;
            resultAlarmTypes.Page.Total = list.Count();
            return resultAlarmTypes;
            #endregion
        }

        public ResultRate DeleteAlarmScaleByID(int id)
        {
            ResultRate result = new ResultRate();
            result.IsSucess = _dal.DeleteAlarmScaleByID(id);
            return result;
        }

        public ResultRate SaveAlarmScale(QueryAlarmScaleSetting query)
        {
            ResultRate result = new ResultRate();
            if (query != null)
            {
                if (query.ID == 0)
                {
                    result.IsSucess = _dal.InsertAlarmScale(query.AlarmType, query.Scale);
                }
                else
                {
                    result.IsSucess = _dal.UpdateAlarmScaleByID(query.ID, query.Scale);
                }
            }
            return result;
        }
        #endregion
    }
}
