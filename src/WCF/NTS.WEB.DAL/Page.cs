using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DBUtility;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class Page : IPage
    {
        public DataTable GetDataByPage(PageModel pmodel)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrEmpty(pmodel.fieldname))
            {
                pmodel.fieldname = "*";
            }


            //SQL数据库语句
            string ptopsql = "select top " + (pmodel.page - 1) * pmodel.pagesize + " " + pmodel.keycol + " from \"" + pmodel.tablename + "\" ";
            strSql.Append("select top " + pmodel.pagesize +
                " " + pmodel.fieldname + "  from \"" + pmodel.tablename + "\" where 1=1");
            if (!pmodel.wherestr.Equals(""))
            {
                strSql.Append(" and " + pmodel.wherestr);
                ptopsql += " where " + pmodel.wherestr;
            }

            if (!pmodel.orderby.Equals(""))
            {
                ptopsql += " order by " + pmodel.orderby;
                strSql.Append(" and " + pmodel.keycol + " not in (" + ptopsql + ") order by " + pmodel.orderby);
            }
            else
            {
                strSql.Append(" and " + pmodel.keycol + " not in (" + ptopsql + ")");
            }

            return SqlHelper.Query(strSql.ToString()).Tables[0];
        }

        public int GetPageCount(PageModel pmodel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count (*) as total from \"" + pmodel.tablename + "\" where 1=1");
            if (!pmodel.wherestr.Equals(""))
            {
                strSql.Append(" and " + pmodel.wherestr);
            }
            return int.Parse(SqlHelper.ExecuteScalar(strSql.ToString()).ToString());
        }
    }
}
