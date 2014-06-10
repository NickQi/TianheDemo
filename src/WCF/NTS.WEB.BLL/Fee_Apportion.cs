using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NTS.WEB.DAL;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using System.Data;

namespace NTS.WEB.BLL
{
    public class Fee_Apportion
    {
        readonly NTS.WEB.ProductInteface.IFee_Apportion _feeApportion = NTS.WEB.ProductInteface.DataSwitchConfig.CreateFee_Apportion();

        private readonly NTS.WEB.ProductInteface.IAccessCommon _accssCommon =
            NTS.WEB.ProductInteface.DataSwitchConfig.CreateAccessCommon();
        public List<TB_Alloction_Config> GetAlloctionConfig(Queryfeeapportion feeApport)
        {
            return _feeApportion.GetAlloctionConfig(feeApport);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultFeeapportion GetFeeApportionData(Queryfeeapportion query)
        {
            try
            {
                ExecuteProcess process = new ExecuteProcess();
                process.ActionName = "";
                process.ActionTime = System.DateTime.Now;
                process.Success = true;
                process.ExceptionMsg = "";

                ResultFeeapportion feeApption = new ResultFeeapportion();
                feeApption.FeeApportionType = "按比例分摊";


                List<TB_Alloction_Config> config = GetAlloctionConfig(query);
                if (config.Count == 0)
                {
                    process.Success = false;
                    process.ExceptionMsg = "此区域费用分摊没有配置";

                    ResultFeeapportion feeApptionReturn = new ResultFeeapportion();
                    feeApption.ActionInfo = process;
                    return feeApption;
                }

                Queryfeeapportion queryLastMonth = new Queryfeeapportion();
                queryLastMonth.ItemCode = query.ItemCode;
                queryLastMonth.ObjType = query.ObjType;
                queryLastMonth.ObjectId = query.ObjectId;
                queryLastMonth.StartTime = query.StartTime.AddMonths(-1);

                List<TB_Alloction_Config> configLast = GetAlloctionConfig(queryLastMonth);

                FeeApportionValClass feeappVal = new FeeApportionValClass();

                FeeApportionChartsClass chartClass = new FeeApportionChartsClass();


                // 数据。
                List<SeriesClass> serialClass = new List<SeriesClass>();

                if (config.Count > 0)
                {
                    DateTime dtBegin = config[0].ALLoction_StartDate;
                    DateTime dtEnd = config[0].ALLoction_EndDate;

                    double feeDai = config[0].ALLoction_Fee;
                    feeappVal.ApportionVal = config[0].ALLoction_Fee;

                    //List<CostQueryModel> refModel = _feeApportion.GetCostQuery(query, dtBegin, dtEnd);

                    EnergyContrast energy = new EnergyContrast();
                    BasicQuery query2 = new BasicQuery();
                    query2.AreaType = query.ObjType;
                    query2.StartTime = config[0].ALLoction_StartDate;
                    query2.EndTime = config[0].ALLoction_EndDate;
                    query2.ObjectNum = query.ObjectId;
                    query2.QueryType = EnergyAnalyseQueryType.Default;
                    query2.Unit = 1;
                    var lineChart = energy.GetSingleItemCodeByObject(query2, query.ItemCode);
                    double befVal = 0;
                    befVal = (double)lineChart.Values.Sum();
                    AccessCommon acess= new AccessCommon();
                    decimal flPrice = acess.GetFeePrice(query.ItemCode);
                    befVal = befVal*(double)flPrice;
                    //foreach (var costQueryModel in refModel)
                    //{
                    //    befVal += costQueryModel.TOTAL_COST;
                    //}
                    feeappVal.BeforeVal = Math.Round(befVal,2);
                    feeappVal.TotalVal = Math.Round((befVal + feeappVal.ApportionVal),2);
                }

                List<SeriesData> sidatas = new List<SeriesData>();
                SeriesData seri = new SeriesData();
                // 统计明细数据

                SeriesData data1 = new SeriesData();
                SeriesData data2 = new SeriesData();
                List<SeriesClass> serClass1 = new List<SeriesClass>();
                List<SeriesClass> serClass2 = new List<SeriesClass>();

                int iCout = 0;
                // 列表
                List<FeeApportionListClass> appList = new List<FeeApportionListClass>();
                foreach (TB_Alloction_Config conf in config)
                {
                    FeeApportionListClass listClass = new FeeApportionListClass();

                    SeriesClass serial1 = new SeriesClass();
                    SeriesClass serial2 = new SeriesClass();
                    int objectId = conf.AreaID;
                    List<Model.BaseLayerObject> objectList;
                    string objectName = "";
                    TB_AreaInfo info = _accssCommon.GetAreaInfo(conf.AreaID);
                    if (info != null)
                    {
                        objectName = info.CName;
                    }

                    //Queryfeeapportion queryItem = new Queryfeeapportion();
                    //queryItem = query;
                    //queryItem.ObjectId = conf.AreaID;
                    //List<CostQueryModel> refModel = _feeApportion.GetCostQuery(queryItem, conf.ALLoction_StartDate, conf.ALLoction_EndDate);
                    double BefFee = 0;
                    //if (refModel.Count > 0)
                    //{
                    //    for (int jCount = 0; jCount < refModel.Count; jCount++)
                    //    {
                    //        BefFee += refModel[jCount].TOTAL_COST;
                    //    }
                    //}
                    EnergyContrast energy = new EnergyContrast();
                    BasicQuery query2 = new BasicQuery();
                    query2.AreaType = query.ObjType;
                    query2.StartTime = config[0].ALLoction_StartDate;
                    query2.EndTime = config[0].ALLoction_EndDate;
                    query2.ObjectNum = conf.AreaID;
                    query2.QueryType = EnergyAnalyseQueryType.Default;
                    query2.Unit = 1;
                    var lineChart2 = energy.GetSingleItemCodeByObject(query2, query.ItemCode);
                    BefFee = (double)lineChart2.Values.Sum();
                    AccessCommon acess = new AccessCommon();
                    decimal flPrice = acess.GetFeePrice(query.ItemCode);
                    BefFee = BefFee * (double)flPrice;

                    serial1.name = objectName;
                    serial2.name = objectName;
                    serial1.y = Math.Round(BefFee);
                    serial2.y = Math.Round((conf.ALLoction_Fee * conf.CfgPercent),2);

                    serClass1.Add(serial1);
                    serClass2.Add(serial2);

                    FeeApportionListClass list1 = new FeeApportionListClass();
                    list1.Id = iCout + 1;
                    list1.Obj = objectName;
                    list1.Tm = query.StartTime.ToString("yyyy-MM");
                    list1.BeforeVal = Math.Round(BefFee,2);
                    list1.ApportionVal = Math.Round((conf.ALLoction_Fee * conf.CfgPercent),2);
                    list1.TotalVal = list1.BeforeVal + list1.ApportionVal;
                    appList.Add(list1);
                    iCout++;

                }
                data1.name = "分摊前费用";
                data1.data = serClass1;

                data2.name = "分摊费用";
                data2.data = serClass2;
                sidatas.Add(data1);
                sidatas.Add(data2);
                chartClass.series = sidatas;
                feeApption.FeeApportionCharts = chartClass;


                if (configLast.Count > 0)
                {
                    DateTime dtBegin2 = configLast[0].ALLoction_StartDate;
                    DateTime dtEnd2 = configLast[0].ALLoction_EndDate;

                    double feeDai = configLast[0].ALLoction_Fee;
                    feeappVal.ApportionValLastMonth = configLast[0].ALLoction_Fee;

                    //List<CostQueryModel> refModel = _feeApportion.GetCostQuery(query, dtBegin2, dtEnd2);
                    double beforeApportionVal = 0;
                    //foreach (var costQueryModel in refModel)
                    //{
                    //    beforeApportionVal += costQueryModel.TOTAL_COST;
                    //}
                    EnergyContrast energy = new EnergyContrast();
                    BasicQuery query2 = new BasicQuery();
                    query2.AreaType = query.ObjType;
                    query2.StartTime = configLast[0].ALLoction_StartDate;
                    query2.EndTime = configLast[0].ALLoction_EndDate;
                    query2.ObjectNum = query.ObjectId;
                    query2.QueryType = EnergyAnalyseQueryType.Default;
                    query2.Unit = 1;
                    var lineChart2 = energy.GetSingleItemCodeByObject(query2, query.ItemCode);
                    beforeApportionVal = (double)lineChart2.Values.Sum();
                    AccessCommon acess = new AccessCommon();
                    decimal flPrice = acess.GetFeePrice(query.ItemCode);
                    beforeApportionVal = Math.Round(beforeApportionVal * (double)flPrice,2);

                    feeappVal.BeforeValLastMonth = beforeApportionVal;
                    feeappVal.TotalValLastMonth = beforeApportionVal + feeappVal.ApportionValLastMonth;

                }
                if (feeappVal.ApportionValLastMonth > 0)
                {
                    double compare = (feeappVal.ApportionVal-feeappVal.ApportionValLastMonth)*100/
                                     feeappVal.ApportionValLastMonth;
                    feeappVal.ApportionValCompare =Math.Round(compare,2).ToString();
                }
                else
                {
                    feeappVal.ApportionValCompare = "-";
                }

                if (feeappVal.BeforeValLastMonth > 0)
                {
                    double compare = (feeappVal.BeforeVal-feeappVal.BeforeValLastMonth) * 100 / feeappVal.BeforeValLastMonth;
                    feeappVal.BeforeValCompare = Math.Round(compare, 2).ToString();
                }
                else
                {
                    feeappVal.BeforeValCompare = "-";
                }

                if (feeappVal.TotalValLastMonth > 0)
                {
                    double compare = (feeappVal.TotalVal-feeappVal.TotalValLastMonth)*100/feeappVal.TotalValLastMonth;
                    feeappVal.TotalValCompare = Math.Round(compare, 2).ToString();
                }
                else
                {
                    feeappVal.TotalValCompare = "-";
                }
                FeeApportionTblClass tblClass = new FeeApportionTblClass();

                tblClass.FeeApportionList = appList;
                feeApption.FeeApportionTbl = tblClass;
                feeApption.FeeApportionVal = feeappVal;
                feeApption.ActionInfo = process;

                return feeApption;
            }
            catch (Exception ex)
            {
                ExecuteProcess process = new ExecuteProcess();
                process.ActionName = "";
                process.ActionTime = System.DateTime.Now;
                process.Success = false;
                process.ExceptionMsg = ex.Message;
                ResultFeeapportion feeApption = new ResultFeeapportion();
                feeApption.ActionInfo = process;
                return feeApption;
            }

        }

        /// <summary>
        /// 数据库获取导出数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FeeApportionListClass> GetFeeApportDataList(Queryfeeapportion query)
        {
            Queryfeeapportion queryLastMonth = query;
            List<TB_Alloction_Config> config = GetAlloctionConfig(query);

            int iCout = 0;
            // 列表
            List<FeeApportionListClass> appList = new List<FeeApportionListClass>();
            foreach (TB_Alloction_Config conf in config)
            {
                FeeApportionListClass listClass = new FeeApportionListClass();

                SeriesClass serial1 = new SeriesClass();
                SeriesClass serial2 = new SeriesClass();
                int objectId = conf.AreaID;
                List<Model.BaseLayerObject> objectList;
                string objectName = "";
                if (query.ObjType == AreaType.Area)
                {
                    TB_AreaInfo info = _accssCommon.GetAreaInfo(conf.AreaID);
                    //objectList = new BLL.BaseLayerObject().GetBaseLayerObjectList(
                    //       string.Format(" and layerobjectid ={0} ", conf.AreaID), " ");

                    if (info != null )
                    {
                        objectName = info.CName;
                    }
                }
                else
                {
                    TB_AreaInfo info = _accssCommon.GetAreaInfo(conf.AreaID);
                    if (info != null)
                    {
                        objectName = info.CName;
                    }
               

                }

                Queryfeeapportion queryItem = new Queryfeeapportion();
                queryItem = query;
                queryItem.ObjectId = conf.AreaID;
                double BefFee = 0;
                //List<CostQueryModel> refModel = _feeApportion.GetCostQuery(queryItem, conf.ALLoction_StartDate, conf.ALLoction_EndDate);
                //double BefFee = 0;
                //if (refModel.Count > 0)
                //{
                //    for (int jCount = 0; jCount < refModel.Count; jCount++)
                //    {
                //        BefFee += refModel[jCount].TOTAL_COST;
                //    }
                //}

                EnergyContrast energy = new EnergyContrast();
                BasicQuery query2 = new BasicQuery();
                query2.AreaType = query.ObjType;
                query2.StartTime = config[0].ALLoction_StartDate;
                query2.EndTime = config[0].ALLoction_EndDate;
                query2.ObjectNum = query.ObjectId;
                query2.QueryType = EnergyAnalyseQueryType.Default;
                query2.Unit = 1;
                var lineChart = energy.GetSingleItemCodeByObject(query2, query.ItemCode);

                BefFee = (double)lineChart.Values.Sum();
                AccessCommon acess = new AccessCommon();
                decimal flPrice = acess.GetFeePrice(query.ItemCode);
                BefFee = BefFee * (double)flPrice;

                FeeApportionListClass list1 = new FeeApportionListClass();
                list1.Id = iCout + 1;
                list1.Obj = objectName;
                list1.Tm = query.StartTime.ToString("yyyy-MM");
                list1.BeforeVal = BefFee;
                list1.ApportionVal = (conf.ALLoction_Fee*conf.CfgPercent)/100;
                list1.TotalVal = list1.BeforeVal + list1.ApportionVal;
                appList.Add(list1);
                iCout++;
            }

            return appList;
        }
    }
}
