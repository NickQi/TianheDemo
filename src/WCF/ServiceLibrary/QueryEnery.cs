using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class QueryEnery : IQueryEnery
    {
        [Log(ModelName = "能耗查询")]
        [CustomException]
        public QueryEneryTotal GetQueryEneryTotal(NTS.WEB.DataContact.BasicQuery query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.QueryEnery().GetQueryEneryTotal(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new QueryEneryTotal() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new QueryEneryTotal() { ActionInfo = pAction };
            }
        }
        [Log(ModelName = "能耗排名")]
        [CustomException]
        public NTS.WEB.ResultView.ShopOrderResult GetShopOrder(NTS.WEB.DataContact.QueryOrder query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.QueryEnery().GetShopOrder(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ShopOrderResult() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ShopOrderResult() { ActionInfo = pAction };
            }
        }

        [Log(ModelName = "能耗排名")]
        [CustomException]
        public NTS.WEB.ResultView.ResultOrder GetShopOrderNew(NTS.WEB.DataContact.QueryOrderObjects query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.QueryEnery().GetShopOrderNew(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ResultOrder() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ResultOrder() { ActionInfo = pAction };
            }
        }


        [Log(ModelName = "实时数据")]
        [CustomException]
        public ResultReal GetRealTime(NTS.WEB.DataContact.RealQuery query)
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.QueryEnery().GetRealTime(query);
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new ResultReal() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new ResultReal() { ActionInfo = pAction };
            }
        }
    }
}
