using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.ProductInteface;
using NTS.EMS.Config.Model.ResultViewFile;
using NTS.EMS.Config.Model.QueryFile;
using NTS.EMS.Config.Model;

namespace NTS.EMS.Config.BLL
{
    /// <summary>
    /// 分摊配置
    /// </summary>
    public class AlloctionBLL
    {
        private readonly IAlloction _dal = DataSwitchConfig.CreateAlloctionData();

        private readonly WEB.ProductInteface.IBaseLayerObject dal = WEB.ProductInteface.DataSwitchConfig.CreateLayer();

        private readonly WEB.ProductInteface.IReportBase _reportBll = WEB.ProductInteface.DataSwitchConfig.CreateReportBase();

        #region 获取能耗值
        //public ResultAlloction GetTreeObjByID(QueryTreeObj obj)
        //{
        //    // 查询分摊配置表中是否有数据
        //    int year = obj.SelectDate.Year;
        //    int month = obj.SelectDate.Month;
        //    List<Model.TB_ALLOCTION_CONFIG> ListConfig = _dal.GetAlloctionList(" and paytype='" + obj.EnergyID + "' and parentareaid='" + obj.ParentObjID + "' and DATEPART(month,ALLOCTION_EndDate)='" + month + "' and DATEPART(year,ALLOCTION_EndDate)='" + year + "'");
        //    ResultAlloction model = new ResultAlloction();

        //    #region 当前选中的区域下的子数据
        //    List<TreeObjList> listObj = new List<TreeObjList>();
        //    var flag = true;
        //    // 业态树
        //    if (obj.TreeInfo == 1)
        //    {
        //        var listOtherObject = dal.GetBaseFuncLayerObjectList(" and layerobjectparentid=" + obj.ParentObjID + "  ", " order by LayerObjectID");
        //        foreach (WEB.Model.BaseLayerObject o in listOtherObject)
        //        {
        //            listObj.Add(new TreeObjList() { TreeObjID = o.LayerObjectID, TreeObjName = o.LayerObjectName });
        //        }
        //    }
        //    // 区域树
        //    else if (obj.TreeInfo == 2)
        //    {
        //        var listObject = dal.GetBaseLayerObjectList("  and layerobjectparentid=" + obj.ParentObjID + " ", " order by LayerObjectID");
        //        foreach (WEB.Model.BaseLayerObject o in listObject)
        //        {
        //            listObj.Add(new TreeObjList() { TreeObjID = o.LayerObjectID, TreeObjName = o.LayerObjectName });
        //        }
        //        flag = false;
        //    }

        //    #endregion
        //    if (listObj.Count > 0)
        //    {
        //        #region 能源类型ID对应的 转化成钱的系数
        //        Model.TB_BECM_COUNTTYPE countTypeModel = _dal.GetCountType(obj.EnergyID);
        //        if (countTypeModel != null)
        //        {
        //            model.ItemMoney = double.Parse(countTypeModel.ItemMoney.ToString());
        //        }
        //        #endregion

        //        #region 所有能耗值
        //        List<int> listInt = new List<int>();
        //        listInt.Add(obj.ParentObjID);
        //        foreach (TreeObjList o in listObj)
        //        {
        //            listInt.Add(o.TreeObjID);
        //        }

        //        var model1 = new NTS.WEB.Model.BaseQueryModel();

        //        model1.IsDevice = 0;
        //        model1.ObjectList = listInt;
        //        model1.ItemCode = obj.EnergyID;
        //        model1.Unit = NTS.WEB.Model.ChartUnit.unit_month;
        //        model1.Starttime = obj.SelectDate.AddMonths(-1);
        //        model1.Endtime = obj.SelectDate;
        //        NTS.WEB.DAL.ReportBase baseBll = new WEB.DAL.ReportBase();
        //        var list = flag ? baseBll.GetBaseEneryDataList(model1, flag) : baseBll.GetBaseEneryDataList(model1);
        //        if (list.BaseLayerObjectResults == null)
        //        {
        //            return model;
        //        }
        //        #endregion

        //        Dictionary<string, NTS.WEB.Model.BaseData> dic = list.BaseLayerObjectResults;

