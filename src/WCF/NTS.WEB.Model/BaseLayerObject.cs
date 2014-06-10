using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;
using System.Data;

namespace NTS.WEB.Model
{
    public class BaseLayerObject
    {

        [DataMapping("LayerObjectID", "LayerObjectID", DbType.Int32)]
        public int LayerObjectID { get; set; }
        [DataMapping("LayerObjectNumber", "LayerObjectNumber", DbType.String)]
        public string LayerObjectNumber { get; set; }
        [DataMapping("LayerObjectName", "LayerObjectName", DbType.String)]
        public string LayerObjectName { get; set; }
        [DataMapping("LayerObjectPic", "LayerObjectPic", DbType.String)]
        public string LayerObjectPic { get; set; }
        [DataMapping("LayerObjectContent", "LayerObjectContent", DbType.String)]
        public string LayerObjectContent { get; set; }
        [DataMapping("LayerObjectParentID", "LayerObjectParentID", DbType.Int32)]
        public int LayerObjectParentID { get; set; }
        [DataMapping("BgFlag", "BgFlag", DbType.Int32)]
        public int BgFlag { get; set; }
        [DataMapping("AreaType", "AreaType", DbType.Int32)]
        public int AreaType { get; set; }
        //[DataMapping("LayerObjectDeepth", "LayerObjectDeepth", DbType.Int32)]
        //public int LayerObjectDeepth { get; set; }
        [DataMapping("AreaNum", "AreaNum", DbType.Double)]
        public double AreaNum { get; set; }
        [DataMapping("PersonNum", "PersonNum", DbType.Int32)]
        public int PersonNum { get; set; }
    }


    public class DeviceAreaID
    {

        [DataMapping("AreaID", "areaid", DbType.Int32)]
        public int AreaID { get; set; }
        
    }
}
