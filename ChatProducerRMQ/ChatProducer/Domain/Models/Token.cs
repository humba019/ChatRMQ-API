using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Domain.Models
{
    public class Token
    {
        public string ClientEmail { get; set; }
        public string ClientToken { get; set; }
    }
}
