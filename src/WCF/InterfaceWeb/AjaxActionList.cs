using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace InterfaceWeb
{
    [XmlRoot(ElementName="ajaxactionlist")]
    public class AjaxActionList
    {
        [XmlElement(ElementName = "ajaxaction")]
        public List<AjaxRequest> AjaxAction { get; set; }
    }

    [XmlRoot(ElementName = "ajaxrequest")]
    public class AjaxRequest
    {
        [XmlAttribute(AttributeName = "ajaxname")]
        public string AjaxName { get; set; }
        [XmlAttribute(AttributeName = "ajaxmethod")]
        public string AjaxMethod { get; set; }
        [XmlAttribute(AttributeName = "ajaxdllspacename")]
        public string AjaxDllSpaceName { get; set; }
    }
}