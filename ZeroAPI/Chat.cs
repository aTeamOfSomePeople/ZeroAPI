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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public Chat()
        {
            Id = 0;
            Name = "";
            Type = "";
        }
        /// <summary>
        /// Не работает.
        /// </summary>
        static Chat GetChatInfo(int Id) { return null; }
        /// <summary>
        /// Не работает.
        /// </summary>
         List<User> GetUsers() { return null; }

        public List<Message> GetMessages()
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + "api/Messages/" + Id);
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return "";
            });
            task.Start();
            task.Wait();
            try
            {
                return new List<Message>(Newtonsoft.Json.JsonConvert.DeserializeObject<Message[]>(task.Result.Result));
            }
            catch { }
            return null;
        }

        public static bool CreateChat(string Name, string Type, List<User> Users)
        {
            if (Users == null || Users.Count == 0)
            {
                return false;
            }
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var content = new Dictionary<string, string>();
                    content.Add("Id", "0");
                    content.Add("Name", Name);
                    content.Add("Type", Type);
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Chats", new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return "";
            });
            task.Start();
            task.Wait();
            var chat = Newtonsoft.Json.JsonConvert.DeserializeObject<Chat>(task.Result.Result);
            foreach (User user in Users)
            {
                chat.AddUser(user);
            }
            return true;
        }

        public bool AddUser(User user)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var content = new Dictionary<string, string>();
                    content.Add("Id", "0");
                    content.Add("ChatId", Id.ToString());
                    content.Add("UserId", user.Id.ToString());
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/UsersInChats", new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return "";
            });
            task.Start();
            task.Wait();
            return true;
        }
    }
}
