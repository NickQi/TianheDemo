using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class UserGroupModel
    {

        /// <summary>
        /// 用户组ID
        /// </summary>
        [DataMapping("UserGroupID", "ID", DbType.Int32)]
        public int UserGroupID
        {
            get;
            set;
        }
        /// <summary>
        /// 用户组名称
        /// </summary>
        [DataMapping("UserGroupName", "CNAME", DbType.String)]
        public string UserGroupName
        {
            get;
            set;
        }
        /// <summary>
        /// 用户组描述
        /// </summary>
        [DataMapping("Description", "DESCRIPTION", DbType.String)]
        public string Description
        {
            get;
            set;
        }
        //[DataMapping("UserGroupMenuRights", "MENUGROUPS", DbType.String)]
        //public string UserGroupMenuRights
        //{
        //    get;
        //    set;
        //}
        //[DataMapping("UserGroupLiquidRights", "LIQUIDGROUPS", DbType.String)]
        //public string UserGroupLiquidRights
        //{
        //    get;
        //    set;
        //}
        //[DataMapping("UserGroupAreaRights", "AREAGROUPS", DbType.String)]
        //public string UserGroupAreaRights
        //{
        //    get;
        //    set;
        //}
    }


}
