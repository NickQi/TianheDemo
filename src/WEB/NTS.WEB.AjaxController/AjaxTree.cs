using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.Model;

namespace NTS.WEB.AjaxController
{

    public class AjaxTree
    {
        private NTS.WEB.ProductInteface.IBaseLayerObject dal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        private readonly HttpContext _ntsPage = HttpContext.Current;
        [Framework.Common.CustomAjaxMethod]
        public string ObjectTree()
        {
            var cacheTree = NTS.WEB.Common.CacheHelper.GetCache("object-tree");
            if (cacheTree != null)
            {

                return cacheTree.ToString();
            }
            //  var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetObjectTree();
            var res = new BaseTree().GetObjectTree();
            NTS.WEB.Common.CacheHelper.SetCache("object-tree", res.TreeJson);
            return res.TreeJson;
        }

        /// <summary>
        /// add by jy （区域树+页树）
        /// </summary>
        /// <returns></returns>
        [Framework.Common.CustomAjaxMethod]
        [CustomException]
        public string objectItemTree_OLD()
        {
            try
            {
                string username = Utils.GetCookie("userid");
                var loginResult =
                    Framework.Common.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").GetUserGroupID(
                        username);

                string itemcode = _ntsPage.Request["ItemCode"].ToString();
                int classid = int.Parse(_ntsPage.Request["ClassId"].ToString());
                string strCacheName = "object-tree" + itemcode + classid + loginResult;

                var cacheTree = NTS.WEB.Common.CacheHelper.GetCache(strCacheName);

                if ((cacheTree != null) && (cacheTree.ToString() != "") && (cacheTree.ToString().Length > 6))
                {
                    return cacheTree.ToString();
                }
                //  var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetObjectTree();
                var res = new BaseTree(itemcode, classid, "").GetObjectTree();
                NTS.WEB.Common.CacheHelper.SetCache(strCacheName, res.TreeJson, 10000);
                if ((res.TreeJson != "") && (res.TreeJson.ToString().Length > 8))
                {
                    return res.TreeJson;
                }
                else
                {
                    return "[]";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            //string itemcode = "01A00";
            //int classid = 1;

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private bool IsJoinItemCode(string areaid)
        //{

        //}

        [Framework.Common.CustomAjaxMethod]
        public string DeviceTree()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            if (string.IsNullOrEmpty(inputValue))
            {
                var cacheTree = NTS.WEB.Common.CacheHelper.GetCache("device-tree");
                if (cacheTree != null)
                {
                    //var treeObject = cacheTree.ToString();
                    return cacheTree.ToString();
                }
                var res = new BaseTree().GetDeviceTree();
                NTS.WEB.Common.CacheHelper.SetCache("device-tree", res.TreeJson);
                // Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetDeviceTree();
                return res.TreeJson;
            }
            else
            {

                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryDevice>(inputValue);
                var res = new BaseTree().GetDeviceListByArea(query);
                //Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IObjectTree>("ObjectTree").GetDeviceListByArea(query);
                return res;
                // return Newtonsoft.Json.JsonConvert.SerializeObject(res);
            }
            //return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }




        [Framework.Common.CustomAjaxMethod]
        public List<TreeItem> objectItemTree2()
        {
            try
            {

                string username = Utils.GetCookie("userid");
                //var loginResult =
                //    Framework.Common.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").GetUserGroupID(
                //        username);

                string itemcode = _ntsPage.Request["ItemCode"].ToString();
                int classid = int.Parse(_ntsPage.Request["ClassId"].ToString());
                var listObject = new List<BaseLayerObject>();
                var deviceareaidlist = new List<DeviceAreaID>();
                if (classid == 1)
                {
                    listObject = dal.GetBaseLayerObjectList(" ", " order by LayerObjectID", username);
                    deviceareaidlist = dal.GetDeviceAreaID1List(itemcode);
                }
                else
                {
                    listObject = dal.GetBaseFuncLayerObjectList(" ", " order by LayerObjectID", username);
                    deviceareaidlist = dal.GetDeviceAreaID2List(itemcode);
                }

                //string itemcode = "01000";
                //string username = "test888";

                deviceareaidlist.Clear();
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 300 });
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 301 });
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 302 });
                deviceareaidlist.Add(new DeviceAreaID() { AreaID = 15 });
                deviceareaidlist.Add(new DeviceAreaID() { AreaID = 4 });

                var deviceAreaids = from i in deviceareaidlist select i.AreaID;
                List<int> existAreaidList = new List<int>();
                var templayerids = from i in listObject select i.LayerObjectID;
                deviceAreaids = from i in deviceAreaids where templayerids.Contains(i) select i;//过滤设备区域集合中不包含区域对象数据
                var layers = from i in listObject where deviceAreaids.Contains(i.LayerObjectID) select i;//过滤区域对象数据集合中不包含设备区域数据
                // listObject = from i in listObject where deviceAreaids.Contains(i.LayerObjectID) select i;//过滤区域对象数据集合中不包含设备区域数据

                if (layers.Count() == 0)
                {
                    return new List<TreeItem>();
                }


                List<BaseLayerObject> removelayersList = new List<BaseLayerObject>();
                int deeps = 1;
                //FilterArea(listObject, removelayersList, deviceAreaids, 0, existAreaidList, ref deeps);
                //int count = deviceAreaids.Count() + 2;

                List<int> deeps_list = new List<int>();

                while (true)
                {
                    deeps_list.Add(deeps);

                    deeps = 1;
                    foreach (var layer in removelayersList)
                    {
                        listObject.Remove(layer);
                    }
                    removelayersList.Clear();
                    if (deeps_list.Count > 1)
                    {
                        if (deeps_list[deeps_list.Count - 1] == deeps_list[deeps_list.Count - 2])
                        {
                            break;
                        }
                    }
                    FilterArea(listObject, removelayersList, deviceAreaids, 0, existAreaidList, ref deeps);
                    //FilterArea(listObject, removelayersList, deviceAreaids, 0, existAreaidList);
                }

                List<TreeItem> result = new List<TreeItem>();
                if (listObject.Count > 0)
                {
                    LayerToResult(result, listObject, 0);
                    return result;
                }

                return new List<TreeItem>();

            }
            catch (Exception ex)
            {
                return new List<TreeItem>();

            }
        }

        [Framework.Common.CustomAjaxMethod]
        public List<TreeItem> objectItemTree4devicearea()
        {
            try
            {

                string username = Utils.GetCookie("userid");
                //var loginResult =
                //    Framework.Common.BaseWcf.CreateChannel<NTS.WEB.ServiceInterface.IUser>("UserLogin").GetUserGroupID(
                //        username);

                string itemcode = _ntsPage.Request["ItemCode"].ToString();
                int classid = int.Parse(_ntsPage.Request["ClassId"].ToString());
                var listObject = new List<BaseLayerObject>();
                var deviceareaidlist = new List<DeviceAreaID>();
                if (classid == 1)
                {
                    listObject = dal.GetBaseLayerObjectList(" ", " order by LayerObjectID", username);
                    deviceareaidlist = dal.GetDeviceAreaID1List(itemcode);
                }
                else
                {
                    listObject = dal.GetBaseFuncLayerObjectList(" ", " order by LayerObjectID", username);
                    deviceareaidlist = dal.GetDeviceAreaID2List(itemcode);
                }

                //string itemcode = "01000";
                //string username = "test888";

                // deviceareaidlist.Clear();
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 300 });
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 301 });
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 302 });
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 15 });
                //deviceareaidlist.Add(new DeviceAreaID() { AreaID = 4 });

                var deviceAreaids = from i in deviceareaidlist select i.AreaID;
                List<int> existAreaidList = new List<int>();
                var templayerids = from i in listObject select i.LayerObjectID;
                deviceAreaids = from i in deviceAreaids where templayerids.Contains(i) select i;//过滤设备区域集合中不包含区域对象数据
                //var layers = from i in listObject where deviceAreaids.Contains(i.LayerObjectID) select i;//过滤区域对象数据集合中不包含设备区域数据

                //if (layers.Count() == 0)
                //{
                //    return new List<TreeItem>();
                //}


                List<BaseLayerObject> removelayersList = new List<BaseLayerObject>();
                FilterArea(listObject, removelayersList, deviceAreaids, 0, existAreaidList);
                while (removelayersList.Count > 0)
                {
                    foreach (var layer in removelayersList)
                    {
                        listObject.Remove(layer);
                    }
                    removelayersList.Clear();
                    FilterArea(listObject, removelayersList, deviceAreaids, 0, existAreaidList);
                }

                List<TreeItem> result = new List<TreeItem>();
                if (listObject.Count > 0)
                {
                    LayerToResult(result, listObject, 0);
                    return result;
                }

                return new List<TreeItem>();

            }
            catch (Exception ex)
            {
                return new List<TreeItem>();

            }
        }



        private void FilterArea(List<BaseLayerObject> layers, List<BaseLayerObject> removelayersList, IEnumerable<int> deviceareaids, int pValue, List<int> exitareaid, ref int deeps)
        {
            deeps++;
            var removeareaidlist = from i in removelayersList select i.LayerObjectID;
            if (!exitareaid.Contains(pValue) && !removeareaidlist.Contains(pValue))
            {
                var child = layers.Where(model => model.LayerObjectParentID == pValue);
                if (child.Count() == 0)
                {
                    var lastchildlayer = layers.Where(model => model.LayerObjectParentID == (layers.Find(m => m.LayerObjectID == pValue).LayerObjectParentID));
                    //lastchildlayer = lastchildlayer.Where(model=>layers.Find(m => m.LayerObjectParentID ==model.LayerObjectID) );
                    // lastchildlayer = from i in lastchildlayer where layers.Find(m => m.LayerObjectParentID ==i.LayerObjectID) == null select i;
                    lastchildlayer = lastchildlayer.Where(i => layers.Find(m => m.LayerObjectParentID == i.LayerObjectID) == null);
                    var removelayers = from i in lastchildlayer where !deviceareaids.Contains(i.LayerObjectID) select i;
                    exitareaid.AddRange(from i in lastchildlayer where deviceareaids.Contains(i.LayerObjectID) select i.LayerObjectID);
                    if (removelayers.Count() > 0)
                    {
                        removelayersList.AddRange(removelayers);
                    }
                }

                foreach (var c in child)
                {
                    FilterArea(layers, removelayersList, deviceareaids, c.LayerObjectID, exitareaid, ref deeps);
                }

            }
        }

        private void FilterArea(List<BaseLayerObject> layers, List<BaseLayerObject> removelayersList, IEnumerable<int> deviceareaids, int pValue, List<int> exitareaid)
        {
            var removeareaidlist = from i in removelayersList select i.LayerObjectID;
            if (!exitareaid.Contains(pValue) && !removeareaidlist.Contains(pValue))
            {
                var child = layers.Where(model => model.LayerObjectParentID == pValue);
                if (child.Count() == 0)
                {
                    var lastchildlayer = layers.Where(model => model.LayerObjectParentID == (layers.Find(m => m.LayerObjectID == pValue).LayerObjectParentID));
                    //lastchildlayer = lastchildlayer.Where(model=>layers.Find(m => m.LayerObjectParentID ==model.LayerObjectID) );
                    // lastchildlayer = from i in lastchildlayer where layers.Find(m => m.LayerObjectParentID ==i.LayerObjectID) == null select i;
                    lastchildlayer = lastchildlayer.Where(i => layers.Find(m => m.LayerObjectParentID == i.LayerObjectID) == null);
                    var removelayers = from i in lastchildlayer where !deviceareaids.Contains(i.LayerObjectID) select i;
                    exitareaid.AddRange(from i in lastchildlayer where deviceareaids.Contains(i.LayerObjectID) select i.LayerObjectID);
                    if (removelayers.Count() > 0)
                    {
                        removelayersList.AddRange(removelayers);
                    }
                }
                foreach (var c in child)
                {
                    FilterArea(layers, removelayersList, deviceareaids, c.LayerObjectID, exitareaid);
                }

            }
        }

        private void LayerToResult(List<TreeItem> treeItems, List<BaseLayerObject> layers, int pValue)
        {
            var child = layers.Where(model => model.LayerObjectParentID == pValue);
            if (child.Count() < 1)
            {
                return;
            }
            foreach (var c in child)
            {
                TreeItem treeItem = new TreeItem()
                {
                    id = c.LayerObjectID,
                    text = c.LayerObjectName,
                    children = new List<TreeItem>()
                };
                treeItems.Add(treeItem);
                LayerToResult(treeItem.children, layers, c.LayerObjectID);
            }

        }


        [Framework.Common.CustomAjaxMethod]
        public List<TreeItem> objectItemTree()
        {
            try
            {
                var inputValue = _ntsPage.Request.Form["Inputs"];
                var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryTree>(inputValue);
                if (query.Level==null)
                {
                    query.Level = 2;
                }
                string where = "";
                if (query.ParentID!=null)
                {
                    where = string.Format(" and layerobjectparentid={0}", query.ParentID);
                }
                var listObject = new List<BaseLayerObject>();
                string username = Utils.GetCookie("userid");
                //string username = "admin";
                if (query.ClassId == AreaType.Area)
                {
                    listObject = dal.GetBaseLayerObjectList(where, " order by LayerObjectID", username);
                }
                else
                {
                    listObject = dal.GetBaseFuncLayerObjectList(where, " order by LayerObjectID", username);
                }
                List<TreeItem> result = new List<TreeItem>();
                if (listObject.Count > 0)
                {
                    //int pvalue = string.IsNullOrEmpty(query.GroupId) ? 0 : int.Parse(query.GroupId);
                    if (query.ParentID==null)
                    {
                        LayerToResult(result, listObject, 0, (int)query.Level);
                    }
                    else
                    {
                        LayerToResult(result, listObject);
                    }


                    return result;
                }

                return new List<TreeItem>();

            }
            catch (Exception ex)
            {
                return new List<TreeItem>();

            }
        }


        private void LayerToResult(List<TreeItem> treeItems, List<BaseLayerObject> layers, int pValue, int level)
        {
            var child = layers.Where(model => model.LayerObjectParentID == pValue);
            if (child.Count() < 1)
            {
                return;
            }
            if (level > 0)
            {
                level--;
                foreach (var c in child)
                {
                    TreeItem treeItem = new TreeItem()
                    {
                        id = c.LayerObjectID,
                        text = c.LayerObjectName,
                        state = level == 1 ? "open" : "closed",
                        iconCls="",
                        children = new List<TreeItem>()
                         
                         
                    };
                    treeItems.Add(treeItem);
                    LayerToResult(treeItem.children, layers, c.LayerObjectID, level);
                }
            }
            else
            {
                if (level == -1)
                {
                    foreach (var c in child)
                    {
                        TreeItem treeItem = new TreeItem()
                        {
                            id = c.LayerObjectID,
                            text = c.LayerObjectName,
                            state =  "open",
                            iconCls = "",
                            children = new List<TreeItem>()
                        };
                        treeItems.Add(treeItem);
                        LayerToResult(treeItem.children, layers, c.LayerObjectID, level);
                    }
                }
            }


        }

        private void LayerToResult(List<TreeItem> treeItems, List<BaseLayerObject> layers)
        {
            foreach (var c in layers)
            {
                TreeItem treeItem = new TreeItem()
                {
                    id = c.LayerObjectID,
                    text = c.LayerObjectName,
                    state = "closed",
                    iconCls = "",
                    children = new List<TreeItem>()
                };
                treeItems.Add(treeItem);

            }
        }



    }
}
