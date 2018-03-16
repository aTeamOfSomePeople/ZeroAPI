using Newtonsoft.Json;
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
        public int Id { get; }
        public int ChatId { get; }
        public int UserId { get; }
        public string Text { get; }
        public DateTime Date { get; }
        bool IsReaded { get; }
        [JsonConstructor]
        private Message(int id, int chatId, int userId, string text, DateTime date, bool isReaded)
        {
            Id = id;
            ChatId = chatId;
            UserId = userId;
            Text = text;
            Date = date;
            IsReaded = isReaded;
        }
        public static void Delete() { }
        public static void GetAttachments() { }
        //public List<Attachment> GetAttachmentsToMessage()
        //{
        //    var task = new Task<Task<string>>(async () =>
        //    {
        //        try
        //        {
        //            var httpClient = new HttpClient();
        //            var response = await httpClient.GetAsync(String.Format("{0}api/Attachments?id={1}", Properties.Resources.ServerUrl, Id));
        //            return await response.Content.ReadAsStringAsync();
        //        }
        //        catch { }
        //        return null;
        //    });
        //    task.Start();
        //    task.Wait();
        //    try
        //    {
        //        return new List<Attachment>(Newtonsoft.Json.JsonConvert.DeserializeObject<Attachment[]>(task.Result.Result));
        //    }
        //    catch { }

        //    return null;
        //}
    }
}
