using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DBUtility;
using Framework.Data;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class Menu : IMenu
    {
//        public DataTable GetMenuTree(string username)
//        {
//            try
//            {

//                string SQL = string.Format(@"select A.ID MenuID,A.MenuName,A.LinkName,A.IconClass,A.ParentID from dbo.TB_MENU A
//JOIN TB_USERGROUPMENURIGHT B ON A.ID=B.MENUID
//JOIN TB_USER C ON B.USERGROUPID=C.GROUPS
//WHERE C.CNAME='{0}'
//order by A.ID", username);
//                return SqlHelper.Query(SQL).Tables[0];
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }


        public List<MenuModel> GetMenus(string username)
        {
            try
            {
               
                var cmd = new DataCommand("getMenus", new SqlCustomDbCommand());
                cmd.SetParameterValue("@CNAME", username);
                return cmd.ExecuteEntityList<MenuModel>();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
    }
}
