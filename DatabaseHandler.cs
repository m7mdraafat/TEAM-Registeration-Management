using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams_Register
{
    public class DatabaseHandler
    {
        private const string ConnectionString = "Data Source=LAPTOP-5H1JN2LT;Initial Catalog=UserDatabase;Integrated Security=True";

        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
