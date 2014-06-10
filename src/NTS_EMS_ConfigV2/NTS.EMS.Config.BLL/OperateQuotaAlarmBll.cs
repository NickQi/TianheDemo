using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace NTS.EMS.Config.BLL
{
    public class OperateQuotaAlarmBll
    {
        NTS.EMS.Config.ProductInteface.IQuotaAlarmObject quotaAlarmOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateQuotaAlarmObject();

        /// <summary>
        /// 获取定额告警列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultQuotaAlarmList GetQuotaAlarmList(Model.QueryQuotaAlarmContact query)
        {
            ResultQuotaAlarmList result = new ResultQuotaAlarmList();
            result.ResultInfo = new ExecuteResult();
            result.QuotaAlarmList = new List<QuotaAlarmData>();
            result.Page = new Padding();
            try
            {
                StringBuilder where = new StringBuilder();

                #region 组织条件
                if (!string.IsNullOrEmpty(query.ItemCode))
                {
                    where.Append(string.Format(" and itemcode='{0}'", query.ItemCode));
                }
                if (query.AlarmType != 0)
                {
                    where.Append(string.Format(" and alarmtype={0}", query.AlarmType));
                }
                if (query.QuotaType != 0)
                {
                    where.Append(string.Format(" and quotatype={0}", query.QuotaType));
                }
                if (!string.IsNullOrEmpty(query.ObjectName))
                {
                    where.Append(string.Format(" and objectdesc like '%{0}%'", query.ObjectName.Trim()));
                }
                #endregion
                
                var list = quotaAlarmOperator.GetQuotaAlarmList(where.ToString());
                while ((query.PageCurrent - 1) * query.PageSize >= list.Count())
                {
                    query.PageCurrent--;
                }
                result.QuotaAlarmList = list
                    .Select(p => new QuotaAlarmData
                    {
                        AlarmType = p.AlarmType,
                        Id = p.Id,
                        ItemCode = p.ItemCode,
                        ObjectDesc = p.ObjectDesc,
                        ObjectId = p.ObjectId,
                        ObjectType = p.ObjectType,
                        Percent = p.Percent,
                        QuotaType = p.QuotaType
                    }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();
                result.Page.Current = query.PageCurrent;
                result.Page.Total = list.Count();

                //设置itemName 及 alarmName(减少全部转换)
                var itemList = new OperateQuotaBll().GetAllItemcodeList();
                var alarmTypeList = GetAlarmTypes("QoutaAlarm");
                foreach (var item in result.QuotaAlarmList)
                {
                    item.ItemName = GetItemNameByItemNumber(item.ItemCode, itemList);
                    item.AlarmName = GetAlarmNameByValue(item.AlarmType.ToString(), alarmTypeList);
                }

                result.ResultInfo.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.ResultInfo.Success = false;
                result.ResultInfo.ExceptionMsg = ex.Message;
                result.ResultInfo.ExtendContent = null;
                return result;
            }
        }

        /// <summary>
        /// 获取定额告警信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultQuotaAlarm GetQuotaAlarmInfo(Model.QueryQuotaAlarmSingle query)
        {
            ResultQuotaAlarm result = new ResultQuotaAlarm();
            result.ResultInfo = new ExecuteResult();
            result.QuotaAlarm = new QuotaAlarmData();
            try
            {
                StringBuilder where = new StringBuilder();

                #region 组织条件
                where.Append(string.Format(" and itemcode='{0}'", query.ItemCode));
                where.Append(string.Format(" and alarmtype={0}", query.AlarmType));
                where.Append(string.Format(" and quotatype={0}", query.QuotaType));
                where.Append(string.Format(" and objectid={0}", query.ObjectId));
                where.Append(string.Format(" and objecttype={0}", query.ObjectType));
                #endregion

                var quotaAlarm = quotaAlarmOperator.GetQuotaAlarm(where.ToString());
                if (quotaAlarm != null)
                {
                    result.QuotaAlarm = new QuotaAlarmData
                    {
                        AlarmType = quotaAlarm.AlarmType,
                        Id = quotaAlarm.Id,
                        ItemCode = quotaAlarm.ItemCode,
                        ObjectDesc = quotaAlarm.ObjectDesc,
                        ObjectId = quotaAlarm.ObjectId,
                        ObjectType = quotaAlarm.ObjectType,
                        Percent = quotaAlarm.Percent,
                        QuotaType = quotaAlarm.QuotaType
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                result.ResultInfo.Success = false;
                result.ResultInfo.ExceptionMsg = ex.Message;
                result.ResultInfo.ExtendContent = null;
                return result;
            }
        }

        /// <summary>
        /// 保存定额告警信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ExecuteResult SaveQuotaAlarm(Model.QuotaAlarmDataContact data)
        {
            ExecuteResult result = new ExecuteResult();
            try
            {
                result.Success = false;
                int count = quotaAlarmOperator.SaveQuotaAlarm(data);
                if (count > 0)
                {
                    QueryQuotaAlarmSingle query = new QueryQuotaAlarmSingle();
                    query.AlarmType = data.AlarmType;
                    query.ItemCode = data.ItemCode;
                    query.QuotaType = data.QuotaType;
                    query.ObjectType = data.ObjectType;
                    query.ObjectId = data.ObjectId;
                    var quotaAlarm = GetQuotaAlarmInfo(query);
                    if (quotaAlarm != null)
                    {
                        result.Success = true;
                        result.ExtendContent = quotaAlarm.QuotaAlarm.Id;
                    }
                }
                if (!result.Success)
                {
                    result.ExceptionMsg = "服务器断开，请联系管理员！";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ExceptionMsg = ex.Message;
                result.ExtendContent = null;
                return result;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExecuteResult DeleteQuotaAlarm(int id)
        {
            ExecuteResult result = new ExecuteResult();
            try
            {
                int count = quotaAlarmOperator.DeleteQuotaAlarm(id);
                if (count > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.ExceptionMsg = "服务器断开，请联系管理员！";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ExceptionMsg = ex.Message;
                result.ExtendContent = null;
                return result;
            }
        }

        /// <summary>
        /// 获取itemName
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public string GetItemNameByItemNumber(string itemNumber, List<NTS.WEB.Model.Itemcode> itemList)
        {
            try
            {
                return itemList.Where(p => p.ItemCodeNumber == itemNumber).First().ItemCodeName;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 根据value获取AlarmTypeName
        /// </summary>
        /// <param name="value"></param>
        /// <param name="alarmTypeList"></param>
        /// <returns></returns>
        public string GetAlarmNameByValue(string value, List<QuotaAlarmType> alarmTypeList)
        {
            try
            {
                return alarmTypeList.Where(p => p.Value == value).First().Name;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 由tagName获取类型
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public List<QuotaAlarmType> GetAlarmTypes(string tagName)
        {
            try
            {
                List<QuotaAlarmType> typeList = new List<QuotaAlarmType>();
                //string fileName = System.Configuration.ConfigurationManager.AppSettings["AlarmType"].ToString();
                //NTS.WEB.Common.XmlHelper.XmlDeserializeFromFile();
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"bin\configs\AlarmType.config";
                /*
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);
                XmlNodeList nodeList = xdoc.GetElementsByTagName(tagName);
                foreach (XmlNode node in nodeList)
                {
                    string name = node.Attributes["Name"].Value;
                    string value = node.Attributes["Value"].Value;
                    typeList.Add(new AlarmTypeData { Name = name, Value = value });
                }
                */
                var alarmTypes = Framework.Common.XmlHelper.XmlDeserializeFromFile<AlarmTypes>(fileName, Encoding.UTF8);
                return alarmTypes.quotaAlarmList.quotaAlarmType.ToList();
            }
            catch
            {
                return new List<QuotaAlarmType>();
            }
        }
    }

    #region 告警类型文件配置
    /// <summary>
    /// 告警类型，对应配置文件AlarmType.config
    /// </summary>
    public class QuotaAlarmType
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    public class QoutaAlarmlist
    {
        [XmlElement(ElementName = "QoutaAlarm")]
        public QuotaAlarmType[] quotaAlarmType { get; set; }
    }

    [XmlRoot("AlarmTypes")]
    public class AlarmTypes
    {
        [XmlElement("QoutaAlarmlist")]
        public QoutaAlarmlist quotaAlarmList { get; set; }
    }
    #endregion
   
}
