using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class ResultUserGroupInfo
    {
        public ExecuteResult ResultInfo { get; set; }
        public UserGroup UserGroupInfo { get; set; }
        public List<MenuRight> MenuRightList { get; set; }
        public List<ObjectRight> ObjectRightList { get; set; }
    }

    public class ResultUserGroups
    {
        public ExecuteResult ResultInfo { get; set; }
        public List<UserGroup> UserGroupList { get; set; }
        public Padding Page { get; set; }
    }

    public class UserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MenuRight 
    {
        public int MenuRightId { get; set; }
        public string MenuName { get; set; }
    }

    public class ObjectRight
    {
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }
        public int Type { get; set; }
    }
}
