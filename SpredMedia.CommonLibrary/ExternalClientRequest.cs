
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SpredMedia.CommonLibrary
{
    public class ExternalClientRequest : IHttpClientService
    {
            private readonly IHttpClientFactory _clientFactory;
            private readonly IHttpContextAccessor _httpcontextAccessor;
            public ExternalClientRequest(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
            {
                _httpcontextAccessor = httpContextAccessor;
                _clientFactory = clientFactory;
            }

            public async Task<TRes> GetRequestAsync<TRes>(string baseUrl, string url, string token = "") where TRes : class
            {
                var client = CreateClient(baseUrl, token);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                return await GetResponseResultAsync<TRes>(client, request);
            }

            public async Task<TRes> PostRequestAsync<TRes>(string baseUrl, string url, string requestModel, string token = "")
                where TRes : class
            {
                var client = CreateClient(baseUrl, token);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(requestModel, null, "application/json")
                };
                return await GetResponseResultAsync<TRes>(client, request);
            }

            private async Task<TRes> GetResponseResultAsync<TRes>(HttpClient client, HttpRequestMessage request) where TRes : class
            {
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TRes>(responseString);
                return response.IsSuccessStatusCode ? result! : throw new ArgumentException(response?.ReasonPhrase);
            }

            private HttpClient CreateClient(string baseUrl, string token)
            {
                var client = _clientFactory.CreateClient();
                string password = _httpcontextAccessor.HttpContext.Request.Headers["password"];
                string username = _httpcontextAccessor.HttpContext.Request.Headers["username"];
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (!string.IsNullOrEmpty(username))
                    client.DefaultRequestHeaders.Add("username", username);
                if (!string.IsNullOrEmpty(password))
                    client.DefaultRequestHeaders.Add("password", password);
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
                return client;
            }
        
    }
}
