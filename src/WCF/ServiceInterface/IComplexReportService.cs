using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace NTS.WEB.ServiceInterface
{
     [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IComplexReportService
    {

        [OperationContract]
        string GetReportType();

        [OperationContract]
        string ExportAllEnergy(string starttime, string endtime, string objectid, string timeunit, string strPath);


        [OperationContract]
        DataTable GetAreaList();

        [OperationContract]
        string ExportItemEnergy(string starttime, string endtime, string objectid, string timeunit, string counttype, string strPath);


        /// <summary>
        /// 根据区域和项目代码获取是否含有数据
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="areaid"></param>
        [OperationContract]
        int GetCountItemCodeAreaId(string itemcode, string areaid,int classid);


    }
}
