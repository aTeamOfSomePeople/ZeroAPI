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
        
        public static void FindPublic() { }

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

        public static List<Chat> FindChats(string name)
        {
            var chats = new List<Chat>();
            try
            {
                chats.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<Chat[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}chats?Name={1}", Resources.ServerUrl, name));
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

    }
}
