using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Resources.Entity
{
    public class ChatResource
    {
        public int ChatId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
