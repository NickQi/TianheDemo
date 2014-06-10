using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
    public class CondictionModel
    {
        private DateTime stime; // 开始时间
        private DateTime etime; // 结束时间
        private string F_EnergyItemCode; // 分类分项
        private string[] objarray; // 查找对象集合
        private string objtable; // 查找对应表
        private string objcol;// 查找的列
        public DateTime _stime
        {
            get { return stime; }
            set { stime = value; }
        }

        public DateTime _etime
        {
            get { return etime; }
            set { etime = value; }
        }

        public string _F_EnergyItemCode
        {
            get { return F_EnergyItemCode; }
            set { F_EnergyItemCode = value; }
        }

        public string[] _objarray
        {
            get { return objarray; }
            set { objarray = value; }
        }

        public string _objtable
        {
            get { return objtable; }
            set { objtable = value; }
        }

        public string _objcol
        {
            get { return objcol; }
            set { objcol = value; }
        }
    }
}
