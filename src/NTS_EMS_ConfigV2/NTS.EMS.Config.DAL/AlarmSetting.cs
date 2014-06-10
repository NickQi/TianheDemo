using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class AlarmSetting : IAlarmSetting
    {
        /// <summary>
        /// 获取告警类型表所有数据
        /// </summary>
        /// <returns>List<Model.TB_AlarmType></returns>
        public List<Model.TB_AlarmType> GetAlarmTypeList(string whereStr)
        {
            var cmd = new DataCommand("getAlarmTypeList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_AlarmType>();
        }

        /// <summary>
        /// 获取告警触发事件表数据
        /// </summary>
        /// <param name="alarmTypeId">告警类型表ID</param>
        /// <returns>List<Model.TB_AlarmEvent></returns>
        public List<Model.TB_AlarmEvent> GetAlarmEventByAlarmTypeId(int alarmTypeId)
        {
            var cmd = new DataCommand("getAlarmEventByAlarmTypeId", new SqlCustomDbCommand());
            cmd.SetParameterValue("@alarmTypeId", alarmTypeId);
            return cmd.ExecuteEntityList<Model.TB_AlarmEvent>();
        }

        /// <summary>
        /// 更新告警触发事件表
        /// </summary>
        /// <param name="alarmEvent"></param>
        /// <returns></returns>
        public int UpdateAlarmEvent(Model.QueryFile.QueryAlarmEvent alarmEvent)
        {
            var aeEntity = this.GetAlarmEventById(alarmEvent.ID);
            if (aeEntity == null)
            {//insert
                var cmd = new DataCommand("insertAlarmEvent", new SqlCustomDbCommand());
                cmd.SetParameterValue("@AlarmTypeId", alarmEvent.AlarmTypeId);
                cmd.SetParameterValue("@PlugIn", alarmEvent.PlugIn);
                cmd.SetParameterValue("@TrigMode", alarmEvent.TrigMode);
                cmd.SetParameterValue("@RunMode", alarmEvent.RunMode);
                cmd.SetParameterValue("@RunCount", alarmEvent.RunCount);
                cmd.SetParameterValue("@RunTime", alarmEvent.RunTime);
                cmd.SetParameterValue("@Options", alarmEvent.Options);
                cmd.SetParameterValue("@PROJECTID", alarmEvent.PROJECTID);
                cmd.SetParameterValue("@SYSTEMID", alarmEvent.SYSTEMID);

                return cmd.ExecuteNonQuery();
            }
            else
            {//update 
                aeEntity.AlarmTypeId = (aeEntity.AlarmTypeId != alarmEvent.AlarmTypeId) ? alarmEvent.AlarmTypeId : aeEntity.AlarmTypeId;
                aeEntity.PlugIn = string.IsNullOrEmpty(alarmEvent.PlugIn) ? aeEntity.PlugIn : alarmEvent.PlugIn;
                aeEntity.TrigMode = (aeEntity.TrigMode != alarmEvent.TrigMode) ? alarmEvent.TrigMode : aeEntity.TrigMode;
                aeEntity.RunMode = (aeEntity.RunMode != alarmEvent.RunMode) ? alarmEvent.RunMode : aeEntity.RunMode;
                aeEntity.RunCount = (aeEntity.RunCount != alarmEvent.RunCount) ? alarmEvent.RunCount : aeEntity.RunCount;
                aeEntity.RunTime = (aeEntity.RunTime != alarmEvent.RunTime) ? alarmEvent.RunTime : aeEntity.RunTime;
                aeEntity.Options = string.IsNullOrEmpty(alarmEvent.Options) ? aeEntity.Options : alarmEvent.Options;
                aeEntity.ProjectId = (aeEntity.ProjectId != alarmEvent.PROJECTID) ? alarmEvent.PROJECTID : aeEntity.ProjectId;
                aeEntity.SystemId = (aeEntity.SystemId != alarmEvent.SYSTEMID) ? alarmEvent.SYSTEMID : aeEntity.SystemId;

                var cmd = new DataCommand("updateAlarmEvent", new SqlCustomDbCommand());
                cmd.SetParameterValue("@AlarmTypeId", aeEntity.AlarmTypeId);
                cmd.SetParameterValue("@PlugIn", aeEntity.PlugIn);
                cmd.SetParameterValue("@TrigMode", aeEntity.TrigMode);
                cmd.SetParameterValue("@RunMode", aeEntity.RunMode);
                cmd.SetParameterValue("@RunCount", aeEntity.RunCount);
                cmd.SetParameterValue("@RunTime", aeEntity.RunTime);
                cmd.SetParameterValue("@Options", aeEntity.Options);
                cmd.SetParameterValue("@PROJECTID", aeEntity.ProjectId);
                cmd.SetParameterValue("@SYSTEMID", aeEntity.SystemId);
                cmd.SetParameterValue("@Id", aeEntity.ID);

                return cmd.ExecuteNonQuery();
            }
        }

        public int UpdateAlarmEventByID(Model.QueryFile.QueryAlarmEvent alarmEvent)
        {
            var cmd = new DataCommand("updateAlarmEventByID", new SqlCustomDbCommand());

            cmd.SetParameterValue("@TrigMode", alarmEvent.TrigMode);
            cmd.SetParameterValue("@RunMode", alarmEvent.RunMode);
            cmd.SetParameterValue("@RunCount", alarmEvent.RunCount);
            cmd.SetParameterValue("@RunTime", alarmEvent.RunTime);
            cmd.SetParameterValue("@Options", alarmEvent.Options);
            cmd.SetParameterValue("@Id", alarmEvent.ID);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取告警触发事件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Model.TB_AlarmEvent GetAlarmEventById(int Id)
        {
            var cmd = new DataCommand("getAlarmEventById", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", Id);

            return cmd.ExecuteEntity<Model.TB_AlarmEvent>();
        }

        /// <summary>
        /// 删除告警触发事件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteAlarmEventById(int Id)
        {
            var cmd = new DataCommand("deleteAlarmEventById", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", Id);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新告警类型等级
        /// </summary>
        /// <param name="levelId">告警等级Id</param>
        /// <param name="Id">告警类型Id</param>
        /// <returns></returns>
        public int UpdateAlarmTypeLevelById(int levelId, int Id)
        {
            var cmd = new DataCommand("updateAlarmTypeLevelById", new SqlCustomDbCommand());
            cmd.SetParameterValue("@alarmLevelId", levelId);
            cmd.SetParameterValue("@Id", Id);

            return cmd.ExecuteNonQuery();
        }


        public List<Model.TE_Alarm_Scale> GetAlarmScaleList()
        {
            var cmd = new DataCommand("getAlarmScaleList", new SqlCustomDbCommand());
            return cmd.ExecuteEntityList<Model.TE_Alarm_Scale>();
        }


        public bool DeleteAlarmScaleByID(int id)
        {
            var cmd = new DataCommand("deleteAlarmScaleByID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", id);

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }


        public bool InsertAlarmScale(int alarmType, int scale)
        {
            var cmd = new DataCommand("insertAlarmScale", new SqlCustomDbCommand());
            cmd.SetParameterValue("@alarmType", alarmType);
            cmd.SetParameterValue("@scale", scale);

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool UpdateAlarmScaleByID(int id, int scale)
        {
            var cmd = new DataCommand("updateAlarmScaleByID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@id", id);
            cmd.SetParameterValue("@scale", scale);

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
