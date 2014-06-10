using System.Data;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IElePrice
    {
        DataTable QueryElEPriceList(ReportQueryModel model, out int recordCount);
    }
}
