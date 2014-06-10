using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;

namespace NTS.WEB.ServiceInterface
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IMenuTreeService
    {
        //[OperationContract]
        //DataTable GetMenuTree(string username);

        //[OperationContract]
        //ResultMenus GetMenuModule(string username);

        [OperationContract]
        ResultMenus GetMenus(string username);


    }
}
