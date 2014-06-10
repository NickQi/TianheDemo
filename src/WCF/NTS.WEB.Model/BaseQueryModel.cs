using System;
using System.Collections.Generic;
using NTS.WEB.DataContact;

namespace NTS.WEB.Model
{
    public class BaseQueryModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Starttime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Endtime
        {
            get;
            set;
        }

        /// <summary>
        /// 最小统计单元
        /// </summary>
        public ChartUnit Unit
        {
            get;
            set;
        }


        /// <summary>
        /// 统计的对象
        /// </summary>
        public List<int> ObjectList
        {
            get;
            set;
        }

        /// <summary>
        /// 分类分项
        /// </summary>
        public string ItemCode
        {
            get; 
            set;
        }
        /// <summary>
        /// 是否设备（1=设备）
        /// </summary>
        public int IsDevice
        {
            get;
            set;
        }

        /// <summary>
        /// 1: 页态树
        /// 2：区域树
        /// </summary>
        public AreaType areaType
        {
            get;
            set;
        }
    }


}
