using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.DataConfiguration;

namespace NTS.EMS.Config.Model
{
   public class Itemcode
    {
       /// <summary>
       /// 分类分项的id
       /// </summary>
       [DataMapping("ItemcodeID", "ItemcodeID", DbType.Int32)]
       public int ItemcodeID { get; set; }

       [DataMapping("ItemCodeNumber", "ItemCodeNumber", DbType.String)]
       public string ItemCodeNumber { get; set; }

       /// <summary>
       /// 分类分项的名称
       /// </summary>
       [DataMapping("ItemCodeName", "ItemCodeName", DbType.String)]
       public string ItemCodeName { get; set; }
       /// <summary>
       /// 分类分项的父类id
       /// </summary>
       [DataMapping("ParentID", "ParentID", DbType.Int32)]
       public int ParentID { get; set; }
       /// <summary>
       /// 分类分项的单位
       /// </summary>
       [DataMapping("Unit", "Unit", DbType.String)]
       public string Unit { get; set; }
       /// <summary>
       /// 转化为煤的比率
       /// </summary>
       [DataMapping("ItemCoal", "ItemCoal", DbType.Decimal)]
       public Double ItemCoal { get; set; }
       /// <summary>
       /// 转化为CO2的比率
       /// </summary>
       [DataMapping("ItemCO2", "ItemCO2", DbType.Decimal)]
       public Double ItemCO2 { get; set; }
    }
}
