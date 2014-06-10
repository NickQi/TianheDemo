using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
   public  class AreaTypeDirctory
   {
       private readonly Dictionary<int, string> _areaTypeData = new Dictionary<int, string>();
       public Dictionary<int, string> DictionaryAreaTypeData
       {
           get { return _areaTypeData; }
       }

       public AreaTypeDirctory()
       {
           _areaTypeData.Add(1, "办公建筑");
           _areaTypeData.Add(2, "商场建筑");
           _areaTypeData.Add(3, "宾馆饭店建筑");
           _areaTypeData.Add(4, "文化教育建筑");
           _areaTypeData.Add(5, "医疗卫生建筑");
           _areaTypeData.Add(6, "体育建筑");
           _areaTypeData.Add(7, "综合建筑");
           _areaTypeData.Add(8, "其他建筑");
       }
   }
}
