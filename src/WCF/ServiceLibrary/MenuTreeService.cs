using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Framework.Common;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
      [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MenuTreeService : IMenuTreeService
    {
        //public DataTable GetMenuTree(string username)
        //{
        //    return new NTS.WEB.BLL.MenuTree().GetMenuTree(username);
        //}
        //[Log(ModelName = "菜单")]
        //[CustomException]
        //public ResultMenus GetMenuModule(string username)
        //{
        //    var pAction = new ExecuteProcess();
        //    try
        //    {

        //        DataTable dt = new NTS.WEB.BLL.MenuTree().GetMenuTree(username);
        //        ResultMenus menuModule = null;
        //        if (dt.Rows.Count > 0)
        //        {
        //            menuModule = new ResultMenus() { data = new List<MenuData>() };
        //            TableToModule(menuModule.data, dt, "ParentID", "0", "MenuID", "MenuName");
        //        }
        //        if (menuModule == null)
        //        {
        //            pAction.Success = false;
        //            pAction.ExceptionMsg = "暂无数据信息";
        //            return new ResultMenus() { ActionInfo = pAction };
        //        }
        //        pAction.Success = true;
        //        menuModule.ActionInfo = pAction;
        //        return menuModule;
        //    }
        //    catch (Exception e)
        //    {
        //        pAction.Success = false;
        //        pAction.ExceptionMsg = e.Message;
        //        return new ResultMenus() { ActionInfo = pAction };
        //    }



        //}
        private void TableToModule(List<MenuData> menudatas, DataTable dt, string pField, string pValue, string kField, string TextField)
        {
            string filter = String.Format("{0}={1} ", pField, pValue);//获取顶级目录.
            DataRow[] drs = dt.Select(filter);
            if (drs.Length < 1)
            {
                return;
            }
            foreach (DataRow dr in drs)
            {
                string pcv = dr[kField].ToString();
                MenuData menuData = new MenuData()
                                        {
                                            id = Convert.ToInt32(dr[kField].ToString()),
                                            text = dr[TextField].ToString(),
                                            href = dr["LinkName"].ToString(),
                                            iconCls = dr["IconClass"].ToString(),
                                            state = "closed",
                                            children = new List<MenuData>()
                                        };
                menudatas.Add(menuData);
                TableToModule(menuData.children, dt, pField, pcv, kField, TextField);
            }

        }

        [Log(ModelName = "菜单")]
        [CustomException]
        public ResultMenus GetMenus(string username)
        {
            ResultMenus result = new ResultMenus();
            var pAction = new ExecuteProcess();
            try
            {
                result = new NTS.WEB.BLL.MenuTree().GetMenus(username);
                if (result != null)
                {
                    pAction.Success = true;
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
    }
}
