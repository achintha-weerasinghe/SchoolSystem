using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models.ViewModels
{
    public class AttendanceViewModel
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public string ClassName { get; set; }
    }
}
