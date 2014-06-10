using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using System.Configuration;
using NTS.WEB.Common;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.DAL;

namespace NTS.WEB.BLL
{
    public class ComplexReport
    {
        private readonly IComplexReport _iDal = DataSwitchConfig.CreateComplexReport();

        public DataTable GetItemCode(string strWhere)
        {
            return _iDal.GetItemCode(strWhere).Tables[0];
        }

        /// <summary>
        /// 导出所有能源信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportAllEnergy(string starttime, string endtime, string objectid, string timeunit, string SavePath)
        {
            try
            {
                ReportQueryModelNew model = new ReportQueryModelNew();
                model.startime = DateTime.Parse(starttime);
                model.endtime = DateTime.Parse(endtime);
                model.objectid = objectid;
                model.timeunit = (ReportStyleNew)(int.Parse(timeunit));
                model.objecttype = 0;

                string strPath = SavePath; // AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                string strTemPath = AppDomain.CurrentDomain.BaseDirectory + "ExcelTemplate\\区域能耗查询.xls";


                #region 第2种方法

                DataTable dtItemCode = GetItemCode(" and itemcodenumber in ('01000','02000','03000','04000')");

                DataTable dtlist = GetItemcodeDataAll(model, dtItemCode);

                string name = GetBaseLayerObjectName(model.objectid);

                #endregion

                if (dtlist != null)
                {
                    string temp_path = strPath; //AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                    if (!Directory.Exists(temp_path))
                    {
                        Directory.CreateDirectory(temp_path);
                        string[] files = Directory.GetFiles(temp_path);
                        foreach (string fn in files)
                        {
                            File.Delete(temp_path + fn);
                        }
                    }
                    string save_path = DateTime.Now.Ticks + ".xls";

                    string templatePath = strTemPath; // AppDomain.CurrentDomain.BaseDirectory + "template\\区域能耗查询.xls";

                    string datetime = model.startime.ToString("yyyy-MM-dd") + "~" + (model.timeunit == ReportStyleNew.DayStyle ? model.endtime.ToString("yyyy-MM-dd HH:59:59") : model.endtime.ToString("yyyy-MM-dd"));
                    TemplateParam param = new TemplateParam(name, new CellParam(0, 0), datetime, new CellParam(3, 0), false, new CellParam(4, 0));
                    //param.DataColumn = new[] { 0, 3, 6, 7, 8, 9};
                    param.DataColumn = new[] { 0, 3, 1, 6, 7, 8, 9, 10 };
                    dtlist.TableName = "能耗查询统计";
                    string strDownFile = temp_path + save_path;
                    ExportHelper.ExportExcel(dtlist, strDownFile, strTemPath, param);

                    return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                    //return "导出失败：当前无任何数据!";
                }
            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败:没有数据\"}";
            }

        }


