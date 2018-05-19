using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace API
{
    public enum Service { Vk, Google, Instagram };
    public class Accounts
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(Properties.Resources.ZeroMessenger) };

        public string accessToken { get; }
        public string refreshToken { get; }
        public long userId { get; }

        [JsonConstructor]
        private Accounts(string accessToken, string refreshToken, long userId)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            this.userId = userId;
        }

        public static async Task<Accounts> OAuth(string accessToken, Service service)
        {
            string serviceString = "";
            switch (service)
            {
                case Service.Vk:
                    serviceString = "vk";
                    break;
                case Service.Google:
                    serviceString = "google";
                    break;
                case Service.Instagram:
                    serviceString = "instagram";
                    break;
            }

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(serviceString), "service");

            var httpResponse = await httpClient.PostAsync("accounts/oauth", content);
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

        public static async Task<Accounts>Auth(string login, string password)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(login), "login");
            content.Add(new StringContent(password), "password");

            var httpResponse = await httpClient.PostAsync("accounts/auth", content);
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

        public static async Task<bool> Register(string login, string password, string name)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(login), "login");
            content.Add(new StringContent(password), "password");
            content.Add(new StringContent(name), "name");

            var httpResponse = await httpClient.PostAsync("accounts/register", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> Delete(string accessToken)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");

            var httpResponse = await httpClient.PostAsync("accounts/delete", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> ChangePassword(string accessToken, string oldPassword, string newPassword)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accesstoken");
            content.Add(new StringContent(oldPassword), "oldpassword");
            content.Add(new StringContent(newPassword), "newpassword");

            var httpResponse = await httpClient.PostAsync("accounts/changepassword", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }
    }
}
