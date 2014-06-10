using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NTS.WEB.Model
{
    [Serializable]
    public class WarningTypeModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string CValue { get; set; }
        public string keycol { get; set; }
    }

    public class WarningType
    {
        [XmlArrayItem("Item")]
        public List<Item> Data = new List<Item>();
    }

    public class Item
    {

        [XmlAttribute("Value")]
        public string ItemValue;
        [XmlAttribute("Text")]
        public string ItemText;
        [XmlAttribute("CValue")]
        public string ItemViewName;
        [XmlAttribute("keycol")]
        public string ItemKeyCol;

    }
}
