using BookReaderApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
            //Locking
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


            //Lock to ensure when adding mutiple users, each user is being added

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

        public static bool AddPitch(string userId, string pitchContent, bool isPublished, bool isReviewed, bool published)
        {
            if (PitchExists(userId, pitchContent))
            {
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Pitch (userid, pitchcontent, isPublished, isReviewed, published) VALUES (@userid, @pitchcontent, @isPublished, @isReviewed, @published)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", userId);
                        command.Parameters.AddWithValue("@pitchcontent", pitchContent);
                        command.Parameters.AddWithValue("@isPublished", isPublished ? 1 : 0);
                        command.Parameters.AddWithValue("@isReviewed", isReviewed ? 1 : 0);

                        command.Parameters.AddWithValue("@published", published ? 1 : 0);


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
                        PitchContent = reader["pitchcontent"].ToString(),
                        IsPublished = reader.GetBoolean(reader.GetOrdinal("isPublished")),
                        IsReviewed = reader.GetBoolean(reader.GetOrdinal("isReviewed")),
                        Published = reader.GetBoolean(reader.GetOrdinal("Published")),
                    };
                    pitches.Add(pitch);
                }
            }
            return pitches;
        }
        public static List<PitchModel> GetUserPitches(string userId)
        {
            List<PitchModel> pitches = new List<PitchModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [Pitch] WHERE userid = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PitchModel pitch = new PitchModel
                    {
                        Pid = reader["pid"].ToString(),
                        Userid = reader["userid"].ToString(),
                        PitchContent = reader["pitchcontent"].ToString(),
                        IsPublished = reader.GetBoolean(reader.GetOrdinal("isPublished")),
                        IsReviewed = reader.GetBoolean(reader.GetOrdinal("isReviewed")),
                        Published = reader.GetBoolean(reader.GetOrdinal("Published")),
                    };
                    pitches.Add(pitch);
                }
            }
            return pitches;
        }

        public static bool canPublish(string pitchId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Pitch SET isPublished = 1 WHERE pid = @pitchId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pitchId", pitchId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                       
                        if (rowsAffected > 0)
                        {
                            return true; 
                        }
                        else
                        {
                            return false; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating pitch: {ex.Message}");
                return false;
            }
        }

        public static bool published(string pitchId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Pitch SET published = 1 WHERE pid = @pitchId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pitchId", pitchId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating pitch: {ex.Message}");
                return false;
            }
        }
        public static bool isReviewed(string pitchId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Pitch SET isReviewed = 1 WHERE pid = @pitchId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pitchId", pitchId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating pitch: {ex.Message}");
                return false;
            }
        }


        public static bool DeletePitch(string pitchId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete the record
                    string deleteQuery = "DELETE FROM Pitch WHERE pid = @pitchId";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@pitchId", pitchId);
                        int deleteRowsAffected = deleteCommand.ExecuteNonQuery();

                        // Check if the record was successfully deleted
                        if (deleteRowsAffected > 0)
                        {
                            return true; // Deletion successful
                        }
                        else
                        {
                            // Handle deletion failure
                            Debug.WriteLine("Failed to delete the pitch record.");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting pitch: {ex.Message}");
                return false;
            }
        }

        public void SendEmail(string senderEmail, string recipientEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(senderEmail))
            {
                throw new ArgumentException("Sender email cannot be null or empty.", nameof(senderEmail));
            }
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, "zofhvzlzzzawgihz"),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(recipientEmail);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}