using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface ICostQuery
    {
        List<CostQueryModel> GetCostQuery(QueryCost query);

        List<StepSettingModel> GetStepSetting(string itemcode);
    }
}
