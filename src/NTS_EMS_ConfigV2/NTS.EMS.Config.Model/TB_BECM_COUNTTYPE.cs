using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.EMS.Config.Model
{
   public class TB_BECM_COUNTTYPE
    {
       [DataMapping("ID", "ID", DbType.Int32)]
       public int ID { get; set; }

       [DataMapping("CNAME", "CNAME", DbType.String)]
       public string CNAME { get; set; }

       [DataMapping("PARENTID", "PARENTID", DbType.Int32)]
       public int PARENTID { get; set; }

       [DataMapping("DESCRIPTION", "DESCRIPTION", DbType.String)]
       public string DESCRIPTION { get; set; }

       [DataMapping("ITEMUNIT", "ITEMUNIT", DbType.String)]
       public string ITEMUNIT { get; set; }

       [DataMapping("ItemMoney", "ItemMoney", DbType.Double)]
       public double ItemMoney { get; set; }
    }
}
