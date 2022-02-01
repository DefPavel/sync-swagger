using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using sync_swagger.Models.Personnel;

namespace sync_swagger.Service.Api
{
    public class ApiService
    {

        private static readonly string _ApiBaseUrl = @"http://localhost:8080/api";

        private readonly HttpClient _client;

  
        public ApiService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(_ApiBaseUrl);
        }

        // async serialize
        private static async Task<string> GetJson(object obj)
        {
            //_ = Configuration["fgdfgdfgdfgdfg"];
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj, obj.GetType());
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        // Post httpClient
        public async Task<T> PostAsync<T>(string url , T obj)
        {
            // Get convert data json
            var content = await GetJson(obj);
            // web respone 
            var httpResponse = await _client.PostAsync(_ApiBaseUrl + url, new StringContent(content, Encoding.UTF8, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }

            var createdTask = JsonSerializer.Deserialize<T>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }

        public async Task<GlobalArray> PostArrayByTokenAsync(string url, string token, GlobalArray objs)
        {
            // Get convert data json
            var content = await GetJson(objs);
            // Add token 
            _client.DefaultRequestHeaders.Add("auth-token", token);
            // web respone  
            var httpResponse = await _client.PostAsync(_ApiBaseUrl + url, new StringContent(content, Encoding.UTF8, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }

            var createdTask = JsonSerializer.Deserialize<GlobalArray>(await httpResponse.Content.ReadAsStringAsync());
            return createdTask;
        }
        // Get httpClient
        public async Task<List<T>> GetAllAsync<T>(string url)
        {
            var httpResponse = await _client.GetAsync(_ApiBaseUrl + url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var tasks = JsonSerializer.Deserialize<List<T>>(content);

            return tasks;
        }



    }
}
