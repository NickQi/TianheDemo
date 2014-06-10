using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.Model;

namespace NTS.WEB.BLL
{
    public class BaseTree
    {
        private  List<Tree> tabel = new List<Tree>();
        private List<Tree> dtabel = new List<Tree>();
         StringBuilder result = new StringBuilder();
         StringBuilder deviceresult = new StringBuilder();
         StringBuilder sb = new StringBuilder();
         StringBuilder devicesb = new StringBuilder();
        private NTS.WEB.ProductInteface.IBaseLayerObject dal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        public BaseTree()
        {
            var listObject = dal.GetBaseLayerObjectList("", " order by LayerObjectID");
            foreach (var l in listObject)
            {
                tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName,  pid = l.LayerObjectParentID });
                dtabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName,  pid = l.LayerObjectParentID });
            }
            GetTreeJsonByList(0);
            GetDeviceTreeJsonByList(0);
        }
        private  void GetTreeJsonByList(int pId)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Count > 0)
            {
                sb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<Tree>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        //sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" +GetClass(row.deepth)  + "\"");
                        sb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                        
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<Tree>();
                        if (childrows.Count > 0)
                        {
                            sb.Append(",\"hasChildren\":false");
                            sb.Append(",\"children\":");
                            GetTreeJsonByList(row.id);
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

        private void GetDeviceTreeJsonByList(int pId)
        {
            var deepth = 0;
            var objectlist = dal.GetBaseLayerObjectList("", " order by  LayerObjectID");
            //if (objectlist.Count > 0)
            //{
            //    deepth = objectlist[0].LayerObjectDeepth;
            //}
            deviceresult.Append(devicesb.ToString());
            devicesb.Clear();
            if (tabel.Count > 0)
            {
                devicesb.Append("[");

                var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<Tree>();
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        //devicesb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + GetClass(row.deepth) + "\"");
                        devicesb.Append("{\"id\":" + row.id + ",\"text\":\"" + row.name + "\",\"expanded\":false,\"classes\":\"" + "build" + "\"");
                        var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<Tree>();
                        if (childrows.Count > 0)
                        {
                            var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + row.id, " order by deviceid");
                            if (deviceList.Count > 0 && row.id == deepth)
                            {
                                devicesb.Append(",\"hasChildren\":true");
                            }
                            else
                            {
                                devicesb.Append(",\"hasChildren\":false");
                            }
                            devicesb.Append(",\"children\":");
                            GetDeviceTreeJsonByList(row.id);
                            deviceresult.Append(devicesb.ToString());
                            devicesb.Clear();
                        }else
                        {
                            var deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + row.id, " order by deviceid");
                            if (deviceList.Count > 0 && row.id == deepth)
                            {
                                devicesb.Append(",\"hasChildren\":true");
                            }
                            else
                            {
                                devicesb.Append(",\"hasChildren\":false");
                            }
                            devicesb.Append(",\"children\":");
                            GetDeviceTreeJsonByList(row.id);
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
            return new ObjectTree() {TreeJson = result.ToString()};
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
           List<Device> deviceList;
           if(query.ItemCode=="00000")
           {
               deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and areaid=" + query.AreaID, " order by deviceid"); 
           }
           else
           {
               string itemCodeAll = query.ItemCode;
               var itemcodeList = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ItemCodeNumber='" + query.ItemCode + "'", " order by itemcodeid")[0];
               var itemcodeListChild = new NTS.WEB.BLL.Itemcode().GetItemcodeList("  and ParentID=" + itemcodeList.ParentID, " order by itemcodeid");
               itemCodeAll = itemcodeListChild.Aggregate(itemCodeAll, (current, itemcode) => current + ("," + itemcode.ItemcodeID));
               deviceList = new NTS.WEB.BLL.BaseLayerObject().GetDeviceObjectList(" and ItemCodeID in (" + itemCodeAll + ") and areaid=" + query.AreaID, " order by deviceid"); 
           }
           sbTree.Append("[");
           for (var device=0; device< deviceList.Count;device++)
           {
               sbTree.Append("{\"text\": \"" + deviceList[device].DeviceName + "\",\"id\": " + deviceList[device].DeviceID + ",\"classes\": \"equip\"}");
               sbTree.Append(device == deviceList.Count - 1 ? "" : ",");
           }
           sbTree.Append("]");
            return sbTree.ToString();

       }

     

    }


   
}
