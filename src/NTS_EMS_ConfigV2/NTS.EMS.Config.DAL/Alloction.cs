using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.ProductInteface;
using Framework.Data;

namespace NTS.EMS.Config.DAL
{
    public class Alloction : IAlloction
    {

        public bool SaveAlloctionAndLog(string sql)
        {
            var cmd = new DataCommand("saveAlloctionAndLog", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#sql#", sql);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }


        public Model.TB_BECM_COUNTTYPE GetCountType(string energyId)
        {
            try
            {
                var cmd = new DataCommand("getCountType", new SqlCustomDbCommand());
                cmd.SetParameterValue("@energyId", energyId);
                return cmd.ExecuteEntity<Model.TB_BECM_COUNTTYPE>();
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public List<Model.TB_ALLOCTION_CONFIG> GetAlloctionList(string whereStr)
        {
            var cmd = new DataCommand("getAlloctionList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_ALLOCTION_CONFIG>();
        }


        public List<Model.TB_ALLOCTION_CONFIG_History> GetConfigLogList(string whereStr, string orderBy)
        {
            var cmd = new DataCommand("getConfigLogList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            cmd.ReplaceParameterValue("#orderBy#", orderBy);
            return cmd.ExecuteEntityList<Model.TB_ALLOCTION_CONFIG_History>();
        }

        public List<Model.TB_AREA_Info> GetAreaInfoList(string whereStr)
        {
            var cmd = new DataCommand("getAreaInfoList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_AREA_Info>();
        }


        public List<Model.TS_FEE_DAY> GetFeeDayList(int year, string whereStr)
        {
            try
            {
                var cmd = new DataCommand("getFeeDay", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#TableName#", "TS_FEE_DAY_" + year);
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                List<Model.TS_FEE_DAY> list = cmd.ExecuteEntityList<Model.TS_FEE_DAY>();
                return list;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return null;
        }
    }
}
