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

namespace NTS.WEB.DAL
{
    public class UserGroup : IUserGroup
    {
        public List<UserGroupModel> GetUserGroups()
        {
            var cmd = new DataCommand("getUserGroupList", new SqlCustomDbCommand());
            return cmd.ExecuteEntityList<Model.UserGroupModel>();
        }

//        public void AddUserGroup(QueryUserGroup model)
//        {
//            try
//            {

//                string sql = " select max(id) + 1 from tb_usergroup ";
//                int MaxID = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
//                List<String> sqlList = new List<string>();
//                List<SqlParameter[]> parasList = new List<SqlParameter[]>();

//                sql = @"insert into tb_usergroup
//                            (id,cname,description)
//                            values
//                            (@id,@cname,@description)";
//                SqlParameter[] paras = new SqlParameter[] {
//                     new SqlParameter("@id",MaxID) ,
//                    new SqlParameter("@cname", model.UserGroupName),
//                    new SqlParameter("@description", model.Description)
                  
                   
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPMENURIGHT where usergroupid=@id";
//                paras = new SqlParameter[] {
//                     new SqlParameter("@id",MaxID)
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPLIQUIDRIGHT where usergroupid=@id";
//                paras = new SqlParameter[] {
//                     new SqlParameter("@id",MaxID)
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPAREARIGHT where usergroupid=@id";
//                paras = new SqlParameter[] {
//                     new SqlParameter("@id",MaxID)
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                foreach (var menu in model.UserGroupMenuRights)
//                {
//                    sql = @"insert into TB_USERGROUPMENURIGHT
//                        (usergroupid,menuid)
//                        values
//                        (@usergroupid,@menuid)";
//                    paras = new SqlParameter[] {
//                    new SqlParameter("@usergroupid", MaxID),
//                    new SqlParameter("@menuid",  menu)};

//                    sqlList.Add(sql);
//                    parasList.Add(paras);
//                }
//                foreach (var liquid in model.UserGroupLiquidRights)
//                {
//                    sql = @"insert into TB_USERGROUPLIQUIDRIGHT
//                        (usergroupid,areaid)
//                        values
//                        (@usergroupid,@areaid)";
//                    paras = new SqlParameter[] {
//                    new SqlParameter("@usergroupid", MaxID),
//                    new SqlParameter("@areaid",  liquid)};

//                    sqlList.Add(sql);
//                    parasList.Add(paras);
//                }

//                foreach (var area in model.UserGroupAreaRights)
//                {
//                    sql = @"insert into TB_USERGROUPAREARIGHT
//                        (usergroupid,areaid)
//                        values
//                        (@usergroupid,@areaid)";
//                    paras = new SqlParameter[] {
//                    new SqlParameter("@usergroupid", MaxID),
//                    new SqlParameter("@areaid",  area)};

//                    sqlList.Add(sql);
//                    parasList.Add(paras);
//                }

//                SqlHelper.ExecuteSql(sqlList, parasList);
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//        }

//        public void UpdateUserGroup(QueryUserGroup model)
//        {
//            try
//            {


//                List<String> sqlList = new List<string>();
//                List<SqlParameter[]> parasList = new List<SqlParameter[]>();

//                string sql = @"update tb_usergroup
//                                set
//                                cname=@cname,
//                                description=@description
//                                where id=@id";
//                SqlParameter[] paras = new SqlParameter[] {
//                    new SqlParameter("@cname", model.UserGroupName),
//                    new SqlParameter("@description", model.Description),
//                     new SqlParameter("@id", model.UserGroupID) 
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPMENURIGHT where usergroupid=@id";
//                paras = new SqlParameter[] {
//                     new SqlParameter("@id",model.UserGroupID)
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPLIQUIDRIGHT where usergroupid=@id";
//                paras = new SqlParameter[] {
//                     new SqlParameter("@id",model.UserGroupID)
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPAREARIGHT where usergroupid=@id";
//                paras = new SqlParameter[] {
//                     new SqlParameter("@id",model.UserGroupID)
//                };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                foreach (var menu in model.UserGroupMenuRights)
//                {
//                    sql = @"insert into TB_USERGROUPMENURIGHT
//                        (usergroupid,menuid)
//                        values
//                        (@usergroupid,@menuid)";
//                    paras = new SqlParameter[] {
//                    new SqlParameter("@usergroupid", model.UserGroupID),
//                    new SqlParameter("@menuid",  menu)};

//                    sqlList.Add(sql);
//                    parasList.Add(paras);
//                }
//                foreach (var liquid in model.UserGroupLiquidRights)
//                {
//                    sql = @"insert into TB_USERGROUPLIQUIDRIGHT
//                        (usergroupid,areaid)
//                        values
//                        (@usergroupid,@areaid)";
//                    paras = new SqlParameter[] {
//                    new SqlParameter("@usergroupid", model.UserGroupID),
//                    new SqlParameter("@areaid",  liquid)};

//                    sqlList.Add(sql);
//                    parasList.Add(paras);
//                }

//                foreach (var area in model.UserGroupAreaRights)
//                {
//                    sql = @"insert into TB_USERGROUPAREARIGHT
//                        (usergroupid,areaid)
//                        values
//                        (@usergroupid,@areaid)";
//                    paras = new SqlParameter[] {
//                    new SqlParameter("@usergroupid", model.UserGroupID),
//                    new SqlParameter("@areaid",  area)};

//                    sqlList.Add(sql);
//                    parasList.Add(paras);
//                }

//                SqlHelper.ExecuteSql(sqlList, parasList);
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//        }

//        public void DeleteUserGroup(int id)
//        {
//            try
//            {
//                List<String> sqlList = new List<string>();
//                List<SqlParameter[]> parasList = new List<SqlParameter[]>();

//                string sql = @"delete from TB_USERGROUPMENURIGHT where usergroupid=@id";
//                SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@id",id)};
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPLIQUIDRIGHT where usergroupid=@id";
//                paras = new SqlParameter[] { new SqlParameter("@id", id) };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from TB_USERGROUPAREARIGHT where usergroupid=@id";
//                paras = new SqlParameter[] { new SqlParameter("@id", id) };
//                sqlList.Add(sql);
//                parasList.Add(paras);

//                sql = @"delete from tb_usergroup where id=@id";
//                paras = new SqlParameter[] { new SqlParameter("@id", id) };
//                sqlList.Add(sql);
//                parasList.Add(paras);


//                SqlHelper.ExecuteSql(sqlList, parasList);
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//        }


