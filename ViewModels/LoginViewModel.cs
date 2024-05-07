using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace BookReaderApp.ViewModels;

class LoginViewModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Level { get; set; }
    public string Pfp { get; set; }



    public async Task<string> GetAllJson()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://localhost:7005/api/User/GetAll";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Failed to retrieve data.");
            }
        }
    }
    public async Task<string> GetUserId(string username, string password)
    {
        try
        {
            string jsonData = await GetAllJson();

            List<LoginViewModel> userList = JsonConvert.DeserializeObject<List<LoginViewModel>>(jsonData);
            foreach (LoginViewModel user in userList)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user.Id;
                }
            }
            return "null";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "null";
        }
    }

    public async Task<XmlDocument> GetById(int id, string format)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://localhost:7005/api/UserUser/Get/{id}";
            if (format.ToLower() == "xml")
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(content);
                    return xmlDocument;
                }
                else
                {
                    throw new Exception("Failed to retrieve data.");
                }
            }
            else if (format.ToLower() == "json")
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(content);
                    return xmlDocument;
                }
                else
                {
                    throw new Exception("Failed to retrieve data.");
                }
            }
            else
            {
                throw new ArgumentException("Unsupported format. Please specify 'xml' or 'json'.");
            }

        }
    }

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

    public async Task Post(string id, string username, string password, string level, string pfp)
    {
        LoginViewModel user = new LoginViewModel

        {
            Id = id,
            Username = username,
            Password = password,
            Level = level,
            Pfp = pfp,
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

/*
< VerticalStackLayout >
    < Label
        Text = "Welcome to .NET MAUI!"
        VerticalOptions = "Center"
        HorizontalOptions = "Center" />

    < Image
   Source = "dotnet_bot.png"
   HeightRequest = "185"
   Aspect = "AspectFit"
   SemanticProperties.Description = "dot net bot in a race car number eight" />

</ VerticalStackLayout >
</ ContentPage >
*/