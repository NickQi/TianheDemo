using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;

namespace NTS.EMS.Config.AjaxHandler
{

    public class BaseTree
    {
        private List<NTS.WEB.Model.Tree> tabel = new List<NTS.WEB.Model.Tree>();
        private List<NTS.WEB.Model.Tree> dtabel = new List<NTS.WEB.Model.Tree>();
        StringBuilder result = new StringBuilder();
        StringBuilder deviceresult = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder devicesb = new StringBuilder();
        private NTS.WEB.ProductInteface.IBaseLayerObject dal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLayer();


        public BaseTree()
        {
            try
            {

                var listObject = dal.GetBaseLayerObjectList(" ", " order by LayerObjectID");

                foreach (var l in listObject)
                {
                    // tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                    tabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                    //dtabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                    dtabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                }
                GetTreeJsonByList(0, 1);
                GetDeviceTreeJsonByList(0, 1);
            }
            catch (Exception ex)
            {


            }
        }

        public BaseTree(string itemcode, int ClassId)
        {
            try
            {
                if (ClassId == 1)
                {
                    var listObject = dal.GetBaseLayerObjectList(" ", " order by LayerObjectID");

                    foreach (var l in listObject)
                    {
                        // tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        tabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                        //dtabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        dtabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                    }
                    //GetTreeJsonByList(0, 1);
                    GetAreaTreeJsonByList(0, itemcode, 1, ClassId);
                    GetDeviceTreeJsonByList(0, 1);
                }
                else
                {
                    var listObject = dal.GetBaseFuncLayerObjectList(" ", " order by LayerObjectID");

                    foreach (var l in listObject)
                    {
                        // tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        tabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                        //dtabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        dtabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                    }
                    //GetTreeJsonByList(0, 1);
                    GetAreaTreeJsonByList(0, itemcode, 1, ClassId);
                    GetDeviceTreeJsonByList(0, 1);
                }

            }
            catch (Exception ex)
            {


            }
        }


        public BaseTree(string itemcode, int ClassId, string strNew)
        {
            try
            {
                if (ClassId == 1)
                {
                    var listObject = dal.GetBaseLayerObjectList(" ", " order by LayerObjectID");

                    foreach (var l in listObject)
                    {
                        // tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        tabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                        //dtabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        dtabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                    }
                    //GetTreeJsonByList(0, 1);
                    if (itemcode == "00000")
                    {
                        GetAreaTreeJsonByListAll(0,  1, ClassId);
                        //GetDeviceTreeJsonByListNew(0, 1);
                    }
                    else
                    {
                        GetAreaTreeJsonByListNew(0, itemcode, 1, ClassId);
                        GetDeviceTreeJsonByListNew(0, 1);
                    }

                }
                else
                {
                    var listObject = dal.GetBaseFuncLayerObjectList(" ", " order by LayerObjectID");

                    foreach (var l in listObject)
                    {
                        // tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        tabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                        //dtabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                        dtabel.Add(new NTS.WEB.Model.Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });
                    }
                    //GetTreeJsonByList(0, 1);
                    if (itemcode == "00000")
                    {
                        GetAreaTreeJsonByListAll(0, 1, ClassId);
                    }
                    else
                    {
                        GetAreaTreeJsonByListNew(0, itemcode, 1, ClassId);
                        GetDeviceTreeJsonByListNew(0, 1);
                    }
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void GetTreeJsonByList(int pId, int level)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Count > 0)
            {
                sb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + GetClass(level) + "\"");
                        //sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                        if (childrows.Count > 0)
                        {
                            sb.Append(",\"hasChildren\":false");
                            sb.Append(",\"children\":");
                            GetTreeJsonByList(row.id, level + 1);
                            result.Append(sb.ToString());
                            sb.Clear();
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
        }

        private void GetTreeJsonByListNew(int pId, int level)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Count > 0)
            {
                sb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"state\":open,\"iconCls\":\"\"");
                        //sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                        if (childrows.Count > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByList(row.id, level + 1);
                            result.Append(sb.ToString());
                            sb.Clear();
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
        }

        private void GetAreaTreeJsonByList(int pId, string itemcode, int level, int classid)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Count > 0)
            {
                sb.Append("[");

                if (IsAreaItemCode(itemcode, pId.ToString(), classid) == true)
                {
                    var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                    if (rows.Count > 0)
                    {
                        bool isIn = false;
                        foreach (var row in rows)
                        {
                            if (IsAreaItemCode(itemcode, row.id.ToString(), classid) == true)
                            {
                                isIn = true;
                                sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + GetClass(level) + "\"");
                                //sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                                var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                                if (childrows.Count > 0)
                                {
                                    //if (IsAreaItemCode(itemcode,row.id.ToString()) == true)
                                    //{
                                    sb.Append(",\"hasChildren\":false");
                                    sb.Append(",\"children\":");
                                    GetAreaTreeJsonByList(row.id, itemcode, level + 1, classid);
                                    result.Append(sb.ToString());
                                    sb.Clear();
                                    //}
                                }
                                result.Append(sb.ToString());
                                sb.Clear();
                                sb.Append("},");
                            }
                        }
                        if (isIn == true)
                        {
                            sb = sb.Remove(sb.Length - 1, 1);
                        }

                    }
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
        }

        private void GetAreaTreeJsonByListAll(int pId, int level, int classid)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Count > 0)
            {
                sb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                if (rows.Count > 0)
                {
                    bool isIn = false;
                    foreach (var row in rows)
                    {
                        isIn = true;
                        sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"state\":\"open\",\"iconCls\":\"\"");
                        //sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                        if (childrows.Count > 0)
                        {
                            //if (IsAreaItemCode(itemcode,row.id.ToString()) == true)
                            //{
                            sb.Append(",\"children\":");
                            GetAreaTreeJsonByListAll(row.id, level + 1, classid);
                            result.Append(sb.ToString());
                            sb.Clear();
                            //}
                        }
                        else
                        {
                            sb.Append(",\"children\":[]");
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    if (isIn == true)
                    {
                        sb = sb.Remove(sb.Length - 1, 1);
                    }
                }
            }
            sb.Append("]");
            result.Append(sb.ToString());
            sb.Clear();
        }


        private void GetAreaTreeJsonByListNew(int pId, string itemcode, int level, int classid)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Count > 0)
            {
                sb.Append("[");

                if (IsAreaItemCode(itemcode, pId.ToString(), classid) == true)
                {
                    var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                    if (rows.Count > 0)
                    {
                        bool isIn = false;
                        foreach (var row in rows)
                        {
                            if (IsAreaItemCode(itemcode, row.id.ToString(), classid) == true)
                            {
                                isIn = true;
                                sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"state\":\"open\",\"iconCls\":\"\"");
                                //sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                                var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                                if (childrows.Count > 0)
                                {
                                    //if (IsAreaItemCode(itemcode,row.id.ToString()) == true)
                                    //{
                                    sb.Append(",\"children\":");
                                    GetAreaTreeJsonByListNew(row.id, itemcode, level + 1, classid);
                                    result.Append(sb.ToString());
                                    sb.Clear();
                                    //}
                                }
                                else
                                {
                                    sb.Append(",\"children\":[]");
                                }
                                result.Append(sb.ToString());
                                sb.Clear();
                                sb.Append("},");
                            }
                        }
                        if (isIn == true)
                        {
                            sb = sb.Remove(sb.Length - 1, 1);
                        }

                    }
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
        }

        /// <summary>
        /// 区域Code
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        private bool IsAreaItemCode(string itemcode, string areaId, int classid)
        {

            try
            {
                //NTS.WEB.BLL.ComplexReport complex = new ComplexReport();
                //int countValue = Framework.Service.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IComplexReportService>("CoplexService").
                //   GetCountItemCodeAreaId(itemcode, areaId, classid);
                //// int countValue = complex.GetCountItemCodeAreaId(itemcode, areaId);
                //if (countValue > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        private string GetClass(int deepth)
        {
            switch (deepth)
            {
                case 1:
                    return "project";
                case 2:
                    return "build";
                case 3:
                    return "floor";
                case 4:
                    return "house";
                default:
                    return "equip";
            }
        }

        private void GetDeviceTreeJsonByList(int pId, int level)
        {
            var deepth = 0;
            // var objectlist = dal.GetBaseLayerObjectList("", " order by LayerObjectDeepth desc,LayerObjectID");
            var objectlist = dal.GetBaseLayerObjectList("", " order by   LayerObjectID");
            if (objectlist.Count > 0)
            {
                //deepth = objectlist[0].LayerObjectDeepth;
                deepth = level;
            }
            Dictionary<int, string> cacheLayer = NTS.WEB.Common.CacheHelper.GetCache("object-Layer") as Dictionary<int, string>;
            if (cacheLayer == null)
            {
                var res = new NTS.WEB.BLL.LayerObjects().GetObjectLayers();
                NTS.WEB.Common.CacheHelper.SetCache("object-Layer", res);
                cacheLayer = res;
            }
            deviceresult.Append(devicesb.ToString());
            devicesb.Clear();
            if (tabel.Count > 0)
            {
                devicesb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        devicesb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + GetClass(level) + "\"");
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                        if (childrows.Count > 0)
                        {
                            var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + row.id, " order by deviceid");
                            // if (deviceList.Count > 0 && 4 == level) //&& row.deepth==4
                            if (deviceList.Count > 0 && cacheLayer.Count == level)
                            {
                                devicesb.Append(",\"hasChildren\":true");
                            }
                            else
                            {
                                devicesb.Append(",\"hasChildren\":false");
                            }
                            devicesb.Append(",\"children\":");
                            GetDeviceTreeJsonByList(row.id, level + 1);
                            deviceresult.Append(devicesb.ToString());
                            devicesb.Clear();
                        }
                        else
                        {
                            var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + row.id, " order by deviceid");
                            //if (deviceList.Count > 0 && 4 == level)
                            if (deviceList.Count > 0 && cacheLayer.Count == level)
                            {
                                devicesb.Append(",\"hasChildren\":true");
                            }
                            else
                            {
                                devicesb.Append(",\"hasChildren\":false");
                            }
                            devicesb.Append(",\"children\":");
                            GetDeviceTreeJsonByList(row.id, level + 1);
                            deviceresult.Append(devicesb.ToString());
                            devicesb.Clear();
                        }
                        deviceresult.Append(devicesb.ToString());
                        devicesb.Clear();
                        devicesb.Append("},");
                    }
                    devicesb = devicesb.Remove(devicesb.Length - 1, 1);
                }
                devicesb.Append("]");
                deviceresult.Append(devicesb.ToString());
                devicesb.Clear();
            }
        }

        private void GetDeviceTreeJsonByListNew(int pId, int level)
        {
            var deepth = 0;
            // var objectlist = dal.GetBaseLayerObjectList("", " order by LayerObjectDeepth desc,LayerObjectID");
            var objectlist = dal.GetBaseLayerObjectList("", " order by   LayerObjectID");
            if (objectlist.Count > 0)
            {
                //deepth = objectlist[0].LayerObjectDeepth;
                deepth = level;
            }
            Dictionary<int, string> cacheLayer = NTS.WEB.Common.CacheHelper.GetCache("object-Layer") as Dictionary<int, string>;
            if (cacheLayer == null)
            {
                var res = new NTS.WEB.BLL.LayerObjects().GetObjectLayers();
                NTS.WEB.Common.CacheHelper.SetCache("object-Layer", res);
                cacheLayer = res;
            }
            deviceresult.Append(devicesb.ToString());
            devicesb.Clear();
            if (tabel.Count > 0)
            {
                devicesb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<NTS.WEB.Model.Tree>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        devicesb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"state\":\"open\",\"iconCls\":\"\"");
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<NTS.WEB.Model.Tree>();
                        if (childrows.Count > 0)
                        {
                            var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + row.id, " order by deviceid");
                            // if (deviceList.Count > 0 && 4 == level) //&& row.deepth==4
                            //if (deviceList.Count > 0 && cacheLayer.Count == level)
                            //{
                            //    devicesb.Append(",\"hasChildren\":true");
                            //}
                            //else
                            //{
                            //    devicesb.Append(",\"hasChildren\":false");
                            //}
                            devicesb.Append(",\"children\":");
                            GetDeviceTreeJsonByListNew(row.id, level + 1);
                            deviceresult.Append(devicesb.ToString());
                            devicesb.Clear();
                        }
                        else
                        {
                            var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + row.id, " order by deviceid");
                            //if (deviceList.Count > 0 && 4 == level)
                            //if (deviceList.Count > 0 && cacheLayer.Count == level)
                            //{
                            //    devicesb.Append(",\"hasChildren\":true");
                            //}
                            //else
                            //{
                            //    devicesb.Append(",\"hasChildren\":false");
                            //}
                            devicesb.Append(",\"children\":");
                            GetDeviceTreeJsonByList(row.id, level + 1);
                            deviceresult.Append(devicesb.ToString());
                            devicesb.Clear();
                        }
                        deviceresult.Append(devicesb.ToString());
                        devicesb.Clear();
                        devicesb.Append("},");
                    }
                    devicesb = devicesb.Remove(devicesb.Length - 1, 1);
                }
                devicesb.Append("]");
                deviceresult.Append(devicesb.ToString());
                devicesb.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ObjectTree GetObjectTree()
        {
            return new ObjectTree() { TreeJson = result.ToString() };
        }




        public ObjectTree GetDeviceTree()
        {
            return new ObjectTree() { TreeJson = deviceresult.ToString() };
        }

        public string GetDeviceListByArea(QueryDevice query)
        {
            StringBuilder sbTree = new StringBuilder();
            //  var result = new ResultDevice();
            //  result.DeviceUnitList = new List<DeviceUnit>();
            List<NTS.WEB.Model.Device> deviceList;
            if (query.ItemCode == "00000" || string.IsNullOrEmpty(query.ItemCode))
            {
                deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + query.AreaID, " order by deviceid");
            }
            else
            {
                string itemCodeAll = query.ItemCode;
                string tempCode = string.Empty;
                var itemcodeList = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ItemCodeNumber='" + query.ItemCode + "'", " order by itemcodeid")[0];
                var itemcodeListChild = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ParentID=" + itemcodeList.ItemcodeID, " order by itemcodeid");
                foreach (NTS.WEB.Model.Itemcode itemcode in itemcodeListChild)
                    tempCode += ",'" + itemcode.ItemCodeNumber + "'";
                itemCodeAll = tempCode.Length > 0 ? tempCode.Substring(1) : "'" + itemCodeAll + "'";
                deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and ItemCodeID in (" + itemCodeAll + ") and areaid=" + query.AreaID, " order by deviceid");
            }
            sbTree.Append("[");
            for (var device = 0; device < deviceList.Count; device++)
            {
                sbTree.Append("{\"text\": \"" + deviceList[device].DeviceName + "\",\"id\": " + deviceList[device].DeviceID + ",\"classes\": \"equip\"}");
                sbTree.Append(device == deviceList.Count - 1 ? "" : ",");
            }
            sbTree.Append("]");
            return sbTree.ToString();

        }
    }
}
