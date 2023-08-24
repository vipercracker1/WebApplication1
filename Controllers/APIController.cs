using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication1.Models;
using APIService;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication1.Controllers
{
    [Route("api")]
    public class APIController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _token;
        private readonly IHttpClientFactory _httpClientFactory;

        public APIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://ducle:3081");
            _token = GetAccessTokenAsync("myclient", "mysecret").Result;
            if (_token == null)
            {
                throw new InvalidOperationException("Failed to retrieve access token.");
            }
            Debug.WriteLine($"Access token: {_token}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await _client.PostAsync("https://ducle:3079/connect/token", tokenRequest);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (jsonObject.TryGetValue("access_token", out var accessToken))
                {
                    return accessToken;
                }
            }

            return null;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string accessToken = await TokenService.GetAccessToken("myclient", "mysecret", @"https://ducle:3079/connect/token");
            if(accessToken != "")
            {
                string jsonObject = await TokenService.CallApiWithToken(@"https://ducle:3081/api/v1/pmquality/batches/", accessToken, _client);
            }
            List<PMAPIClient> productList = new List<PMAPIClient>();
            HttpResponseMessage response = await _client.GetAsync("/api/v1/pmquality/batches");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                productList = JsonConvert.DeserializeObject<List<PMAPIClient>>(data);
            }

            return View(productList);
        }
    }
}