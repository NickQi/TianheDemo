using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IRightObject
    {
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool HasMenuRight(string where);
    }
}
