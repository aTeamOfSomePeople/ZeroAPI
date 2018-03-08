﻿using System;
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
        public string Avatar { get; }

        public User()
        {
            Id = 0;
            Name = "";
            Avatar = "";
        }
        /// <summary>
        /// Не работает.
        /// </summary>
        static User GetUserInfo(int Id) { return null; }

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

        public bool SendMessage(Chat chat, string Text)
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
