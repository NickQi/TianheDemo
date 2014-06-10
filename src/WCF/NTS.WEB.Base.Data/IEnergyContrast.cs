using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IEnergyContrast
    {

        BaseResult GetBaseEneryDataListNew(BaseQueryModel model);

        //BaseResult GetBaseEneryDataList(BaseQueryModel model, bool IsLiquid);
    }
}
