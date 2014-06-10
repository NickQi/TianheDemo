using System.Collections.Generic;
using System.Data;
using System.Text;
using System;
using DBUtility;
using Framework.Data;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using System.Data.SqlClient;

namespace NTS.WEB.DAL
{
    public class ComplexReport : IComplexReport
    {
        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public DataSet GetItemCode(string whereStr)
        {
            //DataSet dsItemCode = new DataSet();
            //string strSql = "select * from \"Becm_ItemCode\" where 1=1";
            //if (!whereStr.Equals(string.Empty))
            //{
            //    strSql = strSql + whereStr;
            //}
            //dsItemCode = SqlHelper.Query(strSql);
            //return dsItemCode;

            try
            {
                var cmd = new DataCommand("GetItemCode", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                //cmd.ReplaceParameterValue("#Sort#", order);
                return cmd.ExecuteDataSet();
            }
            catch (Exception ee)
            {
                throw ee;
            }
            //return ds.Tables[0];
        }

        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public DataTable GetList(string whereStr, string order)
        {
//            StringBuilder strSql = new StringBuilder();
//            strSql.Append(@"select layerobjectid,layerobjectname,layerobjectpic,layerobjectcontent,layerobjectparentid,bgflag,areatype 
//            from (select layerobjectid,layerobjectname,layerobjectpic,layerobjectcontent,layerobjectparentid,bgflag,areatype 
//            from Becm_LayerObject union
//            select layerobjectid,layerobjectname,layerobjectpic,layerobjectcontent,layerobjectparentid,bgflag,areatype 
//            from Becm_FunctionObject) 
//            aa where 1=1");
//            if (!whereStr.Equals(string.Empty))
//            {
//                strSql.AppendFormat(" and {0}", whereStr);
//            }
//            strSql.AppendFormat(" order by {0}", order);

//            DataTable dt = SqlHelper.Query(strSql.ToString(), parameters).Tables[0];
//            return dt;

            try
            {
                var cmd = new DataCommand("GetList", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                //cmd.ReplaceParameterValue("#Sort#", order);
                return cmd.ExecuteDataSet().Tables[0];
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }



        /// <summary>
        /// 根据对象的id获取对象的名称
        /// </summary>
        /// <param name="objectid"></param>
        /// <returns></returns>
        public string GetBaseLayerObjectName(string objectid)
        {

            DataTable dtTable = GetList(" and layerobjectid=" + objectid, "layerobjectid");
            if (dtTable.Rows.Count > 0)
            {
                return dtTable.Rows[0]["layerobjectname"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 合并dataset数据集到一个datatable上
        /// </summary>
        /// <returns></returns>
        private DataTable UniteDataTable(DataTable old, DataTable newdt)
        {
            object[] obj = new object[old.Columns.Count];

            for (int i = 0; i < newdt.Rows.Count; i++)
            {
                newdt.Rows[i].ItemArray.CopyTo(obj, 0);
                old.Rows.Add(obj);
            }
            return old;
        }

        /// <summary>
        /// 根据TB_AREA表获取区域列表信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAreaList()
        {
//            try
//            {
//                //FROM TB_AREA ORDER BY AREAID";
//                string SQL = @"select layerobjectid,layerobjectname,layerobjectparentid,layerobjectdeepth from dbo.Becm_LayerObject
//order by layerobjectid";
//                return SqlHelper.Query(SQL);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }

            try
            {
                var cmd = new DataCommand("GetAreaListNew", new SqlCustomDbCommand());
                //cmd.ReplaceParameterValue("#Sort#", order);
                return cmd.ExecuteDataSet();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public DataTable GetListItemCode(string whereStr, string order)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select * from \"Becm_ItemCode\" where 1=1");
            //if (!whereStr.Equals(string.Empty))
            //{
            //    strSql.AppendFormat(" and {0}", whereStr);
            //}
            //strSql.AppendFormat(" order by {0}", order);
            //DataTable dt = SqlHelper.Query(strSql.ToString(), parameters).Tables[0];
            //return dt;
            try
            {
                var cmd = new DataCommand("GetListItemCode", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#whereStr#", whereStr);
                return cmd.ExecuteDataSet().Tables[0];
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        ///// <summary>
        ///// 根据区域和项目代码获取是否含有数据
        ///// </summary>
        ///// <param name="itemcode"></param>
        ///// <param name="areaid"></param>
        ///// <returns></returns>
        //public int GetCountItemCodeAreaId(string itemcode, string areaid)
        //{
        //    string strSql = @"select count(distinct s1.areaid) from Becm_Device s1 "
        //                    + " inner join ( select * from f_GetChildAreaId(" + areaid + " ) ) s2 on s1.areaid = s2.id "
        //                    + " inner join Becm_ItemCode s3 on s1.itemcodeid = s3.itemcodenumber where s1.itemcodeid = '" + itemcode + "'";

        //    return int.Parse(SqlHelper.ExecuteScalar(strSql.ToString()).ToString());
        //}


        /// <summary>
        /// 根据区域和项目代码获取是否含有数据
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public int GetCountItemCodeAreaId(string itemcode, string areaid, int classid)
        {
            string strSql = "";
            if (classid == 1)
            {
                //strSql = @" select count(distinct s1.areaid) from Becm_Device s1 "
                // + "inner join ( select * from f_GetChildAreaId(" + areaid + ") ) s2 on s1.areaid = s2.id "
                // + " inner join Becm_ItemCode s3 on s1.itemcodeid = s3.itemcodenumber "
                // + " inner join ( select * from f_GetChildItemCode('" + itemcode + "') ) s4 on s1.itemcodeid=s4.code";

                var cmd = new DataCommand("GetCountItemCodeAreaId1", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#Areaid#", areaid);
                cmd.ReplaceParameterValue("#ItemCode#", itemcode);
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            else
            {
                var cmd = new DataCommand("GetCountItemCodeAreaId2", new SqlCustomDbCommand());
                cmd.ReplaceParameterValue("#Areaid#", areaid);
                cmd.ReplaceParameterValue("#ItemCode#", itemcode);
                return int.Parse(cmd.ExecuteScalar().ToString());
      //          strSql = @" select count(distinct s1.areaid) from Becm_Device s1 "
      //+ "inner join ( select * from f_GetChildFuncAreaId(" + areaid + ") ) s2 on s1.areaid2 = s2.id "
      //+ " inner join Becm_ItemCode s3 on s1.itemcodeid = s3.itemcodenumber "
      //+ " inner join ( select * from f_GetChildItemCode('" + itemcode + "') ) s4 on s1.itemcodeid=s4.code";
            }

    
            //return int.Parse(SqlHelper.ExecuteScalar(strSql.ToString()).ToString());
        }
    }
}
