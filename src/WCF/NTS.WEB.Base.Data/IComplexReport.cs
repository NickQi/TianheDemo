using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NTS.WEB.ProductInteface
{
    public interface IComplexReport
    {
        DataSet GetItemCode(string whereStr);

        DataTable GetList(string whereStr, string order);

        string GetBaseLayerObjectName(string objectid);

        DataSet GetAreaList();

        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        DataTable GetListItemCode(string whereStr, string order);

        /// <summary>
        /// 根据区域和项目代码获取是否含有数据
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        int GetCountItemCodeAreaId(string itemcode, string areaid, int classid);
    }
}
