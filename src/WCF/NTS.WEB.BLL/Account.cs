using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.BLL
{
    public class Account
    {
        private readonly IAccount _dal = DataSwitchConfig.CreateAccount();
        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public Model.Account GetAccount(string username)
        {
            return _dal.GetAccount(username);
        }
    }
}
