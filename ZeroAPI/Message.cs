using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Message
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Не работает.
        /// </summary>
         void GetAttachmentsToMessage() { }
        /// <summary>
        /// Не работает.
        /// </summary>
         void GetUser() { }
        /// <summary>
        /// Не работает.
        /// </summary>
         void GetChat() { }

        public static bool SendMessage(int ChatId, int UserId, string Text)
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var content = new Dictionary<string, string>();
                    content.Add("Id", "0");
                    content.Add("ChatId", ChatId.ToString());
                    content.Add("UserId", UserId.ToString());
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
    }
}
