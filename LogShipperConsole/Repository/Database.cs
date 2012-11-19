using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace LogShipperConsole.Repository
{
    public class Database
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["MigrationManager"].ConnectionString;
        private SqlConnection _sqlConnection;

        public Database()
        {
            _sqlConnection = new SqlConnection(_connectionString);
        }

        public void Execute(string command)
        {
            _sqlConnection.Open();
            var thisCommand = _sqlConnection.CreateCommand();
            thisCommand.CommandText = command;
            var reader = thisCommand.ExecuteNonQuery();
            _sqlConnection.Close();
        }

        public SqlDataReader Read(string command)
        {
            _sqlConnection.Open();
            var thisCommand = _sqlConnection.CreateCommand();
            thisCommand.CommandText = command;
            var reader = thisCommand.ExecuteReader();
            _sqlConnection.Close();
            return reader;
        }
    }
}
