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

        public User()
        {
            Id = 0;
            Name = "";
            Avatar = "";
        }

        public static List<User> FindUsers(string name)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(String.Format("{0}api/Users?name={1}", Properties.Resources.ServerUrl, name));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return null;
            });
            task.Start();
            task.Wait();
            try
            {
                return new List<User>(Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(task.Result.Result));
            }
            catch { }

            return null;
        }
        public static User GetUserInfo(int Id)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(String.Format("{0}api/Users?id={1}", Properties.Resources.ServerUrl, Id));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return null;
            });
            task.Start();
            task.Wait();
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(task.Result.Result);
            }
            catch { }

            return null;
        }

        public static User GetUser(string login, string password)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(String.Format("{0}api/Users?login={1}&password={2}", Properties.Resources.ServerUrl, login, password));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return null;
            });
            task.Start();
            task.Wait();
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(task.Result.Result);
            }
            catch { }

            return null;
        }

        public List<Chat> GetUserChats()
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(Properties.Resources.ServerUrl + "api/Chats?id=" + Id);
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return "";

            });
            task.Start();
            task.Wait();
            try
            {
                return new List<Chat>(Newtonsoft.Json.JsonConvert.DeserializeObject<Chat[]>(task.Result.Result));
            }
            catch { }
            return null;
        }

        public static bool CreateUser(string Name, string Login, string Password)
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
                    content.Add("Avatar", "");
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

        public bool SendMessage(Chat chat, string Text, string[] Attachments = null)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var content = new Dictionary<string, string>();
                    content.Add("Id", "0");
                    content.Add("ChatId", chat.Id.ToString());
                    content.Add("UserId", Id.ToString());
                    content.Add("Text", Text);
                    content.Add("Date", DateTime.Now.ToString());
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Messages/", new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return "";
            });
            task.Start();
            task.Wait();
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(task.Result.Result);
            if (Attachments != null)
            {
                foreach (var element in Attachments)
                {
                    task = new Task<Task<string>>(async () =>
                    {
                        try
                        {
                            var content = new Dictionary<string, string>();
                            content.Add("Id", "0");
                            content.Add("MessageId", message.Id.ToString());
                            content.Add("Link", element);
                            var httpClient = new HttpClient();
                            var response = await httpClient.PostAsync(Properties.Resources.ServerUrl + "api/Attachments/", new FormUrlEncodedContent(content));
                            return await response.Content.ReadAsStringAsync();
                        }
                        catch { }
                        return "";
                    });
                    task.Start();
                    task.Wait();
                }
            }
            return true;
        }
    }
}
