using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NTS.WEB.Common
{
  
        #region 枚举字段的类型
        /// <summary>
        /// 字段的类型
        /// </summary>
        public enum ColType
        {
            /// <summary>
            /// 字符串类型
            /// </summary>
            NTSString = 0,
            /// <summary>
            /// int类型
            /// </summary>
            NTSInt,
            /// <summary>
            /// 小数类型
            /// </summary>
            NTSDecimal,
            /// <summary>
            /// 日期类型
            /// </summary>
            NTSDateTime
        }
        #endregion

        #region 生成表结构工具
        public class TableTool
        {
            /// <summary>
            /// 生成表结构
            /// </summary>
            /// <param name="Dir"></param>
            /// <returns></returns>
            public static DataTable CreateTable(Dictionary<string, ColType> Dir)
        {
            DataTable tblDatas = new DataTable("Datas");
            DataColumn dc;
            dc = tblDatas.Columns.Add("ID", Type.GetType(ConvertType(ColType.NTSInt)));
            dc.AutoIncrement = true; //自动增加
            dc.AutoIncrementSeed = 1; //起始为1
            dc.AutoIncrementStep = 1; //步长为1
            dc.AllowDBNull = false; //
            foreach (var d in Dir)
            {
                tblDatas.Columns.Add(d.Key, Type.GetType(ConvertType(d.Value)));
            }
            return tblDatas;
        }

            /// <summary>
            /// 转化工具
            /// </summary>
            /// <param name="v"></param>
            /// <returns></returns>
            private static string ConvertType(ColType v)
            {
                switch (v)
                {
                    case ColType.NTSInt:
                        return "System.Int32";
                    case ColType.NTSString:
                        return "System.String";
                    case ColType.NTSDecimal:
                        return "System.Decimal";
                    case ColType.NTSDateTime:
                        return "System.Int32";
                    default:
                        return "System.DateTime";
                }
            }

            
        }
        #endregion
    
}
