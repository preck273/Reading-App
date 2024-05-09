using BookReaderApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.ViewModels
{
   internal class PitchesViewModel
    {
        private static object _lock = new object();
        private readonly LoginViewModel loginController = new LoginViewModel();
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\g3k01\source\repos\BookReaderApp\BookReader.mdf;Trusted_Connection=true;encrypt=false";

        //Define db connection to pitches database

        public PitchesViewModel() 
        {
            UsersFactory();
        }
        private static void AddUser(LoginModel user)
        {
            //Couse useLocking here
           
            using (SqlConnection connection = new SqlConnection(connectionString))  
            {
                string query = "INSERT INTO User (Id, Username) VALUES (@user_id, @username)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", Int32.Parse(user.Id));
                    command.Parameters.AddWithValue("@username", user.Username);

                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public async void UsersFactory()
        {
            string jsonData = await loginController.GetAllJson();

            List<LoginModel> users = JsonConvert.DeserializeObject<List<LoginModel>>(jsonData);

            // Insert each user into the database
            foreach (var user in users)
            {
                lock(_lock)
                {
                    AddUser(user);
                }
            }
        }

        private static void AddPitch(SqlConnection connection)
            {
                //Add a pitch made by a user too the pitchdb
            
            }
       
    }

}
