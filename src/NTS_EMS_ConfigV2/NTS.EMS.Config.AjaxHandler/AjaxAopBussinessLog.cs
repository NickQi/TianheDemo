using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using PostSharp.Laos;
using NTS.EMS.Config.BLL;
using NTS.EMS.Config.Model.ResultViewFile;

namespace NTS.EMS.Config.AjaxHandler
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AjaxAopBussinessLogAttribute : OnMethodBoundaryAspect
    {
        public OperatorType LogType { get; set; }
        public string ModelName { get; set; }
        public string LogContent { get; set; }

        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
        }

        public override void OnSuccess(MethodExecutionEventArgs eventArgs)
        {
            SetBussnessLog(eventArgs.ReturnValue);
        }

        /// <summary>
        /// 设置SystemLog
        /// </summary>
        /// <param name="id"></param>
        private void SetBussnessLog(object id)
        {
            //设置模块类型
            var modelType = SetModelTypeByModeName(ModelName);
            //如果没有实例化模块，使用Other
            if (modelType == ModelType.Other)
            {
                id = ModelName;
            }

            //实例化模块，获取日志内容
            Formate formateContent = FormateFactory.CreateFormateFactory(modelType);
            LogContent = formateContent.FormateContent(id);

            //组织数据
            var query = new QueryBussinessLog()
            {
                ModelName = ModelName,
                ModelType = (int)LogType,
                OperatorContent = LogContent,
                OperatorTime = System.DateTime.Now,
                UserName = Framework.Common.Utils.GetCookie("userid")
            };
            new LogAndExpiction().SetBussinessLog(query);
        }

        /// <summary>
        /// 设置模块类型
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        private ModelType SetModelTypeByModeName(string modelName)
        {
            ModelType mtype = ModelType.Other;

            switch (modelName)
            {
                case "定额配置":
                    mtype = ModelType.Qouta;
                    break;
                case "分摊配置":
                    mtype = ModelType.Alloction;
                    break;
                case "用户组配置":
                    mtype = ModelType.UserGroup;
                    break;
                case "用户配置":
                    mtype = ModelType.User;
                    break;
            }
            return mtype;
        }
    }

    #region 枚举类型

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperatorType
    {
        Operator = 1,
        Config = 2
    }

    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModelType
    {
        Qouta,
        Alloction,
        UserGroup,
        User,
        Other

    }

    #endregion


    #region 格式化内容

    /// <summary>
    /// 抽象类
    /// </summary>
    public abstract class Formate
    {
        protected string _userName;//用户

        /// <summary>
        /// 无参数构造方法，初始化公共字段
        /// </summary>
        public Formate()
        {
            _userName = Framework.Common.Utils.GetCookie("userid");
        }

        /// <summary>
        /// 由primarykey组织返回的日志内容
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public abstract string FormateContent(object primaryKey);
    }

    /// <summary>
    /// 其他模块日志内容，此时primaryKey 对应的是模块名称
    /// </summary>
    public class OtherForMate : Formate
    {
        public override string FormateContent(object primaryKey)
        {
            string content = string.Empty;
            content = string.Format("{0}用户于{1}操作了【{2}】模块。", _userName, DateTime.Now.ToString(), primaryKey);
            return content;
        }
    }

    /// <summary>
    /// 定额内容
    /// </summary>
    public class QuotaFormate : Formate
    {
        public override string FormateContent(object primaryKey)
        {
            try
            {
                #region 变量

                string content = string.Empty;
                int id = (int)primaryKey;
                string itemCodeName = string.Empty;
                string objectDes = string.Empty;
                string editTime = string.Empty;
                string quotaDate = string.Empty;
                string type = string.Empty;
                string oldValue = string.Empty;
                string newValue = string.Empty;
                string objectUnit = string.Empty;
                OperateQuotaBll quotaOper = new OperateQuotaBll();
                #endregion

                #region 数据来源

                var quotaInfo = quotaOper.GetQuotaInfoById(id);
                var itemCodeInfo = new NTS.WEB.BLL.Itemcode().GetItemcodeList(string.Format(" and itemcodenumber='{0}'", quotaInfo.QuotaData.ItemCode), "")[0];
                QuotaLog newQoutaLog = quotaInfo.QuotaLogList[0];

                #endregion

                #region 组织数据

                itemCodeName = itemCodeInfo.ItemCodeName;
                objectUnit = itemCodeInfo.Unit;
                objectDes = quotaInfo.QuotaData.ObjectDesc;
                newValue = quotaInfo.QuotaData.QuotaValue.ToString();
                editTime = newQoutaLog.LogTime.ToString("yyyy年MM月dd日HH时mm分ss秒");
                type = quotaInfo.QuotaData.QuotaType == QuotaType.Month ? "月" : "年";
                if (quotaInfo.QuotaData.QuotaType == QuotaType.Month)
                {
                    quotaDate = string.Format("{0}年{1}月", quotaInfo.QuotaData.QuotaTime.Year, quotaInfo.QuotaData.QuotaTime.Month);
                }
                else if (quotaInfo.QuotaData.QuotaType == QuotaType.Year)
                {
                    quotaDate = string.Format("{0}年", quotaInfo.QuotaData.QuotaTime.Year);
                }

                if (quotaInfo.QuotaLogList.Count > 1)
                {
                    QuotaLog oldQUotaLog = quotaInfo.QuotaLogList[1];
                    oldValue = oldQUotaLog.QuotaValue.ToString();
                    content = string.Format("{0}用户于{1}修改\"{2}-{3}-{4}定额\":修改前为{5}kwh,修改后为{6}{7}。", _userName, editTime, objectDes, itemCodeName, type, oldValue, newValue, objectUnit);
                }
                else if (quotaInfo.QuotaLogList.Count == 1)
                {
                    content = string.Format("{0}用户于{1}添加\"{2}-{3}-{4}定额\":{5}{6}。", _userName, editTime, objectDes, itemCodeName, type, newValue, objectUnit);
                }
                return content;

                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    /// <summary>
    /// 分摊内容
    /// </summary>
    public class AlloctionFormate : Formate
    {
        public override string FormateContent(object primaryKey)
        {
            try
            {
                #region 变量

                string content = string.Empty;
                string treeName = string.Empty;
                ResultRate model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultRate>(primaryKey.ToString());

                if (model != null)
                {
                    treeName = model.TreeName;
                }
                string editTime = string.Empty;

                #endregion



                #region 组织数据

                editTime = DateTime.Now.ToString("yyyy年MM月dd日hh时mm分ss秒");
                content = string.Format("{0}用户在{1}执行了[{2}]的分摊操作。", _userName, editTime, treeName);

                return content;

                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    /// <summary>
    /// 角色配置
    /// </summary>
    public class UserGroupFormate : Formate
    {

        public override string FormateContent(object primaryKey)
        {
            string content = string.Empty;
            List<string> uGInfo = (primaryKey as ExecuteResult).ExtendContent as List<string>;
            if (uGInfo != null)
            {
                content = string.Format("{0}用户于{1}{2}了【{3}】用户组", _userName, DateTime.Now.ToString(), uGInfo[0], uGInfo[1]);
            }
            else
            {
                content = string.Format("{0}用户于{0} 操作【用户组配置】失败！", _userName, DateTime.Now.ToString());
            }
            return content;
        }
    }

    /// <summary>
    /// 角色配置
    /// </summary>
    public class UserFormate : Formate
    {

        public override string FormateContent(object primaryKey)
        {
            string content = string.Empty;
            List<string> userInfo = (primaryKey as ExecuteResult).ExtendContent as List<string>;
            if (userInfo != null)
            {
                content = string.Format("{0}用户于{1}{2}了【{3}】用户", _userName, DateTime.Now.ToString(), userInfo[0], userInfo[1]);
            }
            else
            {
                content = string.Format("{0}用户于{0}操作【用户配置】失败！", _userName, DateTime.Now.ToString());
            }
            return content;
        }
    }

    /// <summary>
    /// 工厂
    /// </summary>
    public class FormateFactory
    {
        public static Formate CreateFormateFactory(ModelType modeType)
        {
            switch (modeType)
            {
                case ModelType.Qouta:
                    return new QuotaFormate();
                case ModelType.Alloction:
                    return new AlloctionFormate();
                case ModelType.UserGroup:
                    return new UserGroupFormate();
                case ModelType.User:
                    return new UserFormate();
                default:
                    return new OtherForMate();
            }
        }
    }
    #endregion
}




