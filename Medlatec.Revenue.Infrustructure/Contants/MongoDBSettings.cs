using System;
using System.Collections.Generic;
using System.Text;

namespace Medlatec.Revenue.Infrustructure.Contants
{
    public class MongoDBSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UserTls { get; set; }
        public string AuthMechanism { get; set; }
        public string AuthDbName { get; set; }
        public string DbName { get; set; } 
    }
}
