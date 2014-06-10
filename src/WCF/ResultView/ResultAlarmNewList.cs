using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultAlarmNewList
    {
        public ExecuteProcess ActionInfo;

        public List<AlarmNewList> data;

        public int[] pages;

        public int total;

        public int current;

    }


    public class AlarmNewList
    {
        //时间
        public string Time { get; set; }

        //对象
        public string Object { get; set; }

        //位置
        public string Position { get; set; }

        //信息。
        public string Info { get; set; }

        //告警类型
        public string AlarmItem { get; set; }

        //等级
        public string Class { get; set; }

        //告警状态
        public string AlarmStatus { get; set; }

    }

}
