using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;
using Framework.Common;
namespace ServiceLibrary
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BalanceAnalysisService : IBalanceAnalysisService
    {
        //public DataTable GetBalanaceValueByMonth(int pageIndex, int pageSize, string itemcodeid, int areaid, DateTime month, string orderby)
        //{
           
        //    return new NTS.WEB.BLL.BalanceAnalysis().GetBalanaceValueByMonth(pageIndex, pageSize, itemcodeid, areaid, month, orderby);
        //}
       
        public int GetChildAreaCount(int pageSize, int parentid)
        {
            return new NTS.WEB.BLL.BalanceAnalysis().GetChildAreaCount(pageSize, parentid);
        }

        [Log(ModelName = "平衡分析")]
        [CustomException]
        public DataTable GetBalanaceValueByMonth(NTS.WEB.Model.BalanceAnalysisModel model)
        {
            return new NTS.WEB.BLL.BalanceAnalysis().GetBalanaceValueByMonth(model.PageCurrent, model.PageSize, model.ItemCode, model.ObjectNum, model.StartTime, model.OrderWay);
        }
    }
}
