using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.EMS.Config.BLL
{
    public abstract class LayerObjectBll
    {
        public abstract string GetTreeObjects();
    }

    public class LayerBaseObject : LayerObjectBll
    {
        private readonly List<NTS.WEB.Model.BaseLayerObject> _tempList = new List<BaseLayerObject>();
        private readonly int _pId;
        public LayerBaseObject(int pId)
        {
            this._pId = pId;
            GetChildBaseLayerObjects(pId);
        }
        public void GetChildBaseLayerObjects(int parentId)
        {
            var result=
                new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList(
                    string.Format(" and layerobjectparentid={0}", parentId), "");
            if (result.Count <= 0) return;
            foreach (var baseLayerObject in result)
            {
                _tempList.Add(baseLayerObject);
                GetChildBaseLayerObjects(baseLayerObject.LayerObjectID);
            }
        }

        public override string GetTreeObjects()
        {
            var sbBuilder = new StringBuilder();
            foreach (var baseLayerObject in _tempList)
            {
                sbBuilder.Append(string.Format(",{0}", baseLayerObject.LayerObjectID));
            }
            var objectList = sbBuilder.ToString();
            return objectList.Length > 0 ? _pId + objectList : _pId.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class LayerFunObject : LayerObjectBll
    {
        private readonly int _pId;
        private readonly List<NTS.WEB.Model.BaseLayerObject> _tempList = new List<BaseLayerObject>();
        public LayerFunObject(int pId)
        {
            this._pId = pId;
            GetChildBaseLayerObjects(pId);
        }
        public void GetChildBaseLayerObjects(int parentId)
        {
            var result=
                new NTS.WEB.BLL.BaseLayerObject().GetBaseFuncLayerObjectList(
                    string.Format(" and layerobjectparentid={0}", parentId), "");
            if (result.Count <= 0) return;
            foreach (var baseLayerObject in result)
            {
                _tempList.Add(baseLayerObject);
                GetChildBaseLayerObjects(baseLayerObject.LayerObjectID);
            }
        }

        public override string GetTreeObjects()
        {
            var sbBuilder = new StringBuilder();
            foreach (var baseLayerObject in _tempList)
            {
                sbBuilder.Append(string.Format(",{0}", baseLayerObject.LayerObjectID));
            }
            var objectList = sbBuilder.ToString();
            return objectList.Length > 0 ? _pId + objectList : _pId.ToString(CultureInfo.InvariantCulture);
        }
    }
}
