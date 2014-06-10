using System.Collections.Generic;
using System.Data;
using System;

namespace NTS.WEB.TableViews
{
    public class BaseTable
    {

        #region old
        ///// <summary>
        ///// 基础的统计对象的虚拟表
        ///// </summary>
        ///// <returns></returns>
        //public static DataTable CreateBaseDataTable()
        //{
        //    Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
        //    Dir.Add("Date", ColType.NTSString);
        //    Dir.Add("ObjectID", ColType.NTSString);
        //    Dir.Add("ObjectName", ColType.NTSString);
        //    Dir.Add("Starttime", ColType.NTSString);
        //    Dir.Add("Endtime", ColType.NTSString);
        //    Dir.Add("ItemCode", ColType.NTSString);
        //    Dir.Add("EneryValue", ColType.NTSDecimal);
        //    return TableTool.CreateTable(Dir);
        //}


        ///// <summary>
        ///// 带转换对象的基础的统计对象的虚拟表
        ///// </summary>
        ///// <returns></returns>
        //public static DataTable CreateHasConvertDataTable(int convertnum)
        //{
        //    Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();

        //    Dir.Add("Date", ColType.NTSString);
        //    Dir.Add("ObjectID", ColType.NTSString);
        //    Dir.Add("ObjectName", ColType.NTSString);
        //    Dir.Add("Starttime", ColType.NTSString);
        //    Dir.Add("Endtime", ColType.NTSString);
        //    Dir.Add("ItemCode", ColType.NTSString);
        //    Dir.Add("EneryValue", ColType.NTSDecimal);
        //    for (int i = 0; i < convertnum; i++) // 转换对象个数
        //    {
        //        Dir.Add("E" + i.ToString(), ColType.NTSDecimal);
        //    }
        //    return TableTool.CreateTable(Dir);
        //}


        ///// <summary>
        ///// 分类分项的统计对象的虚拟表
        ///// </summary>
        ///// <param name="dtitemcodelist">分项的集合</param>
        ///// <returns></returns>
        //public static DataTable CreateSmallItemCodeDataTable(DataTable dtitemcodelist)
        //{
        //    Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();

        //    Dir.Add("Date", ColType.NTSString);
        //    Dir.Add("ObjectID", ColType.NTSString);
        //    Dir.Add("ObjectName", ColType.NTSString);
        //    Dir.Add("Starttime", ColType.NTSString);
        //    Dir.Add("Endtime", ColType.NTSString);
        //    for (int i = 0; i < dtitemcodelist.Rows.Count; i++)
        //    {
        //        Dir.Add("E" + i.ToString(), ColType.NTSDecimal);
        //    }
        //    return TableTool.CreateTable(Dir);
        //}

        //public static DataTable CreateSmallItemCodeDataTable(string[] ItemcodeArr)
        //{
        //    Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();

        //    Dir.Add("Date", ColType.NTSString);
        //    Dir.Add("ObjectID", ColType.NTSString);
        //    Dir.Add("ObjectName", ColType.NTSString);
        //    Dir.Add("Starttime", ColType.NTSString);
        //    Dir.Add("Endtime", ColType.NTSString);
        //    for (int i = 0; i < ItemcodeArr.Length; i++)
        //    {
        //        Dir.Add("E" + i.ToString(), ColType.NTSDecimal);
        //    }
        //    return TableTool.CreateTable(Dir);
        //}


        ///// <summary>
        ///// 综合报表格式报表
        ///// </summary>
        ///// <param name="ItemcodeArr"></param>
        ///// <returns></returns>
        //public static DataTable CreateBOMDataTable(string[] ItemcodeArr)
        //{
        //    Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();

        //    Dir.Add("Date", ColType.NTSString);
        //    Dir.Add("ObjectID", ColType.NTSString);
        //    Dir.Add("ObjectName", ColType.NTSString);
        //    Dir.Add("Starttime", ColType.NTSString);
        //    Dir.Add("Endtime", ColType.NTSString);
        //    for (int i = 0; i < ItemcodeArr.Length; i++)
        //    {
        //        Dir.Add("E" + i.ToString(), ColType.NTSDecimal);
        //    }
        //    return TableTool.CreateTable(Dir);
        //}

        ///// <summary>
        ///// 单位量object的统计分析表格
        ///// </summary>
        ///// <returns></returns>
        //public static DataTable CreatePerTable(int AreaExtLevel)
        //{
        //    Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();

        //    Dir.Add("Date", ColType.NTSString);
        //    Dir.Add("ObjectID", ColType.NTSString);
        //    Dir.Add("ObjectName", ColType.NTSString);
        //    Dir.Add("Starttime", ColType.NTSString);
        //    Dir.Add("Endtime", ColType.NTSString);
        //    Dir.Add("ItemCode", ColType.NTSString);
        //    Dir.Add("EneryValue", ColType.NTSDecimal);
        //    for (int i = 0; i < AreaExtLevel; i++)
        //    {
        //        Dir.Add("E" + i.ToString(), ColType.NTSDecimal);
        //    }
        //    return TableTool.CreateTable(Dir);
        //}
        #endregion
        public static DataTable CreateBigBaseDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("CountID", ColType.NTSString);
            Dir.Add("ObjectName", ColType.NTSString);
            Dir.Add("Starttime", ColType.NTSString);
            Dir.Add("Endtime", ColType.NTSString);
            Dir.Add("CountValue", ColType.NTSDecimal);
            return TableTool.CreateTable(Dir);
        }
    }


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
