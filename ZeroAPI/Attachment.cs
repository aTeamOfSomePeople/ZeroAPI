using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Attachment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Link { get; set; }

        public Attachment()
        {
            Id = 0;
            MessageId = 0;
            Link = "";
        }
        /// <summary>
        /// Не работает.
        /// </summary>
        void GetAttachment() { }
        /// <summary>
        /// Не работает.
        /// </summary>
        void GetMessage() { }
    }
}
