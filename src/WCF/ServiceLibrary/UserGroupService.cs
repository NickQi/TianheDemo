using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserGroupService : IUserGroupService
    {

        [Log(ModelName = "用户组列表")]
        [CustomException]
        public UserGroupListResult GetUserGroups()
        {
            UserGroupListResult result = new UserGroupListResult();
           // List<QueryUserGroup> list = new List<QueryUserGroup>();
            var pAction = new ExecuteProcess();
            try
            {
                var usergrouplist = new NTS.WEB.BLL.UserGroup().GetUserGroups();

                if (usergrouplist.Count > 0)
                {
                    pAction.Success = true;
                    //foreach (var item in usergrouplist)
                    //{
                    //    QueryUserGroup query = new QueryUserGroup() { UserGroupID=item.UserGroupID,
                    //    UserGroupName=item.UserGroupName,
                    //     Description=item.Description
                    //    };
                        
                    //    foreach (var right in item.UserGroupMenuRights.Split(','))
                    //    {
                    //        query.UserGroupMenuRights.Add(Convert.ToInt32(right));
                    //    }
                    //    foreach (var right in item.UserGroupLiquidRights.Split(','))
                    //    {
                    //        query.UserGroupLiquidRights.Add(Convert.ToInt32(right));
                    //    }
                    //    foreach (var right in item.UserGroupAreaRights.Split(','))
                    //    {
                    //        query.UserGroupAreaRights.Add(Convert.ToInt32(right));
                    //    }
                    //    list.Add(query);
                    //}
                    result.UserGroupList = usergrouplist;
                    result.ActionInfo = pAction;
                    return result;
                }
                else
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    result.ActionInfo = pAction;
                    return result;

                }


            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                result.ActionInfo = pAction;
                return result;
            }
        }
        [Log(ModelName = "获取单个用户组")]
        [CustomException]
        public SingleUserGroupResult GetSingleUserGroup(int usergroupid)
        {
            SingleUserGroupResult result = new SingleUserGroupResult();
            var pAction = new ExecuteProcess();
            try
            {
                var query = new NTS.WEB.BLL.UserGroup().GetSingleUserGroup(usergroupid);

                if (query != null)
                {

                    //query.Password = DESEncrypt.Decrypt(query.Password);
                    pAction.Success = true;
                    pAction.ExceptionMsg = "获取单个用户组成功";
                    result.QueryUserGroup = query;
                    result.ActionInfo = pAction;
                    return result;
                }
                else
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    result.ActionInfo = pAction;
                    return result;

                }


            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                result.ActionInfo = pAction;
                return result;
            }
        }
        [Log(ModelName = "新增用户组")]
        [CustomException]
        public ExecuteProcess AddUserGroup(QueryUserGroup model)
        {
            var pAction = new ExecuteProcess();
            try
            {
                if (new NTS.WEB.BLL.UserGroup().IsExistUserGroupName(model))
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "该用户组已存在";
                    return pAction;
                }
                else
                {


                    new NTS.WEB.BLL.UserGroup().AddUserGroup(model);
                    pAction.Success = true;
                    pAction.ExceptionMsg = "新增用户组成功";
                    return pAction;
                }

            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return pAction;
            }


        }
        [Log(ModelName = "更新用户组")]
        [CustomException]
        public ExecuteProcess UpdateUserGroup(QueryUserGroup model)
        {
            var pAction = new ExecuteProcess();
            try
            {
                if (new NTS.WEB.BLL.UserGroup().IsExistUserGroupName(model))
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "该用户组已存在";
                    return pAction;
                }
                else
                {
                    new NTS.WEB.BLL.UserGroup().UpdateUserGroup(model);
                    pAction.Success = true;
                    pAction.ExceptionMsg = "更新用户组成功";
                    return pAction;
                }
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return pAction;
            }


        }
        [Log(ModelName = "删除用户组")]
        [CustomException]
        public ExecuteProcess DeleteUserGroup(int id)
        {
            var pAction = new ExecuteProcess();
            try
            {
                new NTS.WEB.BLL.UserGroup().DeleteUserGroup(id);
                pAction.Success = true;
                pAction.ExceptionMsg = "删除用户组成功";
                return pAction;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return pAction;
            }

        }


    }
}
