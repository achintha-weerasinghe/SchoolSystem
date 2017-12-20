using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models.ViewModels
{
    public class CreateClassViewModel
    {
        [Required]
        public string ClassRoomName { get; set; }
    }
}
