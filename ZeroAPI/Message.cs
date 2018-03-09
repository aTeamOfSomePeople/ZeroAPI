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
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Message()
        {
            ChatId = 0;
            UserId = 0;
            Text = "";
            Date = DateTime.MinValue;
        }

        public List<Attachment> GetAttachmentsToMessage()
        {
            var task = new Task<Task<string>>(async () =>
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(String.Format("{0}api/Attachments?id={1}", Properties.Resources.ServerUrl, Id));
                    return await response.Content.ReadAsStringAsync();
                }
                catch { }
                return null;
            });
            task.Start();
            task.Wait();
            try
            {
                return new List<Attachment>(Newtonsoft.Json.JsonConvert.DeserializeObject<Attachment[]>(task.Result.Result));
            }
            catch { }

            return null;
        }
    }
}
