using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class DevicePropObject : IDevicePropObject
    {
        /// <summary>
        /// 更新设备属性
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="itemCodeId"></param>
        /// <param name="areaType"></param>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        public int UpdateDeviceProp(int areaId, int itemCodeId, int areaType, List<int> deviceIds)
        {
            string sql = string.Empty;
            string columName = "areaid1";
            if (areaType == 2)
            {
                columName = "areaid2";
            }
            // sql = string.Format("update tb_device_property set {0}=null where device_itemcode in (select ID from dbo.GetAllChildren({1}))  or device_itemcode={2} ", columName, itemCodeId, itemCodeId);

            foreach (var item in deviceIds)
            {
                sql += string.Format(" update tb_device_property set {0}={1} where id= {2} ", columName, areaId, item);
            }
            if (string.IsNullOrEmpty(sql))
            {
                return 0;
            }
            var cmd = new DataCommand("updateDeviceProp", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#sql#", sql);
            int result = cmd.ExecuteNonQuery();
            return result;
        }

        /// <summary>
        /// 获取设备属性信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Model.TB_DeviceProp> GetDevicePropInfo(string where)
        {
            var cmd = new DataCommand("GetDevicePropInfo", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", where);
            return cmd.ExecuteEntityList<Model.TB_DeviceProp>();
        }

        /// <summary>
        /// 更新设备itemCode
        /// </summary>
        /// <param name="itemCodeId"></param>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        public int UpdateDeviceProp(int itemCodeId, List<int> deviceIds, string columnName)
        {
            string sql = string.Empty;
            foreach (var item in deviceIds)
            {
                sql += string.Format(" update tb_device_property set {0}={1} where id= {2} ", columnName, itemCodeId, item);
            }
            if (string.IsNullOrEmpty(sql))
            {
                return 0;
            }
            var cmd = new DataCommand("updateDeviceProp", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#sql#", sql);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取TB_PAYMENT_TYPE
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<NTS.EMS.Config.Model.TB_PAYMENT_TYPE> GetPayTypeList(string where)
        {
            var cmd = new DataCommand("GetPayTypeList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", where);
            return cmd.ExecuteEntityList<Model.TB_PAYMENT_TYPE>();
        }
    }
}
