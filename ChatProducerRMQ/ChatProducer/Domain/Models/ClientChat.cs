using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Domain.Models
{
    public class ClientChat 
    {
        public string ClientEmail { get; set; }
        public string ClientName { get; set; }
        public Chat Chat { get; set; }
    }
}
