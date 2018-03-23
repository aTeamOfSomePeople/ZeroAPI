using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Chat
    {
        public int Id { get; }
        public int Creator { get; }
        public string Name { get; }
        public string Type { get; }
        public string Avatar { get; }

        [JsonConstructor]
        private Chat(int id, int creator, string name, string type, string avatar)
        {
            Id = id;
            Creator = creator;
            Name = name;
            Type = type;
            Avatar = avatar;
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            try
            {
                users.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}users/chatUsers?ChatId={1}", Resources.ServerUrl, Id));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }
            return users;
        }

        public static void Invite() { }

        public static List<Chat> FindPublics(string name, int? start, int? count)
        {
            var chats = new List<Chat>();
            try
            {
                chats.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<Chat[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}chats/findPublics?Name={1}&start={2}&count={3}", Resources.ServerUrl, name, start, count));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }

            return chats;
        }

        public bool ChangeName(string name)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = ToDictionary(this);
                        content["Name"] = name;
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}chats/edit", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool ChangeAvatar(string link)
        {
            var cdnClient = (new ZeroCdnClients.CdnClientsFactory(Resources.ZeroCDNUsername, Resources.ZeroCDNKey)).Files;
            if (System.IO.File.Exists(link))
            {
                ZeroCdnClients.DataTypes.CdnFileInfo result;

                try
                {
                    result = cdnClient.Add(link, $"{DateTime.UtcNow.Ticks}").Result;
                }
                catch
                {
                    return false;
                }
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                        Task.Run(async () =>
                        {
                            var content = ToDictionary(this);
                            content["Avatar"] = link;
                            var httpClient = new HttpClient();
                            var response = await httpClient.PostAsync(String.Format("{0}chats/edit", Resources.ServerUrl), new FormUrlEncodedContent(content));
                            return await response.Content.ReadAsStringAsync();
                        }).Result);
                }
                catch
                {
                    cdnClient.Remove(result.ID).Wait();
                }
            }
            return false;
        }

        public static void MuteUser() { }

        public bool DeleteUser(User user, User user1)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("ChatId", Id.ToString());
                        content.Add("UserId", user1.Id.ToString());
                        content.Add("Login", user.Login.ToString());
                        content.Add("Password", user.Password.ToString());
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}usersInChats/delete", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool Delete(User user)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("ChatId", Id.ToString());
                        content.Add("Login", user.Login.ToString());
                        content.Add("Password", user.Password.ToString());
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}chats/delete", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        internal static Dictionary<string, string> ToDictionary(Chat chat)
        {
            var output = new Dictionary<string, string>();
            output.Add("Id", chat.Id.ToString());
            output.Add("Creator", chat.Creator.ToString());
            output.Add("Name", chat.Name);
            output.Add("Type", chat.Type);
            output.Add("Avatar", chat.Avatar);

            return output;
        }
    }
}
