using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.BLL
{
    public class Itemcode
    {
        NTS.WEB.ProductInteface.IItemcode dal = NTS.WEB.ProductInteface.DataSwitchConfig.CreateItemcode();
        public List<Model.Itemcode> GetItemcodeList(string whereStr, string sortStr)
        {
            return dal.GetItemcodeList(whereStr,sortStr);
        }
    }
}
