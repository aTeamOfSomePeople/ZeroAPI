using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    class Tokens
    {
        private HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44364") };

        public async Task<Account> RefreshTokens(string refreshToken)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(refreshToken), "refreshtoken");

            var httpResponse = await httpClient.PostAsync("tokens/refreshtokens", content);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<Account>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Token> CheckToken(string accessToken)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"tokens/checktoken?accesstoken={accessToken}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<Token>(stringResponse);
            }
            catch
            {
                return null;
            }
        }


        public class Token
        {
            public long userId { get; }
            public DateTime date { get; }
            public DateTime expire { get; }

            [JsonConstructor]
            private Token(long userId, DateTime date, DateTime expire)
            {
                this.userId = userId;
                this.date = date;
                this.expire = expire;
            }
        }

        public class Account
        {
            public string accessToken { get; }
            public string refreshToken { get; }
            public long userId { get; }

            [JsonConstructor]
            private Account(string accessToken, string refreshToken, long userId)
            {
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
                this.userId = userId;
            }
        }
    }
}