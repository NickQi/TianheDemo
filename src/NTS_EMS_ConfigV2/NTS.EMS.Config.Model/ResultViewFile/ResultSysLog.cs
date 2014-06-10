using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.EMS.Config.Model
{
    public class ResultSysLog
    {
       // public ExecuteProcess ActionInfo { get; set; }
        public List<BaseSysLog> SysLogList { get; set; }
        public Padding Page { get; set; }
    }


    public class BaseSysLog
    {
        public int SysNo { get; set; }
        public string ModelName { get; set; }
        public string LogContent { get; set; }
        public DateTime LogTime { get; set; }
        public string LogTimeStr { get { return LogTime.ToString(); } }
        public OpType OpType { get; set; }
        public string typeStr
        {
            get
            {
                if (OpType== OpType.Configure)
                {
                    return "配置";
                }
                else
                {
                    return "操作";
                }
            }
        }
        public string UserName { get; set; }
    }

    public enum OpType
    {
        Operate = 1,
        Configure = 2
    }
}
