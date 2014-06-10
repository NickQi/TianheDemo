using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IDevicePropObject
    {
        /// <summary>
        /// 更新设备属性表
        /// </summary>
        /// <param name="deviceId">设备id</param>
        /// <param name="itemCodeId"></param>
        /// <param name="areaType"></param>
        /// <param name="areaIds"></param>
        /// <returns></returns>
        int UpdateDeviceProp(int areaId, int itemCodeId, int areaType, List<int> deviceIds);

        /// <summary>
        /// 获取设备属性信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Model.TB_DeviceProp> GetDevicePropInfo(string where);

        /// <summary>
        /// 更新设备属性ItemCode值
        /// </summary>
        /// <param name="itemCodeId"></param>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        int UpdateDeviceProp(int itemCodeId, List<int> deviceIds, string columnName);

        /// <summary>
        /// 获取TB_PAYMENT_TYPE
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<NTS.EMS.Config.Model.TB_PAYMENT_TYPE> GetPayTypeList(string where);
    }
}
    