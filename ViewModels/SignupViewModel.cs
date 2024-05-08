using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.ViewModels
{
    internal class SignupViewModel
    {
        public static async Task PostData(string json)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    "https://localhost:7005/api/User/Create",
                     new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Json data posted successfully!");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }

        }

        public async Task Post(string username, string password, string level, string image)
        {
            Random random = new Random();
            int id = random.Next(1000, 9999);

            LoginViewModel user = new LoginViewModel

            {
                Id = id.ToString(),
                Username = username,
                Password = password,
                Level = level,
                Image = image,
            };
            string postData = JsonConvert.SerializeObject(user);
            await PostData(postData);
        }

        public async Task<string> Put(string id, string json)
        {
            try
            {
                string baseUrl = "https://localhost:7005/api/Transaction";

                string url = $"{baseUrl}/Update/{id}";

                using (var client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Send the PUT request
                    var response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Transaction updated successfully!");
                        return "Transaction updated successfully!";
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                string url = $"https://localhost:7005/api/User/Delete/{id}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("User deleted successfully!");
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Failed to delete user: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
