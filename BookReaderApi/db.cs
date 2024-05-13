using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Data;

namespace bookAppApi.Entities
{
    public class db
    {

        private static string connectionString = @"Data Source=DESKTOP-MUE5L5M\SQLEXPRESS06;Initial Catalog=Quintor;Integrated Security=True;encrypt=false";



		//Uses stored procedure to add a superuser id on api start after checking if the database has one
		private static void AddSuperuser(SqlConnection connection)
        {
            using (var command = new SqlCommand("AddSuperUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Superuser added successfully."); 
                connection.Close();
            }
        }


        private static bool CheckIfSuperuserExists(SqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM [User] WHERE CONVERT(nvarchar(50), username) = @username";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", "Superuser");
                connection.Open();
                int count = (int)command.ExecuteScalar();
                connection.Close();

                return count > 0;
            }
        }


        public static void UserFaker()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                // superuser already exists
                bool superuserExists = CheckIfSuperuserExists(conn);

                if (!superuserExists)
                {
                    // Superuser doesn't exist
                    AddSuperuser(conn);
                }
                else
                {
                    Console.WriteLine("Superuser already exists.");
                }
            }
        }

    }
}