using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;

namespace NTS.EMS.Config.BLL
{
    public class OperateDevicePropBll
    {
        NTS.EMS.Config.ProductInteface.IDevicePropObject devicePropOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateDevicePropObject();
        private readonly WEB.ProductInteface.IBaseLayerObject _dal = WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        /// <summary>
        /// 更新TB_DEVICE_PROPERTY
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int UpdateDeviceProp(DevicePropDataContact query)
        {
            try
            {
                return devicePropOperator.UpdateDeviceProp(query.AreaId, query.ItemCodeId, query.AreaType, query.DeviceIds);
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
        public ResultDevicePropList GetDevicePropInfo(QueryDevicePropContact query)
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

        /*
        /// <summary>
        /// 设备树(不用）
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public string GetDeviceTree(List<TB_DeviceProp> devices, QueryDevicePropContact query)
        {
            StringBuilder Result = new StringBuilder();
            if (devices.Count > 0)
            {
                Result.Append("[");
                Result.Append("{ \"id\": 0, \"name\": \"所有设备\", \"open\": true },");
                foreach (var row in devices)
                {
                    Result.Append("{\"id\":" + row.Id + ",\"name\":\"" + row.DeviceName + "\",\"open\":true,\"pId\":0");
                    if (query.AreaType == 1)
                    {
                        Result.Append(string.Format(",\"checked\":{0}", (row.AreaId1 == query.AreaId).ToString().ToLower()));
                    }
                    else
                    {
                        Result.Append(string.Format(",\"checked\":{0}", (row.AreaId2 == query.AreaId).ToString().ToLower()));
                    }
                    Result.Append("},");
                }
                Result = Result.Remove(Result.Length - 1, 1);
                Result.Append("]");
            }
            return Result.ToString();
        }
         * */

        /// <summary>
        /// 获取设备信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<DevicePropInfo> GetDevicePropInfoList(QueryDevicePropContact query, out int total)
        {
            List<DevicePropInfo> resultList = new List<DevicePropInfo>();
            string where = string.Empty;
            if (query.ItemCodeId != 0)
            {
                where = string.Format("and device_itemcode in (select ID from dbo.GetAllChildren({0})) or device_itemcode={1}", query.ItemCodeId, query.ItemCodeId);
            }

            if (!string.IsNullOrEmpty(query.DeviceName))
            {
                 where += string.Format(" and exists (select * from becm_device where becm_device.deviceid=deviceprop.deviceid and devicename like '%{0}%')", query.DeviceName.Trim());
            }
            var devices = devicePropOperator.GetDevicePropInfo(where);
            total = devices.Count();
            resultList = devices.Select(p => new DevicePropInfo
            {
                AreaName = "",
                DeviceName = p.DeviceName,
                ItemCodeName = p.ItemCodeName,
                ID = p.Id,
                AreaId1 = p.AreaId1,
                AreaId2 = p.AreaId2
            }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();
            foreach (var item in resultList)
            {
                item.AreaName = GetAreaName(item.AreaId1, item.AreaId2);
            }
            return resultList;
        }

        /// <summary>
        /// 组织区域名称
        /// </summary>
        /// <param name="areaId1"></param>
        /// <param name="areaId2"></param>
        /// <returns></returns>
        private string GetAreaName(int areaId1, int areaId2)
        {
            string name = string.Empty;
            var funObject = _dal.GetBaseFuncLayerObjectList(string.Format(" and layerobjectid={0} ", areaId2), " order by LayerObjectID");
            var layObject = _dal.GetBaseLayerObjectList(string.Format(" and layerobjectid={0} ", areaId1), " order by LayerObjectID");
            if (funObject.Count > 0)
            {
                name = string.Format("业态功能-{0}", funObject[0].LayerObjectName);
            }
            if (layObject.Count > 0)
            {
                name += string.IsNullOrEmpty(name) ? string.Format("区域位置-{0}", layObject[0].LayerObjectName) : string.Format("，区域位置-{0}", layObject[0].LayerObjectName);
            }
            return name;
        }
    }
}
