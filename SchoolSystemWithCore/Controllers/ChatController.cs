using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSystemWithCore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolSystemWithCore.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Result.Id;
            ViewBag.Email = user.Result.Email;
            
            var userList = _userManager.Users.OrderBy(c => c.Name).Select(x => new { Name = x.Name, Id = x.Id}).Where(x => x.Id != user.Result.Id);
            List<ContactViewModel> UsersAsList = new List<ContactViewModel>();
            foreach (var x in userList)
            {
                UsersAsList.Add(new ContactViewModel() {Name = x.Name, Id = x.Id});
            }
            ViewBag.UsersAsList = UsersAsList;
            ViewBag.UserList = new SelectList(userList, "Id", "Name");

            return View();
        }

        public IActionResult Conversation(ChatSelectorViewModel model)
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Result.Id;
            ViewBag.Email = user.Result.Email;
            ViewBag.Name = user.Result.Name;
            

            if (model.Person == null)
                return RedirectToAction("Index", "Chat");
            var personName = _userManager.Users.FirstOrDefault(x => x.Id == model.Person).Name;
            ViewBag.Person = model.Person;
            ViewBag.PersonName = personName;
            return View();
        }
    }
}
