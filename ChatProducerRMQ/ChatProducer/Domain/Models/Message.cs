using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Domain.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; }
        public string DateSended { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
