using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models
{
    public class ChatSelectorViewModel
    {
        [Required]
        public string Person { get; set; }
    }
}
