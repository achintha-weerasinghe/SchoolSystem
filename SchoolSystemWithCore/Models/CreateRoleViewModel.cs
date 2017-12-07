using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
