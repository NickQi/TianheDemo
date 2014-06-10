using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ItemList
    {
        public ExecuteProcess ActionInfo;
        public List<ItemSet> ItemLst { get; set; }
    }

    public class ItemSet
    {
        //分类分项id
        public string ItemCode { get; set; }
        //分类分项名称
        public string ItemName { get; set; }
    }
}
