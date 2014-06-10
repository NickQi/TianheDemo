using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class EnergyContrast : IEnergyContrast
    {
        public BaseResult GetBaseEneryDataListNew(BaseQueryModel model)
        {
            return EnergyContrastCommon.GetBaseEneryDataListNew(model);
        }
        //public BaseResult GetBaseEneryDataList(BaseQueryModel model, bool IsLiquid)
        //{
        //    return EnergyContrastCommon.GetBaseEneryDataList(model, IsLiquid);
        //}
    }
}
