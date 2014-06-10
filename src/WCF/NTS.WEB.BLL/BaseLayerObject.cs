using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NTS.WEB.BLL
{


    public class BaseLayerObject
    {
        readonly NTS.WEB.ProductInteface.IBaseLayerObject _idal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        readonly NTS.WEB.ProductInteface.IDevice _idalDevice = NTS.WEB.ProductInteface.DataSwitchConfig.CreateDevice();
        public List<Model.BaseLayerObject> GetBaseLayerObjectList(string whereStr, string order)
        {
            return _idal.GetBaseLayerObjectList(whereStr, order);
        }
      
        public List<Model.BaseLayerObject> GetBaseFuncLayerObjectList(string whereStr, string order)
        {
            return _idal.GetBaseFuncLayerObjectList(whereStr, order);
        }

        public List<Model.Device> GetDeviceObjectList(string whereStr, string order)
        {
            return _idalDevice.GetDeviceList(whereStr, order);
        }
    }
}
