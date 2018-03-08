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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }

        /// <summary>
        /// Не работает.
        /// </summary>
        static User GetUserInfo(int Id) { return null; }

        public static int GetUserId(string login, string password)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + String.Format("api/Users?login={0}&password={1}&", login, password));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return null;
            });
            task.Start();
            task.Wait();
            try
            {
                System.Console.WriteLine(task.Result.Result);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<int>(task.Result.Result);
            }
            catch { }

            return 0;
        }

        public List<Chat> GetUserChats()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Chat>>(GetUserChatsAsync(Id).Result);
        }

        public static bool CreateUser(string Name, string Login, string Password, string Avatar = "")
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var content = new Dictionary<string, string>();
                    content.Add("Id", "0");
                    content.Add("Name", Name);
                    content.Add("Login", Login);
                    content.Add("Password", Password);
                    content.Add("Avatar", Avatar);
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Users", new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return "";
            });
            task.Start();
            task.Wait();
            return true;
        }

        private async Task<string> GetUserChatsAsync(int Id)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + "api/Chats/" + Id);
                return await response.Content.ReadAsStringAsync();
            }
            catch { }
            return "";
        }
    }
}
