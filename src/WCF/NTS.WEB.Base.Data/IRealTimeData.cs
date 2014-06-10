using System.Data;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IRealTimeData
    {
        /// <summary>
        /// 返回记录数
        /// </summary>
        /// <param name="model"> </param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetRealTimeData(BaseListModel model,ref int recordCount);
    }
}
