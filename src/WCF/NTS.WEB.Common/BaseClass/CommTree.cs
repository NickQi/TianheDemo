using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;

namespace NTS_BECM.Common.BaseClass
{
    public class CommTree
    {
        /// <summary>
        /// 绑定生成一个有树结构的下拉菜单
        /// </summary>
        /// <param name="dtNodeSets">菜单记录数据所在的表</param>
        /// <param name="strParentColumn">表中用于标记父记录的字段</param>
        /// <param name="strRootValue">第一层记录的父记录值(通常设计为0或者-1或者Null)用来表示没有父记录</param>
        /// <param name="strIndexColumn">索引字段，也就是放在DropDownList的Value里面的字段</param>
        /// <param name="strTextColumn">显示文本字段，也就是放在DropDownList的Text里面的字段</param>
        /// <param name="drpBind">需要绑定的DropDownList</param>
        /// <param name="i">用来控制缩入量的值，请输入-1</param>
        public static void MakeTree(DataTable dtNodeSets, string strParentColumn, string strRootValue, string strIndexColumn, string strTextColumn, DropDownList drpBind, int i)
        {
            //每向下一层，多一个缩入单位
            i++;

            DataView dvNodeSets = new DataView(dtNodeSets);
            dvNodeSets.RowFilter = strParentColumn + "=" + strRootValue;

            string strPading = "";  //缩入字符

            //通过i来控制缩入字符的长度，我这里设定的是一个全角的空格
            for (int j = 0; j < i; j++)
                strPading += "　";//如果要增加缩入的长度，改成两个全角的空格就可以了

            foreach (DataRowView drv in dvNodeSets)
            {
                TreeNode tnNode = new TreeNode();
                ListItem li = new ListItem(strPading + "├" + drv[strTextColumn].ToString(), drv[strIndexColumn].ToString());
                drpBind.Items.Add(li);
                MakeTree(dtNodeSets, strParentColumn, drv[strIndexColumn].ToString(), strIndexColumn, strTextColumn, drpBind, i);
            }

            //递归结束，要回到上一层，所以缩入量减少一个单位
            i--;
        }
    }
}
