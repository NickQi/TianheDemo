using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class QuotaAnalyseModel
    {

        /// <summary>
        /// 定额值
        /// </summary>
        [DataMapping("quotaValue", "quotavalue", DbType.Decimal)]
        public double quotaValue
        {
            get;
            set;
        }
       
        public decimal QuotaValue
        {
            get { return decimal.Parse(quotaValue.ToString()); }

        }
       
       
    }


}
