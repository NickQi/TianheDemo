using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DBUtility;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class WarningAnalysis : IWarningAnalysis
    {
        private Page page = new Page();
        private static List<WarningTypeModel> wt_List;




        public int GetWarningPageCount(int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {
            var count = page.GetPageCount(InitPageModel(1, 20, startTime, endTime, warningTypeId, areaId));
            // return count;
            return count % pageSize == 0 ? count / pageSize : Convert.ToInt32(count / pageSize) + 1;
        }

        public int GetWarningPageCount(WarningAnalysisModel model)
        {
            try
            {

                var count = page.GetPageCount(InitPageModel(model));
                // return count;
                return count % model.PageSize == 0 ? count / model.PageSize : Convert.ToInt32(count / model.PageSize) + 1;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public DataTable GetWarningListByPage(int pageIndex, int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {



            return page.GetDataByPage(InitPageModel(pageIndex, pageSize, startTime, endTime, warningTypeId, areaId));
        }
        public DataTable GetWarningListByPage(WarningAnalysisModel model)
        {
            try
            {
                return page.GetDataByPage(InitPageModel(model));
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        private PageModel InitPageModel(int pageIndex, int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {

            WarningTypeModel wt = wt_List.Find(s => s.Value == warningTypeId);
            if (wt == null)
            {
                throw new Exception("报警类型xml配置有误");
            }
            if (string.IsNullOrEmpty(wt.CValue) || string.IsNullOrEmpty(wt.keycol))
            {
                throw new Exception(wt.Text + "缺少数据库参数信息");
            }

            PageModel pmodel = new PageModel();
            DateTime starttime = Convert.ToDateTime(startTime);
            DateTime endtime = Convert.ToDateTime(endTime).AddDays(1);


            pmodel.tablename = wt.CValue;
            pmodel.keycol = wt.keycol;
            pmodel.page = pageIndex;
            pmodel.pagesize = pageSize;

            pmodel.wherestr = " DATETIME >= '" + starttime.ToShortDateString() + "' and DATETIME < '" + endtime.ToShortDateString() + "'";

            pmodel.wherestr += string.Format(@" and devicename in (select cname from tb_device where devarea in (SELECT a.id
FROM tb_area a,f_Cid({0}) b
WHERE a.id=b.ID ))", areaId);
            pmodel.orderby = wt.keycol;
            return pmodel;
        }

        private PageModel InitPageModel(WarningAnalysisModel model)
        {

            WarningTypeModel wt = wt_List.Find(s => s.Value == model.SelectTypeId);
            if (wt == null)
            {
                throw new Exception("报警类型xml配置有误");
            }
            if (string.IsNullOrEmpty(wt.CValue) || string.IsNullOrEmpty(wt.keycol))
            {
                throw new Exception(wt.Text + "缺少数据库参数信息");
            }
            PageModel pmodel = new PageModel();
            DateTime starttime = model.StartDate;
            DateTime endtime = model.EndDate.AddDays(1);


            pmodel.tablename = wt.CValue;
            pmodel.keycol = wt.keycol;
            pmodel.page = model.PageCurrent;
            pmodel.pagesize = model.PageSize;

            pmodel.wherestr = " DATETIME >= '" + starttime.ToShortDateString() + "' and DATETIME < '" + endtime.ToShortDateString() + "'";

            pmodel.wherestr += string.Format(@" and devicename in (select cname from tb_device where devarea in (SELECT a.id
FROM tb_area a,f_Cid({0}) b
WHERE a.id=b.ID ))", model.BuildId);
            pmodel.orderby = wt.keycol;
            return pmodel;
        }

        /// <summary>
        /// 根据TB_AREA表获取区域列表信息
        /// </summary>
        /// <returns></returns>

        public DataSet GetAreaList()
        {
            try
            {
                //                string SQL = @"SELECT AREAID, CNAME, PARENTID,FLAG
                //FROM TB_AREA ORDER BY AREAID";
                //                string SQL = @"select layerobjectid,layerobjectname,layerobjectparentid,layerobjectdeepth from dbo.Becm_LayerObject
                //order by layerobjectid";
                string SQL = @"select layerobjectid,layerobjectname,layerobjectparentid from dbo.Becm_LayerObject
order by layerobjectid";
                //return SqlHelper.Query(SQL);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public DataSet GetAreaTreeList()
        {
            DataSet ds = new DataSet();
            //string sql = @" select tempid,id,cname,temppid from TB_TEMP  ";
            string sql = @" exec areatree";
            //return SqlHelper.Query(sql);
            return null;
        }

        /// <summary>
        /// 根据WarningType.xml获取告警类型列表
        /// </summary>
        /// <returns></returns>

        public List<WarningTypeModel> GetWarningTypeList()
        {
            //var cacheWarningTypeList = NTS.WEB.Common.CacheHelper.GetCache("WarningTypeList");
            //if (cacheWarningTypeList != null)
            //{
            //    return (List<WarningTypeModel>)cacheWarningTypeList;
            //}
            if (wt_List!=null)
            {
                return wt_List;
            }


            if (ConfigurationManager.AppSettings["WarningTypeXmlPath"] == null)
            {

                throw new Exception("报警类型XML配置不存在");
            }

           string  AppPath = AppDomain.CurrentDomain.BaseDirectory;
           //if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
           //    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            var xmlPath = Path.Combine(AppPath, ConfigurationManager.AppSettings["WarningTypeXmlPath"]);
            //var xmlPath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["WarningTypeXmlPath"]);
            if (File.Exists(xmlPath))
            {
                XElement root = XElement.Load(xmlPath);
                try
                {
                    var items = from el in root.Elements("Item")
                                select new WarningTypeModel()
                                {
                                    Value = el.Attribute("Value").Value,
                                    Text = el.Attribute("Text").Value,
                                    CValue = el.Attribute("CValue").Value,
                                    keycol = el.Attribute("keycol").Value


                                };
                    wt_List = items.ToList();
                    //NTS.WEB.Common.CacheHelper.SetCache("WarningTypeList", items.ToList());
                    return items.ToList();
                }
                catch (Exception ex)
                {

                    throw new Exception("报警类型XML属性配置错误");
                }
            }
            else
            {

                throw new Exception("报警类型XML文件不存在");
            }
        }


        public string GetWaringContentPref(string devicename)
        {
            try
            {
                string SQL = string.Format(@"select top 1 * from f_Parentid(@devicename) order by level");
                SqlParameter para = new SqlParameter("@devicename", devicename);
                //DataTable dt = SqlHelper.Query(SQL, para).Tables[0];
                //return dt.Rows.Count > 0 ? dt.Rows[0]["cname"].ToString() : "";
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
