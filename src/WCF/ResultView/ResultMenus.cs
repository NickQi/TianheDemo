using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    [Serializable]
    public class ResultMenus
    {
        public ExecuteProcess ActionInfo;
        public List<MenuData> data;
    }
    [Serializable]
    public class MenuData
    {
        public int id;
        public string text;
        public string href;
        public string iconCls;
        public string state;
        public List<MenuData> children;
    }
}
