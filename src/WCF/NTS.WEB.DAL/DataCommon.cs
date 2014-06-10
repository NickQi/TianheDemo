using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DBUtility;
using Framework.Data;
using NTS.WEB.Model;
using NTS.WEB.TableViews;

namespace NTS.WEB.DAL
{
    public class DataCommon
    {


        /// <summary>
        /// 获取表的基础数据集
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable[] GetBaseDataAll(BaseDataModelNew model, DataTable dtItemCode)
        {

            DataTable[] dtList = new DataTable[4];

            try
            {
                for (int iCount = 0; iCount < dtItemCode.Rows.Count; iCount++)
                {

                    DataTable dtold = new DataTable();
                    List<string> Tcount = new List<string>();
                    Tcount = GetTcountNames(model, Tcount);

                    for (int i = 0; i < Tcount.Count; i++)
                    {
                        string basesql = "";

                        string sql = "";
                        switch (model.Unit)
                        {
                            case "1":
                                basesql = " select top 1 CountID,Value24 as value,ItemCode from \"{0}\" where CountID={1} and  CONVERT(varchar(100), CDate, 23)='{2}' and ItemCode='{3}'";
                                break;
                            case "2":
                                basesql = " select top 1 CountID,Value365 as value,ItemCode from \"{0}\" where CountID={1} and ItemCode='{3}'";
                                sql = string.Format(basesql, new object[] { Tcount[i], model.ObjectId, model.Starttime.ToString("yyyy-MM-dd"), dtItemCode.Rows[iCount]["ItemCodeId"].ToString() });
                                break;
                            case "3":
                                basesql = " select top 1 CountID,Value30 as value,ItemCode from \"{0}\" where CountID={1}  and ItemCode='{3}'";
                                break;
                        }
                        sql = string.Format(basesql, new object[] { Tcount[i], model.ObjectId, model.Starttime.ToString("yyyy-MM-dd"), dtItemCode.Rows[iCount]["itemcodenumber"].ToString() });
                        DataTable dts = SqlHelper.Query(sql).Tables[0];

                        DataTable dtNew = GetNewDataByDts(model, dts);


                        dtold = i.Equals(0) ? dtNew.Clone() : dtold;
                        dtold = UniteDataTable(dtold, dtNew.Copy());
                        dtList[iCount] = dtold;
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("对象名") > -1)
                {
                    throw new Exception("缺少对应的数据表！");
                }
                else if (ex.ToString().IndexOf("列名") > -1)
                {
                    throw new Exception("缺少对应的数据列！");
                }
                else
                {
                    throw new Exception("数据表发生异常！");
                }
            }

            return dtList;
        }


        /// <summary>
        /// 根据条件获取返回的实际DataTable
        /// </summary>
        /// <param name="model">BaseDataModelNew</param>
        /// <param name="strValue">值</param>
        /// <returns></returns>
        private static DataTable GetNewDataByDts(BaseDataModelNew model, DataTable dts)
        {
            DataTable dtNew = CreateDateValueTable();
            // 有数据。
            if (dts.Rows.Count > 0)
            {
                string strValue = dts.Rows[0]["Value"].ToString();
                string[] strList = strValue.Split(',');
                for (int strCount = 0; strCount < strList.Length; strCount++)
                {
                    string[] strItem = strList[strCount].Split('_');
                    if (strItem.Length > 1)
                    {
                        #region ss

                        DataRow drItem = dtNew.NewRow();

                        switch (model.Unit)
                        {
                            case "1":  // 小时
                                if ((DateTime.Parse(strItem[0]) >= model.Starttime) && (DateTime.Parse(strItem[0]) <= model.Endtime))
                                {
                                    drItem["Starttime"] = DateTime.Parse(strItem[0]);
                                    drItem["Endtime"] = DateTime.Parse(strItem[0]).AddHours(1);
                                    drItem["CountValue"] = Decimal.Parse(strItem[1]);
                                    dtNew.Rows.Add(drItem);
                                }
                                break;
                            case "2": // 天
                                if ((DateTime.Parse(strItem[0]) >= model.Starttime) && (DateTime.Parse(strItem[0]) <= model.Endtime))
                                {
                                    drItem["Starttime"] = DateTime.Parse(strItem[0]);
                                    drItem["Endtime"] = DateTime.Parse(strItem[0]).AddDays(1);
                                    drItem["CountValue"] = Decimal.Parse(strItem[1]);
                                    dtNew.Rows.Add(drItem);
                                }
                                break;
                            case "3": // 年
                                if ((DateTime.Parse(strItem[0]) >= model.Starttime) && (DateTime.Parse(strItem[0]) <= model.Endtime))
                                {
                                    drItem["Starttime"] = DateTime.Parse(strItem[0]);
                                    drItem["Endtime"] = DateTime.Parse(strItem[0]).AddMonths(1);
                                    drItem["CountValue"] = Decimal.Parse(strItem[1]);
                                    dtNew.Rows.Add(drItem);
                                }
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }
                }
            }

            return dtNew;
        }



        /// <summary>
        /// 基础的统计对象的虚拟表
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateDateValueTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Starttime", ColType.NTSString);
            Dir.Add("Endtime", ColType.NTSString);
            Dir.Add("CountValue", ColType.NTSDecimal);
            return TableTool.CreateTable(Dir);
        }


        /// <summary>
        /// 获取表的基础数据集
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable GetBaseDataItem(BaseDataModelNew model)
        {
            DataTable dtold = new DataTable();
            try
            {
                List<string> Tcount = new List<string>();
                Tcount = GetTcountNames(model, Tcount);

                for (int i = 0; i < Tcount.Count; i++)
                {
                    string basesql = "";

                    string sql = "";
                    switch (model.Unit)
                    {
                        case "1":
                            basesql = " select top 1 CountID,Value24 as value,ItemCode from \"{0}\" where CountID='{1}' and  CONVERT(varchar(100), CDate, 23)='{2}' and ItemCode='{3}'";
                            break;
                        case "2":
                            basesql = " select top 1 CountID,Value365 as value,ItemCode from \"{0}\" where CountID={1} and ItemCode='{3}'";
                            sql = string.Format(basesql, new object[] { Tcount[i], model.ObjectId, model.Starttime.ToString("yyyy-MM-dd"), model.ItemCode });
                            break;
                        case "3":
                            basesql = " select top 1 CountID,Value30 as value,ItemCode from \"{0}\" where CountID={1} and ItemCode='{3}'";
                            break;
                        default:
                            basesql = " select top 1 CountID,Value24 as value,ItemCode from \"{0}\" where CountID='{1}' and  CONVERT(varchar(100), CDate, 23)='{2}' and ItemCode='{3}'";
                            break;
                    }
                    //  sql = string.Format(basesql, new object[] { Tcount[i], model.Starttime, model.Endtime, model.ItemCode });
                    sql = string.Format(basesql, new object[] { Tcount[i], model.ObjectId, model.Starttime.ToString("yyyy-MM-dd"), model.ItemCode });
                    var cmd = new DataCommand("GetBaseDataItem", new SqlCustomDbCommand());
                    cmd.ReplaceParameterValue("#SQLSTR#", sql);
                    
                    //DataTable dts = SqlHelper.Query(sql).Tables[0];
                    DataTable dts = cmd.ExecuteDataSet().Tables[0];

                    DataTable dtNew = GetNewDataByDts(model, dts);


                    dtold = i.Equals(0) ? dtNew.Clone() : dtold;
                    dtold = UniteDataTable(dtold, dtNew.Copy());
                }
                return dtold;
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("对象名") > -1)
                {
                    throw new Exception("缺少对应的数据表！");
                }
                else if (ex.ToString().IndexOf("列名") > -1)
                {
                    throw new Exception("缺少对应的数据列！");
                }
                else
                {
                    throw new Exception("数据表发生异常！");
                }
            }

        }

