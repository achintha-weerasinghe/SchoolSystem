using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models
{
    public class Principal
    {
        [Key]
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Grade { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
