using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Newtonsoft.Json;
using SchoolSystemWithCore.Data;
using SchoolSystemWithCore.Models;
using SchoolSystemWithCore.Models.ViewModels;

namespace SchoolSystemWithCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly string _baseUrl = "http://sclmanagement.azurewebsites.net";

        public AdminController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.RolesCount = _context.Roles.Count();

            List<GetClassesViewModel> Classes = new List<GetClassesViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/classroom");

                if (Res.IsSuccessStatusCode)
                {
                    var ClassesResponse = Res.Content.ReadAsStringAsync().Result;
                    Classes = JsonConvert.DeserializeObject<List<GetClassesViewModel>>(ClassesResponse);
                }
            }
            ViewBag.ClassCount = Classes.Count;
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateClass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([Bind("ClassRoomName")]CreateClassViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Message = "Failed!";
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_baseUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var ClassRoom = JsonConvert.SerializeObject(model);
                        var content = new StringContent(ClassRoom.ToString(), Encoding.UTF8, "application/json");
                        HttpResponseMessage Res = await client.PostAsync("api/ClassRoom", content);
                        if (Res.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "Successfully Updated Class Rooms!";
                        }

                    }
                    return View();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                var objRole = await _roleManager.FindByNameAsync(model.Name);
                CreateRoleForAPIModelView objModel = new CreateRoleForAPIModelView()
                {
                    Role_Id = objRole.Id,
                    RoleName = objRole.Name
                };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var _role = JsonConvert.SerializeObject(objModel);
                    var content = new StringContent(_role.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("api/Roles", content);

                    if (result.Succeeded && Res.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "You have successfully created a role";
                        return View();
                    }
                }

                AddErrors(result);
            }

            return View(model);
        }

        public IActionResult AssignClass()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}