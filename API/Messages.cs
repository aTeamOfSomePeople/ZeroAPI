using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Messages
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44364") };

        public static async Task<bool> SendMessage(string accessToken, long chatId, string text,string fileIds)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(chatId)), "chatId");
            content.Add(new StringContent(text), "text");
            content.Add(new StringContent(fileIds), "fileIds");

            var httpResponse = await httpClient.PostAsync("messages/sendmessage", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<Message[]> GetMessages(string accessToken, long chatId, int? count, int direction, DateTime date = new DateTime())
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"messages/getmessages?accessToken={accessToken}&chatid={chatId}&count={count}&direction={direction}&date={date}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<Message[]>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Message> GetMessage(string accessToken, long messageId)
        {
            var content = new MultipartFormDataContent();

            var httpResponse = await httpClient.GetAsync($"messages/getmessages?accessToken={accessToken}&messageid={messageId}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<Message>(stringResponse);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> EditMessage(string accessToken, long messageId, string newText)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(messageId)), "messageId");
            content.Add(new StringContent(newText), "newText");

            var httpResponse = await httpClient.PostAsync("messages/editnessage", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> DeleteMessage(string accessToken, long messageId, bool fromAll)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(accessToken), "accessToken");
            content.Add(new StringContent(Convert.ToString(messageId)), "messageId");
            content.Add(new StringContent(Convert.ToString(fromAll)), "fromAll");

            var httpResponse = await httpClient.PostAsync("messages/deletemessage", content);
            return httpResponse.StatusCode == HttpStatusCode.OK;
        }

        public class Message
        {
            public long id { get; }
            public string text { get; }
            public long userId { get; }
            public long chatId { get; }
            public bool isReaded { get; }
            public DateTime date { get; }

            [JsonConstructor]
            private Message(long id, string text, long userId, long chatId, bool isReaded, DateTime date)
            {
                this.id = id;
                this.text = text;
                this.userId = userId;
                this.chatId = chatId;
                this.isReaded = isReaded;
                this.date = date;
            }
        }
    }
}
