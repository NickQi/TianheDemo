using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.EMS.Config.Model;
using System.Transactions;

namespace NTS.EMS.Config.BLL
{
    public class RightBll
    {
        NTS.EMS.Config.ProductInteface.IRightObject rightOperator = NTS.EMS.Config.ProductInteface.DataSwitchConfig.CreateRightObject();

        public bool HasMenuRight(string where)
        {
            return rightOperator.HasMenuRight(where);
        }

    }

}
