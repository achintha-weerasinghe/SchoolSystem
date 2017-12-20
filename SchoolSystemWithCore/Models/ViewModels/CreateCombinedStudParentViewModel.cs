using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchoolSystemWithCore.Models.ViewModels
{
    public class CreateCombinedStudParentViewModel
    {
        public string P_Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string StudentPassword { get; set; }
        public string PicUrl { get; set; }
        public string Role_Id { get; set; }
        [Required]
        public string AdmissionNumber { get; set; }
        [Required]
        public string AdmissionDate { get; set; }
        [Required]
        public int ClassRoomId { get; set; }
        [Required]
        public string ParentName { get; set; }
        [Required]
        public string ParentEmail { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string TpNumber { get; set; }
    }
}
