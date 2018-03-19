using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Attachment
    {
        public int Id { get; }
        public int MessageId { get; }
        private string Link { get; }
        public string FileFormat { get; }

        [JsonConstructor]
        private Attachment(int id, int messageId, string link)
        {
            Id = id;
            MessageId = messageId;
            Link = link;
            FileFormat = String.Format(".{0}", link.Split('.').Last());
        }

        public byte[] GetFile()
        {
            byte[] output;
            try
            {
                output = (new HttpClient()).GetByteArrayAsync(Link).Result;
            }
            catch
            {
                output = null;
            }
            return output;
        }
    }
}