        /// <summary>
        /// 导出所有能源信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportItemEnergy(string starttime, string endtime, string objectid, string timeunit, string counttype, string SavePath)
        {
            try
            {
                Itemcode itemcodeBll = new Itemcode();

                ReportQueryModelNew model = new ReportQueryModelNew();
                model.startime = DateTime.Parse(starttime);
                model.endtime = DateTime.Parse(endtime);
                model.objectid = objectid;
                model.timeunit = (ReportStyleNew)(int.Parse(timeunit));
                model.objecttype = 0;
                model.itemcode = counttype;

                string strPath = SavePath; // AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                string strTemPath = AppDomain.CurrentDomain.BaseDirectory + "ExcelTemplate\\项目能耗查询.xls";

                IList<Model.Itemcode> itemcode = itemcodeBll.GetItemcodeList(
                    " and itemcodenumber = '" + counttype + "'", "");
                string strUnit = "mch";
                if(itemcode.Count>0)
                {
                    strUnit = itemcode[0].Unit;
                }

                #region 第2种方法
                DataTable dtlist = GetItemcodeData(model);

                string name = GetBaseLayerObjectName(model.objectid);


                #endregion

                if (dtlist != null)
                {
                    string temp_path = strPath; //AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
                    if (!Directory.Exists(temp_path))
                    {
                        Directory.CreateDirectory(temp_path);
                        string[] files = Directory.GetFiles(temp_path);
                        foreach (string fn in files)
                        {
                            File.Delete(temp_path + fn);
                        }
                    }
                    string save_path = DateTime.Now.Ticks + ".xls";

                    string templatePath = strTemPath; // AppDomain.CurrentDomain.BaseDirectory + "template\\区域能耗查询.xls";

                    string datetime = model.startime.ToString("yyyy-MM-dd") + "~" + (model.timeunit == ReportStyleNew.DayStyle ? model.endtime.ToString("yyyy-MM-dd HH:59:59") : model.endtime.ToString("yyyy-MM-dd"));
                    TemplateParam param = new TemplateParam(name, new CellParam(0, 0), datetime, new CellParam(3, 0), false, new CellParam(4, 0));
                    //param.DataColumn = new[] { 0, 3, 6, 7, 8, 9};
                    param.DataColumn = new[] { 0, 1, 3, 6, 7 };
            
                    param.ItemUnit = "能耗值:(" + strUnit + "）";
                    param.ItemUnitCell = new CellParam(4, 4);
                    dtlist.TableName = "能耗查询统计";

                    string strDownFile = temp_path + save_path;

                    //return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                    ExportHelper.ExportExcel(dtlist, strDownFile, strTemPath, param);

                    //return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";

                    return "{\"status\":\"success\",\"msg\":\"" + "/temp_file/" + save_path + "\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"msg\":\"导出失败：当前无任何数据\"}";
                }
            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"导出失败:没有数据！\"}";
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetReportType()
        {
            try
            {
                List<ReportTypeClass> lstReType = GetReportTypeList();
                StringBuilder sb = new StringBuilder();
                if (lstReType.Count > 0)
                {
                    sb.Append("{");
                    sb.Append("\"ActionInfo\": [{");
                    sb.Append("\"Success\":true,");
                    sb.Append("\"ExceptionMsg\":\"\"");
                    sb.Append(" }],");
                    sb.Append("\"Data\":[");
                    for (int i = 0; i < lstReType.Count; i++)
                    {
                        sb.Append("{");
                        sb.Append("\"Reporttype\":" + lstReType[i].ReportType + "");
                        sb.Append(",\"Reportname\":" + "\"" + lstReType[i].ReportName + "\"");
                        sb.Append(",\"Counttype\":" + "\"" + lstReType[i].CountType + "\"");
                        if (i < lstReType.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                    sb.Append("]");
                    sb.Append("}");
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 根据ReportType.xml获取告警类型列表
        /// </summary>
        /// <returns></returns>
        public List<ReportTypeClass> GetReportTypeList()
        {

            if (ConfigurationManager.AppSettings["ReportTypeXmlPath"] == null)
            {
                throw new Exception("报表类型XML文件不存在！");
            }

            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            var xmlPath = Path.Combine(AppPath, ConfigurationManager.AppSettings["ReportTypeXmlPath"]);
            if (File.Exists(xmlPath))
            {
                XElement root = XElement.Load(xmlPath);
                try
                {
                    var items = from el in root.Elements("Item")
                                select new ReportTypeClass()
                                {
                                    ReportType = el.Attribute("reporttype").Value,
                                    ReportName = el.Attribute("reportname").Value,
                                    CountType = el.Attribute("counttype").Value
                                };
                    return items.ToList();
                }
                catch (NullReferenceException nex)
                {
                    throw new Exception("报表类型XML属性缺失");
                }
                catch (Exception ex)
                {
                    throw new Exception("报表类型XML属性配置错误");
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 根据对象的id获取对象的名称
        /// </summary>
        /// <param name="objectid"></param>
        /// <returns></returns>
        public string GetBaseLayerObjectName(string objectid)
        {

            DataTable dtTable = _iDal.GetList(" and layerobjectid=" + objectid, "layerobjectid");
            if (dtTable.Rows.Count > 0)
            {
                return dtTable.Rows[0]["layerobjectname"].ToString();
            }
            return string.Empty;
        }


        public DataTable GetAreaList()
        {
            DataSet ds = _iDal.GetAreaList();
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


        #region ReportBll


        public ReportQueryModelNew getRequestModel()
        {
            string Faild = "Error";
            string starttime = Strings.getRequestString(System.Web.HttpContext.Current.Request["starttime"], RequestDataType.DataTime);
            string endtimestr = Strings.getRequestString(System.Web.HttpContext.Current.Request["endtime"], RequestDataType.DataString);
            if (endtimestr.Contains("24:00:00"))
            {
                endtimestr = endtimestr.Replace("24:00:00", "23:59:59");
                DateTime dts = DateTime.Parse(endtimestr);
                dts = dts.AddSeconds(1);
                endtimestr = dts.ToShortDateString();
            }

            string endtime = Strings.getRequestString(endtimestr, RequestDataType.DataTime);
            string itemcode = Strings.getRequestString(System.Web.HttpContext.Current.Request["itemcode"], RequestDataType.DataInt);
            string objectid = Strings.getRequestString(System.Web.HttpContext.Current.Request["objectid"], RequestDataType.DataString);
            string unit = Strings.getRequestString(System.Web.HttpContext.Current.Request["unit"], RequestDataType.DataInt);
            string objecttype = Strings.getRequestString(System.Web.HttpContext.Current.Request["objecttype"], RequestDataType.DataInt);
            if (starttime.Equals(Faild) || endtime.Equals(Faild) || itemcode.Equals(Faild) || objectid.Equals(Faild) || unit.Equals(Faild))
            {
                return null;
                //return "{msg:'参数传递的格式不正确。'}";
            }
            ReportQueryModelNew model = new ReportQueryModelNew();
            model.startime = DateTime.Parse(starttime);
            model.objectid = objectid;
            model.endtime = DateTime.Parse(endtime);
            model.itemcode = itemcode;
            model.timeunit = (ReportStyleNew)int.Parse(unit);
            model.objecttype = Convert.ToInt32(objecttype);
            return model;
        }

        public DataTable GetItemcodeDataAll(ReportQueryModelNew model, DataTable dtItemCode)
        {
            int objectid;
            float total1 = 0;
            float total2 = 0;
            float total3 = 0;
            float total4 = 0;


            // 电
            float decITEMCOAL1 = 0;
            // 水
            float decITEMCOAL2 = 0;
            // 气
            float decITEMCOAL3 = 0;
            // 煤
            float decITEMCOAL4 = 0;

            if (dtItemCode.Rows[0]["ITEMCOAL"] != DBNull.Value)
            {
                decITEMCOAL1 = float.Parse(dtItemCode.Rows[0]["ITEMCOAL"].ToString());
            }
            if (dtItemCode.Rows[1]["ITEMCOAL"] != DBNull.Value)
            {
                decITEMCOAL2 = float.Parse(dtItemCode.Rows[1]["ITEMCOAL"].ToString());
            }
            if (dtItemCode.Rows[2]["ITEMCOAL"] != DBNull.Value)
            {
                decITEMCOAL3 = float.Parse(dtItemCode.Rows[2]["ITEMCOAL"].ToString());
            }
            if (dtItemCode.Rows[3]["ITEMCOAL"] != DBNull.Value)
            {
                decITEMCOAL4 = float.Parse(dtItemCode.Rows[3]["ITEMCOAL"].ToString());
            }

            float totalzhm = 0;
            DataTable dt = CreateTable.CreateBaseDataTableAll();
            //if (!new CreateTable().ObjectIsConfigAll(model, out objectid))
            //{
            //    return null;
            //}
            try
            {
                string objectname = Getobjectname(model.objectid);
                string itemcodename = Getitemcodename(model.itemcode);
                ////List<int> lstCountId = new List<int>();
                ////for (int jCount = 0; jCount < dtItemCode.Rows.Count; jCount++)
                ////{
                ////    int countId = GetObjectCountId(model.objectid, model.objecttype, CreateTable.ConvertBaseCountType(model.timeunit), int.Parse(dtItemCode.Rows[jCount]["ItemCodeId"].ToString()));
                ////    lstCountId.Add(countId);
                ////}

                BaseDataModelNew DataModel = new BaseDataModelNew();
                DataModel.Starttime = model.startime;
                DataModel.Endtime = model.endtime;
                DataModel.ObjectId = int.Parse(model.objectid);
                DataModel.Unit = CreateTable.ConvertBaseCountType(model.timeunit).ToString();

                #region 处理加工

                DataTable[] dtdata = DataCommon.GetBaseDataAll(DataModel, dtItemCode);


                #endregion

                if (model.timeunit != ReportStyleNew.YearStyle)
                {
                    #region 日月
                    for (DateTime i = model.startime; i <= model.endtime; i = (model.timeunit == ReportStyleNew.DayStyle) ? i.AddHours(1) : i.AddDays(1))
                    {
                        //if (model.timeunit == ReportStyleNew.DayStyle && i.Hour==0)
                        //{
                        //    continue;

                        //}

                        DataRow dr = dt.NewRow();
                        dr[1] = CreateTable.ReportFormatDate(i.ToString(), model.timeunit);
                        dr[2] = model.objectid;
                        dr[3] = objectname;
                        dr[4] = i;
                        dr[5] = i.AddDays(1);
                        //dr[6] = itemcodename;
                        //dr[7] = 0;
                        dr[6] = 0;
                        dr[7] = 0;
                        dr[8] = 0;
                        dr[9] = 0;
                        dr[10] = 0;

                        DataTable dt1 = dtdata[0];
                        DataTable dt2 = dtdata[1];
                        DataTable dt3 = dtdata[2];
                        DataTable dt4 = dtdata[3];


                        #region  电
                        if (dt1.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt1.Rows[j]["Starttime"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.ReportFormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[6] = Math.Round(float.Parse(dt1.Rows[j]["CountValue"].ToString()), 2);
                                        total1 += float.Parse(dr[6].ToString());
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt1.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.ReportFormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[6] = Math.Round(float.Parse(dt1.Rows[j]["CountValue"].ToString()), 2);
                                        total1 += float.Parse(dr[6].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion
                        #region  水
                        if (dt2.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt2.Rows[j]["Starttime"]).Hour == i.Hour)
                                    {
                                        dr[7] = Math.Round(float.Parse(dt2.Rows[j]["CountValue"].ToString()), 2);
                                        total2 += float.Parse(dr[7].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt2.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.ReportFormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dt2.Rows[j]["CountValue"].ToString()), 2);
                                        total2 += float.Parse(dr[7].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion
                        #region  气
                        if (dt3.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt3.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt3.Rows[j]["Starttime"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[8] = Math.Round(float.Parse(dt3.Rows[j]["CountValue"].ToString()), 2);
                                        total3 += float.Parse(dr[8].ToString());

                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt3.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[8] = Math.Round(float.Parse(dt3.Rows[j]["CountValue"].ToString()), 2);
                                        total3 += float.Parse(dr[8].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion
                        #region  供暖
                        if (dt4.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt4.Rows[j]["Starttime"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[9] = Math.Round(float.Parse(dt4.Rows[j]["CountValue"].ToString()), 2);
                                        total4 += float.Parse(dr[9].ToString());

                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt4.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[9] = Math.Round(float.Parse(dt4.Rows[j]["CountValue"].ToString()), 2);
                                        total4 += float.Parse(dr[9].ToString());

                                        break;
                                    }
                                }


                            }

                        }

                        dr[10] = float.Parse(dr[6].ToString()) * decITEMCOAL1 +
                              float.Parse(dr[7].ToString()) * decITEMCOAL2 +
                              float.Parse(dr[8].ToString()) * decITEMCOAL3 +
                              float.Parse(dr[9].ToString()) * decITEMCOAL4;

                        #endregion
                        dt.Rows.Add(dr);

                    }
                    #endregion
                }
                else
                {
                    #region 年

                    for (DateTime i = model.startime; i <= model.endtime; i = i.AddMonths(1))
                    {
                        //if (model.timeunit == ReportStyleNew.DayStyle && i.Hour==0)
                        //{
                        //    continue;

                        //}

                        DataRow dr = dt.NewRow();
                        dr[1] = CreateTable.ReportFormatDate(i.ToString(), model.timeunit);
                        dr[2] = model.objectid;
                        dr[3] = objectname;
                        dr[4] = i;
                        dr[5] = i.AddDays(1);
                        //dr[6] = itemcodename;
                        //dr[7] = 0;
                        dr[6] = 0;
                        dr[7] = 0;
                        dr[8] = 0;
                        dr[9] = 0;

                        DataTable dt1 = dtdata[0];
                        DataTable dt2 = dtdata[1];
                        DataTable dt3 = dtdata[2];
                        DataTable dt4 = dtdata[3];


                        #region  电
                        if (dt1.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt1.Rows[j]["ENDTIME"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[6] = Math.Round(float.Parse(dt1.Rows[j]["CountValue"].ToString()), 2);
                                        total1 += float.Parse(dr[6].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt1.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[6] = Math.Round(float.Parse(dt1.Rows[j]["CountValue"].ToString()), 2);
                                        total1 += float.Parse(dr[6].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion
                        #region  水
                        if (dt2.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt2.Rows[j]["ENDTIME"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dt2.Rows[j]["CountValue"].ToString()), 2);
                                        total2 += float.Parse(dr[7].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt2.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dt2.Rows[j]["CountValue"].ToString()), 2);
                                        total2 += float.Parse(dr[7].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion
                        #region  气
                        if (dt3.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt3.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt3.Rows[j]["ENDTIME"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[8] = Math.Round(float.Parse(dt3.Rows[j]["CountValue"].ToString()), 2);
                                        total3 += float.Parse(dr[8].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt3.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[8] = Math.Round(float.Parse(dt3.Rows[j]["CountValue"].ToString()), 2);
                                        total3 += float.Parse(dr[8].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion
                        #region  供暖
                        if (dt4.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dt4.Rows[j]["ENDTIME"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[9] = Math.Round(float.Parse(dt4.Rows[j]["CountValue"].ToString()), 2);
                                        total4 += float.Parse(dr[9].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dt4.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[9] = Math.Round(float.Parse(dt4.Rows[j]["CountValue"].ToString()), 2);
                                        total4 += float.Parse(dr[9].ToString());
                                        break;
                                    }
                                }


                            }

                        }

                        #endregion

                        dr[10] = float.Parse(dr[6].ToString()) * decITEMCOAL1 +
                                  float.Parse(dr[7].ToString()) * decITEMCOAL2 +
                                  float.Parse(dr[8].ToString()) * decITEMCOAL3 +
                                  float.Parse(dr[9].ToString()) * decITEMCOAL4;

                        dt.Rows.Add(dr);

                    }
                    #endregion
                }





                #region
                DataRow totaldr = dt.NewRow();
                totaldr[1] = CreateTable.ReportFormatDate(model.startime.ToString(), model.timeunit) + "-" +
                             CreateTable.ReportFormatDate(model.endtime.ToString(),
                                                     model.timeunit);
                totaldr[2] = model.objectid;
                totaldr[3] = objectname;
                totaldr[4] = "-";
                totaldr[5] = "-";
                //totaldr[6] = itemcodename;
                totaldr[6] = Math.Round(total1, 2).ToString();
                totaldr[7] = Math.Round(total2, 2).ToString();
                totaldr[8] = Math.Round(total3, 2).ToString();
                totaldr[9] = Math.Round(total4, 2).ToString();
                // 加转换煤



                totaldr[10] = Math.Round((total1 * decITEMCOAL1 + total2 * decITEMCOAL2 + total3 * decITEMCOAL3 + total4 * decITEMCOAL4), 2).ToString();
                dt.Rows.Add(totaldr);



                #endregion

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return null;
            }
        }

        public DataTable GetItemcodeData(ReportQueryModelNew model)
        {
            int objectid;
            float total = 0;
            DataTable dt = CreateTable.CreateBaseDataTable();
            //if (!new CreateTable().ObjectIsConfig(model, out objectid))
            //{
            //    return null;
            //}
            try
            {
                string objectname = Getobjectname(model.objectid);
                string itemcodename = Getitemcodename(model.itemcode);

                BaseDataModelNew DataModel = new BaseDataModelNew();
                DataModel.Starttime = model.startime;
                DataModel.Endtime = model.endtime;
                DataModel.ObjectId = int.Parse(model.objectid);
                DataModel.ItemCode = model.itemcode;
                DataModel.Unit = CreateTable.ConvertBaseCountType(model.timeunit).ToString();
                // Datamodel.timeunit = CreateTable.ConvertBaseCountType(model.timeunit).ToString(); // 
                #region 处理加工

                DataTable dtdata = DataCommon.GetBaseDataItem(DataModel);



                #region 非年
                if (model.timeunit != ReportStyleNew.YearStyle) // 非年
                {
                    //if (model.timeunit == ReportStyleNew.DayStyle)
                    //{
                    //    model.startime = model.startime.AddHours(1);
                    //}
                    for (DateTime i = model.startime; i <= model.endtime; i = (model.timeunit == ReportStyleNew.DayStyle) ? i.AddHours(1) : i.AddDays(1))
                    {
                        //if (model.timeunit == ReportStyleNew.DayStyle && i.Hour==0)
                        //{
                        //    continue;

                        //}

                        DataRow dr = dt.NewRow();
                        dr[1] = CreateTable.ReportFormatDate(i.ToString(), model.timeunit);
                        dr[2] = model.objectid;
                        dr[3] = objectname;
                        dr[4] = i;
                        dr[5] = i.AddDays(1);
                        dr[6] = itemcodename;
                        dr[7] = 0;


                        if (dtdata.Rows.Count > 0)
                        {

                            for (int j = 0; j < dtdata.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dtdata.Rows[j]["Starttime"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dtdata.Rows[j]["CountValue"].ToString()), 2);
                                        total += float.Parse(dr[7].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dtdata.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.FormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dtdata.Rows[j]["CountValue"].ToString()), 2);
                                        total += float.Parse(dr[7].ToString());
                                        break;
                                    }
                                }


                            }

                        }
                        dt.Rows.Add(dr);

                    }
                }
                #endregion

                #region 年
                else // 年
                {
                    for (DateTime i = model.startime; i <= model.endtime; i = i.AddMonths(1))
                    {
                        DataRow dr = dt.NewRow();
                        dr[1] = CreateTable.ReportFormatDate(i.ToString(), model.timeunit);
                        dr[2] = model.objectid;
                        dr[3] = objectname;
                        dr[4] = i;
                        dr[5] = i.AddDays(1);
                        dr[6] = itemcodename;
                        dr[7] = 0;


                        if (dtdata.Rows.Count > 0)
                        {

                            for (int j = 0; j < dtdata.Rows.Count; j++)
                            {
                                if (model.timeunit == ReportStyleNew.DayStyle)
                                {
                                    if (Convert.ToDateTime(dtdata.Rows[j]["Starttime"]).Hour == i.Hour)
                                    {
                                        //dr[1] = CreateTable.ReportFormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dtdata.Rows[j]["CountValue"].ToString()), 2);
                                        total += float.Parse(dr[7].ToString());
                                        break;
                                    }

                                }
                                else
                                {
                                    if (Convert.ToDateTime(dtdata.Rows[j]["Starttime"]).Date == i.Date)
                                    {
                                        //dr[1] = CreateTable.ReportFormatDate(dtdata.Rows[j]["Starttime"].ToString(), model.timeunit);
                                        dr[7] = Math.Round(float.Parse(dtdata.Rows[j]["CountValue"].ToString()), 2);
                                        total += float.Parse(dr[7].ToString());
                                        break;
                                    }
                                }


                            }

                        }
                        dt.Rows.Add(dr);

                    }
                }

                #endregion
                DataRow totaldr = dt.NewRow();
                totaldr[1] = CreateTable.ReportFormatDate(model.startime.ToString(), model.timeunit) + "-" +
                             CreateTable.ReportFormatDate(model.endtime.ToString(),
                                                     model.timeunit);
                totaldr[2] = model.objectid;
                totaldr[3] = objectname;
                totaldr[4] = "-";
                totaldr[5] = "-";
                totaldr[6] = itemcodename;
                totaldr[7] = Math.Round(total, 2).ToString();
                dt.Rows.Add(totaldr);



                #endregion

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return null;
            }
        }

        #region 基础方法
        /// <summary>
        /// 获取对象的名称
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public string Getobjectname(string oid)
        {
            DataTable Dtobject = _iDal.GetList(" and layerobjectid= " + oid, "layerobjectid");
            if (Dtobject.Rows.Count > 0)
            {
                return Dtobject.Rows[0]["layerobjectname"].ToString();
            }
            return string.Empty;
        }


        //public int GetObjectCountId(string objectId, int objectType, int unit, int itemcode)
        //{
        //    IList<DbParameters> parameter = new List<DbParameters>();
        //    parameter.Add(new DbParameters("@itemcodeid", DbType.Int32, itemcode));
        //    parameter.Add(new DbParameters("@objectid", DbType.String, objectId));
        //    parameter.Add(new DbParameters("@objecttype", DbType.Int32, objectType));
        //    parameter.Add(new DbParameters("@unit", DbType.Int32, unit));

        //    DataTable DtItemcode = new ObjectConfig().GetList(" itemcodeid=@itemcodeid and objectid=@objectid and objecttype=@objecttype and unit=@unit", "itemcodeid", parameter);
        //    if (DtItemcode.Rows.Count > 0)
        //    {
        //        return int.Parse(DtItemcode.Rows[0]["countid"].ToString());
        //    }
        //    return 0;

        //}

        public string Getitemcodename(string oid)
        {
            DataTable DtItemcode = _iDal.GetListItemCode(" and itemcodenumber='" + oid+"'", "itemcodenumber");
            if (DtItemcode.Rows.Count > 0)
            {
                return DtItemcode.Rows[0]["itemcodename"].ToString();
            }
            return string.Empty;
        }

        public DataTable GetItemcodeList(int parentid)
        {
            SqlParameter para = new SqlParameter("@parentid", DbType.Int32);
            para.Value = parentid;

            DataTable DtItemcode = _iDal.GetListItemCode(" parentid=" + parentid.ToString(), "itemcodeid");
            return DtItemcode;
        }

        /// <summary>
        /// 根据区域和项目代码获取是否含有数据
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public int GetCountItemCodeAreaId(string itemcode, string areaid, int classid)
        {
            return _iDal.GetCountItemCodeAreaId(itemcode, areaid, classid);
        }



        #endregion

        #endregion
    }
}


public class ReportTypeClass
{
    public string ReportType { get; set; }
    public string ReportName { get; set; }
    public string CountType { get; set; }
}
