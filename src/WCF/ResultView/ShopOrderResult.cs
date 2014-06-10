using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ShopOrderResult
    {
        public ExecuteProcess ActionInfo { get; set; }

        public List<BaseOrder> OrderList { get; set; }
        public Padding page { get; set; }
    }


    public class BaseOrder
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Energy { get; set; }

        public string Units { get; set; }
    }

    public class Padding
    {
        public int Current { get; set; }
        public int Total { get; set; }
    }
}
