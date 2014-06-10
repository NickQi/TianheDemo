using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CostQueryService : ICostQueryService
    {
      


        [Log(ModelName = "费用查询")]
        [CustomException]
        public ResultCostQuery GetCostQuery(QueryCost query)
        {
            var pAction = new ExecuteProcess();
            try
            {

                var result = new NTS.WEB.BLL.CostQuery().GetCostQuery(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ResultCostQuery() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ResultCostQuery() { ActionInfo = pAction };
            }
        }
    }
}
