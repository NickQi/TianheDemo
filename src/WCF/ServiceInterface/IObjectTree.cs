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
    public interface IObjectTree
    {
        [OperationContract]
        ResultView.ObjectTree GetObjectTree();
        [OperationContract]
        ResultView.ObjectTree GetDeviceTree();

        [OperationContract]
        string GetDeviceListByArea(QueryDevice query);
 
       
    }
}
