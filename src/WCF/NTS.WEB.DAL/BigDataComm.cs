using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.Model;

namespace NTS.WEB.DAL
{
    public class BigDataComm
    {
        /// <summary>
        /// 能耗集合
        /// </summary>
        /// <param name="model">查询的对象封装</param>
        /// <returns></returns>
        public static BaseResult GetBaseEneryDataList(BaseQueryModel model)
        {
            BaseResult Res = new BaseResult();
            Res.BaseLayerObjectResults = new Dictionary<string, BaseData>();
            //Res.DeviceResults = new Dictionary<Model.Device, BaseData>();
            bool IsDevice = model.IsDevice == 0 ? false : true;
            var datas = new DataTable();
            if (model != null && model.ObjectList.Count > 0)
            {
                foreach (var objects in model.ObjectList)
                {
                    decimal tempCount = 0;
                    BaseDataModel bmodel = new BaseDataModel();
                    bmodel.Starttime = model.Starttime;
                    if (model.Unit == ChartUnit.unit_hour)
                    {
                        bmodel.Endtime = (model.Starttime == model.Endtime)
                                             ? model.Starttime.AddDays(1).AddHours(-1)
                                             : model.Endtime;
                    }
                    else
                    {
                        bmodel.Endtime = model.Endtime;
                    }
                    bmodel.IsDevice = model.IsDevice;
                    bmodel.ItemCode = model.ItemCode;
                    bmodel.ObjectId = objects;
                    bmodel.Unit = model.Unit;
                    datas = GetBaseData(bmodel);
                    BaseData bsDatas = new BaseData();
                    bsDatas.itemCode = new Model.Itemcode();
                    var ItemCodeList = new DAL.Itemcode().GetItemcodeList(string.Format(" and ItemCodeNumber='{0}'", model.ItemCode), string.Format(" order by ItemcodeID"));
                    bsDatas.itemCode = ItemCodeList.Count > 0 ? ItemCodeList[0] : null;
                    Model.BaseLayerObject ObjectInfo = new Model.BaseLayerObject();

                    Model.Device DeviceObjectInfo = new Model.Device();
                    if (!IsDevice)
                    {
                        var infoList = new DAL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectID={0}", objects), string.Format(" order by LayerObjectID"));
                        ObjectInfo = infoList.Count > 0 ? infoList[0] : null;
                        bsDatas.Datas = new List<DataItems>();
                        foreach (DataRow item in datas.Rows)
                        {
                           
                            tempCount += decimal.Parse(item["CountValue"].ToString());
                            bsDatas.Datas.Add(new DataItems()
                            {
                                DatePick = item["Starttime"].ToString(),
                                DataValue = decimal.Parse(item["CountValue"].ToString()),
                               
                                CoalDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCoal,
                                Co2DataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCO2,
                                MoneyDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemMoney,
                                DataValueAndDept = item["CountValue"].ToString() + bsDatas.itemCode.Unit
                            });
                        }
                        bsDatas.baseLayerObject = ObjectInfo;
                    }
                    else
                    {
                        var infoList = new DAL.Device().GetDeviceList(string.Format(" and deviceid={0}", objects), string.Format(" order by DeviceID"));
                        DeviceObjectInfo = infoList.Count > 0 ? infoList[0] : null;
                        bsDatas.Datas = new List<DataItems>();
                        foreach (DataRow item in datas.Rows)
                        {
                            tempCount += decimal.Parse(item["CountValue"].ToString());
                            bsDatas.Datas.Add(new DataItems()
                            {
                                DatePick = item["Starttime"].ToString(),
                                DataValue = decimal.Parse(item["CountValue"].ToString()),
                                CoalDataValue = decimal.Parse(item["CountValue"].ToString()) *(decimal)bsDatas.itemCode.ItemCoal,
                                Co2DataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCO2,
                                MoneyDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemMoney,
                                DataValueAndDept = item["CountValue"].ToString() + bsDatas.itemCode.Unit
                            });
                        }
                        bsDatas.device = DeviceObjectInfo;
                    }
                    bsDatas.Total = tempCount;
                    bsDatas.ConvertDataValueList = new System.Collections.Hashtable();
                    bsDatas.ConvertDataValueList.Add("ITEMCOAL", decimal.Round(tempCount * (decimal)bsDatas.itemCode.ItemCoal, 4).ToString());
                    bsDatas.ConvertDataValueList.Add("ITEMCO2", decimal.Round(tempCount * (decimal)bsDatas.itemCode.ItemCO2, 4).ToString());
                    bsDatas.ConvertDataValueList.Add("ITEMMONEY", decimal.Round(tempCount * (decimal)bsDatas.itemCode.ItemMoney, 4).ToString());
                    Res.BaseLayerObjectResults.Add(
                        !IsDevice ? ObjectInfo.LayerObjectID.ToString() : DeviceObjectInfo.DeviceID.ToString(), bsDatas);
                }
                return Res;
            }
            return null;
        }
        /// <summary>
        /// 能耗集合（可区分液态及区域）
        /// </summary>
        /// <param name="model">查询的对象封装</param>
        /// <param name="IsLiquid">是否液态</param>
        /// <returns></returns>
        public static BaseResult GetBaseEneryDataList(BaseQueryModel model, bool IsLiquid)
        {
            try
            {

                BaseResult Res = new BaseResult();
                Res.BaseLayerObjectResults = new Dictionary<string, BaseData>();
                //Res.DeviceResults = new Dictionary<Model.Device, BaseData>();
                bool IsDevice = model.IsDevice == 0 ? false : true;
                var datas = new DataTable();
                if (model != null && model.ObjectList.Count > 0)
                {
                    foreach (var objects in model.ObjectList)
                    {
                        decimal tempCount = 0;
                        BaseDataModel bmodel = new BaseDataModel();
                        bmodel.Starttime = model.Starttime;
                        if (model.Unit == ChartUnit.unit_hour)
                        {
                            bmodel.Endtime = (model.Starttime == model.Endtime)
                                                 ? model.Starttime.AddDays(1).AddHours(-1)
                                                 : model.Endtime;
                        }
                        else
                        {
                            bmodel.Endtime = model.Endtime;
                        }
                        bmodel.IsDevice = model.IsDevice;
                        bmodel.ItemCode = model.ItemCode;
                        bmodel.ObjectId = objects;
                        bmodel.Unit = model.Unit;
                        datas = GetBaseData(bmodel);
                        BaseData bsDatas = new BaseData();
                        bsDatas.itemCode = new Model.Itemcode();
                        var ItemCodeList = new DAL.Itemcode().GetItemcodeList(string.Format(" and ItemCodeNumber='{0}'", model.ItemCode), string.Format(" order by ItemcodeID"));
                        bsDatas.itemCode = ItemCodeList.Count > 0 ? ItemCodeList[0] : null;
                        Model.BaseLayerObject ObjectInfo = new Model.BaseLayerObject();

                        Model.Device DeviceObjectInfo = new Model.Device();
                        if (!IsDevice)
                        {
                            var infoList = new List<Model.BaseLayerObject>();
                            if (IsLiquid)
                            {//取液态
                                infoList = new DAL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and LayerObjectID={0}", objects), string.Format(" order by LayerObjectID"));
                            }
                            else
                            {//取区域
                                infoList = new DAL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectID={0}", objects), string.Format(" order by LayerObjectID"));
                            }


                            ObjectInfo = infoList.Count > 0 ? infoList[0] : null;
                            if (ObjectInfo==null)
                            {
                                continue;
                            }
                            bsDatas.Datas = new List<DataItems>();
                            foreach (DataRow item in datas.Rows)
                            {
                                tempCount += decimal.Parse(item["CountValue"].ToString());
                                bsDatas.Datas.Add(new DataItems()
                                {
                                    DatePick = item["Starttime"].ToString(),
                                    DataValue = decimal.Parse(item["CountValue"].ToString()),
                                    CoalDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCoal,
                                    Co2DataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCO2,
                                    MoneyDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemMoney,
                                    DataValueAndDept = item["CountValue"].ToString() + bsDatas.itemCode.Unit
                                });
                            }
                            bsDatas.baseLayerObject = ObjectInfo;
                        }
                        else
                        {
                            var infoList = new DAL.Device().GetDeviceList(string.Format(" and deviceid={0}", objects), string.Format(" order by DeviceID"));
                            DeviceObjectInfo = infoList.Count > 0 ? infoList[0] : null;
                            if (DeviceObjectInfo == null)
                            {
                                continue;
                            }
                            bsDatas.Datas = new List<DataItems>();
                            foreach (DataRow item in datas.Rows)
                            {
                                tempCount += decimal.Parse(item["CountValue"].ToString());
                                bsDatas.Datas.Add(new DataItems()
                                {
                                    DatePick = item["Starttime"].ToString(),
                                    DataValue = decimal.Parse(item["CountValue"].ToString()),
                                    CoalDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCoal,
                                    Co2DataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemCO2,
                                    MoneyDataValue = decimal.Parse(item["CountValue"].ToString()) * (decimal)bsDatas.itemCode.ItemMoney,
                                    DataValueAndDept = item["CountValue"].ToString() + bsDatas.itemCode.Unit
                                });
                            }
                            bsDatas.device = DeviceObjectInfo;
                        }
                        bsDatas.Total = tempCount;
                        bsDatas.ConvertDataValueList = new System.Collections.Hashtable();
                        bsDatas.ConvertDataValueList.Add("ITEMCOAL", decimal.Round(tempCount * (decimal)bsDatas.itemCode.ItemCoal, 4).ToString());
                        bsDatas.ConvertDataValueList.Add("ITEMCO2", decimal.Round(tempCount * (decimal)bsDatas.itemCode.ItemCO2, 4).ToString());
                        bsDatas.ConvertDataValueList.Add("ITEMMONEY", decimal.Round(tempCount * (decimal)bsDatas.itemCode.ItemMoney, 4).ToString());
                        Res.BaseLayerObjectResults.Add(
                            !IsDevice ? ObjectInfo.LayerObjectID.ToString() : DeviceObjectInfo.DeviceID.ToString(), bsDatas);
                    }
                    return Res;
                }
                return null;
            }
            catch (Exception ee)
            {
                throw new Exception("数据有误");
               // return null;
            }
        }


