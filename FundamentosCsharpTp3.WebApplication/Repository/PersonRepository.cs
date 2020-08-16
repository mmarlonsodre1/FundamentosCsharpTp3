using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FundamentosCsharpTp3.Models;
using Microsoft.Extensions.Configuration;

namespace FundamentosCsharpTp3.WebApplication.Repository
{
    public class PersonRepository
    {
        private string ConnectionString { get; set; }

        public PersonRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("AtConnectionString");
        }
        
        public List<Person> GetAllBirthdays()
        {
            List<Person> result = new List<Person>();
            
            using(var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM Person";

                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person(
                        Guid.Parse(reader["Id"].ToString()), 
                        reader["Name"].ToString(), 
                        reader["SurName"].ToString(),
                        reader["Email"].ToString(),
                        DateTime.Parse(reader["Birthday"].ToString()));
                    result.Add(person);
                }
                
                connection.Close();
                return result;
            }
        }

        public List<Person> GetBirthdaysByName(string name)
        {
            List<Person> result = new List<Person>();
            
            using(var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM Person WHERE Name LIKE @P1";
                sqlCommand.Parameters.AddWithValue("P1", name);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person(
                        Guid.Parse(reader["Id"].ToString()), 
                        reader["Name"].ToString(), 
                        reader["SurName"].ToString(),
                        reader["Email"].ToString(),
                        DateTime.Parse(reader["Birthday"].ToString()));
                    result.Add(person);
                }
                
                connection.Close();
                return result;
            }
        }
        
        public Person GetBirthdayById(Guid id)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM Person WHERE Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", id);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                
                if (reader.Read())
                {
                    Person person = new Person(
                    Guid.Parse(reader["Id"].ToString()),
                    reader["Name"].ToString(),
                    reader["SurName"].ToString(),
                    reader["Email"].ToString(),
                    DateTime.Parse(reader["Birthday"].ToString()));

                    connection.Close();
                    return person;
                }

                connection.Close();
                return null;
            }
        }

        public void Save(Person person)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "INSERT INTO Person(Id, Name, SurName, Email, Birthday) VALUES (@P1, @P2, @P3, @P4, @P5)";
                sqlCommand.Parameters.AddWithValue("P1", person.Id);
                sqlCommand.Parameters.AddWithValue("P2", person.Name);
                sqlCommand.Parameters.AddWithValue("P3", person.SurName);
                sqlCommand.Parameters.AddWithValue("P4", person.Email);
                sqlCommand.Parameters.AddWithValue("P5", person.Birthday);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        
        public void Update(Person person)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "UPDATE Person SET Name = @P1, SurName = @P2, Email = @P3, Birthday = @P4 where Id = @P5";
                sqlCommand.Parameters.AddWithValue("P1", person.Name);
                sqlCommand.Parameters.AddWithValue("P2", person.SurName);
                sqlCommand.Parameters.AddWithValue("P3", person.Email);
                sqlCommand.Parameters.AddWithValue("P4", person.Birthday);
                sqlCommand.Parameters.AddWithValue("P5", person.Id);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        
        public void Delete(Guid id)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "DELETE FROM Person Where Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", id);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

    }

}