using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.DataConfiguration;

namespace NTS.WEB.Model
{
    public class MenuModel
    {

       
        [DataMapping("MenuID", "MenuID", DbType.Int32)]
        public int MenuID
        {
            get;
            set;
        }
      
        [DataMapping("MenuName", "MenuName", DbType.String)]
        public string MenuName
        {
            get;
            set;
        }
      
        [DataMapping("LinkName", "LinkName", DbType.String)]
        public string LinkName
        {
            get;
            set;
        }
        [DataMapping("IconClass", "IconClass", DbType.String)]
        public string IconClass
        {
            get;
            set;
        }
        [DataMapping("ParentID", "ParentID", DbType.Int32)]
        public int ParentID
        {
            get;
            set;
        }
       
    }


}
