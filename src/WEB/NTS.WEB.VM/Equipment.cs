using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.VM
{
    public class Equipment
    {
        public Hashtable GetKeyHash()
        {
            var res = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList("", "order by deviceid");
            var device = 0;
            device = (res.Count > 0 ? res[0].DeviceID : 0);
            var h= new Hashtable {{"DeviceNum", device}};
            return h;
        }
    }
}
