using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Resources
{
    public class SaveTokenResource
    {
        [Required]
        [MaxLength(500)]
        public string ClientEmail { get; set; }
        [Required]
        public string ClientPass { get; set; }
    }
}
