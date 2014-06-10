using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.EMS.Config.Model
{
        /// <summary>
        /// 导入的临时表
        /// </summary>
        public class ImportTemp
        {
            /// <summary>
            /// 自动编号
            /// </summary>
            [DataMapping("Id", "Id", DbType.Int32)]
            public int Id { get; set; }
            /// <summary>
            /// 对象的id
            /// </summary>
            [DataMapping("ObjectId", "ObjectId", DbType.Int32)]
            public int ObjectId { get; set; }
            /// <summary>
            /// 是否是区域
            /// </summary>
            [DataMapping("IsArea", "IsArea", DbType.Int32)]
            public int IsArea { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            [DataMapping("StartTime", "StartTime", DbType.DateTime)]
            public DateTime StartTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            [DataMapping("EndTime", "EndTime", DbType.DateTime)]
            public DateTime EndTime { get; set; }
            /// <summary>
            /// 导入的数值
            /// </summary>
            [DataMapping("ImportValue", "ImportValue", DbType.Decimal)]
            public Decimal ImportValue { get; set; }
            /// <summary>
            /// 分类分项的代码
            /// </summary>
            [DataMapping("ItemCode", "ItemCode", DbType.String)]
            public string ItemCode { get; set; }
            /// <summary>
            /// 时间的颗粒
            /// </summary>
            [DataMapping("MonthType", "MonthType", DbType.Int32)]
            public int MonthType { get; set; }

            /// <summary>
            /// ExcelId
            /// </summary>
            [DataMapping("ExcelId", "ExcelId", DbType.Int32)]
            public int ExcelId { get; set; }

        }
    
}
