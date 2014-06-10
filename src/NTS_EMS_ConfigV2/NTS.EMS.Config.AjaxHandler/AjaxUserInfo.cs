using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Web;
using Framework.Common;

namespace NTS.EMS.Config.AjaxHandler
{
    public class AjaxUserInfo
    {
        private const string constPassWord = "pa_wo^rd^-bc(gd)";
        private readonly HttpContext _ntsPage = HttpContext.Current;

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "用户配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult InsertUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDataContact>(inputValue);
            var validResult = ValidatePara(query);
            if (!validResult.Success)
            {
                return validResult;
            }
            string md5PassWord = DESEncrypt.EncryptMd5(query.PassWord);
            query.PassWord = md5PassWord;
            var result = new NTS.EMS.Config.BLL.OperateUserBll().InsertUser(query);
            if (result.Success)
            {
                result.ExtendContent = new List<string>() { "新增", query.Name };
            }
            return result;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "用户配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult UpdateUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDataContact>(inputValue);
            var validResult = ValidatePara(query);
            if (!validResult.Success)
            {
                return validResult;
            }
            if (!query.PassWord.Equals(constPassWord))
            {
                string md5PassWord = DESEncrypt.EncryptMd5(query.PassWord);
                query.PassWord = md5PassWord;
            }
            else
            {
                query.PassWord = string.Empty;
            }
            var result = new NTS.EMS.Config.BLL.OperateUserBll().UpdateUser(query);
            if (result.Success)
            {
                result.ExtendContent = new List<string>() { "修改", query.Name };
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultUserInfo GetUserInfo()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(inputValue);
            var result = new NTS.EMS.Config.BLL.OperateUserBll().GetUserInfo(query);
            if (result.UserInfo != null)
            {
                result.UserInfo.PassWord = constPassWord;
            }
            return result;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultUsers GetUserList()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUserContact>(inputValue);
            var result = new NTS.EMS.Config.BLL.OperateUserBll().GetUserList(query);
            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [AjaxAopBussinessLog(ModelName = "用户配置", LogType = OperatorType.Config)]
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ExecuteResult DeleteUser()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(inputValue);
            var userInfo = new NTS.EMS.Config.BLL.OperateUserBll().GetUserInfo(query);
            var result = new NTS.EMS.Config.BLL.OperateUserBll().DeleteUser(query);
            if (result.Success)
            {
                result.ExtendContent = new List<string>() { "删除", userInfo.UserInfo.UserName };
            }
            return result;
        }

        /// <summary>
        /// 获取所有的用户组
        /// </summary>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        [Framework.Ajax.CustomAjaxMethod]
        public ResultAllUserGroup GetAllUserGroup()
        {
            var result = new NTS.EMS.Config.BLL.OperateUserBll().GetAllUserGroup();
            return result;
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Framework.LogAndException.CustomException]
        private ExecuteResult ValidatePara(UserDataContact query)
        {
            ExecuteResult execResult = new ExecuteResult();
            if (string.IsNullOrEmpty(query.Name))
            {
                execResult.Success = false;
                execResult.ExceptionMsg = "请填写用户名！";
                return execResult;
            }
            if (string.IsNullOrEmpty(query.PassWord))
            {
                execResult.Success = false;
                execResult.ExceptionMsg = "请填写用户密码！";
                return execResult;
            }
            execResult.Success = true;
            execResult.ExceptionMsg = "";
            return execResult;
        }

    }
}
