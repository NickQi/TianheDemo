using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.WEB.Model
{
    public class MonthSumData
    {
        [DataMapping("ID", "ID", DbType.Int32)]
        public int ID { get; set; }

        [DataMapping("COUNTID", "COUNTID", DbType.Int32)]
        public int COUNTID { get; set; }

        [DataMapping("CNAME", "CNAME", DbType.String)]
        public string CNAME { get; set; }

        [DataMapping("ITEMCODE", "ITEMCODE", DbType.String)]
        public string ITEMCODE { get; set; }

        [DataMapping("STARTTIME", "STARTTIME", DbType.DateTime)]
        public DateTime STARTTIME { get; set; }

        [DataMapping("ENDTIME", "ENDTIME", DbType.DateTime)]
        public DateTime ENDTIME { get; set; }

        [DataMapping("COUNTVALUE", "COUNTVALUE", DbType.Double)]
        public double COUNTVALUE { get; set; }

        [DataMapping("RESERVED", "RESERVED", DbType.String)]
        public string RESERVED { get; set; }
    }
}
