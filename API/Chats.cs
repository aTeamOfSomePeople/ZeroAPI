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
    class Chats
    {
        private HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44364") };

        public async Task<long[]> FindPublicByName(string name, int? count, int start = 0)
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

        public async Task<Chat> GetChatInfo(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"chats/getchatinfo?accessToken={accessToken}&chatId={chatId}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<Chat>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public async Task<long[]> GetUsers(string accessToken,long chatId, int? count, int start = 0)
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

        public async Task<bool> CreateDialog(string accessToken, long secondUserId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(secondUserId.ToString()), "secondUserId ");

            var httpResponse = await httpClient.PostAsync("chats/createdialog", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> CreateGroup(string accessToken, string name, string userIds)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(userIds), "userIds ");

            var httpResponse = await httpClient.PostAsync("chats/creategroup", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> CreatePublic(string accessToken, string name, string userIds)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(userIds), "userIds ");

            var httpResponse = await httpClient.PostAsync("chats/createpublic", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }


        public async Task<bool> ChangeName(string accessToken, string newName, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(newName), "newName ");
            content.Add(new StringContent(chatId.ToString()), "chatId ");

            var httpResponse = await httpClient.PostAsync("chats/createpublic", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> ChangeAvatar(string accessToken, long chatId, long fileId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(chatId.ToString()), "chatId ");
            content.Add(new StringContent(fileId.ToString()), "fileId ");

            var httpResponse = await httpClient.PostAsync("chats/changeavatar", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> JoinThePublic(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");

            var httpResponse = await httpClient.PostAsync("chats/jointhepublic", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<long[]> GetBannedUsers(string accessToken, long chatId, int? count, int start = 0)
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

        public async Task<bool> RemoveUserFromGroup(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/removeuserfromgroup", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Leave(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");

            var httpResponse = await httpClient.PostAsync("chats/leave", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> BanUser(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/banuser", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> UnBanUser(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/unbanuser", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> InviteToGroup(string accessToken, long chatId, long userId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(Convert.ToString(userId)), "userId");

            var httpResponse = await httpClient.PostAsync("chats/invitetogroup", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> SetMessagesReaded(string accessToken, long chatId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(chatId.ToString()), "chatId");

            var httpResponse = await httpClient.PostAsync("chats/setmessagesreaded", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public class Chat
        {
            public string name { get; }
            public string avatar { get; }
            public int creator { get; }
            public string type { get; }
            public int unreadedmessagescount { get; }
            public int memberscount{ get; }

            [JsonConstructor]
            private Chat(string name, string avatar, int creator,string type, int unreadedmessagescount, int memberscount)
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
