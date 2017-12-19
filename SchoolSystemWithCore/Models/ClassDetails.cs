using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models
{
    public class ClassDetails
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string ClassName { get; set; }
    }
}
