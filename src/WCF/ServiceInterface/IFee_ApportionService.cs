using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IFee_ApportionService
    {
        [OperationContract]
        ResultFeeapportion GetFeeApportionData(Queryfeeapportion query);

        [OperationContract]
        List<FeeApportionListClass> GetFeeApportDataList(Queryfeeapportion query);
    }
}
