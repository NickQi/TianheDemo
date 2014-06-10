using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Framework.Common;
using Framework.Data;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class BaseLayerObject : IBaseLayerObject
    {
        private static string GetCookie(string strName)
        {
            string result = "";
            if (HttpContext.Current.Request.Cookies.Count > 0)
            {
                if (HttpContext.Current.Request.Cookies[strName] != null)
                {
                    result = HttpContext.Current.Request.Cookies[strName].Value;
                }
            }
            else
            {
                result = "";
            }
            return result;
        }
        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public List<Model.BaseLayerObject> GetBaseLayerObjectList(string whereStr, string sortStr, string username = "")
        {
            // var cmd = new DataCommand("getBaseLayerObject", new SqlCustomDbCommand());
            //string username = "";
            //try
            //{
            //    //username = HttpContext.Current.Cache["username"].ToString();
            //    username = GetCookie("userid");
            //     username = HttpContext.Current.Session["userid"].ToString() ;

            //    username = NTS.WEB.Common.CacheHelper.GetCache("username").ToString();

            //}
            //catch(Exception ee)
            //{
            //}
            string viewname = "";
            if (!string.IsNullOrEmpty(username))
            {
                viewname = "Becm_LayerObjectByUser";
                whereStr += string.Format(" and username='{0}'", username);
            }
            else
            {
                viewname = "getBaseLayerObject";
            }
            var cmd = new DataCommand(viewname, new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            cmd.ReplaceParameterValue("#Sort#", sortStr);



            //username = HttpContext.Current.Cache["username"].ToString();
            //username = GetCookie("username");
            //if (HttpContext.Current.Session["username"] != null)
            //{
            //    username = HttpContext.Current.Session["username"].ToString();
            //}

            //if (!string.IsNullOrEmpty(username))
            //{
            //    cmd.SetParameterValue("@username", username);
            //}
            //else
            //{
            //    cmd.SetParameterValue("@username", DBNull.Value);
            //}
            return cmd.ExecuteEntityList<Model.BaseLayerObject>();
        }



        /// <summary>
        /// 获取查询的列表
        /// </summary>
        /// <param name="whereStr">查询的条件</param>
        /// <param name="order">排序的方式</param>
        /// <param name="parameters">查询的参数</param>
        /// <returns></returns>
        public List<Model.BaseLayerObject> GetBaseFuncLayerObjectList(string whereStr, string sortStr, string username = "")
        {
            string viewname = "";
            if (!string.IsNullOrEmpty(username))
            {
                viewname = "Becm_FunctionObjectByUser";
                whereStr += string.Format(" and username='{0}'", username);
            }
            else
            {
                viewname = "getBaseFuncLayerObject";
            }
            var cmd = new DataCommand(viewname, new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            cmd.ReplaceParameterValue("#Sort#", sortStr);

            return cmd.ExecuteEntityList<Model.BaseLayerObject>();
        }

        ///// <summary>
        ///// 根据对象的id和属性分类的id获取对象的值
        ///// </summary>
        ///// <param name="objectid"></param>
        ///// <param name="classid"></param>
        ///// <returns></returns>
        //   public decimal GetBaseLayerObjectCommAttribute (string objectid,int classid)
        //   {
        //       StringBuilder strSql = new StringBuilder();
        //       strSql.Append("select attributevalue from Becm_LayerObjectCommAttribute where 1=1");
        //       SqlParameter[] parameters = { new SqlParameter("@layerobjectid", SqlDbType.NVarChar), new SqlParameter("@classid", SqlDbType.Int) };
        //       parameters[0].Value = objectid;
        //       parameters[1].Value = classid;
        //       strSql.Append(" and layerobjectid=@layerobjectid and classid=@classid");
        //       DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        //       if (dt.Rows.Count > 0)
        //       {
        //           return decimal.Parse(dt.Rows[0]["attributevalue"].ToString());
        //       }
        //    return -1.00M;
        //   }

        /// <summary>
        /// 设备关联的区域ID列表
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public List<DeviceAreaID> GetDeviceAreaID1List(string itemcode)
        {
            string viewname = "";
            viewname = "GetDeviceAreaID1List";
            var cmd = new DataCommand(viewname, new SqlCustomDbCommand());
            
            if (itemcode == "00000")
            {
                cmd.SetParameterValue("@itemcode",DBNull.Value);
            }
            else
            {
                cmd.SetParameterValue("@itemcode", itemcode);
            }

           
            return cmd.ExecuteEntityList<DeviceAreaID>();
        }
        /// <summary>
        ///  设备关联的液态区域ID列表
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public List<DeviceAreaID> GetDeviceAreaID2List(string itemcode)
        {
            string viewname = "";
            viewname = "GetDeviceAreaID2List";
            var cmd = new DataCommand(viewname, new SqlCustomDbCommand());
            if (itemcode == "00000")
            {
                cmd.SetParameterValue("@itemcode", DBNull.Value);
            }
            else
            {
                cmd.SetParameterValue("@itemcode", itemcode);
            }
            return cmd.ExecuteEntityList<DeviceAreaID>();
        }
    }
}
