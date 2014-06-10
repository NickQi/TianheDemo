using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;
using Framework.Data;

namespace DBUtility
{
    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public abstract class SqlHelper
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        public static string connectionString = GetConnectString();

       

        #region 公用方法

        public static string GetConnectString()
        {
            //DataCommand connCmd = new DataCommand("GetAreaInfo", new SqlCustomDbCommand());
            //return connCmd.ActualDatabase.ConnectionString;
            return "";
        }

        /// <summary>
        /// 获取最大番号+1
        /// </summary>
        /// <param name="FieldName">列名</param>
        /// <param name="TableName">表名</param>
        /// <returns>番号</returns>
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="strSql">SQL</param>
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="cmdParms">查询参数</param>
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion


        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

       

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", DbType.String);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object ExecuteScalar(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public static object ExecuteScalar(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                try
                {
                  
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = 0;
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        public static void ExecuteSql(List<string> SQLString_list, List<SqlParameter[]> cmdParms_list)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction;
            transaction = connection.BeginTransaction();
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.Text;
            try
            {
                for (int i = 0; i <= SQLString_list.Count - 1; i++)
                {
                    cmd.CommandText = SQLString_list[i];
                    foreach (SqlParameter sp in cmdParms_list[i])
                    {
                        sp.Value = ToDBType(sp.Value);
                        cmd.Parameters.Add(sp);
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                }
                transaction.Commit();
            }
            catch (Exception ee)
            {
                transaction.Rollback();
                throw new ApplicationException("Transaction Error: " + ee.Message);

            }




        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OracleParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="spName">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet QuerySP(string spName, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, spName, cmdParms);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 180;//设置过期时间为3分钟
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="cmdParms">查询语句</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    parm.Value = ToDBType(parm.Value);
                    cmd.Parameters.Add(parm);
                }

            }
        }

        #endregion
        public static object ToDBType(object value)
        {
            try
            {
                if (value == null)
                {
                    //if (value.GetType() == typeof(Guid))
                    //{
                    //    if ((Guid)value == Guid.Empty)
                    //    {
                    //        return DBNull.Value;
                    //    }

                    //}
                    return DBNull.Value;
                }
            }
            catch (Exception ee)
            {

            }


            return value;
        }

        public static void SetModelByDataReader(object model, SqlDataReader sdr)
        {
            PropertyInfo[] propertys = model.GetType().GetProperties();

            for (int i = 0; i < sdr.FieldCount; i++)
            {
                foreach (PropertyInfo property in propertys)
                {
                    string name = property.Name;
                    if (sdr.GetName(i).ToUpper() == name.ToUpper())
                    {
                        SetFieldValueInSource(model, name, sdr[i]);
                        break;
                    }

                }
            }
        }

        public static void SetFieldValueInSource(object source, string name, object value)
        {
            if (source != null && !string.IsNullOrEmpty(name))
            {
                string[] names = name.Split('.');
                object obj = source;
                PropertyInfo property = null;

                for (int i = 0; i < names.Length - 1; i++)
                {
                    property = obj.GetType().GetProperty(names[i]);


                    if (property.GetValue(obj, null) == null)
                    {
                        property.SetValue(obj, Activator.CreateInstance(property.PropertyType), null);
                    }

                    obj = property.GetValue(obj, null);
                }

                if (obj != null)
                {
                    property = obj.GetType().GetProperty(names[names.Length - 1]);
                    if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Nullable<Guid>))
                    {
                        if (value.ToString() == "")
                        {
                            property.SetValue(obj, FormatValueByType(value, property.PropertyType), null);
                        }
                        else
                        {
                            property.SetValue(obj, new Guid(value.ToString()), null);
                        }

                    }
                    else
                    {
                        property.SetValue(obj, FormatValueByType(value, property.PropertyType), null);
                    }
                }
            }
        }

        public static object FormatValueByType(object value, Type propertyType)
        {
            string text = value.ToString();

            if (!string.IsNullOrEmpty(text))
            {

                string outType = (propertyType.IsGenericType) ? Nullable.GetUnderlyingType(propertyType).Name : propertyType.Name;
                if (propertyType.IsEnum)
                {
                    outType = "Int32";
                }
                if (propertyType.Name == "Guid")
                {
                    outType = "String";
                }
                Type type = typeof(Convert);

                MethodInfo method = type.GetMethod("To" + outType, new Type[] { typeof(string) });


                return method.Invoke(null, new object[] { text });


            }

            return null;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dt">要插入的数据（列名与数据库中的列名相同）</param>
        /// <param name="destinationTableName">数据库中的目标表的名称</param>
        /// <returns>如果插入成功则返回true，否则返回false</returns>
        /// <remarks>作成者：高奇，作成日：2013-9-6</remarks>
        public static bool BulkInsert(DataTable dt, string destinationTableName)
        {
            if (dt == null || dt.Rows.Count == 0) return false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
                bulkCopy.DestinationTableName = destinationTableName;
                bulkCopy.BatchSize = dt.Rows.Count;
                try
                {
                    connection.Open();
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    if (bulkCopy != null)
                        bulkCopy.Close();
                }
                return true;
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dts">要插入的数据列表（列名与数据库中的列名相同）</param>
        /// <param name="destinationTableNames">数据库中的目标表的名称列表（列表名与dts顺序相同）</param>
        /// <returns>如果插入成功则返回true，否则返回false</returns>
        /// <remarks>作成者：高奇，作成日：2013-9-6</remarks>
        public static bool BulkInsert(DataTable[] dts, string[] destinationTableNames)
        {
            if (dts == null || dts.Length == 0 || destinationTableNames == null || destinationTableNames.Length == 0) return false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    for (int i = 0; i < dts.Length; i++)
                    {
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, tran);
                        bulkCopy.DestinationTableName = destinationTableNames[i];
                        bulkCopy.BatchSize = dts[i].Rows.Count;

                        bulkCopy.WriteToServer(dts[i]);
                        bulkCopy.Close();
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
                connection.Close();
                return true;
            }
        }
    }
}
