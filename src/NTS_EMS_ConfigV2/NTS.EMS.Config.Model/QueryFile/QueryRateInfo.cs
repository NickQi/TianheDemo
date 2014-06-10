using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model.QueryFile
{
    /// <summary>
    /// 平价模式
    /// </summary>
    public class QueryComm
    {
        #region 共用属性
        /// <summary>
        /// 应用在更新时
        /// </summary>
        public int ID { get; set; }

        #endregion

        #region Old

        /// <summary>
        /// 能源分项代码
        /// </summary>
        [DataMember]
        public string EnergyCode { get; set; }

        /// <summary>
        /// 单价数值
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        #endregion

        #region New

        /// <summary>
        /// 能源类型编号
        /// </summary>
        [DataMember]
        public string TYPEID { get; set; }

        /// <summary>
        /// 名称（电平价）
        /// </summary>
        [DataMember]
        public string CNAME { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        public double PRICE { get; set; }

        /// <summary>
        /// 结算日
        /// </summary>
        [DataMember]
        public int DATE { get; set; }

        #endregion

    }

    /// <summary>
    /// 阶梯模式
    /// </summary>
    public class QueryRate
    {
        #region Old
        /// <summary>
        /// 能源分项代码
        /// </summary>
        [DataMember]
        public string EnergyCode { get; set; }

        /// <summary>
        /// 计费方式
        /// </summary>
        [DataMember]
        public int TypeID { get; set; }

        /// <summary>
        /// 上限值
        /// </summary>
        [DataMember]
        public int MaxValue { get; set; }

        /// <summary>
        /// 下限值
        /// </summary>
        [DataMember]
        public int MinValue { get; set; }

        /// <summary>
        /// 单价数值
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        #region 更新时使用
        /// <summary>
        /// 阶梯规则ID
        /// </summary>
        [DataMember]
        public int RuleID { get; set; }

        /// <summary>
        /// 价格表中自动编号
        /// </summary>
      //  [DataMember]
     //   public int ID { get; set; }

        /// <summary>
        /// 计费小项ID
        /// </summary>
        [DataMember]
        public int ItemID { get; set; }

        #endregion
        #endregion

        #region New
        /// <summary>
        /// 主键ID  更新时使用
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        [DataMember]
        public int GEARSID { get; set; }

        /// <summary>
        /// 能源类型ID 
        /// </summary>
        [DataMember]
        public string TYPEID { get; set; }

        /// <summary>
        /// 档位名称
        /// </summary>
        [DataMember]
        public string GEARNAME { get; set; }

        /// <summary>
        /// 开始入档电度
        /// </summary>
        [DataMember]
        public int START_GEARS_VALUE { get; set; }

        /// <summary>
        /// 结束入档电度
        /// </summary>
        [DataMember]
        public int END_GEARS_VALUE { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        public double PRICE { get; set; }

        /// <summary>
        /// 结算日
        /// </summary>
        [DataMember]
        public int DATE { get; set; }
        #endregion
    }

    /// <summary>
    /// 分时模式
    /// </summary>
    public class QueryTime
    {
        #region Old
        /// <summary>
        /// ID 自动编号  更新时使用
        /// </summary>
        //public int ID { get; set; }

        /// <summary>
        /// 计费小项ID
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 单价数值
        /// </summary>
        public decimal Price { get; set; }
        #endregion

        #region New
        /// <summary>
        /// 主键ID 
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// 能源类型ID 
        /// </summary>
        [DataMember]
        public string TYPEID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string CNAME { get; set; }

        /// <summary>
        /// 开始时间 小时
        /// </summary>
        [DataMember]
        public int STARTHOUR { get; set; }

        /// <summary>
        /// 开始时间 分钟
        /// </summary>
        [DataMember]
        public int STARTMINUTE { get; set; }

        /// <summary>
        /// 结束时间 小时
        /// </summary>
        [DataMember]
        public int ENDHOUR { get; set; }

        /// <summary>
        /// 结束时间 分钟
        /// </summary>
        [DataMember]
        public int ENDMINUTE { get; set; }

        /// <summary>
        /// 尖、峰、平、谷类型ID 
        /// </summary>
        [DataMember]
        public int TYPE { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        public double PRICE { get; set; }

        /// <summary>
        /// 结算日
        /// </summary>
        [DataMember]
        public int DATE { get; set; }

        #endregion
    }
}
