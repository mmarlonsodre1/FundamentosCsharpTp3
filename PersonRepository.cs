using System;
using System.Configuration;
using System.Data.SqlClient;

public class PersonRepository
{
	private string ConnectionString { get; set; }

	public PersonRepository(IConfigurationSystem configuration)
	{
		this.ConnectionString = configuration.GetConfig.ConnectionString("AtConnectionString");
	}

	public void Save(Person person)
    {
		using(var connection = new SqlConnection(this.ConnectionString))
        {
			if (connection.State != System.Data.ConnectionState.Open) connection.Open();
			
			SqlCommand sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = "Insert into Person(Name, SurName, Birthday) values (@P1, @P2, @P3)";
			sqlCommand.Parameters.AddWithValue("P1", person.Name);

			sqlCommand.ExecuteNonQuery();
			connection.Close();

		}

	}

}
