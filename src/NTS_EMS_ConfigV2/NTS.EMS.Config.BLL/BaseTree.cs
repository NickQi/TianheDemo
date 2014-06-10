using System.Collections.Generic;
using System.Text;
using NTS.WEB.Model;

namespace NTS.EMS.Config.BLL
{

    public class BaseTree
    {
        public StringBuilder Result = new StringBuilder();
        StringBuilder _sb = new StringBuilder();
        public StringBuilder OtherTreeResult = new StringBuilder();
        private readonly WEB.ProductInteface.IBaseLayerObject _dal = WEB.ProductInteface.DataSwitchConfig.CreateLayer();


        public BaseTree()
        {
            var listObject = _dal.GetBaseLayerObjectList(" ", " order by LayerObjectID");
            var listOtherObject = _dal.GetBaseFuncLayerObjectList(" ", " order by LayerObjectID");
            GetAreaTree(listObject, 1);
            GetAreaTree(listOtherObject, 0);
        }

        private static string Formate(int pId)
        {
            return pId > 0 ? "false" : "true";
        }

        private void GetAreaTree(ICollection<BaseLayerObject> listObject, int treeType)
        {
            Result.Append(_sb);
            _sb.Clear();
            if (listObject.Count <= 0) return;
            _sb.Append("[");
            foreach (var row in listObject)
            {
                _sb.Append("{\"id\":" + row.LayerObjectID + ",\"name\":\"" + row.LayerObjectName + "\",\"open\":" + Formate(row.LayerObjectParentID) + ",\"pId\":\"" + row.LayerObjectParentID + "\"");
                _sb.Append("},");
            }
            _sb = _sb.Remove(_sb.Length - 1, 1);
            _sb.Append("]");
            if (treeType == 0)
            {
                Result.Append(_sb);
            }
            else
            {
                OtherTreeResult.Append(_sb);
            }
            _sb.Clear();
        }

    }
}
