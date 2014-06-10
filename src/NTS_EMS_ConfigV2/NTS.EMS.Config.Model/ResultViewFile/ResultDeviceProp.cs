using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class ResultDevicePropList
    {
        public ExecuteResult ResultInfo { get; set; }
        //设备树
        //public string DeviceTree { get; set; }
        public List<DevicePropInfo> DevicePropList { get; set; }

        public Padding Page { get; set; }
    }

    public class DevicePropInfo
    {
        public string DeviceName { get; set; }     //设备名称
        public string ItemCodeName { get; set; }   //分类分项名称
        public string AreaName { get; set; }       //区域名称
        public int ID { get; set; }                //设备属性ID
        public int AreaId1 { get; set; }
        public int AreaId2 { get; set; }
        public string PayTypeName { get; set; }   //费率类型名称
    }
}
