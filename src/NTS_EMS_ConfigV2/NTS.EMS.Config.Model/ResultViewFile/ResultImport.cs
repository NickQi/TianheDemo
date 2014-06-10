using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model.ResultViewFile
{

    public class HistoryImport
    {
        public bool Success { get; set; }
        public List<ResultImport> HistoryItem { get; set; }
        public string ErrorMsg { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class PageInfo
    {
        public int CuttrentPage { get; set; }
        public int Total { get; set; }
        public int Pages { get; set; }
    }

    public  class ResultImport
    {
       /// <summary>
        /// 序号
       /// </summary>
       [Framework.DataConfiguration.DataMapping("Id","Id",DbType.Int32)]
       public int Id { get; set; }

       /// <summary>
       /// 对象Id
       /// </summary>
       [Framework.DataConfiguration.DataMapping("ObjectId", "ObjectId", DbType.Int32)]
       public int ObjectId { get; set; }

       /// <summary>
       /// 对象名称
       /// </summary>
       public string ObjectName { get; set; }

       /// <summary>
       /// 是否区域
       /// </summary>
       [Framework.DataConfiguration.DataMapping("IsArea", "IsArea", DbType.Int32)]
       public int IsArea { get; set; }

       /// <summary>
       /// 开始时间
       /// </summary>
       [Framework.DataConfiguration.DataMapping("StartTime", "StartTime", DbType.DateTime)]
       public DateTime StartTime { get; set; }
       public string StartTimeJson
       {
           get { return StartTime.ToString("yyyy-MM-dd HH:mm:ss"); }
       }
       /// <summary>
       /// 结束时间
       /// </summary>
       [Framework.DataConfiguration.DataMapping("EndTime", "EndTime", DbType.DateTime)]
       public DateTime EndTime { get; set; }

       /// <summary>
       /// 录入值
       /// </summary>
       [Framework.DataConfiguration.DataMapping("ImportValue", "ImportValue", DbType.Decimal)]
       public decimal ImportValue { get; set; }

       /// <summary>
       /// 分类分项代码
       /// </summary>
       [Framework.DataConfiguration.DataMapping("ItemCode", "ItemCode", DbType.String)]
       public string ItemCode { get; set; }

        [Framework.DataConfiguration.DataMapping("ItemCodeName", "ItemCodeName", DbType.String)]
       public string ItemCodeName { get; set; }
        
       /// <summary>
       /// 时间颗粒
       /// </summary>
       [Framework.DataConfiguration.DataMapping("MonthType", "MonthType", DbType.Int32)]
       public int MonthType { get; set; }

       public string TimeType
       {
           get
           {
               switch (MonthType)
               {
                   case  1:
                       return "月";
                   case 2:
                       return "日";
                   default:
                       return "小时";
               }
           }
       }

       /// <summary>
       /// 操作时间
       /// </summary>
       [Framework.DataConfiguration.DataMapping("OpTime", "OpTime", DbType.DateTime)]
       public DateTime OpTime { get; set; }
       public string OpTimeJson {
           get { return OpTime.ToString("yyyy-MM-dd HH:mm:ss"); }
       }
       /// <summary>
       /// 执行Excel的批号
       /// </summary>
       [Framework.DataConfiguration.DataMapping("ExcelId", "ExcelId", DbType.Int32)]
       public int ExcelId { get; set; }

       /// <summary>
       /// 操作方式
       /// </summary>
       public string OptType {
           get { return ExcelId > -1 ? "批量导入" : "手工录入"; }
       }
    }


    #region query's condition and execute's result
    public class SaveResult
    {
        public bool IsOK { get; set; }
        public string MessageContent { get; set; }
    }

    public class ResultEnery
    {
        public decimal Total { get; set; }
    }


    public class QueryEnery
    {
        public string ItemCode { get; set; }
        public int IsArea { get; set; }
        public int DeviceId { get; set; }
        public int DateUnit { get; set; }
        public int ObjectId { get; set; }
        public int IsAreaTree { get; set; }
        public DateTime Starttime { get; set; }
        public object ObjectValue { get; set; }
    }

    public class QueryDevice
    {
        public int areaid { get; set; }
        public int treeInfo { get; set; }
        public string itemCode { get; set; }
    }

    public class HistoryQuery
    {
        public string ItemCode { get; set; }
        public int IsArea { get; set; }
        public int? DeviceId { get; set; }
        public int DateUnit { get; set; }
        public int? ObjectId { get; set; }
        public DateTime? Starttime { get; set; }
        public DateTime? Endtime { get; set; }
        public PaddingInfo PaddingInfo { get; set; }
    }

    public class PaddingInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    // upload's result
    public class ResultExcelImport
    {
        public bool Success { get; set; }
        public string MsgContent { get; set; }
    }

    #endregion
}
