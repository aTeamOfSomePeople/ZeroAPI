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
            try
            {
                return new List<Message>(Newtonsoft.Json.JsonConvert.DeserializeObject<Message[]>(GetMessagesAsync(Id).Result));
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Не работает.
        /// </summary>
        bool CreateChat(string Name, ChatType Type, List<User> Users) { return false; }
        /// <summary>
        /// Не работает.
        /// </summary>
         bool AddUser(User user) { return false; }

        private async Task<string> GetMessagesAsync(int ChatId)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + "api/Messages/" + ChatId);
                return await response.Content.ReadAsStringAsync();
            }
            catch { }
            return "";
        }
    }
}