        /// <summary>
        /// 获取表的基础数据集
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable GetBaseData(BaseDataModel model)
        {

            var dtold = new DataTable();
            var tcount = new List<string>();
            var cmdName = string.Empty;
            tcount = GetTcountNames(model, tcount);
            switch (model.Unit)
            {
                case ChartUnit.unit_hour:
                    cmdName = "getBaseDataByHour";
                    break;
                case ChartUnit.unit_day:
                    cmdName = "getBaseDataByDate";
                    break;
                case ChartUnit.unit_month:
                    cmdName = "getBaseDataByMonth";
                    break;
                default:
                    cmdName = "getBaseDataByMonth";
                    break;
            }
           
           
            for (var i = 0; i < tcount.Count; i++)
            {
                var cmd = new DataCommand(cmdName, new SqlCustomDbCommand());
                cmd.SetParameterValue("@countid", model.ObjectId);
                cmd.SetParameterValue("@itemcode", model.ItemCode);
                cmd.ReplaceParameterValue("#TableName#", tcount[i]);
                //cmd.SetParameterValue("@tname", tcount[i]);
                DataTable dts=null;
                if (model.Unit == ChartUnit.unit_hour)
                {
                    cmd.SetParameterValue("@cdate", model.Starttime.ToString("yyyy-MM-dd"));
                }
                try
                {
                    dts = cmd.ExecuteDataSet().Tables[0];
                    

                    dtold = i.Equals(0) ? dts.Clone() : dtold;
                    dtold = UniteDataTable(dtold, dts.Copy());

                  //  cmd.ReplaceParameterValue("#TableName#", "");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return MakerData(dtold, model);
        }


        private static DataTable MakerData(DataTable dt, BaseDataModel model)
        {
            var valueList = new StringBuilder();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (model.Unit)
                        {
                            case ChartUnit.unit_hour:
                                valueList.AppendFormat(",{0}", dt.Rows[i]["Value24"].ToString());
                                break;
                            case ChartUnit.unit_month:
                                valueList.AppendFormat(",{0}", dt.Rows[i]["Value30"].ToString());
                                break;
                            case ChartUnit.unit_day:
                                valueList.AppendFormat(",{0}", dt.Rows[i]["Value365"].ToString());
                                break;
                            default:
                                valueList.AppendFormat(",{0}", dt.Rows[i]["Value30"].ToString());
                                break;
                        }
                    }

                    var listV = valueList.ToString().Length > 0
                                         ? valueList.ToString().Substring(1).Split(',')
                                         : null;
                    if (listV == null) return null;
                    DataTable dataTable = TableViews.BaseTable.CreateBigBaseDataTable();

                    Dictionary<string, decimal> YearsDir = new Dictionary<string, decimal>();


                    var year = model.Endtime.Year - model.Starttime.Year + 1;
                    for (int y = model.Starttime.Year; y < model.Endtime.Year + 1; y++)
                    {
                        YearsDir.Add(y.ToString(), 0);
                    }

                    decimal[] YearData = new decimal[year];

                    if (model.Unit == ChartUnit.unit_year)
                    {
                        var count = 0;
                        foreach (var yd in YearsDir)
                        {
                            foreach (var l in listV)
                            {
                                if (l != "")
                                {
                                    if (Convert.ToDateTime(l.Split('_')[0]).Year == int.Parse(yd.Key))
                                    {
                                        YearData[count] = YearData[count] + decimal.Parse(l.Split('_')[1]);
                                    }
                                }
                            }
                            count++;
                        }
                        for (int i = 0; i < YearsDir.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["CountID"] = model.ObjectId;
                            dr["Starttime"] = model.Starttime.AddYears(i).ToString("yyyy-1-1");
                            dr["Endtime"] = model.Starttime.AddYears(i + 1).ToString("yyyy-1-1");
                            dr["CountValue"] = YearData[i];

                            dataTable.Rows.Add(dr);
                        }

                    }
                    else
                    {
                        foreach (string t in listV)
                        {
                            if (t != "")
                            {
                                //model
                                switch (model.Unit)
                                {
                                    case ChartUnit.unit_day:

                                        if (Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Starttime).Days >= 0 &&
                                            Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Endtime).Days <= 0)
                                        {
                                            DataRow dr = dataTable.NewRow();
                                            dr["CountID"] = dt.Rows[0]["CountID"].ToString();
                                            dr["Starttime"] = t.Split('_')[0];
                                            dr["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddDays(1);
                                            dr["CountValue"] = t.Split('_')[1];
                                            dataTable.Rows.Add(dr);
                                        }

                                        break;
                                    case ChartUnit.unit_hour:
                                        if (Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Starttime).Hours >= 0 &&
                                            Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Endtime).Hours <= 0)
                                        {
                                            DataRow dr = dataTable.NewRow();
                                            dr["CountID"] = dt.Rows[0]["CountID"].ToString();
                                            dr["Starttime"] = t.Split('_')[0];
                                            dr["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddHours(1);
                                            dr["CountValue"] = t.Split('_')[1];
                                            dataTable.Rows.Add(dr);
                                        }
                                        break;
                                    case ChartUnit.unit_month:
                                        if (Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Starttime).Days >= 0 &&
                                            Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Endtime).Days <= 0)
                                        {
                                            DataRow dr = dataTable.NewRow();
                                            dr["CountID"] = dt.Rows[0]["CountID"].ToString();
                                            dr["Starttime"] = t.Split('_')[0];
                                            dr["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddMonths(1);
                                            dr["CountValue"] = t.Split('_')[1];
                                            dataTable.Rows.Add(dr);
                                        }
                                        break;
                                    //case ChartUnit.unit_year:
                                    //    if (Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Starttime).Days >= 0 &&
                                    //    Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Endtime).Days <= 0)
                                    //    {

                                    //        DataRow dr = dataTable.NewRow();
                                    //        dr["CountID"] = dt.Rows[0]["CountID"].ToString();
                                    //        dr["Starttime"] = t.Split('_')[0];
                                    //        dr["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddMonths(1);
                                    //        dr["CountValue"] = t.Split('_')[1];

                                    //        dataTable.Rows.Add(dr);
                                    //    }
                                    //    break;
                                }

                            }
                        }
                    }

