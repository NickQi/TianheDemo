using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;
using System.ServiceModel;

namespace NTS.WEB.BLL
{
    public class Real
    {
        public ResultReal GetResultReal()
        {
            var result = new ResultReal();
            return null;
        }


        public DataTable GetRealTimeData(BaseListModel model)
        {
            int page = model.Page;
            int pagesize = model.PageSize;

            //string path = GetXmlFilePath(model);

            //var doc = new XmlDocument();

            //doc.Load(path.Trim());

            //var dt = NTS.WEB.Common.XmlHelper.Query(doc, "RealDataInfo", page, pagesize);
            //return dt;

            #region 测试
            string xml = string.Empty;
            using (ChannelFactory<Framework.Processing.Utility.IRealDataService> channelFactory = new ChannelFactory<Framework.Processing.Utility.IRealDataService>(new WSHttpBinding(), ConfigurationManager.AppSettings["realtimedataurl"]))
            {
                Framework.Processing.Utility.IRealDataService proxy = channelFactory.CreateChannel();
                using (proxy as IDisposable)
                {
                    xml = proxy.GetRealDataByDeviceId(model.ObjectId);
                }
            }

            var doc = new XmlDocument();

            doc.LoadXml(xml);

            var dt = NTS.WEB.Common.XmlHelper.Query(doc, "RealDataInfo", page, pagesize);

            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            DataRow[] rows = dt.Select("DataPoint_Type=" + model.CategoryId);
            foreach (DataRow row in rows)
            {
                newdt.Rows.Add(row.ItemArray);
            }
            return newdt;
            #endregion
        }

        private string GetXmlFilePath(BaseListModel model)
        {
            //string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "realdata\\2012-10-19-9-4-27_AI.xml";
            string filePath;
            try
            {
                int sobjectid = model.ObjectId;
                int stype = model.CategoryId;

                string queryString = "sobjectid=" + sobjectid + "&stype=" + stype;
                string queryCache = NTS.WEB.Common.CacheHelper.GetCache("CurQueryString") == null ? "" : NTS.WEB.Common.CacheHelper.GetCache("CurQueryString").ToString();
                object queryPath = NTS.WEB.Common.CacheHelper.GetCache("CurQueryPath");

                if (queryCache != string.Empty && queryPath != null && queryCache == queryString)
                {
                    filePath = queryPath.ToString();
                }
                else
                {

                    var inter = new IRealtimeData();

                    filePath = inter.sergetcurrentdata(sobjectid, 3, stype);

                    if (ConfigurationManager.AppSettings["realtimedataurl"].IndexOf("localhost", System.StringComparison.Ordinal) == -1)
                    {
                        filePath = string.Format("{0}{1}", ConfigurationManager.AppSettings["serverurl"], filePath.Substring(filePath.LastIndexOf("\\", StringComparison.Ordinal)));
                    }
                    NTS.WEB.Common.CacheHelper.SetCache("CurQueryString", queryString);
                    NTS.WEB.Common.CacheHelper.SetCache("CurQueryPath", filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return filePath;
        }


    }
}
