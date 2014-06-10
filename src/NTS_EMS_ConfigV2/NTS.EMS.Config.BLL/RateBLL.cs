using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.ProductInteface;
using NTS.EMS.Config.Model.ResultViewFile;
using NTS.EMS.Config.Model.QueryFile;

namespace NTS.EMS.Config.BLL
{
    public class RateBLL
    {
        private readonly IRate _dal = DataSwitchConfig.CreateRateData();

        #region Old
        /// <summary>
        /// 平价模式保存  （新增、修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultRate SaveCommPrice(QueryComm model)
        {
            ResultRate resultModel = new ResultRate();
            Model.TB_Price_CommBill commModel = new TB_Price_CommBill() { Price = model.Price, EnergyCode = model.EnergyCode, ID = model.ID };
            if (model.ID == 0)
            {
                resultModel.IsSucess = _dal.AddCommPrice(commModel);
            }
            else
            {
                resultModel.IsSucess = _dal.UpdateCommPrice(commModel);
            }
            return resultModel;
        }

        /// <summary>
        /// 阶梯模式保存 （新增、修改）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ResultRate SaveRatePrice(List<QueryRate> list)
        {
            ResultRate resultModel = new ResultRate();
            string sql = @" declare @ItemID int,@RuleID int ";
            foreach (QueryRate model in list)
            {
                if (model.ItemID == 0)
                {
                    sql += string.Format(@"insert into TB_BillItem(ItemName,EnergyCode,TypeID)values('{0}','{1}',{2}) select @@IDENTITY;
                                        select @ItemID=MAX(ItemID) from TB_BillItem
                                        update TB_BillItem set ItemName='{0}'+CONVERT(varchar(10),@ItemID) where ItemID=@ItemID
                                        insert into TB_Rule_RateBill(MaxValue,MinValue,ItemID)values({3},{4},@ItemID) select @@IDENTITY;
                                        select @RuleID=MAX(RuleID) from TB_Rule_RateBill
                                        insert into TB_Price_RateBill(RuleID,Price) values(@RuleID,{5})", "阶梯",
                             model.EnergyCode, model.TypeID, model.MaxValue, model.MinValue, model.Price);
                }
                else
                {
                    sql += string.Format(@"update TB_Rule_RateBill set MaxValue={0},MinValue={1} where RuleID={2};
                                            update TB_Price_RateBill set Price={3} where ID={4};",
                             model.MaxValue, model.MinValue, model.RuleID, model.Price, model.ID);
                }
            }
            resultModel.IsSucess = _dal.SaveRatePrice(sql);
            return resultModel;
        }

        /// <summary>
        /// 分时模式 （新增、修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultRate SaveTimePrice(QueryTime model)
        {
            ResultRate resultModel = new ResultRate();
            string sql = "";
            //if (model.listPrice != null)
            //{
            //    foreach (TimePrice m in model.listPrice)
            //    {
            //        if (m != null)
            //            if (m.ID == 0)
            //            {
            //                sql += string.Format(@" insert into TB_TIME_PERIOD_SET (ItemID,Price) values({0},{1}); ", m.ItemID, m.Price);
            //            }
            //            else
            //            {
            //                sql += string.Format(@" update TB_Price_TimeBill set Price={0} where ID={1}", m.Price, m.ID);
            //            }
            //    }
            //}
            //if (model.ListRule != null)
            //{
            //    foreach (TimeRule m in model.ListRule)
            //    {
            //        if (m != null)
            //            if (m.RuleID == 0)
            //            {
            //                sql += string.Format(@" insert into TB_Rule_TimeBill (StartTime,EndTime,ItemID) values({0},{1},{2}); ", m.StartTime, m.EndTime, m.ItemID);
            //            }
            //            else
            //            {
            //                sql += string.Format(@" update TB_Rule_TimeBill set StartTime={0},EndTime={1} where RuleID={2}", m.StartTime, m.EndTime, m.RuleID);
            //            }
            //    }
            //}

            resultModel.IsSucess = _dal.SaveRatePrice(sql);
            return resultModel;
        }

        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="energyType"></param>
        /// <returns></returns>
        public ResultRatePriceList GetRateInfoList(string energyType, string timeType, string rateType)
        {
            ResultRatePriceList modelList = new ResultRatePriceList();
            if (string.IsNullOrEmpty(energyType))
                return modelList;
            modelList.CommModel = _dal.GetCommPriceByEnergyCode(energyType);
            if (!string.IsNullOrEmpty(rateType))
            {
                modelList.RatePriceList = _dal.GetRatePriceByEnergyCodeType(energyType, rateType);
            }
            if (!string.IsNullOrEmpty(timeType))
            {
                modelList.PriceTimeList = _dal.GetTimePriceByEnergyCodeType(energyType, timeType);
                modelList.RuleTimeList = _dal.GetRuleTimeByEnergyCodeType(energyType, timeType);
            }
            return modelList;
        }

        #endregion

        #region New
        /// <summary>
        /// 平价模式保存  （新增、修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultRate SaveParValue(QueryComm model)
        {
            ResultRate resultModel = new ResultRate();
            Model.TB_PAR_VALUE_SET commModel = new TB_PAR_VALUE_SET() { PRICE = model.PRICE, TYPEID = model.TYPEID, ID = model.ID, CNAME = model.CNAME, DATE = model.DATE };
            if (model.ID == 0)
            {
                resultModel.IsSucess = _dal.AddParValue(commModel);
            }
            else
            {
                resultModel.IsSucess = _dal.UpdateParValue(commModel);
            }
            return resultModel;
        }