        //        #region 子能耗值、分摊前费用(子类的能耗消耗值×转化钱的系数)
        //        for (int i = 0; i < listObj.Count; i++)
        //        {
        //            TreeObjList o = listObj[i];
        //            NTS.WEB.Model.BaseData data = dic[o.TreeObjID.ToString()];
        //            if (data != null)
        //            {
        //                o.AreaEnergyValue = data.Total;
        //                // 分摊前费用
        //                o.AreaEnergyFTValue = double.Parse(((double)data.Total * model.ItemMoney).ToString("F"));
        //            }
        //        }

        //        #endregion

        //        #region 父总能耗值
        //        // 父的总能耗值
        //        double total = 0;
        //        NTS.WEB.Model.BaseData parentObj = dic[obj.ParentObjID.ToString()];
        //        if (parentObj != null)
        //        {
        //            total = (double)parentObj.Total;
        //        }

        //        #endregion

        //        #region 待分摊费用
        //        // 子能耗总和
        //        double totalSum = listObj.Sum(a => (double)a.AreaEnergyValue);
        //        // 待分摊费用（父总能耗值-其下子能耗总和 ）×转化钱的系数
        //        model.TotalFTMoney = (total - totalSum) * model.ItemMoney;

        //        #endregion

        //        #region 分摊推荐比例  分摊推荐值
        //        for (int i = 0; i < listObj.Count; i++)
        //        {
        //            TreeObjList o = listObj[i];
        //            NTS.WEB.Model.BaseData data = dic[o.TreeObjID.ToString()];
        //            if (data != null)
        //            {
        //                // 分摊推荐比例  子类的能耗消耗值/（待分摊父类下所有子类能耗值的总和）
        //                if (totalSum != 0)
        //                {
        //                    o.AreaFTTJBL = (double)data.Total / totalSum;
        //                }
        //                else
        //                {
        //                    o.AreaFTTJBL = 0;
        //                }
        //                o.AreaFTTJBL = double.Parse((o.AreaFTTJBL * 100).ToString("F"));
        //                // 分摊推荐值  待分摊费用×分摊推荐比例
        //                var va = (model.TotalFTMoney * o.AreaFTTJBL);
        //                o.AreaFTTJZ = double.Parse(va.ToString("F"));
        //            }
        //            // 分摊实际比例
        //            if (ListConfig != null)
        //            {
        //                if (ListConfig.Count > 0)
        //                {
        //                    Model.TB_ALLOCTION_CONFIG config = ListConfig.Where(a => a.AREAID == o.TreeObjID).FirstOrDefault();
        //                    if (config != null)
        //                    {
        //                        o.AreaFTSJBL = config.CFGPERCENT;
        //                        // 给数据赋值 主键
        //                        o.ID = config.ID;
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region 按面积分摊推荐比例、按面积分摊推荐值
        //        if (listObj.Count > 0)
        //        {
        //            string whereStr = " and AreaId in (";
        //            foreach (TreeObjList o in listObj)
        //            {
        //                whereStr += o.TreeObjID + " , ";
        //            }
        //            whereStr = whereStr.Substring(0, whereStr.LastIndexOf(','));
        //            whereStr += " )";

        //            List<TB_AREA_Info> listAreaInfo = _dal.GetAreaInfoList(whereStr);
        //            if (listAreaInfo != null)
        //            {
        //                // 待分摊父类下所有子类面积值的总和
        //                double mjSum = listAreaInfo.Sum(a => (double)a.AREANUM);
        //                for (int i = 0; i < listObj.Count; i++)
        //                {
        //                    TreeObjList t = listObj[i];
        //                    TB_AREA_Info info = listAreaInfo.Where(a => a.AREAID == t.TreeObjID).FirstOrDefault();
        //                    // 按面积分摊推荐比例 (子类的面积值/待分摊父类下所有子类面积值的总和)
        //                    if (mjSum != 0)
        //                    {
        //                        t.AreaMJFTTJBL = (double)info.AREANUM / mjSum;
        //                    }
        //                    else
        //                    {
        //                        t.AreaMJFTTJBL = 0;
        //                    }
        //                    t.AreaMJFTTJBL = double.Parse((t.AreaMJFTTJBL * 100).ToString("F"));
        //                    // 按面积分摊推荐值  待分摊费用×按面积分摊推荐比例
        //                    t.AreaMJFTTJZ = double.Parse((model.TotalFTMoney * t.AreaMJFTTJBL).ToString("F"));

