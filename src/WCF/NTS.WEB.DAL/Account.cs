using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class Account : IAccount
    {
        /// <summary>
        /// 根据用户名获取用户的对象
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Model.Account GetAccount(string username)
        {
            var cmd = new DataCommand("getaccount", new SqlCustomDbCommand());
            cmd.SetParameterValue("@username",username);
            return cmd.ExecuteEntity<Model.Account>();
        }
    }
}
