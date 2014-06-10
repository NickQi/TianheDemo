using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
     [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IQueryEnery
    {
        [OperationContract]
        QueryEneryTotal GetQueryEneryTotal(NTS.WEB.DataContact.BasicQuery query);

        [OperationContract]
        ResultView.ShopOrderResult GetShopOrder(NTS.WEB.DataContact.QueryOrder query);

        [OperationContract]
        ResultView.ResultOrder GetShopOrderNew(NTS.WEB.DataContact.QueryOrderObjects query);
        [OperationContract]
        ResultReal GetRealTime(NTS.WEB.DataContact.RealQuery query);
    }
}
