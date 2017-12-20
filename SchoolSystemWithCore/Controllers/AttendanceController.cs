using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolSystemWithCore.Data;
using SchoolSystemWithCore.Models.ViewModels;

namespace SchoolSystemWithCore.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly string _baseUrl = "http://sclmanagement.azurewebsites.net";
        private readonly ApplicationDbContext _context;
        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAttendance()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetAttendance([Bind("Date", "ClassName")]AttendanceViewModel model)
        {
            List<StudentAttendance> Students = new List<StudentAttendance>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/AttendanceDetails/" + model.Date + "/" + model.ClassName);
                
                if (Res.IsSuccessStatusCode)
                {
                    var AttendanceResponse = Res.Content.ReadAsStringAsync().Result;
                    Students = JsonConvert.DeserializeObject<List<StudentAttendance>>(AttendanceResponse);
                }
            }
            foreach (var Stud in Students)
            {
                Stud.Date = model.Date;
            }
            ViewBag.Date = model.Date;
            return View(Students);
        }

        [HttpPost]
        public async Task<IActionResult> SendAttendance(List<StudentAttendance> objModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var StudentList = JsonConvert.SerializeObject(objModel);
                var content = new StringContent(StudentList.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/AttendanceDetails", content);
                if (Res.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully Updated attendance!";
                }
                
            }
            ViewBag.Date = objModel.FirstOrDefault().Date;
            return View("GetAttendance", objModel);
        }
    }
}