using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.DataContact;
using NTS.WEB.Model;

namespace NTS.WEB.DAL
{
    public class EnergyContrastCommon
    {
        /// <summary>
        /// 能耗集合
        /// </summary>
        /// <param name="model">查询的对象封装</param>
        /// <returns></returns>
        public static BaseResult GetBaseEneryDataListNew(BaseQueryModel model)
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
                    double tempCount = 0;
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
                        List<Model.BaseLayerObject> infoList = new List<Model.BaseLayerObject>();
                        if (model.areaType == WEB.DataContact.AreaType.Area)
                        {
                            infoList = new DAL.BaseLayerObject().GetBaseLayerObjectList(string.Format(" and LayerObjectID={0}", objects), string.Format(" order by LayerObjectID"));
                        }
                        else
                        {
                            infoList = new DAL.BaseLayerObject().GetBaseFuncLayerObjectList(string.Format(" and LayerObjectID={0}", objects), string.Format(" order by LayerObjectID"));
                        }

                        ObjectInfo = infoList.Count > 0 ? infoList[0] : null;
                        bsDatas.Datas = new List<DataItems>();
                        foreach (DataRow item in datas.Rows)
                        {
                            tempCount += double.Parse(item["CountValue"].ToString());
                            bsDatas.Datas.Add(new DataItems()
                            {
                                DatePick = item["Starttime"].ToString(),
                                DataValue = decimal.Parse(item["CountValue"].ToString()),
                                CoalDataValue = Convert.ToDecimal(double.Parse(item["CountValue"].ToString()) * bsDatas.itemCode.ItemCoal),
                                Co2DataValue = Convert.ToDecimal(double.Parse(item["CountValue"].ToString()) * bsDatas.itemCode.ItemCO2),
                                MoneyDataValue = Convert.ToDecimal(double.Parse(item["CountValue"].ToString()) * bsDatas.itemCode.ItemMoney),
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
                            tempCount += double.Parse(item["CountValue"].ToString());
                            bsDatas.Datas.Add(new DataItems()
                            {
                                DatePick = item["Starttime"].ToString(),
                                DataValue = Convert.ToDecimal(item["CountValue"].ToString()),
                                CoalDataValue = Convert.ToDecimal(double.Parse(item["CountValue"].ToString()) * bsDatas.itemCode.ItemCoal),
                                Co2DataValue = Convert.ToDecimal(double.Parse(item["CountValue"].ToString()) * bsDatas.itemCode.ItemCO2),
                                MoneyDataValue = Convert.ToDecimal(double.Parse(item["CountValue"].ToString()) * bsDatas.itemCode.ItemMoney),
                                DataValueAndDept = item["CountValue"].ToString() + bsDatas.itemCode.Unit
                            });
                        }
                        bsDatas.device = DeviceObjectInfo;
                    }
                    bsDatas.Total = Convert.ToDecimal(tempCount);
                    bsDatas.ConvertDataValueList = new System.Collections.Hashtable();
                    bsDatas.ConvertDataValueList.Add("ITEMCOAL", Math.Round((tempCount * bsDatas.itemCode.ItemCoal), 4).ToString());
                    bsDatas.ConvertDataValueList.Add("ITEMCO2", Math.Round((tempCount * bsDatas.itemCode.ItemCO2), 4).ToString());
                    bsDatas.ConvertDataValueList.Add("ITEMMONEY", Math.Round((tempCount * bsDatas.itemCode.ItemCO2), 4).ToString());
                    Res.BaseLayerObjectResults.Add(
                        !IsDevice ? ObjectInfo.LayerObjectID.ToString() : DeviceObjectInfo.DeviceID.ToString(), bsDatas);
                }
                return Res;
            }
            return null;
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
            //var cmd = new DataCommand(cmdName, new SqlCustomDbCommand());
            //cmd.SetParameterValue("@countid", model.ObjectId);
            //cmd.SetParameterValue("@itemcode", model.ItemCode);
            for (var i = 0; i < tcount.Count; i++)
            {
                var cmd = new DataCommand(cmdName, new SqlCustomDbCommand());
                cmd.SetParameterValue("@countid", model.ObjectId);
                cmd.SetParameterValue("@itemcode", model.ItemCode);

                cmd.ReplaceParameterValue("#TableName#", tcount[i]);
                //cmd.SetParameterValue("#TableName#", tcount[i]);
                //object param = cmd.GetParameterValue("#TableName#");
                //cmd.SetParameterValue("@tname", tcount[i]);
                DataTable dts;
                if (model.Unit == ChartUnit.unit_hour)
                {
                    cmd.SetParameterValue("@cdate", model.Starttime.ToString("yyyy-MM-dd"));
                }
                try
                {
                    dts = cmd.ExecuteDataSet().Tables[0];

                    dtold = i.Equals(0) ? dts.Clone() : dtold;
                    dtold = UniteDataTable(dtold, dts.Copy());
                }
                catch (Exception e)
                {
                    //throw new Exception(e.Message);
                }
            }
            return MakerData(dtold, model);
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
                    string strValue = valueList.ToString().Replace(",,", ",");
                    var listV = strValue.Length > 0
                                         ? strValue.Substring(1).Split(',')
                                         : null;
                    //listV = from m in listV where m != "" OrderBy(m=>m).ToArray();
                    listV = listV.Where(m => m != "").OrderBy(m => m).ToArray();
                    // listV     OrderBy(m=>m).ToArray();
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
                        int hourCount = 0;
                        foreach (string t in listV)
                        {
                            if (t != "")
                            {
                                #region 组合数据
                                switch (model.Unit)
                                {
                                    case ChartUnit.unit_day:

                                        if (Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Starttime).Days >= 0 &&
                                            Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Endtime).Days <= 0)
                                        {
                                            int Count = 0;
                                            if ((Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Starttime).Days != 0) &&(hourCount > 0))
                                            {
                                                TimeSpan ts = Convert.ToDateTime(listV[hourCount].Split('_')[0]) - Convert.ToDateTime(listV[hourCount - 1].Split('_')[0]);
                                                Count = ts.Days;
                                            }

                                            for (int row = 1; row < Count; row++)
                                            {
                                                DataRow drAdd = dataTable.NewRow();
                                                drAdd["CountID"] = dt.Rows[0]["CountID"].ToString();
                                                drAdd["Starttime"] = Convert.ToDateTime(t.Split('_')[0]).AddDays(row - 1);
                                                drAdd["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddDays(row);
                                                drAdd["CountValue"] = 0;
                                                dataTable.Rows.Add(drAdd);
                                            }

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
                                            int Count = 0;
                                            if (hourCount > 0)
                                            {
                                                Count = Convert.ToDateTime(listV[hourCount].Split('_')[0]).Hour - Convert.ToDateTime(listV[hourCount - 1].Split('_')[0]).Hour;
                                            }

                                            for (int row = 1; row < Count; row++)
                                            {
                                                DataRow drAdd = dataTable.NewRow();
                                                drAdd["CountID"] = dt.Rows[0]["CountID"].ToString();
                                                drAdd["Starttime"] = Convert.ToDateTime(t.Split('_')[0]).AddHours(row - 1);
                                                drAdd["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddHours(row);
                                                drAdd["CountValue"] = 0;
                                                dataTable.Rows.Add(drAdd);
                                            }

                                            DataRow dr = dataTable.NewRow();
                                            dr["CountID"] = dt.Rows[0]["CountID"].ToString();
                                            dr["Starttime"] = t.Split('_')[0];
                                            dr["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddHours(1);
                                            dr["CountValue"] = t.Split('_')[1];
                                            dataTable.Rows.Add(dr);
                                        }
                                        break;
                                    case ChartUnit.unit_month:
                                        if (Convert.ToDateTime(t.Split('_')[0]).Subtract(DateTime.Parse(model.Starttime.ToString("yyyy-MM-01"))).Days >= 0 &&
                                            Convert.ToDateTime(t.Split('_')[0]).Subtract(model.Endtime).Days <= 0)
                                        {
                                            int Count = 0;
                                            if (hourCount > 0)
                                            {
                                                int yearCha = Convert.ToDateTime(listV[hourCount].Split('_')[0]).Year - Convert.ToDateTime(listV[hourCount - 1].Split('_')[0]).Year;
                                                int monthCha = Convert.ToDateTime(listV[hourCount].Split('_')[0]).Month - Convert.ToDateTime(listV[hourCount - 1].Split('_')[0]).Month;
                                                Count = yearCha * 12 + monthCha;
                                            }

                                            for (int row = 1; row < Count; row++)
                                            {
                                                DataRow drAdd = dataTable.NewRow();
                                                drAdd["CountID"] = dt.Rows[0]["CountID"].ToString();
                                                drAdd["Starttime"] = Convert.ToDateTime(t.Split('_')[0]).AddMonths(row - 1);
                                                drAdd["Endtime"] = Convert.ToDateTime(t.Split('_')[0]).AddMonths(row);
                                                drAdd["CountValue"] = 0;
                                                dataTable.Rows.Add(drAdd);
                                            }

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
                                #endregion
                            }
                            hourCount++;
                        }
                    }

                    return dataTable;
                }
            }
            return dt;
        }
    }
}
