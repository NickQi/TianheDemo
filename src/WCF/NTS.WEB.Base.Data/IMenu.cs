using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
   public interface IMenu
   {
       //DataTable GetMenuTree(string username);

       List<MenuModel> GetMenus(string username);
   }
}
