using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.BLL
{
    public class WarningAnalysis
    {
        private static Hashtable ht = new Hashtable();
        private readonly IWarningAnalysis dal = DataSwitchConfig.CreateWarningAnalysis();

        public int GetWarningPageCount(int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {
            return dal.GetWarningPageCount(pageSize, startTime, endTime, warningTypeId, areaId);
        }

        public DataTable GetWarningListByPage(int pageIndex, int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {
            //return dal.GetWarningListByPage(pageIndex, pageSize, startTime, endTime, warningTypeId, areaId);
            DataTable dt_result = new DataTable();
            dt_result.TableName = "GetWarningListByPage";
            dt_result.Columns.Add("id");
            dt_result.Columns.Add("datetime");
            dt_result.Columns.Add("Alarmcontent");
            dt_result.Columns.Add("Devicename");


            //DataTable dt_result = dal.GetWarningListByPage(pageIndex, pageSize, startTime, endTime, warningTypeId, areaId);
            //dt_result.TableName = "GetWarningListByPage";
            return dt_result;
        }

        public int GetWarningPageCount(WarningAnalysisModel model)
        {
            return dal.GetWarningPageCount(model);
        }

        public DataTable GetWarningListByPage(WarningAnalysisModel model)
        {
            //return dal.GetWarningListByPage(pageIndex, pageSize, startTime, endTime, warningTypeId, areaId);
            DataTable dt_result = new DataTable();
            dt_result.TableName = "GetWarningListByPage";
            //dt_result.Columns.Add("id",typeof(int));
            dt_result.Columns.Add("datetime", typeof(DateTime));
            dt_result.Columns.Add("Alarmcontent");
            dt_result.Columns.Add("Devicename");
            DataTable dt = dal.GetWarningListByPage(model);
            ht = new Hashtable();

            foreach (DataRow dr in dt.Rows)
            {

                if (!ht.ContainsKey(dr["Devicename"].ToString()))
                {

                    ht.Add(dr["Devicename"].ToString(), dal.GetWaringContentPref(dr["Devicename"].ToString()));
                }
                string Alarmcontent = ht[dr["Devicename"].ToString()].ToString();
                dt_result.Rows.Add(dr["datetime"], Alarmcontent + dr["Alarmcontent"].ToString(), dr["Devicename"]);
            }

            //DataTable dt_result = dal.GetWarningListByPage(model);
            //dt_result.TableName = "GetWarningListByPage";
            return dt_result;
        }

        public DataTable GetAreaList()
        {

            DataTable dt_result = dal.GetAreaList().Tables[0];
            dt_result.TableName = "GetAreaList";
            return dt_result;
        }



        public List<WarningTypeModel> GetWarningTypeList()
        {
            return dal.GetWarningTypeList();
        }
    }
}
