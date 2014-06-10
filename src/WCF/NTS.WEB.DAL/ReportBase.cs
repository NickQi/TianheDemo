using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using NTS.WEB.Base.Data;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class ReportBase : IReportBase
    {
       //// static int AreaExtLevel = int.Parse(ConfigurationManager.AppSettings["AreaExtLevel"]);  
       // public DataTable GetItemcodeData(ReportQueryModel model)
       // {    
       //     int objectid;
       //     decimal total = 0;
       //     DataTable dt = TableViews.BaseTable.CreateBaseDataTable();
       //     if (!new CommDataTool().ObjectIsConfig(model,out objectid))
       //     {
       //         return null;
       //     }
       //     try
       //     {
       //         string objectname = Getobjectname(model.Objectid);
       //         string itemcodename = Getitemcodename(model.Itemcode);
       //         BaseDataModel DataModel = new BaseDataModel();
       //         DataModel.Starttime = model.Startime;
       //         DataModel.Endtime = model.Endtime;
       //         DataModel.ObjectId = objectid;
       //         DataModel.ReportClass = model.Unit;
       //         #region 处理加工

       //         //DataTable dtdata = DataComm.GetBaseData(DataModel);
       //         DataTable dtdata = BigDataComm.GetBaseData(DataModel);
                
       //         if (dtdata.Rows.Count > 0)
       //         {
       //             for (int i = 0; i < dtdata.Rows.Count; i++)
       //             {
       //                 DataRow dr = dt.NewRow();
       //                 dr[1] = CommDataTool.FormatDate(dtdata.Rows[i]["Starttime"].ToString(), model.Unit);
       //                 dr[2] = model.Objectid;
       //                 dr[3] = objectname;
       //                 dr[4] = dtdata.Rows[i]["Starttime"].ToString();
       //                 dr[5] = dtdata.Rows[i]["endtime"].ToString();
       //                 dr[6] = itemcodename;
       //                 dr[7] = decimal.Round(decimal.Parse(dtdata.Rows[i]["CountValue"].ToString()), 2);
       //                 total += decimal.Parse(dr[7].ToString());
       //                 dt.Rows.Add(dr);
       //             }
       //             DataRow totaldr = dt.NewRow();
       //             totaldr[1] = CommDataTool.FormatDate(dtdata.Rows[0]["Starttime"].ToString(), model.Unit) + "-" +
       //                          CommDataTool.FormatDate(dtdata.Rows[dtdata.Rows.Count - 1]["Starttime"].ToString(),
       //                                                  model.Unit);
       //             totaldr[2] = model.Objectid;
       //             totaldr[3] = objectname;
       //             totaldr[4] = "-";
       //             totaldr[5] = "-";
       //             totaldr[6] = itemcodename;
       //             totaldr[7] = decimal.Round(total, 2).ToString();
       //             dt.Rows.Add(totaldr);
       //         }

       //         #endregion

       //         return dt;
       //     }
       //     catch (Exception e)
       //     {
       //         return null;
       //     }
       // }


       // /// <summary>
       // /// 根据分类分项集合，对象集合，以及查询量获取所有的数据
       // /// </summary>
       // /// <param name="ItemcodeArr">分类分项集合</param>
       // /// <param name="ObjectArr">对象集合</param>
       // /// <param name="model">查询量实体类</param>
       // /// <returns></returns>
       // public DataTable GetManayObjectAndItemcodeData(string[] ItemcodeArr, string[] ObjectArr, ReportQueryModel model)
       // {
       //     StringBuilder countidArr = new StringBuilder();
       //     countidArr = new CommDataTool().GetTrueCountID(ItemcodeArr, ObjectArr, model);
       //     if (string.IsNullOrEmpty(countidArr.ToString()))
       //     {
       //         return null;
       //     }
       //     try
       //     {
       //         BaseMDataModel MDataModel = new BaseMDataModel();
       //         MDataModel.Starttime = model.Startime;
       //         MDataModel.Endtime = model.Endtime;
       //         MDataModel.ObjectId = countidArr.ToString().Substring(1);

       //         #region 处理加工

       //        // DataTable dtdata = DataComm.GetBOMBaseData(MDataModel);
       //         DataTable dtdata = BigDataComm.GetBomBaseData(MDataModel);
       //         return dtdata;

       //         #endregion
       //     }
       //     catch (Exception)
       //     {
       //         return null;
       //     }

       // }

       // /// <summary>
       // /// 
       // /// </summary>
       // /// <param name="model"></param>
       // /// <returns></returns>

       // public DataTable GetItemcodeDataOnlyTotal(ReportQueryModel model)
       // {
       //     int objectid;
       //     decimal total = 0;
       //     DataTable dt = TableViews.BaseTable.CreateBaseDataTable();
       //     if (!new CommDataTool().ObjectIsConfig(model, out objectid))
       //     {
       //         return null;
       //     }
       //     try
       //     {
       //         string objectname = Getobjectname(model.Objectid);
       //         string itemcodename = Getitemcodename(model.Itemcode);
       //         BaseDataModel DataModel = new BaseDataModel();
       //         DataModel.Starttime = model.Startime;
       //         DataModel.Endtime = model.Endtime;
       //         DataModel.ObjectId = objectid;
       //         DataModel.ReportClass = model.Unit;
       //         #region 处理加工

       //         //DataTable dtdata = DataComm.GetBaseData(DataModel);
       //         DataTable dtdata = BigDataComm.GetBaseData(DataModel);
                
       //         if (dtdata.Rows.Count > 0)
       //         {
       //             DataRow totaldr = dt.NewRow();
       //             decimal _total = 0;
       //             foreach (DataRow dr in dtdata.Rows)
       //             {
       //                // _total += decimal.Parse(dr[0].ToString()); 大数据量时修改
       //                 _total += decimal.Parse(dr["CountValue"].ToString());
       //             }
       //             totaldr[7] = decimal.Round(_total,2).ToString();
       //             dt.Rows.Add(totaldr);
       //         }

       //         #endregion

       //         return dt;
       //     }
       //     catch (Exception e)
       //     {
       //         return null;
       //     }
       // }

       // /// <summary>
       // /// 获取子分项的能耗值
       // /// </summary>
       // /// <param name="model"></param>
       // /// <param name="recordCount"></param>
       // /// <returns></returns>
       // public DataTable GetSmallItemcodeData(ReportQueryModel model, out int recordCount)
       // {
       //     DataTable ChildTableAll = new DataTable();
       //     DataTable TempTable = new DataTable();
       //     recordCount = 0;
       //     DataTable itemdt =
       //         GetItemcodeList(model.Itemcode);
       //     if (itemdt.Rows.Count > 0)
       //     {
       //         DataTable dtnew = TableViews.BaseTable.CreateSmallItemCodeDataTable(itemdt);

       //         for (int i = 0; i < itemdt.Rows.Count; i++)
       //         {
       //             ReportQueryModel newmodel = new ReportQueryModel();
       //             newmodel.Endtime = model.Endtime;
       //             newmodel.Startime = model.Startime;
       //             newmodel.Objectid = model.Objectid;
       //             newmodel.Itemcode = int.Parse(itemdt.Rows[i]["itemcodeid"].ToString());
       //             newmodel.Unit = model.Unit;
       //             DataTable ChildTable = GetItemcodeData(newmodel);
       //             if (ChildTable == null) { return null; }
       //             if (i == 0)
       //             {
       //                 recordCount = ChildTable.Rows.Count;
       //                 ChildTableAll = ChildTable.Copy();
       //                 TempTable = ChildTable;
       //             }
       //             else
       //             {
       //                 // 获取总共的记录数
       //                 ChildTableAll = Common.DataTool.UniteDataTable(ChildTableAll, ChildTable);
       //             }
       //         }

       //         for (int j = 0; j < TempTable.Rows.Count; j++)
       //         {
       //             DataRow dr = dtnew.NewRow();
       //             dr[1] = TempTable.Rows[j][1].ToString();//Date
       //             dr[2] = TempTable.Rows[j][2].ToString();//ObjectID
       //             dr[3] = TempTable.Rows[j][3].ToString();//ObjectName
       //             dr[4] = TempTable.Rows[j][4].ToString();//Starttime
       //             dr[5] = TempTable.Rows[j][5].ToString();//Endtime
       //             for (int k = 0; k < itemdt.Rows.Count; k++)
       //             {
       //                 string date = TempTable.Rows[j][1].ToString();
       //                 string ItemCode = itemdt.Rows[k]["itemcodeid"].ToString();
       //                 DataRow[] drs = ChildTableAll.Select(string.Format(" Date='{0}' and ItemCode='{1}'", date, ItemCode));
       //                 dr[k + 6] = drs.Length > 0 ? drs[0]["EneryValue"].ToString() : "0.00";
       //             }
                    
       //             dtnew.Rows.Add(dr);
       //         }
       //         return dtnew;

       //     }
       //     return null;
       // }


       // #region 基础方法
       // /// <summary>
       // /// 获取对象的名称
       // /// </summary>
       // /// <param name="oid"></param>
       // /// <returns></returns>
       // public string Getobjectname(string oid)
       // {
       //     SqlParameter[] parameter = {
       //                                     new SqlParameter("@layerobjectid", SqlDbType.Int)
       //                                 };
       //     parameter[0].Value = int.Parse(oid);

       //     DataTable Dtobject = new BaseLayerObject().GetList(" layerobjectid=@layerobjectid ", "layerobjectid", parameter);
       //     if (Dtobject.Rows.Count > 0)
       //     {
       //         return Dtobject.Rows[0]["layerobjectname"].ToString();
       //     }
       //     return string.Empty;
       // }

       // public string Getitemcodename(int oid)
       // {
       //     SqlParameter[] parameter = {
       //                                     new SqlParameter("@itemcodeid", SqlDbType.Int)
       //                                 };
       //     parameter[0].Value = oid;

       //     DataTable DtItemcode = new Itemcode().GetList(" itemcodeid=@itemcodeid ", "itemcodeid", parameter);
       //     if (DtItemcode.Rows.Count > 0)
       //     {
       //         return DtItemcode.Rows[0]["itemcodename"].ToString();
       //     }
       //     return string.Empty;
       // }

       // public DataTable GetItemcodeList(int parentid)
       // {
       //     SqlParameter[] parameter = {
       //                                     new SqlParameter("@parentid", SqlDbType.Int)
       //                                 };
       //     parameter[0].Value = parentid;

       //     DataTable DtItemcode = new Itemcode().GetList(" parentid=@parentid ", "itemcodeid", parameter);
       //     return DtItemcode;
       // }


       // /// <summary>
       // /// 计算对象属性的单位能耗值
       // /// </summary>
       // /// <param name="values">能耗值</param>
       // /// <param name="classid">属性类型id</param>
       // /// <param name="objectid">对象id</param>
       // /// <returns></returns>
       // private static string GetPerData(string values, int classid, string objectid)
       // {

       //     decimal attribute = new BaseLayerObject().GetBaseLayerObjectCommAttribute(objectid, classid);
       //     if (attribute > 0)
       //     {
       //         return decimal.Round(decimal.Parse(values) / attribute, 2).ToString();
       //     }
       //     return "0.00";

       // }
       // #endregion

       // /// <summary>
       // /// 返回区域属性需要的单位能耗数据集
       // /// </summary>
       // /// <param name="Report"></param>
       // /// <returns></returns>
       // public  DataTable GetPerDataJson(DataTable Report)
       // {
       //     DataTable DtPerReport = TableViews.BaseTable.CreatePerTable(AreaExtLevel);
       //     for (int i = 0; i < Report.Rows.Count; i++)
       //     {
       //         DataRow dr = DtPerReport.NewRow();
       //         for (int k = 1; k < 8;k++ )
       //         {
       //             dr[k] = Report.Rows[i][k].ToString();
       //         }
                
       //         for (int j = 0; j < AreaExtLevel; j++)
       //         {

       //             dr[8 + j] =
       //                 decimal.Round(
       //                     decimal.Parse(GetPerData(Report.Rows[i][7].ToString(), j + 1,
       //                                              Report.Rows[i][2].ToString())), 2).ToString();

       //         }
       //         DtPerReport.Rows.Add(dr);

       //     }
       //     return DtPerReport;
       // }

        public BaseResult GetBaseEneryDataList(BaseQueryModel model)
        {
            return BigDataComm.GetBaseEneryDataList(model);
        }
        public BaseResult GetBaseEneryDataList(BaseQueryModel model, bool IsLiquid)
        {
            return BigDataComm.GetBaseEneryDataList(model,IsLiquid);
        }
        
        
    }
}
