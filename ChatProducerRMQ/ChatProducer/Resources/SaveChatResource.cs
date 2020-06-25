using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Resources
{
    public class SaveChatResource
    {
        [Required]
        [MaxLength(50)]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
    }
}
