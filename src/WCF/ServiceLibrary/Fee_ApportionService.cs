using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Fee_ApportionService : IFee_ApportionService
    {
        NTS.WEB.BLL.Fee_Apportion apportBLL = new NTS.WEB.BLL.Fee_Apportion();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultFeeapportion GetFeeApportionData(Queryfeeapportion query)
        {
            return apportBLL.GetFeeApportionData(query);
        }

        /// <summary>
        /// 获取费用分摊列表数据。
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FeeApportionListClass> GetFeeApportDataList(Queryfeeapportion query)
        {
            return apportBLL.GetFeeApportDataList(query);
        }
    }
}
