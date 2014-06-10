using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DBUtility;
using Framework.Data;
using NTS.WEB.DataContact;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;
using NTS.WEB.ResultView;

namespace NTS.WEB.DAL
{
    public class User : IUser
    {
        public List<UserList> GetUsers()
        {
         
            var cmd = new DataCommand("getUserList", new SqlCustomDbCommand());
           
            return cmd.ExecuteEntityList<UserList>();
            
        }

//        public void AddUser(QueryUser model)
//        {
//            try
//            {
//                string sql = " select max(id) + 1 from tb_user ";
//                int MaxID = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
//                sql = @"insert into tb_user
//                            (id,cname,password,status,groups)
//                            values
//                            (@id,@cname,@password,@status,@groups)";
//                SqlParameter[] paras = new SqlParameter[] {
//                    new SqlParameter("@cname", model.UserName),
//                    new SqlParameter("@password", model.Password),
//                    new SqlParameter("@status", model.Status),
//                    new SqlParameter("@groups", model.GroupId) ,
//                     new SqlParameter("@id", MaxID) 
//                };
//                SqlHelper.ExecuteSql(sql, paras);
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//        }

//        public void UpdateUser(QueryUser model)
//        {
//            try
//            {
//                string sql = @"update tb_user
//                                set
//                                cname=@cname,
//                                password=@password,
//                                status=@status,
//                                groups=@groups
//                                where id=@id";
//                SqlParameter[] paras = new SqlParameter[] {
//                    new SqlParameter("@cname", model.UserName),
//                    new SqlParameter("@password", model.Password),
//                    new SqlParameter("@status", model.Status),
//                    new SqlParameter("@groups", model.GroupId),
//                     new SqlParameter("@id", model.UserID) 
//                };
//                SqlHelper.ExecuteSql(sql, paras);
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//        }

//        public void DeleteUser(int id)
//        {
//            try
//            {
//                string sql = @"delete from tb_user
//                                where id=@id";
//                SqlParameter[] paras = new SqlParameter[] {
//                     new SqlParameter("@id", id) 
//                };
//                SqlHelper.ExecuteSql(sql, paras);
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//        }


        //public bool IsExistUserName(QueryUser model)
        //{
        //    string sql = "select id from tb_user where cname=@cname";
        //    SqlParameter[] paras = new SqlParameter[] {
        //            new SqlParameter("@cname", model.UserName)
        //        };
        //    if (model.UserID > 0)
        //    {
        //        int id = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, paras));
        //        if (id > 0 && id != model.UserID)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {

        //        return Convert.ToInt32(SqlHelper.ExecuteScalar(sql, paras)) > 0 ? true : false;
        //    }


        //}


        public QueryUser GetSingleUser(int userid)
        {
            try
            {
                var cmd = new DataCommand("getSingleUser", new SqlCustomDbCommand());
                cmd.SetParameterValue("@id", userid);
              //  List<QueryUser> list = cmd.ExecuteEntityList<QueryUser>();
                return cmd.ExecuteEntity<QueryUser>();
              
            }
            catch(Exception ee)
            {
                return null;
            }
           
        }


        public List<QueryUserMenu> GetSingleUserMenu(string username)
        {
            try
            {
                var cmd = new DataCommand("getSingleUserMenuList", new SqlCustomDbCommand());
                cmd.SetParameterValue("@name", username);
                return cmd.ExecuteEntityList<QueryUserMenu>();

            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
