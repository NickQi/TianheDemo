using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;

namespace NTS.EMS.Config.BLL
{
    public class OperateUserBll
    {
        NTS.EMS.Config.ProductInteface.IUserObject userOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateUserObject();

        #region 用户管理

        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ExecuteResult InsertUser(UserDataContact data)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                Model.User user = new User();
                user.GroupId = data.GroupId;
                user.Id = userOperator.GetMaxId() + 1;
                user.PassWord = data.PassWord;
                user.Status = data.Status;
                user.UserName = data.Name;

                if (userOperator.IsContainUser(user.UserName))
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "已有相同的用户名，请重新填写！";
                    return resultInfo;
                }
                int count = userOperator.InsertUser(user);
                if (count == 0)
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "服务连接断开，请联系管理员！";
                    return resultInfo;
                }
                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                return resultInfo;
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return resultInfo;
            }

        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ExecuteResult UpdateUser(UserDataContact data)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                Model.User user = new User();
                user.GroupId = data.GroupId;
                user.Id = data.ID;
                user.PassWord = data.PassWord;
                if (string.IsNullOrEmpty(user.PassWord))
                {
                    //表示密码没修改
                    user.PassWord = userOperator.GetUserInfo(data.ID).PassWord;
                }
                else
                {
                    user.PassWord = data.PassWord;
                }
                user.Status = data.Status;
                user.UserName = data.Name;
                if (userOperator.IsContainUser(user.UserName) && !userOperator.GetUserInfo(user.Id).Name.Equals(user.UserName))
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "已有相同的用户名，请重新填写！";
                    return resultInfo;
                }
                int count = userOperator.UpdateUser(user);
                if (count == 0)
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "服务连接断开，请联系管理员！";
                    return resultInfo;
                }
                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                return resultInfo;
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return resultInfo;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ExecuteResult DeleteUser(int userId)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                int count = userOperator.DeleteUser(userId);
                if (count == 0)
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "服务连接断开，请联系管理员！";
                    return resultInfo;
                }
                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                return resultInfo;
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return resultInfo;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultUsers GetUserList(QueryUserContact query)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                ResultUsers resultUsers = new ResultUsers();
                resultUsers.Page = new Padding();
                resultUsers.UserList = new List<User>();

                var result = userOperator.GetUserList(" order by id desc");
                if (result.Count > 0)
                {
                    resultUsers.Page.Current = query.PageCurrent;
                    resultUsers.Page.Total = result.Count;
                    resultUsers.UserList = result.Select(p =>
                        new Model.User
                        {
                            GroupId = p.GroupId,
                            GroupName = userOperator.GetUserGroupName(int.Parse(p.GroupId)),
                            Id = p.Id,
                            PassWord = p.PassWord,
                            Status = p.Status,
                            UserName = p.Name
                        }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();
                    resultInfo.Success = true;
                    resultInfo.ExceptionMsg = string.Empty;
                    resultUsers.ResultInfo = resultInfo;
                    return resultUsers;
                }
                else
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "暂无数据！";
                    return new ResultUsers { ResultInfo = resultInfo };
                }
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return new ResultUsers { ResultInfo = resultInfo };
            }
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultUserInfo GetUserInfo(int userId)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                ResultUserInfo userDetail = new ResultUserInfo();

                var user = userOperator.GetUserInfo(userId);
                userDetail.UserInfo = new User();
                userDetail.UserInfo.GroupId = user.GroupId;
                userDetail.UserInfo.GroupName = string.Empty;
                userDetail.UserInfo.Id = user.Id;
                userDetail.UserInfo.PassWord = user.PassWord;
                userDetail.UserInfo.Status = user.Status;
                userDetail.UserInfo.UserName = user.Name;

                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                userDetail.ResultInfo = resultInfo;
                return userDetail;
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return new ResultUserInfo { ResultInfo = resultInfo };
            }
        }

        /// <summary>
        /// 获取所有的用户组
        /// </summary>
        /// <returns></returns>
        public ResultAllUserGroup GetAllUserGroup()
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                ResultAllUserGroup userGroups = new ResultAllUserGroup();
                userGroups.UserGroupList = new List<SingleUserGroup>();
                var userGroup = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateUserGroupObject();
                userGroups.UserGroupList = userGroup.GetUserGroupList("").Select(p =>
                    new SingleUserGroup { GroupName = p.Name, GroupId = p.Id }).ToList();
                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                userGroups.ResultInfo = resultInfo;
                return userGroups;
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return new ResultAllUserGroup { ResultInfo = resultInfo };
            }
        }

        /// <summary>
        /// 不带分页的用查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<TB_User> GetUserListNotPage(string where)
        {
            return userOperator.GetUserList(where);
        }
        #endregion

    }

}
