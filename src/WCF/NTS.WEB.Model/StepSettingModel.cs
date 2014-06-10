using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    /// <summary>
    ///阶梯配置
    /// </summary>
    public class StepSettingModel
    {

        /// <summary>
        /// id
        /// </summary>
        [DataMapping("GEARSID", "GEARSID", DbType.Int32)]
        public int GEARSID
        {
            get;
            set;
        }
        /// <summary>
        /// 阶梯档
        /// </summary>
        [DataMapping("GEARNAME", "GEARNAME", DbType.String)]
        public string GEARNAME
        {
            get;
            set;
        }
       
        [DataMapping("START_GEARS_VALUE", "START_GEARS_VALUE", DbType.Int32)]
        public int START_GEARS_VALUE
        {
            get;
            set;
        }
       
        [DataMapping("END_GEARS_VALUE", "END_GEARS_VALUE", DbType.Int32)]
        public int END_GEARS_VALUE
        {
            get;
            set;
        }
       
        [DataMapping("PRICE", "PRICE", DbType.Decimal)]
        public decimal PRICE
        {
            get;
            set;
        }
        
        [DataMapping("DATE", "DATE", DbType.Int32)]
        public int DATE
        {
            get;
            set;
        }
       
    }


}