        public QueryUserGroup GetSingleUserGroup(int usergroupid)
        {
            try
            {
               
                var cmd = new DataCommand("getSingleUserGroup", new SqlCustomDbCommand());
                cmd.SetParameterValue("@id", usergroupid);
                QueryUserGroup result = cmd.ExecuteEntity<QueryUserGroup>();
                if (result!=null)
                {
                    result.UserGroupMenuRights = new List<int>();
                    result.UserGroupLiquidRights = new List<int>();
                    result.UserGroupAreaRights = new List<int>();

                    cmd = new DataCommand("getMenuRightByUserGroupID", new SqlCustomDbCommand());
                    cmd.SetParameterValue("@id", usergroupid);
                    List<GroupRight> rightlist = cmd.ExecuteEntityList<GroupRight>();
                    foreach (var right in rightlist)
                    {
                        result.UserGroupMenuRights.Add(right.RightID);
                    }


                    cmd = new DataCommand("getLiquidRightByUserGroupID", new SqlCustomDbCommand());
                    cmd.SetParameterValue("@id", usergroupid);
                    rightlist = cmd.ExecuteEntityList<GroupRight>();
                    foreach (var right in rightlist)
                    {
                        result.UserGroupLiquidRights.Add(right.RightID);
                    }
                    cmd = new DataCommand("getAreaRightByUserGroupID", new SqlCustomDbCommand());
                    cmd.SetParameterValue("@id", usergroupid);
                    rightlist = cmd.ExecuteEntityList<GroupRight>();
                    foreach (var right in rightlist)
                    {
                        result.UserGroupAreaRights.Add(right.RightID);
                    }
                }

               

                return result;

            }
            catch (Exception ee)
            {
                return null;
            }
        }

        //public bool IsExistUserGroupName(QueryUserGroup model)
        //{
        //    string sql = "select id from TB_USERGROUP where cname=@cname";
        //    SqlParameter[] paras = new SqlParameter[] {
        //            new SqlParameter("@cname", model.UserGroupName)
        //        };
        //    if (model.UserGroupID > 0)
        //    {
        //        int id = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, paras));
        //        if (id > 0 && id != model.UserGroupID)
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
    }
}
