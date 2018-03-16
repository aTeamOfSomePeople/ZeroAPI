using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Attachment
    {
        public int Id { get; }
        public int MessageId { get; }
        public string Link { get; }

        [JsonConstructor]
        private Attachment(int id, int messageId, string link)
        {
            Id = id;
            MessageId = messageId;
            Link = link;
        }
    }
}
