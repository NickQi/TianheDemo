using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IIndexEnery
    {
        /// <summary>
        /// 首页弹框的接口
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        IndexWindowResult GetIndexWindowResult(QueryIndexWindow query);

        [OperationContract]
        IndexMonthEnery GetIndexMonthEneryResult();

        [OperationContract]
        IndexCompareEnery GetIndexCompareEnery();

        [OperationContract]
        MainInfo GetIndexCompareEneryNew();

        [OperationContract]
        IndexShopOrder GetIndexShopOrder();

        [OperationContract]
        IndexLimit GetIndexLimit();
    }
}
