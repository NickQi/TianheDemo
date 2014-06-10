using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class RightObject : IRightObject
    {
        public bool HasMenuRight(string where)
        {
            try
            {
                var cmd = new DataCommand("HasMenuRight", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#whereStr#", where);
                return int.Parse(cmd.ExecuteScalar().ToString()) == 0 ? false : true;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
