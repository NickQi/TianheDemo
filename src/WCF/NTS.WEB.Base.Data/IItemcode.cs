using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.WEB.ProductInteface
{
    /// <summary>
    /// 分类分项的相关接口类
    /// </summary>
    public interface IItemcode
    {
        List<Model.Itemcode> GetItemcodeList(string whereStr, string sortStr);
    }
}
