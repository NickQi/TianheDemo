using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultItemCode
    {
        public List<string> ObjectName { get; set; }
        public List<string> Dept { get; set; }
        public Dictionary<string, List<decimal>> Enery { get; set; }
    }


    public class ResultCompare
    {
        public List<string> ObjectName { get; set; }
        public List<string> Dept { get; set; }
        public Dictionary<string, List<decimal>> Enery { get; set; }
    }
}
