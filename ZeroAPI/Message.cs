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

        public bool Delete(User user)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(Task.Run(async () =>
                {
                    var content = new Dictionary<string, string>();
                    content.Add("messageId", Id.ToString());
                    content.Add("chatId", ChatId.ToString());
                    content.Add("userLogin", user.Login);
                    content.Add("userPassword", user.Password);
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(String.Format("{0}messages/delete", Resources.ServerUrl), new FormUrlEncodedContent(content));
                    return await response.Content.ReadAsStringAsync();
                }).Result);
            }
            catch { }
            
            return false;
        }

        public List<Attachment> GetAttachments(Message message)
        {
            var attachments = new List<Attachment>();
            try
            {
                attachments.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<Attachment[]>(
                    Task.Run(async () => {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("{0}attachments/messageAttachments?MessageId={1}", Resources.ServerUrl, message.Id));
                        return await response.Content.ReadAsStringAsync();
                    }).Result));
            }
            catch { }
            
            return attachments;
        }
    }
}
