using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NTS.WEB.DataContact;

namespace InterfaceWeb
{
    public class EneryOrderList
    {
        public List<EneryOrder> TotalEneryOrderList { get; set; }
        public List<EneryOrder> AreaEneryOrderList { get; set; }
    }


    public class EneryOrder
    {
        public int OrderNum { get; set; }
        public string BuildingName { get; set; }
        public double EneryValue { get; set; }
    }

    public partial class _Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //EneryOrderList list = new EneryOrderList();
            //list.TotalEneryOrderList = new List<EneryOrder>();
            //list.TotalEneryOrderList.Add(new EneryOrder() { OrderNum = 1, BuildingName = "新城科技园1号楼", EneryValue = 1230.22 });
            //list.TotalEneryOrderList.Add(
            //    new EneryOrder() { OrderNum = 2, BuildingName = "新城科技园2号楼", EneryValue = 2230.22 }
            //    );
            //list.TotalEneryOrderList.Add(
            //    new EneryOrder() { OrderNum = 3, BuildingName = "新城科技园3号楼", EneryValue = 3230.22 }
            //    );
            //list.AreaEneryOrderList = new List<EneryOrder>();
            //list.AreaEneryOrderList.Add(new EneryOrder() { OrderNum = 1, BuildingName = "新城科技园6号楼", EneryValue = 130.22 });
            //list.AreaEneryOrderList.Add(
            //    new EneryOrder() { OrderNum = 2, BuildingName = "新城科技园7号楼", EneryValue = 230.22 }
            //    );
            //list.AreaEneryOrderList.Add(
            //    new EneryOrder() { OrderNum = 3, BuildingName = "新城科技园8号楼", EneryValue = 330.22 }
            //    );

            NTS.WEB.DataContact.QueryOrder query = new QueryOrder();
            query.ItemCode = "00000";
            query.EndTime = DateTime.Parse("2013-11-30");
            query.ObjectNum = new List<int>() {889,1};
            query.OrderWay = "asc";
            query.PageCurrent = 1;
            query.PageSize = 10;
            query.Particle = "all";
            query.StartTime = DateTime.Parse("2013-10-1");
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(query)); 
        }
    }
}
