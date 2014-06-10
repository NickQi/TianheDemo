using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
    /// <summary>
    /// 查询数据的底层modle
    /// </summary>
    public class BaseDataModelNew
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
        /// 统计对象的类型
        /// </summary>
        public int ObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 统计对象的CountID
        /// </summary>
        public string ItemCode
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

    }

    public class BaseMDataModelNew
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
        public string ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 统计对象的CountID
        /// </summary>
        public int CountId
        {
            get;
            set;
        }
    }
}
