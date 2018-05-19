using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Tokens
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(Properties.Resources.ZeroMessenger) };

        public long userId { get; }
        public DateTime date { get; }
        public DateTime expire { get; }

        [JsonConstructor]
        private Tokens(long userId, DateTime date, DateTime expire)
        {
            this.userId = userId;
            this.date = date;
            this.expire = expire;
        }

        public static async Task<Accounts> RefreshTokens(string refreshToken)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(refreshToken), "refreshtoken");

            var httpResponse = await httpClient.PostAsync("tokens/refreshtokens", content);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<Accounts>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Tokens> CheckToken(string accessToken)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"tokens/checktoken?accesstoken={accessToken}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<Tokens>(stringResponse);
            }
            catch
            {
                return null;
            }
        }
    }
}