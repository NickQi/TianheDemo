using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS_BECM.Common
{
    public class FSum
    {

        /// <summary>
        /// 计算总体数值
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static decimal SumBuild(decimal[] d)
        {
            decimal sum = 0.00M;
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i] != -1.00M)
                {
                    sum += d[i];
                }
            }
            return sum;
        }
    }
}
