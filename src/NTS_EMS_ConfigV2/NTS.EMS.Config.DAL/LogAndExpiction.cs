using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using NTS.EMS.Config.ProductInteface;
using Framework.Data;

namespace NTS.EMS.Config.DAL
{
    public class LogAndExpiction : IBussinessLog
    {
        public int SetBussinessLog(BussinessLogModel bussinessLog)
        {
            // add something to database
            DataCommand command = new DataCommand("InsertSystemLog", new SqlCustomDbCommand());
            command.SetParameterValue("@modelname", bussinessLog.ModelName);
            command.SetParameterValue("@logtime", bussinessLog.OperatorTime);
            command.SetParameterValue("@logcontent", string.IsNullOrEmpty(bussinessLog.OperatorContent) ? (bussinessLog.ModelName + "执行了操作。") : bussinessLog.OperatorContent);
            command.SetParameterValue("@optype", bussinessLog.ModelType);
            command.SetParameterValue("@username", bussinessLog.UserName);
            command.ExecuteNonQuery();
            return 0;
        }
    }
}
