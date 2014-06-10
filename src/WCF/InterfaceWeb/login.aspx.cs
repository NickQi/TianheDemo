using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterfaceWeb
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ajaxActionList=new AjaxActionList();
            ajaxActionList.AjaxAction = new List<AjaxRequest>();
            ajaxActionList.AjaxAction.Add(new AjaxRequest { AjaxName = "e21e1", AjaxMethod = "11111111111111111111111" });
            Framework.Common.SerializationHelper.Save(ajaxActionList, "c:\\ajax.config");
        }
    }
}