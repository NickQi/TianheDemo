using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IPage
    {
        /// <summary>
        /// 获取分页的数据信息
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        DataTable GetDataByPage(PageModel pmodel);
        /// <summary>
        /// 获取分页的记录数
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        int GetPageCount(PageModel pmodel);
    }
}
