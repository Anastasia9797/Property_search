using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property_search
{
    class DBMySQLUtils
    {
        public static MySqlConnection
            GetDBConnection(string host, int port, string database, string username, string password)
        {
            String connString = "Server = " + host + ";Database = " + database +
                ";port = " + port + ";User id = " + username + ";password = " + password;
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }
    }
}
