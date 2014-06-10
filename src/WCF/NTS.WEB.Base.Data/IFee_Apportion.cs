using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IFee_Apportion
    {
        List<TB_Alloction_Config> GetAlloctionConfig(Queryfeeapportion feeApport);

        List<CostQueryModel> GetCostQuery(Queryfeeapportion query, DateTime dtBegin, DateTime dtEnd);
    }
}
