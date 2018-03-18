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
        private string Email { get; }
        private string Login { get; }
        private string Password { get; }
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
                        var response = await httpClient.PostAsync(String.Format("{0}users/create", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public static User OAuthorization(string userId)
        {
            return null;
        }
        public static User Authorization(string login, string password)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(
                        Task.Run(async () =>
                        {
                            var httpClient = new HttpClient();
                            var response = await httpClient.GetAsync(String.Format("{0}users/IsExists?login={1}&password={2}", Properties.Resources.ServerUrl, login, password));
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
                            var response = await httpClient.GetAsync(String.Format("{0}users/FindUsers?Name={1}&Start={2}&Count={3}", Properties.Resources.ServerUrl, name, Start, Count));
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
                        var response = await httpClient.GetAsync(String.Format("{0}users/Details?id={1}", Properties.Resources.ServerUrl, id));
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
                        var response = await httpClient.GetAsync(String.Format("{0}chats/UserChats?UserId={1}&Start={2}&Count={3}", Properties.Resources.ServerUrl, Id, Start, Count));
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
                        var response = await httpClient.GetAsync(String.Format("{0}messages/ChatMessages?ChatId={1}&UserId={2}&Start={3}&Count={4}", Properties.Resources.ServerUrl, chat.Id, Id, Start, Count));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }

            return messages;
        }

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
                        var response = await httpClient.PostAsync(String.Format("{0}chats/create", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
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
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}usersInChats/create", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
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
                        var response = await httpClient.PostAsync(String.Format("{0}users/edit", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }

        public bool ChangeAvatar(string link)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(
                    Task.Run(async () =>
                    {
                        var content = ToDictionary(this);
                        content["Avatar"] = link;
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}users/edit", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

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
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}users/edit", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
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
                        var httpClient = new HttpClient();
                        var response = await httpClient.PostAsync(String.Format("{0}usersInChats/delete", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
                        return await response.Content.ReadAsStringAsync();
                    }).Result);
            }
            catch { }

            return false;
        }
        public static void BanUser() {}

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
                    var response = await httpClient.PostAsync(String.Format("{0}messages/create", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }).Result);
                Connection.hubProxy.Invoke("newMessage", chat, message);
            }
            catch { return false; }

            if (message != null)
            {
                if (attachments != null)
                {
                    var cdnClient = (new ZeroCdnClients.CdnClientsFactory(Properties.Resources.ZeroCDNUsername, Properties.Resources.ZeroCDNKey)).Files;
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
                                    var httpClient = new HttpClient();
                                    var response = await httpClient.PostAsync(String.Format("{0}attachments/create", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
                                    return await response.Content.ReadAsStringAsync();
                                }).Wait();
                            }
                            catch
                            {
                                cdnClient.Remove(result.ID);
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
                        var response = await httpClient.PostAsync(String.Format("{0}deletedMessages/create", Properties.Resources.ServerUrl), new FormUrlEncodedContent(content));
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
    //public static List<User> FindUsers(string name)
    //{
    //    var task = new Task<Task<string>>(async () =>
    //    {
    //        try
    //        {
    //            var httpClient = new HttpClient();
    //            var response = await httpClient.GetAsync(String.Format("{0}api/Users?name={1}", Properties.Resources.ServerUrl, name));
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //        catch { }
    //        return null;
    //    });
    //    task.Start();
    //    task.Wait();
    //    try
    //    {
    //        return new List<User>(Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(task.Result.Result));
    //    }
    //    catch { }

    //    return null;
    //}
    //public static User GetUserInfo(int Id)
    //{
    //    var task = new Task<Task<string>>(async () =>
    //    {
    //        try
    //        {
    //            var httpClient = new HttpClient();
    //            var response = await httpClient.GetAsync(String.Format("{0}api/Users?id={1}", Properties.Resources.ServerUrl, Id));
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //        catch { }
    //        return null;
    //    });
    //    task.Start();
    //    task.Wait();
    //    try
    //    {
    //        return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(task.Result.Result);
    //    }
    //    catch { }

    //    return null;
    //}

    //public static User GetUser(string login, string password)
    //{
    //    var task = new Task<Task<string>>(async () =>
    //    {
    //        try
    //        {
    //            var httpClient = new HttpClient();
    //            var response = await httpClient.GetAsync(String.Format("{0}api/Users?login={1}&password={2}", Properties.Resources.ServerUrl, login, password));
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //        catch { }
    //        return null;
    //    });
    //    task.Start();
    //    task.Wait();
    //    try
    //    {
    //        return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(task.Result.Result);
    //    }
    //    catch { }

    //    return null;
    //}

    //public List<Chat> GetUserChats()
    //{
    //    var task = new Task<Task<string>>(async () =>
    //    {
    //        try
    //        {
    //            var httpClient = new HttpClient();
    //            var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + "api/Chats?id=" + Id);
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //        catch { }
    //        return "";

    //    });
    //    task.Start();
    //    task.Wait();
    //    try
    //    {
    //        return new List<Chat>(Newtonsoft.Json.JsonConvert.DeserializeObject<Chat[]>(task.Result.Result));
    //    }
    //    catch { }
    //    return null;
    //}

    //public static bool CreateUser(string Name, string Login, string Password)
    //{
    //    var task = new Task<Task<string>>(async () =>
    //    {
    //        try
    //        {
    //            var content = new Dictionary<string, string>();
    //            content.Add("Id", "0");
    //            content.Add("Name", Name);
    //            content.Add("Login", Login);
    //            content.Add("Password", Password);
    //            content.Add("Avatar", "");
    //            var httpClient = new HttpClient();
    //            var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Users", new FormUrlEncodedContent(content));
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //        catch { }
    //        return "";
    //    });
    //    task.Start();
    //    task.Wait();
    //    return true;
    //}

    //public bool SendMessage(Chat chat, string Text, string[] Attachments = null)
    //{
    //    var task = new Task<Task<string>>(async () =>
    //    {
    //        try
    //        {
    //            var content = new Dictionary<string, string>();
    //            content.Add("Id", "0");
    //            content.Add("ChatId", chat.Id.ToString());
    //            content.Add("UserId", Id.ToString());
    //            content.Add("Text", Text);
    //            content.Add("Date", DateTime.Now.ToString());
    //            var httpClient = new HttpClient();
    //            var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Messages/", new FormUrlEncodedContent(content));
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //        catch { }
    //        return "";
    //    });
    //    task.Start();
    //    task.Wait();
    //    var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(task.Result.Result);
    //    if (Attachments != null)
    //    {
    //        foreach (var element in Attachments)
    //        {
    //            task = new Task<Task<string>>(async () =>
    //            {
    //                try
    //                {
    //                    var content = new Dictionary<string, string>();
    //                    content.Add("Id", "0");
    //                    content.Add("MessageId", message.Id.ToString());
    //                    content.Add("Link", element);
    //                    var httpClient = new HttpClient();
    //                    var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Attachments/", new FormUrlEncodedContent(content));
    //                    return await response.Content.ReadAsStringAsync();
    //                }
    //                catch { }
    //                return "";
    //            });
    //            task.Start();
    //            task.Wait();
    //        }
    //    }
    //    return true;
    //}
}
}
