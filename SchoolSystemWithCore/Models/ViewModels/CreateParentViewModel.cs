using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models.ViewModels
{
    public class CreateParentViewModel
    {
        public string P_Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PicUrl { get; set; }
        public string Role_Id { get; set; }
        public string TpNumber { get; set; }
    }
}
