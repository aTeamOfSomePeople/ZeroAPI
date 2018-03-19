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

        [JsonConstructor]
        private Chat(int id, int creator, string name, string type)
        {
            Id = id;
            Creator = creator;
            Name = name;
            Type = type;
        }
        
        public static void GetInfo() { }
        public List<User> GetUsers()
        {
            var users = new List<User>();
            try
            {
                users.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}users/chatUsers?ChatId={1}", Properties.Resources.ServerUrl, Id));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }
            return users;
        }
        public static void Invite() { }
        public static List<Chat> FindChats(string name)
        {
            var chats = new List<Chat>();
            try
            {
                chats.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<Chat[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}chats?Name={1}",Properties.Resources.ServerUrl, name));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }
            return chats;
        }
        public static void ChangeName() { }
        public static void ChangeAvatar() { }
        public static void BanUser() { }
        public static void MuteUser() { }
        public static void DeleteUser() { }
        public static void DeleteChat() { }

        ///// <summary>
        ///// Не работает.
        ///// </summary>
        //static Chat GetChatInfo(int Id) { return null; }
        ///// <summary>
        ///// Не работает.
        ///// </summary>
        //List<User> GetUsers() { return null; }

        //public List<Message> GetMessages()
        //{
        //    var task = new Task<Task<string>>(async () =>
        //    {
        //        try
        //        {
        //            var httpClient = new HttpClient();
        //            var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + "api/Messages/" + Id);
        //            return await response.Content.ReadAsStringAsync();
        //        }
        //        catch { }
        //        return "";
        //    });
        //    task.Start();
        //    task.Wait();
        //    try
        //    {
        //        return new List<Message>(Newtonsoft.Json.JsonConvert.DeserializeObject<Message[]>(task.Result.Result));
        //    }
        //    catch { }
        //    return null;
        //}

        //public static bool CreateChat(string Name, string Type, List<User> Users)
        //{
        //    if (Users == null || Users.Count == 0)
        //    {
        //        return false;
        //    }
        //    var task = new Task<Task<string>>(async () =>
        //    {
        //        try
        //        {
        //            var content = new Dictionary<string, string>();
        //            content.Add("Id", "0");
        //            content.Add("Name", Name);
        //            content.Add("Type", Type);
        //            var httpClient = new HttpClient();
        //            var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Chats", new FormUrlEncodedContent(content));
        //            return await response.Content.ReadAsStringAsync();
        //        }
        //        catch { }
        //        return "";
        //    });
        //    task.Start();
        //    task.Wait();
        //    var chat = Newtonsoft.Json.JsonConvert.DeserializeObject<Chat>(task.Result.Result);
        //    foreach (User user in Users)
        //    {
        //        chat.AddUser(user);
        //    }
        //    return true;
        //}

        //public bool AddUser(User user)
        //{
        //    var task = Task.Run(async () =>
        //    {
        //        var content = new Dictionary<string, string>();
        //        content.Add("Id", "0");
        //        content.Add("ChatId", Id.ToString());
        //        content.Add("UserId", user.Id.ToString());
        //        var httpClient = new HttpClient();
        //        var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/UsersInChats", new FormUrlEncodedContent(content));
        //        return await response.Content.ReadAsStringAsync();
        //    });
        //    task.Wait();
        //    return true;
        //}
    }
}
