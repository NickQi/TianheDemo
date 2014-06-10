using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.Model;

namespace NTS.WEB.ProductInteface
{
    public interface IAccessCommon
    {
        TB_AreaInfo GetAreaInfo(int areaId);

        int GetUserGroupID(string username);
        decimal GetFeePrice(string itemcode);
    }
}
