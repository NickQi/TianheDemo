using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.ProductInteface
{
    public interface ISysLogObject
    {
        List<Model.SysLog> GetSysLogList(string whereStr);
    }
}
