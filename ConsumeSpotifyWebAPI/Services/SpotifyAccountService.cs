using ConsumeSpotifyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumeSpotifyWebAPI.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    { 
        private readonly HttpClient _httpclient;

        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpclient = httpClient;
        }

        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type" , "client_credentials" }
            });

            var response = await _httpclient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var resposeStream = await response.Content.ReadAsStreamAsync();
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(resposeStream);

            return authResult.access_token;
        }
    }
}
