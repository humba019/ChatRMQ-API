using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Domain.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
