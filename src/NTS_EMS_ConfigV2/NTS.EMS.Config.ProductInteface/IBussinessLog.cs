using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;

namespace NTS.EMS.Config.ProductInteface
{
    public interface IBussinessLog
    {
        int SetBussinessLog(BussinessLogModel bussinessLog);
    }
}
