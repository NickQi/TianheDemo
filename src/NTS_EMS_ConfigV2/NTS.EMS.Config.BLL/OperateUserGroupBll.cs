using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;

namespace NTS.EMS.Config.BLL
{
    public class OperateUserGroupBll
    {
        NTS.EMS.Config.ProductInteface.IUserGroupObject userGroupOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateUserGroupObject();

        #region 用户组管理
        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ExecuteResult SaveUserGroup(UserGroupDataContact data)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                #region 组织参数
                Model.UserGroup userGroup = new UserGroup();
                userGroup.Id = data.UGData.ID;
                userGroup.Name = data.UGData.Name;
                userGroup.Description = data.UGData.Description;
                string menuIds = "";
                string objectIds = "";
                string objectTypes = "";
                if (data.UGMenuRightDataList != null && data.UGMenuRightDataList.Count > 0)
                {
                    int[] menuArr = data.UGMenuRightDataList.Select(p => p.MenuId).ToArray();
                    menuIds = string.Join(",", Array.ConvertAll(menuArr, p => p.ToString()));
                }
                if (data.UGObjectRightDataList != null && data.UGObjectRightDataList.Count > 0)
                {
                    int[] objectArr = data.UGObjectRightDataList.Select(p => p.ObjdetId).ToArray();
                    objectIds = string.Join(",", Array.ConvertAll(objectArr, p => p.ToString()));
                    int[] objectTypeArr = data.UGObjectRightDataList.Select(p => p.Type).ToArray();
                    objectTypes = string.Join(",", Array.ConvertAll(objectTypeArr, p => p.ToString()));
                }
                #endregion

                int count = userGroupOperator.SaveUserGroupInfo(userGroup, menuIds, objectIds, objectTypes);
                //if (count == 0)
                //{
                //    resultInfo.Success = false;
                //    resultInfo.ExceptionMsg = "服务连接断开，请联系管理员！";
                //    return resultInfo;
                //}
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
        /// 获取用户组列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ResultUserGroups GetUserGroupList(QueryUserGroupContact query)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                ResultUserGroups resultUserGroups = new ResultUserGroups();
                resultUserGroups.Page = new Padding();
                resultUserGroups.UserGroupList = new List<UserGroup>();

                var result = userGroupOperator.GetUserGroupList(string.Empty);
                if (result.Count > 0)
                {
                    resultUserGroups.Page.Current = query.PageCurrent;
                    resultUserGroups.Page.Total = result.Count;
                    resultUserGroups.UserGroupList = result.Select(p =>
                        new Model.UserGroup
                        {
                            Description = p.Description,
                            Id = p.Id,
                            Name = p.Name
                        }).Skip((query.PageCurrent - 1) * query.PageSize).Take(query.PageSize).ToList();
                    resultInfo.Success = true;
                    resultInfo.ExceptionMsg = string.Empty;
                    resultUserGroups.ResultInfo = resultInfo;
                    return resultUserGroups;
                }
                else
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "暂无数据！";
                    return new ResultUserGroups { ResultInfo = resultInfo };
                }
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return new ResultUserGroups { ResultInfo = resultInfo };
            }
        }

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="id">用户组ID</param>
        /// <returns></returns>
        public ResultUserGroupInfo GetUserGroupInfo(int id)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                ResultUserGroupInfo userGroupDetail = new ResultUserGroupInfo();
                userGroupDetail.UserGroupInfo = new UserGroup();
                userGroupDetail.MenuRightList = new List<MenuRight>();
                userGroupDetail.ObjectRightList = new List<ObjectRight>();

                var userGroupInfo = userGroupOperator.GetUserGroupInfo(id);
                if (userGroupInfo != null)
                {
                    userGroupDetail.UserGroupInfo.Description = userGroupInfo.Description;
                    userGroupDetail.UserGroupInfo.Id = userGroupInfo.Id;
                    userGroupDetail.UserGroupInfo.Name = userGroupInfo.Name;
                }
                var userGroupMenuList = userGroupOperator.GetUserGroupMenuRightList(id);
                userGroupDetail.MenuRightList = userGroupMenuList.Select(
                    p => new MenuRight { MenuName = string.Empty, MenuRightId = p.MenuId }).ToList();
                var userGroupObjectList = userGroupOperator.GetUserGroupObjectRightList(id);
                userGroupDetail.ObjectRightList = userGroupObjectList.Select(
                    p => new ObjectRight { ObjectId = p.ObjectId, ObjectName = string.Empty, Type = p.Type }).ToList();
                resultInfo.Success = true;
                resultInfo.ExceptionMsg = string.Empty;
                userGroupDetail.ResultInfo = resultInfo;
                return userGroupDetail;
            }
            catch (Exception ex)
            {
                resultInfo.Success = false;
                resultInfo.ExceptionMsg = ex.Message;
                return new ResultUserGroupInfo { ResultInfo = resultInfo };
            }
        }

        /// <summary>
        /// 是否包含用户判定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsContainUser(int id)
        {
            return userGroupOperator.ContainUser(id);
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExecuteResult DeleteUserGroup(int id)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                int count = userGroupOperator.DeleteUserGroup(id);
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
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public string GetMenuTree()
        {
            StringBuilder Result = new StringBuilder();
            var menuList = userGroupOperator.GetMenuList("", " order by ParentID asc");
            if (menuList.Count > 0)
            {
                Result.Append("[");
                foreach (var row in menuList)
                {
                    Result.Append("{\"id\":" + row.Id + ",\"name\":\"" + row.Name + "\",\"open\":" + (row.ParentId == 0 ? "true" : "false") + ",\"pId\":\"" + row.ParentId + "\"");
                    Result.Append("},");
                }
                Result = Result.Remove(Result.Length - 1, 1);
                Result.Append("]");
            }
            return Result.ToString();
        }

        /// <summary>
        /// 根据条件获取菜单
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<TB_Menu> GetTbMenu(string where, string order)
        {
            List<TB_Menu> menuList = new List<TB_Menu>();
            try
            {
                return userGroupOperator.GetMenuList(where, order);
            }
            catch (Exception)
            {
                return menuList;
            }

        }

        /// <summary>
        /// 是否存在相同的用户组名称
        /// </summary>
        /// <param name="userGroupName"></param>
        /// <returns></returns>
        public ExecuteResult HasExistUserGroup(string userGroupName, int userGroupId)
        {
            ExecuteResult resultInfo = new ExecuteResult();
            try
            {
                if (userGroupOperator.GetUserGroupList(string.Format(" and cname='{0}' ", userGroupName)).Where(p => p.Id != userGroupId).Count() > 0)
                {
                    resultInfo.Success = false;
                    resultInfo.ExceptionMsg = "已有相同用户组，请修改用户组名！";
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
        #endregion

    }
}
