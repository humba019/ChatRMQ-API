using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Resources
{
    public class SaveClientResource
    {
        [Required]
        [MaxLength(100)]
        public string ClientName { get; set; }
        [Required]
        [MaxLength(40)]
        public string ClientEmail { get; set; }
        [Required]
        public string ClientPass { get; set; }
    }
}
