using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolSystemWithCore.Data;
using SchoolSystemWithCore.Models;
using SchoolSystemWithCore.Models.ViewModels;

namespace SchoolSystemWithCore.Controllers
{
    public class DisAccountController : Controller
    {
        private readonly string _baseUrl = "http://sclmanagement.azurewebsites.net";
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DisAccountController(ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> StudentRegister(string userId, string password)
        {
            List<GetClassesViewModel> ClassList = new List<GetClassesViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ClassRoom");

                if (Res.IsSuccessStatusCode)
                {
                    var ClassesResponse = Res.Content.ReadAsStringAsync().Result;
                    ClassList = JsonConvert.DeserializeObject<List<GetClassesViewModel>>(ClassesResponse);
                }
            }
            var Classes = ClassList.OrderBy(c => c.ClassRoomName).Select(x => new { Id = x.ClassRoomId, ClassName = x.ClassRoomName}); 
            ViewBag.ClassList = new SelectList(Classes, "Id", "ClassName");
            
            ViewBag.UserId = userId;
            ViewBag.Password = password;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegister(CreateStudentViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.P_Id).Result;
            var role = _roleManager.FindByNameAsync("Student").Result;

            model.P_Id = user.Id;
            model.Name = user.Name;
            model.Role_Id = role.Id;
            model.Email = user.Email;
            model.Password = model.Password;
            
            Student Student = new Student()
            {
                AdmissionDate = model.AdmissionDate,
                AdmissionNumber = model.AdmissionNumber,
                ApplicationUserId = model.P_Id,
                ClassRoomId = model.ClassRoomId
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(Student);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var SingleStudent = JsonConvert.SerializeObject(model);
                var content = new StringContent(SingleStudent.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/StudentDetails", content);
                if (Res.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully Updated the Student Database!";
                }
            }
            
            return RedirectToAction("Register", "Account");
        }

        public async Task<IActionResult> ParentRegister()
        {
            return View();
        }
    }
}