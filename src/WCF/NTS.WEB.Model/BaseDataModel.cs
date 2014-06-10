using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
    /// <summary>
    /// 查询数据的底层modle
    /// </summary>
    public class BaseDataModel
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
        /// 统计的对象
        /// </summary>
        public int ObjectId
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

    }

    public class BaseMDataModel
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
        /// 统计的对象
        /// </summary>
        public List<int> ObjectId
        {
            get;
            set;
        }
    }
}
