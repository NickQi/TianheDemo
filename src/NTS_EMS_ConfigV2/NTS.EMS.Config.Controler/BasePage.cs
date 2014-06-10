using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web;
using Framework.Common;
using Framework.Common.VM;

namespace NTS.EMS.Config.Controler
{
    public class BasePage : PageCore, IHttpHandler
    {
        #region 基础全局数据
        /// <summary>
        /// EMS系统中公共的全局信息数据
        /// </summary>
        /// <returns></returns>
        public Hashtable GetCommon()
        {
            var globalData = new Hashtable
            {
                {"LoginUser", string.IsNullOrEmpty(Utils.GetCookie("userid")) ? "" : Utils.GetCookie("userid")},
                {"Config", new Model.Config()},
                {"Years", GetYears()},
                {"Months",new int[]{1,2,3,4,5,6,7,8,9,10,11,12}},
                {"DayHours",new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23}},
                {"LeftTree",new BLL.BaseTree().Result.ToString()},
                {"LeftAreaTree",new BLL.BaseTree().OtherTreeResult.ToString()},
                {"itemcodeList", new NTS.EMS.Config.BLL.OperateQuotaBll().GetItemcodeList()},
                {"allitemcodeList",new NTS.EMS.Config.BLL.OperateQuotaBll().GetAllItemcodeList()},
                {
                    "CuttureDate",new CuttureDate()
                }
            };
            return globalData;
        }


        public class CuttureDate
        {
            public string Year
            {
                get { return DateTime.Now.Year.ToString(CultureInfo.InvariantCulture); }
            }
            public string Month
            {
                get { return DateTime.Now.Month.ToString(CultureInfo.InvariantCulture); }
            }
            public string Day
            {
                get { return DateTime.Now.ToString("yyyy-MM-dd"); }
            }
            public string Hour
            {
                get { return DateTime.Now.Hour.ToString(CultureInfo.InvariantCulture); }
            }
        }

        /// <summary>
        /// 定义系统年份
        /// </summary>
        /// <returns></returns>
        private List<int> GetYears()
        {

            // WEB.ProductInteface.IReportBase _reportBll = WEB.ProductInteface.DataSwitchConfig.CreateReportBase();
            //List<int> listInt = new List<int>();
            //listInt.Add(1);
            //var model1 = new NTS.WEB.Model.BaseQueryModel
            //{
            //    IsDevice = 0,
            //    ObjectList = listInt,
            //    ItemCode = "01000",
            //    Unit = NTS.WEB.Model.ChartUnit.unit_month,
            //    Starttime = DateTime.Now.AddMonths(-6),
            //    Endtime = DateTime.Now
            //};

            //var list = new NTS.WEB.DAL.ReportBase().GetBaseEneryDataList(model1);


            var arrYear = new List<int>();
            var thisYear = DateTime.Now.Year;
            for (var i = thisYear - 10; i < thisYear + 10; i++)
            {
                arrYear.Add(i);
            }
            return arrYear;
        }
        #endregion

        #region 框架渲染页面
        /// <summary>
        /// 渲染页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Handler_Load(object sender, EventArgs e)
        {
            var callIndex = Request.QueryString["CallIndex"];
            if (callIndex != "login")
            {
                if (string.IsNullOrEmpty(Utils.GetCookie("IsLogin")))
                {
                    Response.Redirect("Login.html");
                   // Response.End();
                }
                else if(callIndex != "home")
                {
                    string userName = Framework.Common.Utils.GetCookie("userid");
                    if (!CanRedirect(callIndex, userName))
                    {
                        Response.Redirect("~/html/403.htm");
                       // Response.End();
                    }
                }
            }
            var page = new PageCore(callIndex);
            TemplateFile = page._TemplateFile;
            TemplateData = page._TemplateData;
        }


        private bool CanRedirect(string callIndex, string userName)
        {
            return new NTS.EMS.Config.BLL.RightBll().HasMenuRight(string.Format(" and upper(callindex)='{0}' and upper(U.cname)='{1}'", callIndex.ToUpper(), userName));
        }
        #endregion
    }

    #region 默认无后台处理页面
    public class BaseView
    {
        public Hashtable GetKeyHash()
        {
            return null;
        }
    }
    #endregion
}