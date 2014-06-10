using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model.ResultViewFile
{
    public class ResultAlloction
    {
        public List<TreeObjList> ListTreeObjList { get; set; }

        /// <summary>
        /// 推荐分摊费用
        /// </summary>
        public double TotalFTMoney { get; set; }

        /// <summary>
        /// 实际分摊费用
        /// </summary>
        public double SJFTMoney { get; set; }

        /// <summary>
        /// 能源类型对应的  转化钱系数
        /// </summary>
        public double ItemMoney { get; set; }
    }

    public class TreeObjList
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int TreeObjID { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string TreeObjName { get; set; }

        /// <summary>
        /// 能耗值
        /// </summary>
        public double AreaEnergyValue { get; set; }

        /// <summary>
        /// 分摊钱费用
        /// </summary>
        public double AreaEnergyFTValue { get; set; }

        /// <summary>
        /// 分摊推荐比例
        /// </summary>
        public double AreaFTTJBL { get; set; }

        /// <summary>
        /// 按面积分摊推荐比例
        /// </summary>
        public double AreaMJFTTJBL { get; set; }

        /// <summary>
        /// 分摊实际比例
        /// </summary>
        public double AreaFTSJBL { get; set; }

        /// <summary>
        /// 分摊推荐值
        /// </summary>
        public double AreaFTTJZ { get; set; }

        /// <summary>
        /// 按面积分摊推荐值
        /// </summary>
        public double AreaMJFTTJZ { get; set; }

        /// <summary>
        /// 配置表中的主键 更新时使用
        /// </summary>
        public int ID { get; set; }

    }

    public class ResultEnergy
    {
        /// <summary>
        /// 能源类型
        /// </summary>
        public string EnergyCode { get; set; }
    }
}
