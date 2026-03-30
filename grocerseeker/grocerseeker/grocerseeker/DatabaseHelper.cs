using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
namespace grocerseeker
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=grocerseeker;uid=root;pwd=;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
