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
        public int ChatId { get; }
        public int UserId { get; }
        public string Text { get; }
        public DateTime Date { get; }

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
    }
}
