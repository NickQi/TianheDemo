using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NTS.EMS.Config.Model;

namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxUserGroupInfo
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        /// <summary>
        /// 保存用户组
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "用户组配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult SaveUserGroup()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<UserGroupDataContact>(inputValue);
            var operatorUserGroup = new NTS.EMS.Config.BLL.OperateUserGroupBll();
            var existExecuteResult = operatorUserGroup.HasExistUserGroup(query.UGData.Name, query.UGData.ID);
            if (!existExecuteResult.Success)
            {
                return existExecuteResult;
            }
            var result = operatorUserGroup.SaveUserGroup(query);
            if (result.Success)
            {
                result.ExtendContent = new List<string>() { query.UGData.ID == 0 ? "新增" : "修改", query.UGData.Name };
            }
            return result;
        }

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultUserGroupInfo GetUserGroupInfo()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(inputValue);
            return new NTS.EMS.Config.BLL.OperateUserGroupBll().GetUserGroupInfo(query);
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultUserGroups GetUserGroupList()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUserGroupContact>(inputValue);
            var result = new NTS.EMS.Config.BLL.OperateUserGroupBll().GetUserGroupList(query);
            return result;
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "用户组配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult DeleteUserGroup()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(inputValue);
            var operaterUserGroup = new NTS.EMS.Config.BLL.OperateUserGroupBll();
            var userGroup = operaterUserGroup.GetUserGroupInfo(query);
            if (userGroup.ResultInfo.Success)
            {
                if (userGroup.UserGroupInfo.Name.Equals("系统组"))
                {
                    ExecuteResult systemResult = new ExecuteResult();
                    systemResult.Success = false;
                    systemResult.ExceptionMsg = "系统组不允许删除！";
                    return systemResult;
                }
            }
            else
            {
                return userGroup.ResultInfo;
            }
            var result = operaterUserGroup.DeleteUserGroup(query);
            result.ExtendContent = new List<string> { "删除", userGroup.UserGroupInfo.Name };
            return result;
        }

        /// <summary>
        /// 判断是否包含用户
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public int ContainUser()
        {
            //不能返沪bool 类型？？？ 0表示不包含，1表示包含 
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(inputValue);
            var result = new NTS.EMS.Config.BLL.OperateUserGroupBll().IsContainUser(query);
            return result ? 1 : 0;
        }

    }
}
