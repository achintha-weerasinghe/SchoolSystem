using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models
{
    public class Student
    {
        [Key]
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string AdmissionNumber { get; set; }
        [Required]
        public string AdmissionDate { get; set; }
        public int ClassRoomId { get; set; }
    }
}
