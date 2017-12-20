using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models.ViewModels
{
    public class CreateRoleForAPIModelView
    {
        public string Role_Id { get; set; }
        public string RoleName { get; set; }
    }
}
