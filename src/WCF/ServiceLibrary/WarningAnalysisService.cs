using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;
using Framework.Common;

namespace ServiceLibrary
{
        //[AspNetCompatibilityRequirements(
        //RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WarningAnalysisService : IWarningAnalysisService
    {
        public List<WarningTypeModel> GetWarningTypeList()
        {
            // return new Json().GetWarningTypeList();

            return new NTS.WEB.BLL.WarningAnalysis().GetWarningTypeList();
        }


        public DataTable GetAreaList()
        {
            // return new Json().GetAreaList(flag);

            return new NTS.WEB.BLL.WarningAnalysis().GetAreaList();
        }

        public int GetWarningPageCount(int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {
            //return new Json().GetAlarmListCount(startTime, endTime, warningTypeId, areaId);

            return new NTS.WEB.BLL.WarningAnalysis().GetWarningPageCount(pageSize, startTime, endTime, warningTypeId, areaId);
        }

        public DataTable GetWarningListByPage(int pageIndex, int pageSize, string startTime, string endTime, string warningTypeId, int areaId)
        {

            //return new Json().GetAlarmList(pageIndex, pageSize,startTime, endTime, warningTypeId, areaId);
            return new NTS.WEB.BLL.WarningAnalysis().GetWarningListByPage(pageIndex, pageSize, startTime, endTime, warningTypeId, areaId);
        }
        public int GetWarningPageCount(WarningAnalysisModel model)
        {


            return new NTS.WEB.BLL.WarningAnalysis().GetWarningPageCount(model);

        }
        [Log(ModelName = "告警分析")]
        [CustomException]
        public DataTable GetWarningListByPage(WarningAnalysisModel model)
        {

            return new NTS.WEB.BLL.WarningAnalysis().GetWarningListByPage(model);


        }
    }
}
