using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;

namespace NTS.EMS.Config.DAL
{
    public class UserObject : IUserObject
    {

        public List<Model.TB_User> GetUserList(string whereStr)
        {
            var cmd = new DataCommand("GetUserList", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.TB_User>();
        }

        public int InsertUser(Model.User user)
        {
            var cmd = new DataCommand("InsertUser", new SqlCustomDbCommand());
            cmd.SetParameterValue("@ID", user.Id);
            cmd.SetParameterValue("@Name", user.UserName);
            cmd.SetParameterValue("@PassWord", user.PassWord);
            cmd.SetParameterValue("@Status", user.Status);
            cmd.SetParameterValue("@GroupId", user.GroupId);
            return cmd.ExecuteNonQuery();
        }

        public int UpdateUser(Model.User user)
        {
            var cmd = new DataCommand("UpDateUser", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", user.Id);
            cmd.SetParameterValue("@Name", user.UserName);
            cmd.SetParameterValue("@PassWord", user.PassWord);
            cmd.SetParameterValue("@Status", user.Status);
            cmd.SetParameterValue("@GroupId", user.GroupId);
            return cmd.ExecuteNonQuery();
        }


        public int DeleteUser(int id)
        {
            var cmd = new DataCommand("DeleteUser", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", id);
            return cmd.ExecuteNonQuery();
        }

        public bool IsContainUser(string userName)
        {
            var cmd = new DataCommand("IsContainUser", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Name", userName);
            return int.Parse(cmd.ExecuteScalar().ToString()) == 0 ? false : true;
        }

        public Model.TB_User GetUserInfo(int userId)
        {
            var cmd = new DataCommand("GetUserInfo", new SqlCustomDbCommand());
            cmd.SetParameterValue("@Id", userId);
            return cmd.ExecuteEntity<Model.TB_User>();
        }

        public string GetUserGroupName(int userGroupId)
        {
            try
            {
                var cmd = new DataCommand("GetUserGroupName", new SqlCustomDbCommand());
                cmd.SetParameterValue("@Id", userGroupId);
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }


        public int GetMaxId()
        {
            try
            {
                var cmd = new DataCommand("GetMaxUserId", new SqlCustomDbCommand());
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