        /// <summary>
        /// 合并dataset数据集到一个datatable上
        /// </summary>
        /// <returns></returns>
        private static DataTable UniteDataTable(DataTable old, DataTable newdt)
        {
            object[] obj = new object[old.Columns.Count];

            for (int i = 0; i < newdt.Rows.Count; i++)
            {
                newdt.Rows[i].ItemArray.CopyTo(obj, 0);
                old.Rows.Add(obj);
            }
            return old;
        }


        #region 根据查询的起始时间获取需要取的表的表名集合
        /// <summary>
        /// 根据查询的起始时间获取需要取的表的表名集合
        /// </summary>
        /// <param name="model">查询条件实体类</param>
        /// <param name="Tcount">返回的表的集合</param>
        /// <returns></returns>
        private static List<string> GetTcountNames(BaseDataModelNew model, List<string> Tcount)
        {
            if (model.Unit == "1")  // 小时
            {
                //取的是小时数据，不存在跨天
                //Tcount.Add("TS_DataCenter_Area_Hour_" + model.Starttime.Year.ToString());
                if (model.ObjectType == 0)
                {
                    Tcount.Add("TS_DataCenter_Area_Hour_" + model.Starttime.Year.ToString());
                }
                else
                {
                    Tcount.Add("TS_DataCenter_Device_Hour_" + model.Starttime.Year.ToString());
                }
            }
            else if (model.Unit == "3") // 年
            {

                //Tcount.Add("TS_DataCenter_Area_Month_" + model.Starttime.Year.ToString());
                //// 夸年。
                //if (model.Starttime.Year < model.Endtime.Year)
                //{
                //    Tcount.Add("TS_DataCenter_Area_Month_" + model.Starttime.Year.ToString());
                //}
                // 
                if (model.Starttime.Year.Equals(model.Endtime.Year))
                {
                    // 同年
                    if (model.ObjectType == 0)
                    {
                        Tcount.Add("TS_DataCenter_Area_Month_" + model.Starttime.Year.ToString());
                    }
                    else
                    {
                        Tcount.Add("TS_DataCenter_Device_Month_" + model.Starttime.ToString());
                    }
                }
                else
                {
                    DateTime temp = model.Starttime;
                    while (temp.Year <= model.Endtime.Year)
                    //  while (Convert.ToDateTime(temp.ToString("yyyy-MM-")+model.endtime.Day.ToString()) <= model.endtime)
                    {
                        if (model.ObjectType == 0)
                        {
                            Tcount.Add("TS_DataCenter_Area_Month_" + temp.Year.ToString());
                        }
                        else
                        {
                            Tcount.Add("TS_DataCenter_Device_Month_" + temp.Year.ToString());
                        }
                        temp = temp.AddYears(1);
                    }
                }
            }
            else
            {
                //如果是取按天的数据,存在跨年不跨年
                //if (model.Starttime.Year.Equals(model.Endtime.Year) && model.Starttime.Month.Equals(model.Endtime.Month))
                //{
                //    //同月
                //    Tcount.Add("TS_COUNT_DAY_" + model.Starttime.Year.ToString());
                //}
                if (model.Starttime.Year.Equals(model.Endtime.Year))
                {


                    if (model.ObjectType == 0)
                    {
                        //同月
                        Tcount.Add("TS_DataCenter_Area_Day_" + model.Starttime.Year.ToString());
                    }
                    else
                    {
                        //同月
                        Tcount.Add("TS_DataCenter_Device_Day_" + model.Starttime.Year.ToString());
                    }
                }

                else
                {
                    //跨月
                    //DateTime temp = model.Starttime;
                    for (int i = int.Parse(model.Starttime.Year.ToString()), j = int.Parse(model.Endtime.Year.ToString()); i <= j; i++)
                    {
                        Tcount.Add("TS_DataCenter_Area_Day_" + i);
                    }
                    //DateTime temp = model.Starttime;
                    //while ((temp.Year * 12 + temp.Month) <= model.Endtime.Year * 12 + model.Endtime.Month)
                    ////  while (Convert.ToDateTime(temp.ToString("yyyy-MM-")+model.endtime.Day.ToString()) <= model.endtime)
                    //{
                    //    Tcount.Add("TS_COUNT_DAY_" + temp.Year.ToString() + (temp.Month > 9 ? temp.Month.ToString() : "0" + temp.Month.ToString()));
                    //    temp = temp.AddMonths(1);
                    //}
                }

            }
            //if (model.Starttime.Year.Equals(model.Endtime.Year) && model.Starttime.Month.Equals(model.Endtime.Month))
            //{
            //    // 同月
            //    //Tcount.Add("TS_COUNT_" + model.Starttime.Year.ToString() + (model.Starttime.Month > 9 ? model.Starttime.Month.ToString() : "0" + model.Starttime.Month.ToString()));
            //}
            //else
            //{
            //    DateTime temp = model.Starttime;
            //    while ((temp.Year * 12 + temp.Month) <= model.Endtime.Year * 12 + model.Endtime.Month)
            //    //  while (Convert.ToDateTime(temp.ToString("yyyy-MM-")+model.endtime.Day.ToString()) <= model.endtime)
            //    {
            //        Tcount.Add("TS_COUNT_" + temp.Year.ToString() + (temp.Month > 9 ? temp.Month.ToString() : "0" + temp.Month.ToString()));
            //        temp = temp.AddMonths(1);
            //    }
            //}
            return Tcount;
        }

        #endregion



        /// <summary>
        /// 为datatable中某一列求和
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static float ColumnSum(DataTable dt, string ColumnName)
        {
            float d = 0;
            foreach (DataRow row in dt.Rows)
            {
                d += float.Parse(row[ColumnName].ToString());
            }
            return d;
        }
    }
}
