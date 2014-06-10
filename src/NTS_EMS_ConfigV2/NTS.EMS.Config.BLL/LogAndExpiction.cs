using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.BLL
{
    public class LogAndExpiction : ModelContactTransfer
    {
        private readonly IBussinessLog _dal = DataSwitchConfig.CreateBussinessLog();
        public int SetBussinessLog(QueryBussinessLog bussinessLog)
        {
            var bussinessLogModel = TransferToModel<QueryBussinessLog, BussinessLogModel>(bussinessLog);
            return _dal.SetBussinessLog(bussinessLogModel);
        }

        public override TModel TransferToModel<TContact, TModel>(TContact contact)
        {
            var bussinessLogModel = new BussinessLogModel();
            var model = contact as QueryBussinessLog;
            if (model != null)
            {
                bussinessLogModel.ModelName = model.ModelName;
                bussinessLogModel.ModelType = model.ModelType;
                bussinessLogModel.OperatorContent = model.OperatorContent;
                bussinessLogModel.OperatorTime = model.OperatorTime;
                bussinessLogModel.UserName = model.UserName;
            }
            return bussinessLogModel as TModel;
        }
    }

    public  class ModelContactTransfer
    {
        public virtual TModel TransferToModel<TContact, TModel>(TContact model) where TModel : class
        {
            return null;
        }
    }
}
