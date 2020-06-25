using System;
using System.Collections.Generic;
using System.Text;

namespace ChatConsumer.Domain
{
    class Chat
    {
        public int ChatId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
