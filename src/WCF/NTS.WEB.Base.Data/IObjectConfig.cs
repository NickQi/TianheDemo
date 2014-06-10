using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.WEB.ProductInteface
{
    public  interface IObjectConfig
    {
        DataTable GetList(string whereStr, string order, SqlParameter[] parameters);
    }
}
