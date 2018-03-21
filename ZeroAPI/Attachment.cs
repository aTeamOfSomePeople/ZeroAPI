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
        internal string Link { get; }
        public string FileName { get; }
        public long FileSize { get; }
        internal long CDNId { get; }

        [JsonConstructor]
        private Attachment(int id, int messageId, string link, long fileSize, long cdnId)
        {
            Id = id;
            MessageId = messageId;
            Link = link;
            FileName = link.Split('/').Last();
            FileSize = fileSize;
            CDNId = cdnId;
        }

        public byte[] GetFileBytes()
        {
            byte[] output = null;
            try
            {
                output = (new HttpClient()).GetByteArrayAsync(Link).Result;
            }
            catch { }
            
            return output;
        }
    }
}
