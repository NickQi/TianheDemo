using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtility;
using NTS.WEB.ProductInteface;
using System.Data;
namespace NTS.WEB.DAL
{
   public class AreaTree:IAreaTree
    {
        public DataTable GetAreaTree()
        {
            try
            {
                string SQL = @"select layerobjectid,layerobjectname,layerobjectparentid from dbo.Becm_LayerObject
order by layerobjectid";
               // return SqlHelper.Query(SQL).Tables[0];
                return new DataTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
