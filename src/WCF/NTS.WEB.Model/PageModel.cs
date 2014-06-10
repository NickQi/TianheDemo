using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
    public class PageModel
    {
        public string fieldname { get; set; } // 查询字段
        public string tablename { get; set; } // 分页的表名
        public string keycol { get; set; } // 主键的名称
        public int page { get; set; } // 当前页码
        public int pagesize { get; set; } // 每页显示记录数
        public string wherestr { get; set; } // 查询的条件
        public string orderby { get; set; } // 排序的条件
    }
}
