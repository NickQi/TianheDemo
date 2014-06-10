using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Framework.Configuration;

namespace NTS.WEB.AjaxController
{
    [XmlRoot(ElementName = "BECM_Project")]
    public class BECMProject
    {
        [XmlElement]
        public string ProjectName { get; set; }
        [XmlElement]
        public string ProjectContent { get; set; }
        [XmlElement]
        public string ProjectUnit { get; set; }
    }

    public static class ProjectInfo
    {
        //public static BECMProject Project = Framework.Configuration.XmlHelper.XmlDeserializeFromFile<BECMProject>(GlobalConfigurationSource.RootConfigurationFilePath(GlobalConfigurationSource.GlobalAppSettings["CurrentProject"]), Encoding.UTF8);
        public static BECMProject Project = Framework.Configuration.XmlHelper.XmlDeserializeFromFile<BECMProject>(GlobalConfigurationSource.RootConfigurationFilePath(GlobalConfigurationSource.GlobalAppSettings["CurrentProject"]), Encoding.UTF8);
    }

}
