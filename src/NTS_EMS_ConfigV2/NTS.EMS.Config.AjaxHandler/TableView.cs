using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NTS.WEB.Common;

namespace NTS.EMS.Config.AjaxHandler
{
    public class TableView
    {
        public static DataTable CreateSysLogDataTable()
        {
            Dictionary<string, ColType> Dir = new Dictionary<string, ColType>();
            Dir.Add("Number", ColType.NTSString);
            Dir.Add("ModelName", ColType.NTSString);
            Dir.Add("LogContent", ColType.NTSString);
            Dir.Add("LogTime", ColType.NTSString);
            Dir.Add("OpType", ColType.NTSString);
            Dir.Add("USerName", ColType.NTSString);
            return TableTool.CreateTable(Dir);
        }
    }
}
