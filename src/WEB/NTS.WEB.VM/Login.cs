using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Framework.Common;

namespace NTS.WEB.VM
{
    public class Login
    {
        public Hashtable GetKeyHash()
        {
            var dataHash = new Hashtable
                {
                    {"server1", ConfigurationManager.AppSettings["server1"]},
                    {"server2", ConfigurationManager.AppSettings["server2"]}
                };
            return dataHash;
        }
    }
}
