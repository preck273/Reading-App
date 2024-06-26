﻿using BookReaderApp.Models;
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

            List<LoginModel> userList = JsonConvert.DeserializeObject<List<LoginModel>>(jsonData);
            foreach (LoginModel user in userList)
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

    public async Task<string> GetUserImage(string username, string password)
    {
        try
        {
            string jsonData = await GetAllJson();

            List<LoginModel> userList = JsonConvert.DeserializeObject<List<LoginModel>>(jsonData);
            foreach (LoginModel user in userList)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user.Image;
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
    public async Task<string> GetUserEmail(string username, string password)
    {
        try
        {
            string jsonData = await GetAllJson();

            List<LoginModel> userList = JsonConvert.DeserializeObject<List<LoginModel>>(jsonData);
            foreach (LoginModel user in userList)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user.Email;
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

    public async Task<string> GetById(string id, string format)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://localhost:7005/api/User/Get/{id}";
            if (format.ToLower() == "xml")
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
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
                    
                    return content;
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

   
}
