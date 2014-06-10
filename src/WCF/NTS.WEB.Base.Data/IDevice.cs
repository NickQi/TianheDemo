using System.Collections.Generic;

namespace NTS.WEB.ProductInteface
{
    /// <summary>
    /// 设备的相关接口类
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// 获取设备的相关信息
        /// </summary>
        /// <param name="whereStr">查询设备的条件</param>
        /// <param name="sortStr">排序</param>
        /// <returns></returns>
        List<NTS.WEB.Model.Device> GetDeviceList(string whereStr, string sortStr);
    }
}
