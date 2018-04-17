using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Users
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44364") };

        public static async Task<bool> ChangeName(string accessToken, string newName)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accesstoken");
            content.Add(new StringContent(newName), "newname");

            var httpResponse = await httpClient.PostAsync("users/changename", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> ChangeDescription(string accessToken, string newDescription)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accesstoken");
            content.Add(new StringContent(newDescription), "newdescription");

            var httpResponse = await httpClient.PostAsync("users/changedescription", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> ChangeAvatar(string accessToken, long fileid)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accesstoken");
            content.Add(new StringContent(Convert.ToString(fileid)), "fileid");

            var httpResponse = await httpClient.PostAsync("users/changeavatar", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> BanUser(string accessToken, string userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accesstoken");
            content.Add(new StringContent(userId), "userid");

            var httpResponse = await httpClient.PostAsync("users/banuser", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> UnBanUser(string accessToken, string userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accesstoken");
            content.Add(new StringContent(userId), "userid");

            var httpResponse = await httpClient.PostAsync("users/unbanuser", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<long[]> GetBannedUsers(string accessToken, int? count, int start = 0)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"users/getbannedusers?accessToken={accessToken}&count={count}&start={start}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<long[]>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<long[]> FindUsersByName(string name, int? count, int start = 0)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"users/findusersbyname?name={name}&count={count}&start={start}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<long[]>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User> GetUserInfo(long userId)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"users/getuserinfo?userid={userId}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<User>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<long[]> GetChats(string accessToken, int? count = 10, int start = 0)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"users/getchats?accesstoken={accessToken}&count={count}&start={start}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<long[]>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static byte[] GetFileAsBytes(string link)
        {
            try
            {
                return (new HttpClient()).GetByteArrayAsync(link).Result;
            }
            catch { }

            return null;
        }

        public class User
        {
            public int id { get; }
            public string name { get; }
            public string avatar { get; }
            public string description { get; }

            [JsonConstructor]
            private User(int id,string name, string avatar, string description)
            {
                this.id = id;
                this.name = name;
                this.avatar = avatar;
                this.description = description;
            }
        }

        public class Chat
        {
            public string name { get; }
            public string avatar { get; }
            public int creator { get; }
            public string type { get; }
            public int unreadedmessagescount { get; }
            public int memberscount { get; }

            [JsonConstructor]
            private Chat(string name, string avatar, int creator, string type, int unreadedmessagescount, int memberscount)
            {
                this.name = name;
                this.avatar = avatar;
                this.creator = creator;
                this.type = type;
                this.unreadedmessagescount = unreadedmessagescount;
                this.memberscount = memberscount;
            }
        }

    }
}
