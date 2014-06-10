using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IBaseLayerObject
    {

        List<Model.BaseLayerObject> GetBaseLayerObjectList(string whereStr, string order,string username="");

        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        List<Model.BaseLayerObject> GetBaseFuncLayerObjectList(string whereStr, string sortStr, string username = "");

        // decimal GetBaseLayerObjectCommAttribute(string objectid, int classid);
        List<DeviceAreaID> GetDeviceAreaID1List(string itemcode);
        List<DeviceAreaID> GetDeviceAreaID2List(string itemcode);
    }
}
