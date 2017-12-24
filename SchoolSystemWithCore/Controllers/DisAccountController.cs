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
            ViewBag.ClassList = await GetClassesFromApi();
            
            ViewBag.UserId = userId;
            ViewBag.Password = password;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegister(CreateCombinedStudParentViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.P_Id).Result;
            var role = _roleManager.FindByNameAsync("Student").Result;
            var parentRole = _roleManager.FindByNameAsync("Parent").Result;
            
            Student Student = new Student()
            {
                AdmissionDate = model.AdmissionDate,
                AdmissionNumber = model.AdmissionNumber,
                ApplicationUserId = model.P_Id,
                ClassRoomId = model.ClassRoomId
            };

            var parentUser = new ApplicationUser()
            {
                UserName = model.ParentEmail,
                Email = model.ParentEmail,
                Name = model.ParentName
            };
            var result = await _userManager.CreateAsync(parentUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(parentUser, "Parent");
            }

            Parent Parent = new Parent()
            {
                ApplicationUserId = parentUser.Id,
                PhoneNumber = model.TpNumber
            };
            var localStudentSuccess = await SaveToDatabase(Student);
            var localParentSuccess = await SaveToDatabase(Parent);

            CreateStudentViewModel StudentApi = new CreateStudentViewModel()
            {
                P_Id = user.Id,
                AdmissionDate = model.AdmissionDate,
                AdmissionNumber = model.AdmissionNumber,
                ClassRoomId = model.ClassRoomId,
                Email = user.Email,
                Name = user.Name,
                Password = model.StudentPassword,
                Role_Id = role.Id
            };

            CreateParentViewModel ParentApi = new CreateParentViewModel()
            {
                Email = model.ParentEmail,
                Name = model.ParentName,
                P_Id = parentUser.Id,
                Password = model.Password,
                Role_Id = parentRole.Id,
                TpNumber = model.TpNumber,
                StudentId = model.P_Id
            };

            var studentSuccess = await PostOnApi("AddStudentDetails", StudentApi);
            var success = await PostOnApi("ParentDetails", ParentApi);

            if (success && studentSuccess && localStudentSuccess && localParentSuccess)
            {
                ViewBag.Message = "You have successfully registered a student and a parent";
            }
            
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> TeacherRegister(string userId, string password)
        {
            ViewBag.ClassList = await GetClassesFromApi();

            ViewBag.UserId = userId;
            ViewBag.Password = password;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TeacherRegister(CreateTeacherViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.P_Id).Result;
            var role = _roleManager.FindByNameAsync("Teacher").Result;

            Teacher Teacher = new Teacher()
            {
                ApplicationUserId = model.P_Id,
                Grade = model.teacherGrade,
                OwnClass = model.ClassRoomId,
                Phone = model.TpNumber
            };

            var localSuccess = await SaveToDatabase(Teacher);

            model.Email = user.Email;
            model.Name = user.Name;
            model.Role_Id = role.Id;

            var success = await PostOnApi("TeacherDetails", model);
            if (success && localSuccess)
            {
                ViewBag.Message = "You have successfully registered a teacher";
            }
            
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        public IActionResult PrincipalRegister(string userid, string password)
        {
            ViewBag.UserId = userid;
            ViewBag.Password = password;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PrincipalRegister(CreatePrincipalViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.P_Id).Result;
            var role = _roleManager.FindByNameAsync("Principal").Result;

            Principal Principal = new Principal()
            {
                ApplicationUserId = model.P_Id,
                Grade = model.PrincipalGrade,
                Phone = model.TpNumber
            };

            var localSuccess = await SaveToDatabase(Principal);
            
            model.Email = user.Email;
            model.Name = user.Name;
            model.Role_Id = role.Id;

            var success = await PostOnApi("PrincipalDetails", model);
            if (success)
            {
                ViewBag.Message = "You have successfully registered a principal";
            }
            
            return RedirectToAction("Register", "Account");
        }

        private async Task<bool> PostOnApi(string link, object obj)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var singleObject = JsonConvert.SerializeObject(obj);
                var content = new StringContent(singleObject.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/" + link, content);

                if (Res.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<Object> GetClassesFromApi()
        {
            List<GetClassesViewModel> classList = new List<GetClassesViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ClassRoom");

                if (Res.IsSuccessStatusCode)
                {
                    var classesResponse = Res.Content.ReadAsStringAsync().Result;
                    classList = JsonConvert.DeserializeObject<List<GetClassesViewModel>>(classesResponse);
                }
            }
            var classes = classList.OrderBy(c => c.ClassRoomName).Select(x => new { Id = x.ClassRoomId, ClassName = x.ClassRoomName });
            return new SelectList(classes, "Id", "ClassName");
        }

        private async Task<bool> SaveToDatabase(Object obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(obj);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}