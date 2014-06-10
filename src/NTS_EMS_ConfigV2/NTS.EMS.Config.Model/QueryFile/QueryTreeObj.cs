using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NTS.EMS.Config.Model.QueryFile
{
   public class QueryTreeObj
    {
       /// <summary>
       /// 父区域节点
       /// </summary>
       [DataMember]
       public int ParentObjID { get; set; }

       /// <summary>
       /// 选择树的标识   1：业态树  2：区域树
       /// </summary>
       [DataMember]
       public int TreeInfo { get; set; }

       /// <summary>
       /// 能耗类型ID值
       /// </summary>
       public string EnergyID { get; set; }

       /// <summary>
       /// 查询日期
       /// </summary>
       public DateTime SelectDate { get; set; }
    }
}
