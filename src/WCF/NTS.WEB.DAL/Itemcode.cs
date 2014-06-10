using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NTS.WEB.Base.Data;
using Framework.Data;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class Itemcode : IItemcode
    {

        public List<Model.Itemcode> GetItemcodeList(string whereStr, string sortStr)
        {
            try
            {
                var cmd = new DataCommand("getItemCode", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                cmd.ReplaceParameterValue("#Sort#", sortStr);
                return cmd.ExecuteEntityList<Model.Itemcode>();
            }
            catch(Exception ee)
            {
                return null;
            }

            
        }

    }
}