        //                }
        //            }
        //        }

        //        #endregion
        //        model.ListTreeObjList = listObj;

        //        // 从 分摊配置表中取数据
        //        if (ListConfig != null)
        //        {
        //            if (ListConfig.Count > 0)
        //            {
        //                //  model.TotalFTMoney = ListConfig[0].ALLOCTION_FEE;
        //                model.SJFTMoney = ListConfig[0].ALLOCTION_FEE;
        //            }
        //        }
        //        model.TotalFTMoney = double.Parse(model.TotalFTMoney.ToString("F"));
        //    }
        //    return model;
        //}
        #endregion

        public ResultAlloction GetTreeObjByID(QueryTreeObj obj)
        {
            // 查询分摊配置表中是否有数据
            int year = obj.SelectDate.Year;
            int month = obj.SelectDate.Month;
            List<Model.TB_ALLOCTION_CONFIG> ListConfig = _dal.GetAlloctionList(" and paytype='" + obj.EnergyID + "' and parentareaid='" + obj.ParentObjID + "' and DATEPART(month,ALLOCTION_EndDate)='" + month + "' and DATEPART(year,ALLOCTION_EndDate)='" + year + "'");
            ResultAlloction model = new ResultAlloction();

            #region 当前选中的区域下的子数据
            List<TreeObjList> listObj = new List<TreeObjList>();
            // 业态树
            if (obj.TreeInfo == 1)
            {
                var listOtherObject = dal.GetBaseFuncLayerObjectList(" and layerobjectparentid=" + obj.ParentObjID + "  ", " order by LayerObjectID");
                foreach (WEB.Model.BaseLayerObject o in listOtherObject)
                {
                    listObj.Add(new TreeObjList() { TreeObjID = o.LayerObjectID, TreeObjName = o.LayerObjectName });
                }
            }
            // 区域树
            else if (obj.TreeInfo == 2)
            {
                var listObject = dal.GetBaseLayerObjectList("  and layerobjectparentid=" + obj.ParentObjID + " ", " order by LayerObjectID");
                foreach (WEB.Model.BaseLayerObject o in listObject)
                {
                    listObj.Add(new TreeObjList() { TreeObjID = o.LayerObjectID, TreeObjName = o.LayerObjectName });
                }
            }

            #endregion
            if (listObj.Count > 0)
            {
                #region 所有能耗值
                string whereStr = " and TIMEID>='" + obj.SelectDate.AddMonths(-1).ToString("yyyy-MM-dd") + "' and TIMEID< '" + obj.SelectDate.ToString("yyyy-MM-dd") + "'and OBJECTTYPE='32' and ITEMCODE='" + obj.EnergyID + "' and OBJECTID in ( ";
                foreach (TreeObjList o in listObj)
                {
                    whereStr += " '" + o.TreeObjID + "', ";
                }
                whereStr += " '" + obj.ParentObjID + "')";

                // 获取所有对象的能耗值与费用值
                List<TS_FEE_DAY> listDay = _dal.GetFeeDayList(year, whereStr);

                #endregion

                if (listDay != null)
                {
                    if (listDay.Count > 0)
                    {
                        #region 子能耗值、分摊前费用(子类的能耗消耗值×转化钱的系数)
                        for (int i = 0; i < listObj.Count; i++)
                        {
                            TreeObjList o = listObj[i];
                            // NTS.WEB.Model.BaseData data = dic[o.TreeObjID.ToString()];
                            TS_FEE_DAY dayModel = listDay.Where(a => a.OBJECTID == o.TreeObjID).FirstOrDefault();
                            if (dayModel != null)
                            {
                                //  if (data != null)
                                // {
                                // 能耗值
                                if (dayModel.TOTAL != null)
                                {
                                    o.AreaEnergyValue = double.Parse(dayModel.TOTAL.ToString("F"));
                                }
                                // 分摊前费用
                                if (dayModel.TOTAL_COST != null)
                                {
                                    o.AreaEnergyFTValue = double.Parse(dayModel.TOTAL_COST.ToString("F"));
                                }
                                // }
                            }
                        }

                        #endregion

                        #region 父总能耗值
                        double total = 0;
                        double totalmoney = 0;
                        // NTS.WEB.Model.BaseData parentObj = dic[obj.ParentObjID.ToString()];
                        TS_FEE_DAY parentFeeDay = listDay.Where(a => a.OBJECTID == obj.ParentObjID).FirstOrDefault();
                        if (parentFeeDay != null)
                        {
                            total = parentFeeDay.TOTAL;
                            totalmoney = parentFeeDay.TOTAL_COST;
                        }

                        #endregion

                        #region 待分摊费用
                        // 子能耗总和
                        double totalSum = listObj.Sum(a => (double)a.AreaEnergyValue);
                        double totalSumMoney = listObj.Sum(a => (double)a.AreaEnergyFTValue);
                        // 待分摊费用（父总能耗值-其下子能耗总和 ）×转化钱的系数
                        
                      //  model.TotalFTMoney = total - totalSum;
                        model.TotalFTMoney = totalmoney - totalSumMoney;
                        #endregion

                        #region 分摊推荐比例  分摊推荐值
                        for (int i = 0; i < listObj.Count; i++)
                        {
                            TreeObjList o = listObj[i];
                            // 分摊推荐比例  子类的能耗消耗值/（待分摊父类下所有子类能耗值的总和）
                            if (totalSum != 0)
                            {
                                o.AreaFTTJBL = o.AreaEnergyValue / totalSum;
                            }
                            else
                            {
                                o.AreaFTTJBL = 0;
                            }
                            o.AreaFTTJBL = double.Parse((o.AreaFTTJBL * 100).ToString("F"));
                            // 分摊推荐值  待分摊费用×分摊推荐比例
                            var va = (model.TotalFTMoney*o.AreaFTTJBL)*0.01; // 恢复到小数
                            o.AreaFTTJZ = double.Parse(va.ToString("F"));

                            // 分摊实际比例
                            if (ListConfig != null)
                            {
                                if (ListConfig.Count > 0)
                                {
                                    Model.TB_ALLOCTION_CONFIG config = ListConfig.Where(a => a.AREAID == o.TreeObjID).FirstOrDefault();
                                    if (config != null)
                                    {
                                        o.AreaFTSJBL = config.CFGPERCENT;
                                        // 给数据赋值 主键
                                        o.ID = config.ID;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 按面积分摊推荐比例、按面积分摊推荐值
                        if (listObj.Count > 0)
                        {
                            string whereStr1 = " and AreaId in (";
                            foreach (TreeObjList o in listObj)
                            {
                                whereStr1 += o.TreeObjID + " , ";
                            }
                            whereStr1 = whereStr1.Substring(0, whereStr1.LastIndexOf(','));
                            whereStr1 += " )";

                            List<TB_AREA_Info> listAreaInfo = _dal.GetAreaInfoList(whereStr1);
                            if (listAreaInfo != null)
                            {
                                // 待分摊父类下所有子类面积值的总和
                                double mjSum = listAreaInfo.Sum(a => (double)a.AREANUM);
                                for (int i = 0; i < listObj.Count; i++)
                                {
                                    TreeObjList t = listObj[i];
                                    TB_AREA_Info info = listAreaInfo.Where(a => a.AREAID == t.TreeObjID).FirstOrDefault();
                                    // 按面积分摊推荐比例 (子类的面积值/待分摊父类下所有子类面积值的总和)
                                    if (mjSum != 0)
                                    {
                                        t.AreaMJFTTJBL = (double)info.AREANUM / mjSum;
                                    }
                                    else
                                    {
                                        t.AreaMJFTTJBL = 0;
                                    }
                                    t.AreaMJFTTJBL = double.Parse((t.AreaMJFTTJBL * 100).ToString("F"));
                                    // 按面积分摊推荐值  待分摊费用×按面积分摊推荐比例
                                    t.AreaMJFTTJZ = double.Parse((model.TotalFTMoney*t.AreaMJFTTJBL*0.01).ToString("F"));

                                }
                            }
                        }

                        #endregion
                        model.ListTreeObjList = listObj;

                        // 从 分摊配置表中取数据
                        if (ListConfig != null)
                        {
                            if (ListConfig.Count > 0)
                            {
                                model.SJFTMoney = ListConfig[0].ALLOCTION_FEE;
                            }
                        }
                        model.TotalFTMoney = double.Parse(model.TotalFTMoney.ToString("F"));
                    }
                }
            }
            return model;
        }

        public ResultRate SaveAlloctionAndLog(QueryAlloction model)
        {
            ResultRate modelResult = new ResultRate();
            string sql = "";
            if (model.ListConfig != null)
            {
                foreach (Model.TB_ALLOCTION_CONFIG c in model.ListConfig)
                {
                    if (c.ID == 0)
                    {
                        sql += string.Format(@"insert into TB_ALLOCTION_CONFIG(ParentAREAID,AREAID,ALLOCTION_FEE,CFGPERCENT,ALLOCTION_StartDate,PAYTYPE,ALLOCTION_EndDate,PAYClass)
                        values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", c.ParentAREAID, c.AREAID, c.ALLOCTION_FEE, c.CFGPERCENT, c.ALLOCTION_StartDate,
                                                                    c.PAYTYPE, c.ALLOCTION_EndDate, c.PAYClass);
                    }
                    else
                    {
                        sql += string.Format(@"update TB_ALLOCTION_CONFIG set ALLOCTION_FEE='{0}',CFGPERCENT='{1}',ALLOCTION_StartDate='{2}',ALLOCTION_EndDate='{3}',PAYClass='{4}' where ID={5}"
                                            , c.ALLOCTION_FEE, c.CFGPERCENT, c.ALLOCTION_StartDate, c.ALLOCTION_EndDate, c.PAYClass, c.ID);
                    }
                }
            }
            if (model.ConfigLog != null)
            {
                Model.TB_ALLOCTION_CONFIG_History h = model.ConfigLog;
                sql += string.Format(@" insert into TB_ALLOCTION_CONFIG_History (OPTIONUSER,CFGOBJECT,CFGDEC,CFGDATE,OPTIONTIME,PAYClass)
                                        values('{0}','{1}','{2}','{3}','{4}','{5}')", h.OPTIONUSER, h.CFGOBJECT, h.CFGDEC, h.CFGDATE, h.OPTIONTIME, h.PAYClass);
            }
            modelResult.IsSucess = _dal.SaveAlloctionAndLog(sql);
            return modelResult;
        }

        public ResultConfigLog GetConfigLog(QueryConfigLog query)
        {
            #region 返回类型
            ResultConfigLog resultLog = new ResultConfigLog();
            resultLog.LogList = new List<BaseConfigLog>();
            resultLog.Page = new Padding();
            #endregion

            #region 查询条件
            string where = string.Empty;
            if (query.StartTime != null)
            {
                if (!string.IsNullOrEmpty(query.StartTime.ToString()))
                {
                    where += " and convert(varchar(10),h.OPTIONTIME,120)>='" + query.StartTime.Value.ToString("yyyy-MM-dd") + "' ";
                }
            }
            if (query.EndTime != null)
            {
                if (!string.IsNullOrEmpty(query.EndTime.ToString()))
                {
                    where += " and convert(varchar(10),h.OPTIONTIME,120)<='" + query.EndTime.Value.ToString("yyyy-MM-dd") + "' ";
                }
            }
            if (query.AreaID != 0)
            {
                where += " and h.CFGOBJECT=" + query.AreaID;
            }
            string orderBy = " order by h.optionTime desc";
            #endregion

            #region 组织数据

            var list = _dal.GetConfigLogList(where, orderBy);
            int total = list.Count();
            resultLog.Page.Total = total;
            resultLog.Page.Current = query.PageCurrent;
            resultLog.LogList = list.Select(p =>
                new BaseConfigLog
                {
                    SysNo = p.ID,
                    CFGDATE = p.CFGDATE.ToString("yyyy-MM-dd"),
                    CFGDEC = p.CFGDEC,
                    CFGOBJECT = p.CFGOBJECT,
                    OPTIONUSER = p.OPTIONUSER,
                    OPTIONTIME = p.OPTIONTIME.ToString("yyyy-MM-dd HH:mm:ss"),
                    CNAME = p.CNAME
                }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();

            #endregion

            return resultLog;
        }
    }
}