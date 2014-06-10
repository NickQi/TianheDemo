using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using NTS.WEB.Common;
using NTS.WEB.Model;

namespace NTS.WEB.AjaxController
{
    public class AjaxComplexReport
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public string GetJsonReportType()
        {
            try
            {
                return Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IComplexReportService>("CoplexService").GetReportType();

            }
            catch (Exception)
            {
                return "";
            }
        }

        #region webservice


        [Framework.Common.CustomAjaxMethod]
        public string ExportEnergyNew(string starttime, string endtime, string objectid, string timeunit, string reporttype, string counttype)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "temp_file\\";
            if (reporttype == "1")
            {
                return ExportAllEnergy(starttime, endtime, objectid, timeunit, strPath);
            }
            else
            {
                return ExportItemEnergy(starttime, endtime, objectid, timeunit, counttype, strPath);
            }
        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportEnergy()
        {
            //return "";
            try
            {
                string starttime = _ntsPage.Request.Params["Starttime"].Trim();
                string endtime = DateTime.Parse(_ntsPage.Request.Params["Endtime"].Trim()).ToString("yyyy-MM-dd 23:59:59");

                string objectid = _ntsPage.Request.Params["AreaId"].Trim();

                string timeunit = _ntsPage.Request["Timeunit"].Trim();
                string reporttype = _ntsPage.Request.Params["Reporttype"].Trim();
                string counttype = _ntsPage.Request.Params["itemCode"].Trim();

                string strReturn = this.ExportEnergyNew(starttime, endtime, objectid, timeunit, reporttype, counttype);
                return strReturn;

                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                string msg = "{\"status\":\"error\",\"msg\":\"导出失败:" + ex.Message + "\"}";
                return msg;
            }
        }


        [Framework.Common.CustomAjaxMethod]
        public string ExportAllEnergy(string starttime, string endtime, string objectid, string timeunit, string strPath)
        {
            try
            {
                return Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IComplexReportService>("CoplexService").ExportAllEnergy(starttime, endtime, objectid, timeunit, strPath);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        [Framework.Common.CustomAjaxMethod]
        public string ExportItemEnergy(string starttime, string endtime, string objectid, string timeunit, string counttype, string strPath)
        {
            try
            {
                return Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IComplexReportService>("CoplexService").ExportItemEnergy(starttime, endtime, objectid, timeunit, counttype, strPath);
            }
            catch (Exception ex)
            {
                string msg = "{\"status\":\"error\",\"msg\":\"导出失败:" + ex.Message + "\"}";
                return msg;
            }

        }

        [Framework.Common.CustomAjaxMethod]
        public string GetReportType()
        {
            return Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IComplexReportService>("CoplexService").GetReportType();
        }

        /// <summary>
        /// 根据区域和项目代码获取是否含有数据
        /// <summary>
        [Framework.Common.CustomAjaxMethod]
        public int GetCountItemCodeAreaId(string itemcode, string areaid,int classid)
        {
            return
                Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IComplexReportService>("CoplexService").
                    GetCountItemCodeAreaId(itemcode, areaid, classid);
        }


        #endregion
    }
}
