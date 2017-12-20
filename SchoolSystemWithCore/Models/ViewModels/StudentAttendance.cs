using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSystemWithCore.Models.ViewModels
{
    public class StudentAttendance
    {
        public string P_Id { get; set; }
        public string Name { get; set; }
        public bool PresentAbsent { get; set; }
        public string Date { get; set; }
    }
}
