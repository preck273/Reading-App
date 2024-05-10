using BookReaderApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            lock (_lock)
            {
                Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} starting");
                using (SqlConnection connection = new SqlConnection(connectionString))  
                {
                    string query = "INSERT INTO [User] (user_Id, username) VALUES (@user_Id, @username)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_Id", Int32.Parse(user.Id));
                        command.Parameters.AddWithValue("@username", user.Username);

                        connection.Open();

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Thread.Sleep(2000);
                Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} exiting lock");
            }
        }

        public async void UsersFactory()
        {
            string jsonData = await loginController.GetAllJson();

            List<LoginModel> users = JsonConvert.DeserializeObject<List<LoginModel>>(jsonData);


            //Lock incase a user has been deleted or the api isn't working or a new user is being added at the same time and to make sure it behaves accordingly

            // Insert each user into the database
            foreach (var user in users)
            {
                if (!UserExists(user))
                {
                    new Thread(() => AddUser(user)).Start();
                }
            }
        }

        private bool UserExists(LoginModel user)
        {
            bool exists = false;
            string query = "SELECT COUNT(*) FROM [User] WHERE user_Id = @user_Id";
            using (SqlConnection connection = new SqlConnection(connectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@user_Id", Int32.Parse(user.Id));
                connection.Open();
                int count = (int)command.ExecuteScalar();
                exists = (count > 0);
            }
            return exists;
        }

        public static bool AddPitch(string userId, string pitchContent)
        {
            if (PitchExists(userId, pitchContent))
            {
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Pitch (userid, pitchcontent) VALUES (@userid, @pitchcontent)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", userId);
                        command.Parameters.AddWithValue("@pitchcontent", pitchContent);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding pitch: {ex.Message}");
                return false;
            }
        }

        private static bool PitchExists(string userId, string pitchContent)
        {
            bool exists = false;
            string query = "SELECT COUNT(*) FROM Pitch WHERE userid = @userid AND CONVERT(nvarchar(max), pitchcontent) = @pitchcontent";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userid", userId);
                command.Parameters.Add("@pitchcontent", SqlDbType.NVarChar, -1).Value = pitchContent;

                connection.Open();
                int count = (int)command.ExecuteScalar();
                exists = (count > 0);
            }
            return exists;
        }

        public static List<PitchModel> GetAllPitches()
        {
            List<PitchModel> pitches = new List<PitchModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [Pitch]";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PitchModel pitch = new PitchModel
                    {
                        Pid = reader["pid"].ToString(),
                        Userid = reader["userid"].ToString(),
                        PitchContent = reader["pitchcontent"].ToString()
                    };
                    pitches.Add(pitch);
                }
            }
            return pitches;
        }
    }
}