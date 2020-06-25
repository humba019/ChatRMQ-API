using System;
using System.Collections.Generic;
using System.Text;

namespace ChatConsumer.Domain
{
    class Message
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; }
        public Chat Chat { get; set; }
    }
}
