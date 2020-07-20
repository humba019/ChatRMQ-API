using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Resources.Entity
{
    public class MessageResource
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; }
        public string DateSended { get; set; }
        public ChatResource Chat { get; set; }
    }
}
