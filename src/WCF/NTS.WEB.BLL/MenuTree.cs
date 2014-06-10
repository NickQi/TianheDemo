using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;

namespace NTS.WEB.BLL
{
    public class MenuTree
    {
        private readonly IMenu dal = DataSwitchConfig.CreateMenuTree();
        //public DataTable GetMenuTree(string username)
        //{
        //    return dal.GetMenuTree(username);
        //}

        

        public ResultMenus GetMenus(string username)
        {
           
            var menus = dal.GetMenus(username);
          
            ResultMenus result = null;
            if (menus.Count > 0)
            {
                //menus.Insert(0,new MenuModel {  MenuID=-1, MenuName = "总览页", ParentID = 0, LinkName = "index.html", IconClass = "main" });
                result = new ResultMenus() { data = new List<MenuData>() };
                MenuModelToResult(result.data, menus, 0);
               // ReSetResultMenus(result.data);
             
                return result;
            }
            
            return null;
        }
        private void ReSetResultMenus(List<MenuData> data)
        {
            if (data.Count>0)
            {
                foreach(MenuData item in data)
                {
                    if(item.children.Count>0)
                    {
                        item.href = item.children[0].href;
                    }
                }
               

            }
        }



        private void MenuModelToResult(List<MenuData> menudatas, List<MenuModel> menuModelList, int pValue)
        {


            var child = menuModelList.Where(model => model.ParentID == pValue);
            if (child.Count() < 1)
            {
                return;
            }
            foreach (var c in child)
            {
                MenuData menuData = new MenuData()
                {
                    id = c.MenuID,
                    text = c.MenuName,
                    href = c.LinkName,
                    iconCls = c.IconClass,
                    state = "closed",
                    children = new List<MenuData>()
                };
                menudatas.Add(menuData);
                MenuModelToResult(menuData.children, menuModelList, c.MenuID);
            }

        }
    }
}
