using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SchoolSystemWithCore.Models
{
    public class StudentClass
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("ClassDetails")]
        public string ClassDetailsId { get; set; }
        public ClassDetails ClassDetails { get; set; }
    }
}
