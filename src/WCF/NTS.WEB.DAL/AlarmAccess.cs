using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DBUtility;
using Framework.Data;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class AlarmAccess : IAlarmAccess
    {
        /// <summary>
        /// 获取告警类型
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public DataTable GetAlarmType(string whereStr)
        {
            //DataTable dtAlarmType = new DataTable();
            //string strSql = "select TYPE,NAME from TB_ALARMTYPE where 1=1";
            //if (!whereStr.Equals(string.Empty))
            //{
            //    strSql = strSql + whereStr;
            //}
            //dtAlarmType = SqlHelper.Query(strSql).Tables[0];
            //return dtAlarmType;

            var cmd = new DataCommand("GetAlarmType", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteDataSet().Tables[0];
        }

        /// <summary>
        /// 获取告警数量
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public int GetAlarmCount(string whereStr)
        {
            //DataTable dtAlarmType = new DataTable();
            //string strSql = "select count(id) from TE_ALARM where 1=1 ";
            //if (!whereStr.Equals(string.Empty))
            //{
            //    strSql = strSql + whereStr;
            //}
            //Object obj = SqlHelper.GetSingle(strSql);
            //int Count = 0;
            //if (obj != DBNull.Value)
            //{
            //    Count = int.Parse(obj.ToString());
            //}
            //return Count;

            var cmd = new DataCommand("GetAlarmCount", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }


        /// <summary>
        /// 获取告警类型数据
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public DataTable GetAlarmListOld(QueryAlarmNew alarmNew)
        {
            int intPazeSize = alarmNew.PageSize;
            int intNext = intPazeSize * (alarmNew.PageIndex - 1);
            DataTable dtAlarmType = new DataTable();
            string strSql = @" SELECT distinct TOP " + intPazeSize + " id, ALARMOBJID,ALARMTYPE,ALARMTYPENAME,ALARMLEVEL,ALARMOBJNAME,ALARMCONTENT,STATUS,ALARMTIME,location " +
                 " from AlarmAreaDevice WHERE id not in " +
               "  ( SELECT TOP " + intNext + " id from AlarmAreaDevice ) and 1=1 ";
            string whereStr = "";
            string orderStr = "ORDER BY id desc";
            whereStr += " and ALARMTIME between '" + alarmNew.StartTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + alarmNew.EndTime.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (alarmNew.AlarmLevel != "")
            {
                whereStr += " and ALARMLEVEL = " + alarmNew.AlarmLevel;
            }
            if (alarmNew.AlarmStatus != "")
            {
                whereStr += " and STATUS = " + alarmNew.AlarmStatus;
            }
            if (alarmNew.AlarmType != "")
            {
                whereStr += " and alarmtype = " + alarmNew.AlarmType;
            }
            IList<string> lstAreaId = GetAlarmListAreaId(alarmNew.ObjectId, alarmNew.ObjType);
            IList<string> lstDeviceId = GetAlarmListDeviceId(alarmNew.ObjectId, alarmNew.ObjType);
            whereStr += " and (";
            if (lstAreaId.Count > 0)
            {
                string strAreaId = string.Join(",", lstAreaId.ToArray());
                whereStr += " ALARMOBJID in (" + strAreaId + ") and alarmObjType=32  ";
            }
            else
            {
                whereStr += "1=2";
            }
            whereStr += " or ";
            if (lstDeviceId.Count > 0)
            {
                string strAreaId = string.Join(",", lstAreaId.ToArray());
                whereStr += " ALARMOBJID in (" + strAreaId + ") and alarmObjType=31 ";
            }
            else
            {
                whereStr += "1=2";
            }
            whereStr += ")";

            if (!whereStr.Equals(string.Empty))
            {
                strSql = strSql + whereStr;
            }
            if (!orderStr.Equals(string.Empty))
            {
                strSql = strSql + orderStr;
            }
            dtAlarmType = SqlHelper.Query(strSql).Tables[0];
            return dtAlarmType;
        }


        /// <summary>
        /// 获取告警类型数据
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public DataTable GetAlarmList(QueryAlarmNew alarmNew, string groupId)
        {
            try
            {
                int intPazeSize = alarmNew.PageSize;
                int intNext = intPazeSize * (alarmNew.PageIndex - 1);

                string whereStr = "";
                string orderStr = "ORDER BY id desc";
                whereStr += " and ALARMTIME between '" + alarmNew.StartTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + alarmNew.EndTime.ToString("yyyy-MM-dd 23:59:59") + "'";
                if (alarmNew.AlarmLevel != "")
                {
                    whereStr += " and ALARMLEVEL = " + alarmNew.AlarmLevel;
                }
                if (alarmNew.AlarmStatus != "")
                {
                    whereStr += " and STATUS = " + alarmNew.AlarmStatus;
                }
                if (alarmNew.AlarmType != "")
                {
                    whereStr += " and alarmtype = " + alarmNew.AlarmType;
                }
                IList<string> lstAreaId = GetAlarmListAreaId(alarmNew.ObjectId, alarmNew.ObjType);
                IList<string> lstDeviceId = GetAlarmListDeviceId(alarmNew.ObjectId, alarmNew.ObjType);
                whereStr += " and (";
                if (lstAreaId.Count > 0)
                {
                    string strAreaId = string.Join(",", lstAreaId.ToArray());
                    whereStr += " ALARMOBJID in (" + strAreaId + ") and alarmObjType=32  ";
                }
                else
                {
                    whereStr += "1=2";
                }
                whereStr += " or ";
                if (lstDeviceId.Count > 0)
                {
                    string strDeviceId = string.Join(",", lstDeviceId.ToArray());
                    whereStr += " ALARMOBJID in (" + strDeviceId + ") and alarmObjType=31 ";
                }
                else
                {
                    whereStr += "1=2";
                }
                whereStr += ")";

                var cmd = new DataCommand("GetAlarmList", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#PageSize#", intPazeSize.ToString());
                cmd.ReplaceParameterValue("#Next#", intNext.ToString());
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                cmd.ReplaceParameterValue("#orderStr#", orderStr);
                return cmd.ExecuteDataSet().Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// 获取告警类型数据
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public DataTable GetAlarmListIndex(QueryAlarmNew alarmNew)
        {
            int intPazeSize = alarmNew.PageSize;
            int intNext = intPazeSize * (alarmNew.PageIndex - 1);
            DataTable dtAlarmType = new DataTable();
            //string strSql = @" SELECT distinct TOP " + intPazeSize + " id, ALARMOBJID,ALARMTYPE,ALARMTYPENAME,ALARMLEVEL,ALARMOBJNAME,ALARMCONTENT,STATUS,ALARMTIME,location " +
            //     " from AlarmAreaDevice WHERE id not in " +
            //   "  ( SELECT TOP " + intNext + " id from AlarmAreaDevice ) and 1=1 ";
            string whereStr = "";
            string orderStr = "ORDER BY id desc";
            whereStr += " and ALARMTIME between '" + alarmNew.StartTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + alarmNew.EndTime.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (alarmNew.AlarmStatus != "")
            {
                whereStr += " and STATUS = " + alarmNew.AlarmStatus;
            }

            var cmd = new DataCommand("GetAlarmListIndex", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#PageSize#", intPazeSize.ToString());
            cmd.ReplaceParameterValue("#Next#", intNext.ToString());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            cmd.ReplaceParameterValue("#orderStr#", orderStr);
            return cmd.ExecuteDataSet().Tables[0];
        }

        /// <summary>
        /// 获取告警类型
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public int GetAlarmListCount(QueryAlarmNew alarmNew)
        {
            int intPazeSize = alarmNew.PageSize;
            int intNext = intPazeSize * (alarmNew.PageIndex - 1);
            DataTable dtAlarmType = new DataTable();

            string whereStr = "";
            whereStr += " and ALARMTIME between '" + alarmNew.StartTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + alarmNew.EndTime.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (alarmNew.AlarmLevel != "")
            {
                whereStr += " and ALARMLEVEL = " + alarmNew.AlarmLevel;
            }
            if (alarmNew.AlarmStatus != "")
            {
                whereStr += " and STATUS = " + alarmNew.AlarmStatus;
            }
            if (alarmNew.AlarmType != "")
            {
                whereStr += " and alarmtype = " + alarmNew.AlarmType;
            }
            IList<string> lstAreaId = GetAlarmListAreaId(alarmNew.ObjectId, alarmNew.ObjType);
            IList<string> lstDeviceId = GetAlarmListDeviceId(alarmNew.ObjectId, alarmNew.ObjType);
            whereStr += " and (";
            if (lstAreaId.Count > 0)
            {
                string strAreaId = string.Join(",", lstAreaId.ToArray());
                whereStr += " ALARMOBJID in (" + strAreaId + ") ";
            }
            else
            {
                whereStr += "1=2";
            }
            whereStr += " or ";
            if (lstDeviceId.Count > 0)
            {
                string strDeviceId = string.Join(",", lstDeviceId.ToArray());
                whereStr += " ALARMOBJID in (" + strDeviceId + ") ";
            }
            else
            {
                whereStr += "1=2";
            }
            whereStr += ")";


            var cmd = new DataCommand("GetAlarmListCount", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }


        /// <summary>
        /// 获取告警类型
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public int GetAlarmIndexCount(QueryAlarmNew alarmNew)
        {
            int intPazeSize = alarmNew.PageSize;
            int intNext = intPazeSize * (alarmNew.PageIndex - 1);
            DataTable dtAlarmType = new DataTable();
            string strSql = @" SELECT count(id) from AlarmAreaDevice where 1=1";
            string whereStr = "";
            whereStr += " and ALARMTIME between '" + alarmNew.StartTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + alarmNew.EndTime.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (alarmNew.AlarmStatus != "")
            {
                whereStr += " and STATUS = " + alarmNew.AlarmStatus;
            }

            if (!whereStr.Equals(string.Empty))
            {
                strSql = strSql + whereStr;
            }

            // Object obj = SqlHelper.GetSingle(strSql);
            var cmd = new DataCommand("GetBaseDataItem", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#SQLSTR#", strSql);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }


        /// <summary>
        /// 根据选择对象获取所有子区域ID
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public IList<string> GetAlarmListAreaId(int ObjectId, AreaType ObjType)
        {
            string whereStr = "";
            DataTable dtAlarmType = new DataTable();
            IList<string> lstAreaId = new List<string>();
            string strSql = "";
            if (ObjType == AreaType.Area)
            {
                strSql = @" select * from  f_GetChildAreaId(" + ObjectId + ") ";
                if (!whereStr.Equals(string.Empty))
                {
                    strSql = strSql + whereStr;
                }
                var cmd = new DataCommand("f_GetChildAreaId", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#ObjectId#", ObjectId.ToString());
                dtAlarmType = cmd.ExecuteDataSet().Tables[0];

                if (dtAlarmType.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAlarmType.Rows)
                    {
                        if (row["id"] != DBNull.Value)
                        {
                            lstAreaId.Add(row["id"].ToString());
                        }
                    }
                }
            }
            else
            {
                strSql = @" select * from  f_GetChildFuncAreaId(" + ObjectId + ") ";
                if (!whereStr.Equals(string.Empty))
                {
                    strSql = strSql + whereStr;
                }
                var cmd = new DataCommand("f_GetChildFuncAreaId", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#ObjectId#", ObjectId.ToString());
                dtAlarmType = cmd.ExecuteDataSet().Tables[0];
                if (dtAlarmType.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAlarmType.Rows)
                    {
                        if (row["id"] != DBNull.Value)
                        {
                            lstAreaId.Add(row["id"].ToString());
                        }
                    }
                }
            }


            return lstAreaId;
        }


        /// <summary>
        /// 根据选择对象获取所有子区域ID
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        private IList<string> GetAlarmListDeviceId(int ObjectId, AreaType ObjType)
        {
            string whereStr = "";
            DataTable dtAlarmType = new DataTable();
            IList<string> lstDeviceId = new List<string>();
            string strSql = "";
            if (ObjType == AreaType.Area)
            {
                strSql = @" select distinct s2.deviceid as id from  f_GetChildAreaId(" + ObjectId + ") s1 inner join Becm_Device s2 on s1.id = s2.areaid ";
                if (!whereStr.Equals(string.Empty))
                {
                    strSql = strSql + whereStr;
                }

                var cmd = new DataCommand("f_GetChildAreaIdDevice", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#ObjectId#", ObjectId.ToString());
                dtAlarmType = cmd.ExecuteDataSet().Tables[0];
                // dtAlarmType = SqlHelper.Query(strSql).Tables[0];

                if (dtAlarmType.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAlarmType.Rows)
                    {
                        if (row["id"] != DBNull.Value)
                        {
                            lstDeviceId.Add(row["id"].ToString());
                        }
                    }
                }
            }
            else
            {
                strSql = @" select distinct s2.deviceid as id from  f_GetChildFuncAreaId(" + ObjectId + ") s1 inner join Becm_Device s2 on s1.id = s2.areaid2 ";
                if (!whereStr.Equals(string.Empty))
                {
                    strSql = strSql + whereStr;
                }
                //dtAlarmType = SqlHelper.Query(strSql).Tables[0];
                var cmd = new DataCommand("f_GetChildFuncAreaIdDevice", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#ObjectId#", ObjectId.ToString());
                dtAlarmType = cmd.ExecuteDataSet().Tables[0];
                if (dtAlarmType.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAlarmType.Rows)
                    {
                        if (row["id"] != DBNull.Value)
                        {
                            lstDeviceId.Add(row["id"].ToString());
                        }
                    }
                }
            }


            return lstDeviceId;
        }

    }
}
