using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model
{
    /// <summary>
    /// 用户组数据增加、修改
    /// </summary>
    [DataContract]
    public class QueryDevicePropContact
    {
        [DataMember]
        public int ItemCodeId { get; set; }

        //[DataMember]
        //public int AreaId { get; set; }

        ///// <summary>
        ///// 1--区域（Area1），2--业态（Area2）
        ///// </summary>
        //[DataMember]
        //public int AreaType { get; set; }

        [DataMember]
        public string DeviceName { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }

    [DataContract]
    public class DevicePropDataContact
    {
        [DataMember]
        public int AreaId { get; set; }

        /// <summary>
        /// 1--区域（Area1），2--业态（Area2）
        /// </summary>
        [DataMember]
        public int AreaType { get; set; }

        /// <summary>
        /// 此处存取的是devicePropIds
        /// </summary>
        [DataMember]
        public List<int> DeviceIds { get; set; }

        [DataMember]
        public int ItemCodeId { get; set; }
    }

}
