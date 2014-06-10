using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class Rate : IRate
    {
        #region 平价模式

        public bool AddCommPrice(Model.TB_Price_CommBill model)
        {
            var cmd = new DataCommand("addCommPrice", new SqlCustomDbCommand());
            cmd.SetParameterValue("@EnergyCode", model.EnergyCode);
            cmd.SetParameterValue("@Price", model.Price);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool UpdateCommPrice(Model.TB_Price_CommBill model)
        {
            var cmd = new DataCommand("updateCommPrice", new SqlCustomDbCommand());
            cmd.SetParameterValue("@ID", model.ID);
            cmd.SetParameterValue("@Price", model.Price);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        #endregion

        /// <summary>
        /// 阶梯模式保存 （新增、修改）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool SaveRatePrice(string sql)
        {
            var cmd = new DataCommand("addRatePrice", new SqlCustomDbCommand());
            //cmd.SetParameterValue("@sql", sql);
            cmd.ReplaceParameterValue("#sql#", sql);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 根据能源类型获取平价信息
        /// </summary>
        /// <param name="energyType"></param>
        /// <returns></returns>
        public Model.TB_Price_CommBill GetCommPriceByEnergyCode(string energyCode)
        {
            var cmd = new DataCommand("getCommPriceByEnergyCode", new SqlCustomDbCommand());
            cmd.SetParameterValue("@EnergyCode", energyCode);
            return cmd.ExecuteEntity<Model.TB_Price_CommBill>();
        }

        /// <summary>
        /// 根据能源类型和计费方式类型，获取阶梯信息
        /// </summary>
        /// <param name="energyCode"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<Model.RatePriceModel> GetRatePriceByEnergyCodeType(string energyCode, string typeId)
        {
            var cmd = new DataCommand("getRatePriceByEnergyCodeType", new SqlCustomDbCommand());
            cmd.SetParameterValue("@EnergyCode", energyCode);
            cmd.SetParameterValue("@TypeID", typeId);
            return cmd.ExecuteEntityList<Model.RatePriceModel>();
        }

        /// <summary>
        /// 根据能源类型和计费方式类型，获取分时 价格 信息
        /// </summary>
        /// <param name="energyCode"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<Model.TB_Price_TimeBillExend> GetTimePriceByEnergyCodeType(string energyCode, string typeId)
        {
            var cmd = new DataCommand("getTimePriceByEnergyCodeType", new SqlCustomDbCommand());
            cmd.SetParameterValue("@EnergyCode", energyCode);
            cmd.SetParameterValue("@TypeID", typeId);
            return cmd.ExecuteEntityList<Model.TB_Price_TimeBillExend>();
        }

        /// <summary>
        /// 根据能源类型和计费方式类型，获取分时 规则 信息
        /// </summary>
        /// <param name="energyCode"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<Model.TB_Rule_TimeBill> GetRuleTimeByEnergyCodeType(string energyCode, string typeId)
        {
            var cmd = new DataCommand("getRuleTimeByEnergyCodeType", new SqlCustomDbCommand());
            cmd.SetParameterValue("@EnergyCode", energyCode);
            cmd.SetParameterValue("@TypeID", typeId);
            return cmd.ExecuteEntityList<Model.TB_Rule_TimeBill>();
        }

        public List<Model.Itemcode> GetItemcodeList(string whereStr, string sortStr)
        {
            var cmd = new DataCommand("getItemCode", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            cmd.ReplaceParameterValue("#Sort#", sortStr);
            return cmd.ExecuteEntityList<Model.Itemcode>();
        }

        #region New
        public bool AddParValue(Model.TB_PAR_VALUE_SET model)
        {
            var cmd = new DataCommand("addParValue", new SqlCustomDbCommand());
            cmd.SetParameterValue("@CNAME", model.CNAME);
            cmd.SetParameterValue("@DATE", model.DATE);
            cmd.SetParameterValue("@PRICE", model.PRICE);
            cmd.SetParameterValue("@TYPEID", model.TYPEID);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool UpdateParValue(Model.TB_PAR_VALUE_SET model)
        {
            var cmd = new DataCommand("updateParValue", new SqlCustomDbCommand());
            cmd.SetParameterValue("@ID", model.ID);
            cmd.SetParameterValue("@DATE", model.DATE);
            cmd.SetParameterValue("@PRICE", model.PRICE);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        public Model.TB_PAR_VALUE_SET GetParValueByTypeID(string typeId)
        {
            var cmd = new DataCommand("getParValueByTypeID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@TYPEID", typeId);
            return cmd.ExecuteEntity<Model.TB_PAR_VALUE_SET>();
        }

        public List<Model.TB_MULTI_STEP> GetMultiStepListByTypeID(string typeId)
        {
            var cmd = new DataCommand("getMultiStepListByTypeID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@TYPEID", typeId);
            return cmd.ExecuteEntityList<Model.TB_MULTI_STEP>();
        }

        public List<Model.TB_TIME_PERIOD_SET> GetTimePeroidListByTypeID(string typeId)
        {
            var cmd = new DataCommand("getTimePeroidListByTypeID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@TYPEID", typeId);
            return cmd.ExecuteEntityList<Model.TB_TIME_PERIOD_SET>();
        }

        /// <summary>
        ///  删除阶梯数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteStepByID(int id)
        {
            var cmd = new DataCommand("deleteStepByID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@ID", id);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除分时数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeletePeriodByID(int id)
        {
            var cmd = new DataCommand("deletePeriodByID", new SqlCustomDbCommand());
            cmd.SetParameterValue("@ID", id);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

       
    }
}