                    return dataTable;
                }
            }
            return dt;
        }


        /// <summary>
        /// 合并dataset数据集到一个datatable上
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newdt"></param>
        /// <returns></returns>
        private static DataTable UniteDataTable(DataTable old, DataTable newdt)
        {
            var obj = new object[old.Columns.Count];

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
        /// <param name="tcount">返回的表的集合</param>
        /// <returns></returns>
        private static List<string> GetTcountNames(BaseDataModel model, List<string> tcount)
        {
            if (model.Unit == ChartUnit.unit_hour)
            {
                if (model.IsDevice == 0)
                {

                    tcount.Add("TS_DataCenter_Area_Hour_" + model.Starttime.Year.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    tcount.Add("TS_DataCenter_Device_Hour_" + model.Starttime.Year.ToString(CultureInfo.InvariantCulture));
                }
            }
            else if (model.Unit == ChartUnit.unit_day)
            {
                if (model.Starttime.Year.Equals(model.Endtime.Year))
                {
                    // 同年
                    if (model.IsDevice == 0)
                    {
                        tcount.Add("TS_DataCenter_Area_Day_" + model.Starttime.Year.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        tcount.Add("TS_DataCenter_Device_Day_" + model.Starttime.Year.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    DateTime temp = model.Starttime;
                    while (temp.Year <= model.Endtime.Year)
                    //  while (Convert.ToDateTime(temp.ToString("yyyy-MM-")+model.endtime.Day.ToString()) <= model.endtime)
                    {
                        if (model.IsDevice == 0)
                        {
                            tcount.Add("TS_DataCenter_Area_Day_" + temp.Year.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            tcount.Add("TS_DataCenter_Device_Day_" + temp.Year.ToString(CultureInfo.InvariantCulture));
                        }
                        temp = temp.AddYears(1);
                    }
                }
            }
            else
            {
                if (model.Starttime.Year.Equals(model.Endtime.Year))
                {
                    // 同年
                    if (model.IsDevice == 0)
                    {
                        tcount.Add("TS_DataCenter_Area_Month_" + model.Starttime.Year.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        tcount.Add("TS_DataCenter_Device_Month_" + model.Starttime.Year.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    DateTime temp = model.Starttime;
                    while (temp.Year <= model.Endtime.Year)
                    {
                        if (model.IsDevice == 0)
                        {
                            tcount.Add("TS_DataCenter_Area_Month_" + temp.Year.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            tcount.Add("TS_DataCenter_Device_Month_" + temp.Year.ToString(CultureInfo.InvariantCulture));
                        }
                        temp = temp.AddYears(1);
                    }
                }
            }

            return tcount;
        }

        #endregion

        //#region old


        ///// <summary>
        ///// 判断是否是天表
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //private static bool IsDateQuery(BaseDataModel model)
        //{
        //    return (model.Starttime.Hour.Equals(0) && model.Endtime.Hour.Equals(0) && (model.Starttime.ToString("yyyy-MM-dd") != model.Endtime.ToString("yyyy-MM-dd")));
        //}


        //public static DataTable GetBomBaseData(BaseMDataModel model)
        //{
        //    var list = new List<BaseDataModel>();
        //    if (model != null)
        //    {
        //        var arr = model.ObjectId;
        //        list.AddRange(arr.Select(o => new BaseDataModel
        //            {
        //                Starttime = model.Starttime, Endtime = model.Endtime, ObjectId =o
        //            }));
        //        return GetManyBaseData(list);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static DataTable GetManyBaseData(List<BaseDataModel> list)
        //{
        //    var dataTable=new DataTable();
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        DataTable dts = GetBaseData(list[i]);
        //        dataTable = i.Equals(0) ? dts.Clone() : dataTable;
        //        dataTable = UniteDataTable(dataTable, dts.Copy());
        //    }
        //    return dataTable;
        //}



        ///// <summary>
        ///// 为datatable中某一列求和
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="columnName"></param>
        ///// <returns></returns>
        //public static float ColumnSum(DataTable dt, string columnName)
        //{
        //    return dt.Rows.Cast<DataRow>().Sum(row => float.Parse(row[columnName].ToString()));
        //}

        //#endregion
    }
}
