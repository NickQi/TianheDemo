using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
    public class EnumColor
    {
        /// <summary>
        /// 蓝色
        /// </summary>
        public string Blue
        {
            get { return "color:'#058DC7'"; }
        }
        /// <summary>
        /// 深绿色
        /// </summary>
        public string DepthGreen
        {
            get { return "color:'#50B432'"; }
        }

        /// <summary>
        /// 暗红色
        /// </summary>
        public string DepthRed
        {
            get { return "color:'#ED561B'"; }
        }

        /// <summary>
        /// 深黄色
        /// </summary>
        public string DepthYellow
        {
            get { return "color:'#DDDF00'"; }
        }

        /// <summary>
        /// 天蓝色
        /// </summary>
        public string SmallBlue
        {
            get { return "color:'#24CBE5'"; }
        }

        /// <summary>
        /// 淡绿色
        /// </summary>
        public string SmallGreen
        {
            get { return "color:'#64E572'"; }
        }

        /// <summary>
        /// 淡红色
        /// </summary>
        public string SmallRed
        {
            get { return "color:'#FF9655'"; }
        }

        /// <summary>
        /// 淡黄色
        /// </summary>
        public string SmallYellow
        {
            get { return "color:'#FFF263'"; }
        }

        /// <summary>
        /// 颜色库
        /// </summary>
        public string [] ColorKu
        {
            get
            {
                string[] a = {  "#7FFFD4", "#00FFFF", "#FFE4E1", "#800080", "#F0FFFF"
                             ,"#F5F5DC","#FFE4C4","#FDF5E6","#8B4513","#5F9EA0","#D2691E","#7FFF00","#AFEEEE","#6A5ACD"
                             ,"#CD853F","#808000","#8B008B","#CD5C5C","#C0C0C0","#FFDAB9","#FF69B4","#7CFC00","#4B0082"
                             ,"#800000","#D2B48C","#DAA520","#1E90FF","#FF1493"};
                return a;
            }
        }
    }
}
