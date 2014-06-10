using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DBUtility;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class BalanceAnalysis : IBalanceAnalysis
    {
        private Page page = new Page();
        public int GetChildAreaCount(int parentid)
        {
            try
            {

                string SQL = string.Format(@"select count (*) as total from becm_layerobject where layerobjectparentid=@layerobjectparentid");
                SqlParameter para = new SqlParameter("@layerobjectparentid", parentid);
               // return Convert.ToInt32(SqlHelper.ExecuteScalar(SQL, para));
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetChildAreaList(int pageIndex, int pageSize, int parentid)
        {
            try
            {
                PageModel pmodel = new PageModel();

                pmodel.tablename = "becm_layerobject";
                pmodel.keycol = "layerobjectid";
                pmodel.page = pageIndex;
                pmodel.pagesize = pageSize;
                pmodel.wherestr = " layerobjectparentid=" + parentid;
                pmodel.orderby = pmodel.keycol;
                return page.GetDataByPage(pmodel);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetChildAreaList(int parentid)
        {
            try
            {
                //PageModel pmodel = new PageModel();

                //pmodel.tablename = "becm_layerobject";
                //pmodel.keycol = "layerobjectid";
                //pmodel.page = pageIndex;
                //pmodel.pagesize = pageSize;
                //pmodel.wherestr = " layerobjectparentid=" + parentid;
                //pmodel.orderby = pmodel.keycol;
                //return page.GetDataByPage(pmodel);

                string SQL = string.Format(@"select  * from becm_layerobject where layerobjectparentid=@layerobjectparentid");
                SqlParameter para = new SqlParameter("@layerobjectparentid", parentid);
                //return SqlHelper.Query(SQL, para).Tables[0];
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetBalanaceValueByMonth(string itemcodeid, int areaid, System.DateTime month)
        {
            DataTable dt_result = new DataTable();
            dt_result.Columns.Add("Mastervalue");
            dt_result.Columns.Add("Secondarytotalvalue");
            dt_result.Columns.Add("Dvalue");
            dt_result.Columns.Add("Percents");
            dt_result.Columns.Add("unit");
            double Mastervalue = 0;
            double Secondarytotalvalue = 0;
            double Dvalue = 0;
            string Percents = "0%";
            string unit = "";
            try
            {
                //                string SQL = string.Format(@"select 
                //a.value12 value,b.category,d.unit
                //from TS_DataCenter_Device_Month_{0} a
                //join becm_device b on b.deviceid=a.CountID and b.itemcodeid=a.itemcode
                //join becm_layerobject c on c.layerobjectid=b.areaid 
                //join becm_itemcode  d on d.itemcodeid=b.itemcodeid and d.itemcodeid=@itemcodeid
                //where c.layerobjectid in 
                //(SELECT layerobjectid
                //FROM becm_layerobject a,f_Cid(@areaid) b
                //WHERE a.layerobjectid=b.ID 
                //)", month.Year);
                string SQL = string.Format(@"select 
a.Value30 value,b.category,d.unit
from TS_DataCenter_Device_Month_{0} a
join becm_device b on b.deviceid=a.CountID and b.itemcodeid=a.itemcode and b.category=1
 join becm_layerobject c on c.layerobjectid=b.areaid 
join becm_itemcode  d on d.itemcodenumber=b.itemcodeid and d.itemcodenumber=@itemcodeid
where c.layerobjectid in 
(SELECT layerobjectid
FROM becm_layerobject a,f_Cid(@areaid) b
WHERE a.layerobjectid=b.ID)

union all

select c.Value30 value,0 category,e.unit
from TS_DataCenter_Area_Month_{0} c
join tb_area d on c.countid=d.id
join becm_itemcode  e on e.itemcodenumber=c.itemcode and e.itemcodenumber=@itemcodeid
where d.id=@areaid
", month.Year);
                SqlParameter[] parms = { 
                                         new SqlParameter("@itemcodeid", itemcodeid),
                                         new SqlParameter("@areaid", areaid) };

               // DataTable dt = SqlHelper.Query(SQL, parms).Tables[0];
                DataTable dt = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        unit = dr["unit"].ToString();
                        string[] valueArray = dr["value"].ToString().Split(',');
                        if (valueArray.Length > 0)
                        {
                            foreach (string s in valueArray)
                            {
                                if (s.IndexOf('_') > -1)
                                {

                                    if (Convert.ToDateTime(s.Split('_')[0]).Month == month.Month)
                                    {
                                        if (dr["category"].ToString() == "0")
                                        {
                                            Mastervalue += Convert.ToDouble(s.Split('_')[1]);
                                        }
                                        else
                                        {
                                            Secondarytotalvalue += Convert.ToDouble(s.Split('_')[1]);
                                        }
                                    }
                                }

                            }
                        }

                    }

                }
                Mastervalue = Convert.ToDouble(Mastervalue.ToString("0.00"));
                Secondarytotalvalue = Convert.ToDouble(Secondarytotalvalue.ToString("0.00"));
                Dvalue = Convert.ToDouble((Mastervalue - Secondarytotalvalue).ToString("0.00"));
                Percents = Mastervalue != 0 ? (Dvalue / Mastervalue).ToString("0.00%") : "0.00%";
                dt_result.Rows.Add(Mastervalue, Secondarytotalvalue, Dvalue, Percents, unit);
                return dt_result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