        /// <summary>
        /// 阶梯模式保存 （新增、修改）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ResultRate SaveMultiStep(List<QueryRate> list)
        {
            ResultRate resultModel = new ResultRate();
            string sql = "";
            foreach (QueryRate model in list)
            {
                if (model.ID == 0)
                {
                    sql += string.Format(@"INSERT INTO TB_MULTI_STEP(GEARSID,GEARNAME,TYPEID,START_GEARS_VALUE,END_GEARS_VALUE,PRICE,DATE)
                                            VALUES ({0},'{1}','{2}',{3},{4},{5},{6})",
                             model.GEARSID, model.GEARNAME, model.TYPEID, model.START_GEARS_VALUE, model.END_GEARS_VALUE, model.PRICE, model.DATE);
                }
                else
                {
                    sql += string.Format(@"UPDATE TB_MULTI_STEP SET START_GEARS_VALUE={0},END_GEARS_VALUE={1},PRICE={2},DATE={3} where ID={4};",
                             model.START_GEARS_VALUE, model.END_GEARS_VALUE, model.PRICE, model.DATE, model.ID);
                }
            }
            resultModel.IsSucess = _dal.SaveRatePrice(sql);
            return resultModel;
        }

        public ResultRate SavePeriod(List<QueryTime> list)
        {
            ResultRate resultModel = new ResultRate();
            string sql = "";

            foreach (QueryTime m in list)
            {
                if (m != null)
                    if (m.ID == 0)
                    {
                        sql += string.Format(@" INSERT INTO TB_TIME_PERIOD_SET (TYPEID,CNAME,STARTHOUR,ENDHOUR,TYPE,PRICE,DATE,STARTMINUTE,ENDMINUTE) 
                                            values('{0}','{1}',{2},{3},{4},{5},{6},{7},{8}); ", m.TYPEID, m.CNAME, m.STARTHOUR, m.ENDHOUR, m.TYPE, m.PRICE, m.DATE, m.STARTMINUTE, m.ENDMINUTE);
                    }
                    else
                    {
                        sql += string.Format(@" UPDATE TB_TIME_PERIOD_SET SET STARTHOUR={0},ENDHOUR={1},TYPE={2},PRICE={3},DATE={4},STARTMINUTE={5},ENDMINUTE={6},CNAME='{7}' where ID={8}",
                                            m.STARTHOUR, m.ENDHOUR, m.TYPE, m.PRICE, m.DATE, m.STARTMINUTE, m.ENDMINUTE, m.CNAME, m.ID);
                    }
            }

            resultModel.IsSucess = _dal.SaveRatePrice(sql);
            return resultModel;
        }

        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="energyType"></param>
        /// <returns></returns>
        public ResultRatePriceList GetRateList(string energyType)
        {
            ResultRatePriceList modelList = new ResultRatePriceList();
            if (string.IsNullOrEmpty(energyType))
                return modelList;
            // 平价
            modelList.ParValueModel = _dal.GetParValueByTypeID(energyType);

            // 阶梯
            modelList.MultiStepList = _dal.GetMultiStepListByTypeID(energyType);

            List<Model.TB_TIME_PERIOD_SET> list = new List<TB_TIME_PERIOD_SET>();
            list = _dal.GetTimePeroidListByTypeID(energyType);
            // 分时
            modelList.PeroidList = list;

            if (list.Count > 0)
            {
                Model.TB_TIME_PERIOD_SET m1 = list.Where(a => a.TYPE == (int)RateType.尖).FirstOrDefault();
                if (m1 != null)
                    modelList.PriceJ = m1.PRICE;
                Model.TB_TIME_PERIOD_SET m2 = list.Where(a => a.TYPE == (int)RateType.峰).FirstOrDefault();
                if (m2 != null)
                    modelList.PriceF = m2.PRICE;
                Model.TB_TIME_PERIOD_SET m3 = list.Where(a => a.TYPE == (int)RateType.平).FirstOrDefault();
                if (m3 != null)
                    modelList.PriceP = m3.PRICE;
                Model.TB_TIME_PERIOD_SET m4 = list.Where(a => a.TYPE == (int)RateType.谷).FirstOrDefault();
                if (m4 != null)
                    modelList.PriceG = m4.PRICE;
            }

            return modelList;
        }

        public ResultRate DeleteStepByID(int id)
        {
            ResultRate resultModel = new ResultRate();
            resultModel.IsSucess = _dal.DeleteStepByID(id);
            return resultModel;
        }

        public ResultRate DeletePeriodByID(int id)
        {
            ResultRate resultModel = new ResultRate();
            resultModel.IsSucess = _dal.DeletePeriodByID(id);
            return resultModel;
        }

        #endregion

        public ItemList GetItemcodeList()
        {
            ItemList model = new ItemList();
            string whereStr = " and parentId=0 ";
            string sortStr = string.Empty;
            List<Model.Itemcode> list = _dal.GetItemcodeList(whereStr, sortStr);
            model.ItemLst = list;
            return model;
        }
    }
}
