using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;

namespace NTS.EMS.Config.BLL
{
    public class OperateDeviceItemBll
    {
        NTS.EMS.Config.ProductInteface.IDevicePropObject devicePropOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateDevicePropObject();
        private readonly WEB.ProductInteface.IBaseLayerObject _dal = WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        /// <summary>
        /// 更新TB_DEVICE_PROPERTY
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int UpdateDeviceProp(DeviceItemDataContact query)
        {
            try
            {
                return devicePropOperator.UpdateDeviceProp(query.ItemCodeId, query.DeviceIds, "device_itemcode");
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultDevicePropList GetDevicePropInfo(QueryDeviceItemContact query)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                ResultDevicePropList result = new ResultDevicePropList();
                result.DevicePropList = new List<DevicePropInfo>();
                result.Page = new Padding();
                int total = 0;
                //string where = string.Format("and device_itemcode in (select ID from dbo.GetAllChildren({0})) or device_itemcode={1}", query.ItemCodeId, query.ItemCodeId);

                result.DevicePropList = GetDevicePropInfoList(query, out total);
                result.Page.Current = query.PageCurrent;
                result.Page.Total = total;
                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                result.ResultInfo = resultInfo;
                return result;
            }
            catch (Exception ex)
            {

                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return new ResultDevicePropList { ResultInfo = resultInfo };
            }
        }

        /// <summary>
        /// 获取设备信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<DevicePropInfo> GetDevicePropInfoList(QueryDeviceItemContact query, out int total)
        {
            List<DevicePropInfo> resultList = new List<DevicePropInfo>();
            string where = string.Empty;
            if (query.ItemCodeId != 0)
            {
                where = string.Format(" and (device_itemcode in (select ID from dbo.GetAllChildren({0})) or device_itemcode={1}) ", query.ItemCodeId, query.ItemCodeId);
            }
            if (!string.IsNullOrEmpty(query.DeviceName))
            {
                where += string.Format(" and exists (select * from becm_device where becm_device.deviceid=deviceprop.deviceid and devicename like '%{0}%')", query.DeviceName);
            }
            if (query.Status == 1)
            {
                where += string.Format(" and (device_itemcode=0 or device_itemcode is null) ");
            }
            else if (query.Status == 2)
            {
                where += string.Format(" and device_itemcode is not null ");
            }
            var devices = devicePropOperator.GetDevicePropInfo(where);
            total = devices.Count();

            while ((query.PageCurrent - 1) * query.PageSize >= total)
            {
                query.PageCurrent--;
            }

            resultList = devices.Select(p => new DevicePropInfo
            {
                AreaName = "",
                DeviceName = p.DeviceName,
                ItemCodeName = p.ItemCodeName,
                ID = p.Id,
                AreaId1 = p.AreaId1,
                AreaId2 = p.AreaId2
            }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();
            return resultList;
        }
    }
}
