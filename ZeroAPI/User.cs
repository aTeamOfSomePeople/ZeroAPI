using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class User
    {
        public int Id { get; }
        public string Name { get; }
        internal string Email { get; }
        internal string Login { get; }
        internal string Password { get; }
        public string Avatar { get; }
        public bool IsDeleted { get; }

        [JsonConstructor]
        private User(int id, string name, string email, string login, string password, string avatar, bool isDeleted)
        {
            Id = id;
            Name = name;
            Email = email;
            Login = login;
            Password = password;
            Avatar = avatar;
            IsDeleted = isDeleted;
        }

        public static bool Register(string name, string login, string password, string email = null)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () => {
                        var content = new Dictionary<string, string>();
                        content.Add("Name", name);
                        content.Add("Login", login);
                        content.Add("Password", password);
                        if (email != null)
                        {
                            content.Add("Email", email);
                        }
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}users/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }
        //TODO
        public static void OAuthorization() { }

        public static User Authorization(string login, string password)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(
                        Task.Run(async () =>
                        {
                            var httpClient = new HttpClient();
                            var response = await httpClient.GetAsync(String.Format("{0}users/IsExists?login={1}&password={2}", Resources.ServerUrl, login, password));
                            return await response.Content.ReadAsStringAsync();
                        }).Result);
            }
            catch { }

            return null;
        }

        public static List<User> FindUsers(string name, int? Start = null, int? Count = null)
        {
            var users = new List<User>();
            try
            {
                users.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(
                        Task.Run(async () =>
                        {
                            var httpClient = new HttpClient();
                            var response = await httpClient.GetAsync(String.Format("{0}users/FindUsers?Name={1}&Start={2}&Count={3}", Resources.ServerUrl, name, Start, Count));
                            return await response.Content.ReadAsStringAsync();
                        }).Result));
            }
            catch { }

            return users;
        }

        public static User GetInfo(int id)
        {
            User user = null;

            try
            {
                user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}users/Details?id={1}", Resources.ServerUrl, id));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return user;
        }

        public List<Chat> GetChats(int? Start = null, int? Count = null)
        {
            var chats = new List<Chat>();
            try
            {
                chats.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<Chat[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}chats/UserChats?UserId={1}&Start={2}&Count={3}", Resources.ServerUrl, Id, Start, Count));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }

            return chats;
        }

        //public static void GetNotifications() {} ???

        public List<Message> GetMessages(Chat chat, int? Start = null, int? Count = null)
        {
            var messages = new List<Message>();
            try
            {
                messages.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<Message[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}messages/ChatMessages?ChatId={1}&UserId={2}&Start={3}&Count={4}", Resources.ServerUrl, chat.Id, Id, Start, Count));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }

            return messages;
        }
        //TODO
        public bool CreateChat(string name, ChatType type, HashSet<User> users = null)
        {
            if (users == null)
            {
                users = new HashSet<User>();
            }
            users.Add(this);
            
            switch (type)
            {
                case ChatType.Conversation: break;
                case ChatType.Dialog: break;
                case ChatType.PrivateDialog: break;
                case ChatType.Public: break;
            }

            Chat chat;
            try
            {
                chat = Newtonsoft.Json.JsonConvert.DeserializeObject<Chat>(
                    Task.Run(async () => {
                        var content = new Dictionary<string, string>();
                        content.Add("Name", name);
                        content.Add("Type", "public");
                        content.Add("Creator", Id.ToString());
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}chats/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
                if (chat == null)
                {
                    return false;
                }
            }
            catch { return false; }
            try
            {
                foreach (var user in users)
                {
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("UserId", user.Id.ToString());
                        content.Add("ChatId", chat.Id.ToString());
                        content.Add("CanWrite", "True");
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}usersInChats/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    });
                }
            }
            catch { }
            Connection.hubProxy.Invoke("newChat", chat, users.ToList());
            return true;
        }

        public bool Delete()
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = ToDictionary(this);
                        content["IsDeleted"] = true.ToString();
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}users/edit", Resources.ServerUrl), new FormUrlEncodedContent(content));
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
                            var response = await httpClient.PostAsync(String.Format("{0}users/edit", Resources.ServerUrl), new FormUrlEncodedContent(content));
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

        public bool ChangePassword(string password)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = ToDictionary(this);
                        content["Password"] = password;
                        content.Add("oldPassword", Password);
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}users/edit", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
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
                        var response = await httpClient.PostAsync(String.Format("{0}users/edit", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool LeaveFromChat(Chat chat)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("Login", Login);
                        content.Add("Password", Password);
                        content.Add("ChatId", chat.Id.ToString());
                        content.Add("userId", Id.ToString());
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}usersInChats/delete", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool BanUser(int userId)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("BannerId", Id.ToString());
                        content.Add("BannedId", userId.ToString());
                        content.Add("Login", Login);
                        content.Add("Password", Password);
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}bannedUsers/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool UnBanUser(int userId)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("userId", userId.ToString());
                        content.Add("Login", Login);
                        content.Add("Password", Password);
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}bannedUsers/delete", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool SendMessage(string text, Chat chat, List<string> attachments = null)
        {
            Message message = null;
            try
            {
                message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(Task.Run(async () =>
                {
                    var content = new Dictionary<string, string>();
                    content.Add("Text", text);
                    content.Add("ChatId", chat.Id.ToString());
                    content.Add("UserId", Id.ToString());
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(String.Format("{0}messages/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }).Result);
                Connection.hubProxy.Invoke("newMessage", chat, message);
            }
            catch { return false; }

            if (message != null)
            {
                if (attachments != null)
                {
                    var cdnClient = (new ZeroCdnClients.CdnClientsFactory(Resources.ZeroCDNUsername, Resources.ZeroCDNKey)).Files;
                    foreach (var attachment in attachments)
                    {
                        if (System.IO.File.Exists(attachment))
                        {
                            ZeroCdnClients.DataTypes.CdnFileInfo result;
                            
                            try
                            {
                                result = cdnClient.Add(attachment, $"{DateTime.UtcNow.Ticks}").Result;
                            }
                            catch
                            {
                                break;
                            }
                            try
                            {
                                Task.Run(async () =>
                                {
                                    var content = new Dictionary<string, string>();
                                    content.Add("Link", result.ResourceUrl);
                                    content.Add("MessageId", message.Id.ToString());
                                    content.Add("FileSize", result.Size.ToString());
                                    var httpClient = new HttpClient();
                                    var response = await httpClient.PostAsync(String.Format("{0}attachments/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                                    return await response.Content.ReadAsStringAsync();
                                }).Wait();
                            }
                            catch
                            {
                                cdnClient.Remove(result.ID).Wait();
                            }
                        }
                    }
                }
                return true;
            }

            return false;
        }

        public bool DeleteMessage(Message message)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = new Dictionary<string, string>();
                        content.Add("MessageId", message.Id.ToString());
                        content.Add("UserId", Id.ToString());
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}deletedMessages/create", Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        internal static Dictionary<string, string> ToDictionary(User user)
        {
            var output = new Dictionary<string, string>();
            output.Add("Id", user.Id.ToString());
            output.Add("Email", user.Email);
            output.Add("Login", user.Login);
            output.Add("Password", user.Password);
            output.Add("Name", user.Name);
            output.Add("Avatar", user.Avatar);
            output.Add("IsDeleted", user.IsDeleted.ToString());
            return output;
        }
    }
}
