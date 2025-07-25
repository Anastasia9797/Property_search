using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Property_search
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "property_search";
            string username = "monty";
            string password = "some_password1";
            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}

