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
    public class Chats
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(Properties.Resources.ZeroMessenger) };

        public int id { get; }
        public string name { get; }
        public string avatar { get; }
        public int creator { get; }
        public string type { get; }
        public int unreadedmessagescount { get; }
        public int memberscount { get; }

        [JsonConstructor]
        private Chats(int id, string name, string avatar, int creator, string type, int unreadedmessagescount, int memberscount)
        {
            this.id = id;
            this.name = name;
            this.avatar = avatar;
            this.creator = creator;
            this.type = type;
            this.unreadedmessagescount = unreadedmessagescount;
            this.memberscount = memberscount;
        }

        public static async Task<long[]> FindPublicByName(string name, int? count, int start = 0)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"chats/findpublicbyname?name={name}&count={count}&start={start}");
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

        public static async Task<Chats> GetChatInfo(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"chats/getchatinfo?accessToken={accessToken}&chatId={chatId}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<Chats>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<long[]> GetUsers(string accessToken,long chatId, int? count, int start = 0)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"chats/getusers?accessToken={accessToken}&chatid={chatId}&count={count}&start={start}");
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

        public static async Task<bool> CreateDialog(string accessToken, long secondUserId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(secondUserId.ToString()), "secondUserId");

            var httpResponse = await httpClient.PostAsync("chats/createdialog", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> CreateGroup(string accessToken, string name, string userIds)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(userIds), "userIds ");

            var httpResponse = await httpClient.PostAsync("chats/creategroup", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> CreatePublic(string accessToken, string name, string userIds)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(userIds), "userIds ");

            var httpResponse = await httpClient.PostAsync("chats/createpublic", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }


        public static async Task<bool> ChangeName(string accessToken, string newName, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(newName), "newName ");
            content.Add(new StringContent(chatId.ToString()), "chatId ");

            var httpResponse = await httpClient.PostAsync("chats/createpublic", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> ChangeAvatar(string accessToken, long chatId, long fileId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(chatId.ToString()), "chatId ");
            content.Add(new StringContent(fileId.ToString()), "fileId ");

            var httpResponse = await httpClient.PostAsync("chats/changeavatar", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> JoinThePublic(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");

            var httpResponse = await httpClient.PostAsync("chats/jointhepublic", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<long[]> GetBannedUsers(string accessToken, long chatId, int? count, int start = 0)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"chats/getbannedusers?accessToken={accessToken}&chatid={chatId}&count={count}&start={start}");
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

        public static async Task<bool> RemoveUserFromGroup(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/removeuserfromgroup", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> Leave(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");

            var httpResponse = await httpClient.PostAsync("chats/leave", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> BanUser(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/banuser", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> UnBanUser(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/unbanuser", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> InviteToGroup(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/invitetogroup", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> SetMessagesReaded(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(chatId.ToString()), "chatId");

            var httpResponse = await httpClient.PostAsync("chats/setmessagesreaded", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }
    }
}
