using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class IndexShopOrder
    {
        public ExecuteProcess ActionInfo { get; set; }
        public String ShopLevel { get; set; }
        public List<EneryOrder> TotalEneryOrderList { get; set; }
        public List<EneryOrder> AreaEneryOrderList { get; set; }
    }

    public class EneryOrder
    {
        public int OrderNum { get; set; }
        public string BuildingName { get; set; }
        public double EneryValue { get; set; }
    }
}
