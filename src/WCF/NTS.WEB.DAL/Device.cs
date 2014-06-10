using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.Base.Data;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class Device : IDevice
    {
        public List<Model.Device> GetDeviceList(string whereStr, string sortStr)
        {
            var cmd = new DataCommand("getDevice", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#",whereStr);
            cmd.ReplaceParameterValue("#Sort#", sortStr);
            return cmd.ExecuteEntityList<Model.Device>();
        }
    }


}
