using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ResultView;
using Framework.Configuration;
using System.Text;

namespace InterfaceWeb
{
    public partial class Ser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sername = Request["serivename"];
            var InputValue = Request.Form["Inputs"];
            Dictionary<string, AjaxRequest> ajaxCenter = new Dictionary<string, AjaxRequest>();
            if (ajaxCenter == null)
            {
                string AjaxPath = GlobalConfigurationSource.RootConfigurationFilePath(GlobalConfigurationSource.GlobalAppSettings["AjaxActionCenter"]);
                AjaxActionList AjaxList = XmlHelper.XmlDeserializeFromFile<AjaxActionList>(AjaxPath, Encoding.UTF8);
                foreach (var ajax in AjaxList.AjaxAction)
                {
                    ajaxCenter.Add(ajax.AjaxName, ajax);
                }
            }
            switch (sername)
            {
                
                case "loginService":
                   // var login = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryLogin>(InputValue);
                    //Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAll>("UserLogin").UserLogin(login)));
                    break;

                case "userCookies":
                  //  Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IAll>("UserCookies").GetLoginCookiesInfo()));
                    break;
                default:
                    break;
            }


        }
    }
}