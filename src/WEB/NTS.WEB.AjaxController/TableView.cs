using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.Common;
using NTS.WEB.ResultView;

namespace NTS.WEB.AjaxController
{
    public class TableView
    {
        /// <summary>
        /// 基础的统计对象的虚拟表
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateBaseDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("ObjectID", ColType.NTSString);
            Dir.Add("ObjectName", ColType.NTSString);
            Dir.Add("Starttime", ColType.NTSString);
            Dir.Add("Endtime", ColType.NTSString);
            Dir.Add("ItemCode", ColType.NTSString);
            Dir.Add("EneryValue", ColType.NTSString);
            return TableTool.CreateTable(Dir);
        }

        public static DataTable CreateOrderBaseDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("ObjectID", ColType.NTSString);
            Dir.Add("ObjectName", ColType.NTSString);
            Dir.Add("Starttime", ColType.NTSString);
            Dir.Add("Endtime", ColType.NTSString);
            Dir.Add("ItemCode", ColType.NTSString);
            Dir.Add("EneryValue", ColType.NTSString);
            Dir.Add("AreaEneryValue", ColType.NTSString);
            Dir.Add("PersonEneryValue", ColType.NTSString);
            return TableTool.CreateTable(Dir);
        }

        /// <summary>
        /// add by jy
        /// add at 2014-3-23
        /// note: 导出对比对象列表列表。
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateContrastDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("Object", ColType.NTSString);
            Dir.Add("ItemCode", ColType.NTSString);
            Dir.Add("EneryValue", ColType.NTSString);
            return TableTool.CreateTable(Dir);
        }

        public static DataTable CreateFee_ApportionDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("Object", ColType.NTSString);
            Dir.Add("FeeBefore", ColType.NTSDecimal);
            Dir.Add("FeeAfter", ColType.NTSDecimal);
            Dir.Add("FeeALL", ColType.NTSDecimal);
            return TableTool.CreateTable(Dir);
        }


        public static DataTable CreateFee_ForecastDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Date", ColType.NTSString);
            Dir.Add("Yuce", ColType.NTSString);
            Dir.Add("History", ColType.NTSString);
            Dir.Add("PianCha", ColType.NTSString);
            Dir.Add("Pecent", ColType.NTSString);
            return TableTool.CreateTable(Dir);
        }

        public static DataTable CreateCostQueryDataTable(ResultCostQuery result)
        {

            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("时间", ColType.NTSString);
            Dir.Add(string.Format("{0}量总值({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
            Dir.Add(string.Format("{0}费总值(元)", result.FeeTbl.EneType), ColType.NTSString);
            switch ((FeeType)Enum.Parse(typeof(FeeType), result.FeeType))
            {
                case FeeType.分时计费:
                    Dir.Add(string.Format("尖时{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                    Dir.Add(string.Format("尖时{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    Dir.Add(string.Format("峰时{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                    Dir.Add(string.Format("峰时{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    Dir.Add(string.Format("平时{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                    Dir.Add(string.Format("平时{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    Dir.Add(string.Format("谷时{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                    Dir.Add(string.Format("谷时{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    break;
                case FeeType.阶梯计费:
                    if (result.StepSettingID.Contains(4))
                    {
                        Dir.Add(string.Format("第四档{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                        Dir.Add(string.Format("第四档{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    }
                    if (result.StepSettingID.Contains(1))
                    {
                        Dir.Add(string.Format("第一档{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                        Dir.Add(string.Format("第一档{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    }
                    if (result.StepSettingID.Contains(2))
                    {
                        Dir.Add(string.Format("第二档{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                        Dir.Add(string.Format("第二档{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    }
                    if (result.StepSettingID.Contains(3))
                    {
                        Dir.Add(string.Format("第三档{0}量({1})", result.FeeTbl.EneType, result.Unit), ColType.NTSString);
                        Dir.Add(string.Format("第三档{0}费(元)", result.FeeTbl.EneType), ColType.NTSString);
                    }

                    break;
            }

            return TableTool.CreateTable(Dir);
        }
    }
}
