using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Resources
{
    public class SaveMessageResource
    {
        [Required]
        [MaxLength(500)]
        public string MessageContent { get; set; }
        [Required]
        public int ChatId { get; set; }
    }
}
