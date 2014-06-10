using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ResultView;
namespace NTS.WEB.AjaxController
{

    public class AjaxUserGroup
    {
        private readonly HttpContext _ntsPage = HttpContext.Current;

        [Framework.Common.CustomAjaxMethod]
        public UserGroupListResult GetUserGroups()
        {
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUserGroupService>("UserGroupService").GetUserGroups();
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ExecuteProcess AddUserGroup()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUserGroup>(inputValue);

            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUserGroupService>("UserGroupService").AddUserGroup(query);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public SingleUserGroupResult GetSingleUserGroup()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUserGroup>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUserGroupService>("UserGroupService").GetSingleUserGroup(query.UserGroupID);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ExecuteProcess UpdateUserGroup()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUserGroup>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUserGroupService>("UserGroupService").UpdateUserGroup(query);
            return res;
        }

        [Framework.Common.CustomAjaxMethod]
        public ExecuteProcess DeleteUserGroup()
        {
            var inputValue = _ntsPage.Request.Form["Inputs"];
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryUserGroup>(inputValue);
            var res = Framework.Common.BaseWcf.CreateChannel<ServiceInterface.IUserGroupService>("UserGroupService").DeleteUserGroup(query.UserGroupID);
            return res;
        }
    }
}
