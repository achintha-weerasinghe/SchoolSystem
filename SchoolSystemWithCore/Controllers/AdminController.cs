using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SchoolSystemWithCore.Data;
using SchoolSystemWithCore.Models;

namespace SchoolSystemWithCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

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
        public IActionResult Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.RolesCount = _context.Roles.Count();

            var classCount = _context.ClassDetailses.ToList().Count;
            ViewBag.ClassCount = classCount;
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
        public async Task<IActionResult> CreateClass([Bind("ClassName")]ClassDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = "You have successfully created a class";
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

                if (result.Succeeded)
                {
                    ViewBag.Message = "You have successfully created a role";
                    return View();
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