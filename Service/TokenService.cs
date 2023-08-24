using NuGet.Common;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APIService
{
    public static class TokenService
    {
        public static async Task<string> GetAccessToken(string clientId, string clientSecret, string tokenEndpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                var tokenRequestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                });

                HttpResponseMessage tokenResponse = await client.PostAsync(tokenEndpoint, tokenRequestContent);
                string tokenResponseJson = await tokenResponse.Content.ReadAsStringAsync();

                var tokenObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenResponseJson);
                string accessToken = "";

                if (tokenObject != null) accessToken = tokenObject["access_token"];
                return accessToken;
            }
        }

        public static async Task<string> CallApiWithToken(string apiUrl, string accessToken, HttpClient client)
        {
            using(var _client = new HttpClient())
            {
                var test = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkQyNDFFQTI2REJBQjM3MjVFMEVDOUM3Q0YwODkxOUM0RUQ0OTg5NUFSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6IjBrSHFKdHVyTnlYZzdKeDg4SWtaeE8xSmlWbyJ9.eyJuYmYiOjE2OTI3MTIxOTgsImV4cCI6MTY5MjcxNTc5OCwiaXNzIjoiaHR0cHM6Ly9kdWNsZTozMDc5IiwiYXVkIjpbInBtYXBpIiwiaHR0cHM6Ly9kdWNsZTozMDc5L3Jlc291cmNlcyJdLCJjbGllbnRfaWQiOiJteWNsaWVudCIsImp0aSI6IkUzNjI0Q0YwMUU1NTZCRjY3RkJGNkZBQjg5ODFDRTIzIiwiaWF0IjoxNjkyNzEyMTk4LCJzY29wZSI6WyJwbWFwaSJdfQ.WIyz0tTo1JmoEXL7uAAubc-ocsatIxqAAt2iQCv-GbdSxE2XtQbEszj2oyMda6F_HxmlDAojG92QWYT1qUUr8YIFSEgP1BMdoeToBGy8XBnBtXmXkVIm9ea0AS1vAcqdI7fKpFZ0yZWUr_pdwfKiC_2lHReE0cdQfWwisXubdULy7dMW_mwQJSU2BVTLBiLUbaQMefYOQgM6OzHndnlFbKet2Zs4V130p_LxxlrqNNGWKeS59-fBxMvp8_h_dVl44CH1A8jzi0QiIQpp3Cfzb1MlGzwui4WAS-WNlYqK8fWgrvVa0JdlC809SZ2UdfUhTfPh1Y_3meKTFOHtpXPi2A";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", test);
                _client.BaseAddress = new Uri("https://ducle:3081");
                var response = await _client.GetAsync("/api/v1/pmquality/batches");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
           

            //using (HttpClient client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}"); 
            //    var response = await client.GetAsync(apiUrl);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //        return responseBody;
            //    }
            //    else
            //    {
            //        Debug.WriteLine($"Request failed with status code: {response.StatusCode}");
            //        return "";
            //    }
            //}
            return "";
        }
    }
}
