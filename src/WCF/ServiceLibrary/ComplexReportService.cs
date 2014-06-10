using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.BLL;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class ComplexReportService : IComplexReportService
    {
        NTS.WEB.BLL.ComplexReport bll = new ComplexReport();

        public string GetReportType()
        {
            return bll.GetReportType();
        }

        public string ExportAllEnergy(string starttime, string endtime, string objectid, string timeunit, string strPath)
        {
            return bll.ExportAllEnergy(starttime, endtime, objectid, timeunit, strPath);
        }

        public DataTable GetAreaList()
        {
            return bll.GetAreaList();
        }

        public string ExportItemEnergy(string starttime, string endtime, string objectid, string timeunit, string counttype, string strPath)
        {
            return bll.ExportItemEnergy(starttime, endtime, objectid, timeunit, counttype, strPath);
        }

        /// <summary>
        /// 根据区域和项目代码获取是否含有数据
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public int GetCountItemCodeAreaId(string itemcode, string areaid, int classid)
        {
            return bll.GetCountItemCodeAreaId(itemcode, areaid, classid);
        }
    }
}
