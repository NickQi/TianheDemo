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
    public class QueryDeviceItemContact
    {
        [DataMember]
        public int ItemCodeId { get; set; }

        [DataMember]
        public int PageCurrent { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 0--所有，1--未分配 2--已分配
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string DeviceName { get; set; }
    }

    [DataContract]
    public class DeviceItemDataContact
    {
        /// <summary>
        /// 此处存取的是devicePropIds
        /// </summary>
        [DataMember]
        public List<int> DeviceIds { get; set; }

        [DataMember]
        public int ItemCodeId { get; set; }
    }

}
