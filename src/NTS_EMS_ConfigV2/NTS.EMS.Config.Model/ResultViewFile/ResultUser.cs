using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class ResultUserInfo
    {
        public ExecuteResult ResultInfo { get; set; }
        public User UserInfo { get; set; }
    }

    public class ResultUsers
    {
        public ExecuteResult ResultInfo { get; set; }
        public List<User> UserList { get; set; }
        public Padding Page { get; set; }
    }

    public class ResultAllUserGroup
    {
        public ExecuteResult ResultInfo { get; set; }
        public List<SingleUserGroup> UserGroupList { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }//状态
        public string StatusStr { get { return Status == 0 ? "正常" : "禁用"; } }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string PassWord { get; set; }
    }

    
    public class SingleUserGroup
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
    }
}
