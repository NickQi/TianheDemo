using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    /// <summary>
    /// 费率设置接口
    /// </summary>
    public interface IRate
    {
        #region 平价模式

        bool AddCommPrice(Model.TB_Price_CommBill model);

        bool UpdateCommPrice(Model.TB_Price_CommBill model);

        #endregion

        /// <summary>
        /// 阶梯模式保存 （新增、修改）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool SaveRatePrice(string sql);

        /// <summary>
        /// 根据能源类型获取平价信息
        /// </summary>
        /// <param name="energyType"></param>
        /// <returns></returns>
        Model.TB_Price_CommBill GetCommPriceByEnergyCode(string energyCode);

        /// <summary>
        /// 根据能源类型和计费方式类型，获取阶梯信息
        /// </summary>
        /// <param name="energyCode"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<Model.RatePriceModel> GetRatePriceByEnergyCodeType(string energyCode, string typeId);

        /// <summary>
        /// 根据能源类型和计费方式，获取分时 价格 信息
        /// </summary>
        /// <param name="energyCode"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<Model.TB_Price_TimeBillExend> GetTimePriceByEnergyCodeType(string energyCode, string typeId);

        /// <summary>
        /// 根据能源类型和计费方式，获取分时 规则 信息
        /// </summary>
        /// <param name="energyCode"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<Model.TB_Rule_TimeBill> GetRuleTimeByEnergyCodeType(string energyCode, string typeId);

        List<Model.Itemcode> GetItemcodeList(string whereStr, string sortStr);

        #region New
        bool AddParValue(Model.TB_PAR_VALUE_SET model);

        bool UpdateParValue(Model.TB_PAR_VALUE_SET model);

        Model.TB_PAR_VALUE_SET GetParValueByTypeID(string typeId);

        List<Model.TB_MULTI_STEP> GetMultiStepListByTypeID(string typeId);

        List<Model.TB_TIME_PERIOD_SET> GetTimePeroidListByTypeID(string typeId);

        bool DeleteStepByID(int id);

        bool DeletePeriodByID(int id);
        #endregion
    }
}
