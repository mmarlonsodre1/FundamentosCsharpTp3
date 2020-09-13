using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FundamentosCsharpTp3.Models;
using Microsoft.Extensions.Configuration;

namespace FundamentosCsharpTp3.WebApplication.Repository
{
    public class UserRepository
    {
        private string ConnectionString { get; set; }

        public UserRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("AtConnectionString");
        }

        public User GetByUsername(string username)
        {
            List<User> result = new List<User>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM [dbo].[User] WHERE Username Like @P1";
                sqlCommand.Parameters.AddWithValue("P1", username);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    User person = new User(
                        Guid.Parse(reader["Id"].ToString()),
                        reader["Username"].ToString());
                    result.Add(person);
                }

                connection.Close();
                if (result.Count > 0) return result[0];
                else return null;
            }
        }

        public void Save(User user)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[User](Id, Username) VALUES (@P1, @P2)";
                sqlCommand.Parameters.AddWithValue("P1", user.Id);
                sqlCommand.Parameters.AddWithValue("P2", user.Username);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
