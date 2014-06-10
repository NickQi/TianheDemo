using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AreaTreeService : IAreaTreeService
    {
        public DataTable GetAreaTree()
        {
            //return new NTS.WEB.BLL.AreaTree().GetAreaTree();
            return null;
        }
    }
}
