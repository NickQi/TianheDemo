using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.Model;
namespace NTS.WEB.BLL
{
    /// <summary>
    /// 生成层级-对象id键值对
    /// </summary>
    public class LayerObjects
    {

        private List<Tree> tabel = new List<Tree>();
        private NTS.WEB.ProductInteface.IBaseLayerObject dal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateLayer();
        private Dictionary<int, string> layerDics = new Dictionary<int, string>();
        public LayerObjects()
        {

            var listObject = new NTS.WEB.BLL.BaseLayerObject().GetBaseLayerObjectList("", " order by LayerObjectID");

            foreach (var l in listObject)
            {
                // tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, deepth = l.LayerObjectDeepth, pid = l.LayerObjectParentID });
                tabel.Add(new Tree() { id = l.LayerObjectID, name = l.LayerObjectName, pid = l.LayerObjectParentID });

            }
            BuildLayers(0, 1);
        }

        
        private void BuildLayers(int pId, int level)
        {
            var rows = (from l in tabel where l.pid.Equals(pId) select l).ToList<Tree>();
            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    if (layerDics.ContainsKey(level))
                    {
                        layerDics[level] += "," + row.id.ToString();
                    }
                    else
                    {
                        layerDics.Add(level, row.id.ToString());
                    }
                    var childrows = (from l in tabel where l.pid.Equals(row.id) select l).ToList<Tree>();
                    if (childrows.Count > 0)
                    {
                        BuildLayers(row.id, level + 1);
                    }
                }
            }
        }


        public Dictionary<int, string> GetObjectLayers()
        {
            return layerDics;
        }
    }
}
