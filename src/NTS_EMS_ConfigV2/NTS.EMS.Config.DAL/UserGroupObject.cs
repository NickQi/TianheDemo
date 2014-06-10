using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class UserGroupObject : IUserGroupObject
    {

        public List<Model.TB_UserGroup> GetUserGroupList(string whereStr)
        {
            var cmd = new DataCommand("GetUserGroupList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_UserGroup>();
        }

        public int SaveUserGroupInfo(Model.UserGroup userGroup, string menuIds, string objectIds, string objectTypes)
        {
            var cmd = new DataCommand("SaveUserGroupInfo", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", userGroup.Id);
            cmd.SetParameterValue("@Name", userGroup.Name);
            cmd.SetParameterValue("@Description", userGroup.Description);
            cmd.SetParameterValue("@Groups", string.Empty);
            cmd.SetParameterValue("@MenuRightIds", menuIds);
            cmd.SetParameterValue("@ObjectRightIds", objectIds);
            cmd.SetParameterValue("@ObjectTypes", objectTypes);
            var result = cmd.ExecuteNonQuery();
            return result;
             
        }

        public int DeleteUserGroup(int userGroupId)
        {
            var cmd = new DataCommand("DeleteUserGroup", new SqlCustomDbCommand());
            cmd.SetParameterValue("@UserGroupId", userGroupId);
            return cmd.ExecuteNonQuery();
        }

        public bool ContainUser(int uerGroupId)
        {
            var cmd = new DataCommand("ContainUser", new SqlCustomDbCommand());
            cmd.SetParameterValue("@UserGroupId", uerGroupId);
            return int.Parse(cmd.ExecuteScalar().ToString()) == 0 ? false : true;
        }

        public Model.TB_UserGroup GetUserGroupInfo(int userGroupId)
        {
            var cmd = new DataCommand("GetUserGroupInfo", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", userGroupId);
            return cmd.ExecuteEntity<Model.TB_UserGroup>();
        }

        public List<Model.TB_UserGroupMenuRight> GetUserGroupMenuRightList(int userGroupId)
        {
            var cmd = new DataCommand("GetUserGroupMenuRightList", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", userGroupId);
            return cmd.ExecuteEntityList<Model.TB_UserGroupMenuRight>();
        }

        public List<Model.TB_UserGroupObjectRight> GetUserGroupObjectRightList(int userGroupId)
        {
            var cmd = new DataCommand("GetUserGroupObjectRightList", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", userGroupId);
            return cmd.ExecuteEntityList<Model.TB_UserGroupObjectRight>();
        }

        public int GetMaxId()
        {
            try
            {
                var cmd = new DataCommand("GetMaxUserGroupId", new SqlCustomDbCommand());
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Model.TB_Menu> GetMenuList(string where, string order)
        {
            var cmd = new DataCommand("GetMenuList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", where);
            cmd.ReplaceParameterValue("#orderby#", order);
            return cmd.ExecuteEntityList<Model.TB_Menu>();
        }
    }
}
