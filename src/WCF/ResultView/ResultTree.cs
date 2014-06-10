using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
   public class ResultTree
    {
        public ExecuteProcess ActionInfo;
        public List<TreeItem> data;
    }
   public class TreeItem
   {
       public int id;
       public string text;
       public string state;

       public string iconCls;
      
       public List<TreeItem> children;
   }
}
