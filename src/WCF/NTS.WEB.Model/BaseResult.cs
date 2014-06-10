using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NTS.WEB.Model
{
    public class BaseResult
    {
        public Dictionary<string, BaseData> BaseLayerObjectResults { get; set; }
        //public Dictionary<Device, BaseData> DeviceResults { get; set; }
    }

    public class BaseData
    {
        public BaseLayerObject baseLayerObject { get; set; }
        public Device device { get; set; }
        public Itemcode itemCode { get; set; }
        public decimal Total { get; set; }
        public Hashtable ConvertDataValueList { get; set; }
        public List<DataItems> Datas { get; set; }
    }

    public class DataItems
    {
        public string DatePick { get; set; }
        public decimal DataValue { get; set; }
        public decimal CoalDataValue { get; set; }
        public decimal Co2DataValue { get; set; }
        public decimal MoneyDataValue { get; set; }
        public string DataValueAndDept { get; set; }
    }
}
