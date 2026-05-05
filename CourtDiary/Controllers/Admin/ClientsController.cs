using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]/{id?}")]
    public class ClientsController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        public IActionResult Index()
        {
            var clients = db.Clients.ToList();
            ViewBag.Users = db.Users.ToList();
            return View("~/Views/Admin/Clients/Index.cshtml", clients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = db.Users.ToList();
            return View("~/Views/Admin/Clients/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client c, IFormFile avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                c.ImagePath = "/images/" + fileName;
            }

            c.created_at = DateTime.Now;

            db.Clients.Add(c);
            db.SaveChanges();

            var email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(email))
            {
                var loggedInUser = db.Users.FirstOrDefault(usr => usr.email == email);
                if (loggedInUser != null)
                {
                    db.Notifications.Add(new Notification
                    {
                        user_id = loggedInUser.user_id,
                        title = "New Client Added",
                        message = $"Client {c.name} was successfully added.",
                        created_at = DateTime.Now,
                        is_read = false
                    });
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == id);
            if (client == null) return NotFound();
            ViewBag.Users = db.Users.ToList();
            return View("~/Views/Admin/Clients/Edit.cshtml", client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Client c, IFormFile avatar)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == c.client_id);
            if (client == null) return NotFound();

            client.name = c.name;
            client.phone = c.phone;
            client.address = c.address;
            client.description = c.description;
            client.user_id = c.user_id;

            if (avatar != null && avatar.Length > 0)
            {
                if (!string.IsNullOrEmpty(client.ImagePath))
            {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", client.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                client.ImagePath = "/images/" + fileName;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Viewdetails(int id)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == id);
            if (client == null) return NotFound();
            ViewBag.Users = db.Users.ToList();
            return View("~/Views/Admin/Clients/Viewdetails.cshtml", client);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == id);
            if (client != null)
            {
                if (!string.IsNullOrEmpty(client.ImagePath))
                {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", client.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                db.Clients.Remove(client);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

//using Microsoft.AspNetCore.Mvc;

//namespace CourtDiary.Controllers.Admin
//{
//    //[Area("Admin")]
//    //[Route("Admin/[controller]/[action]")]
//    public class ClientsController : Controller
//    {
//        // =======================
//        // LIST CLIENTS
//        // =======================
//        public IActionResult Index()
//        {
//            return View(); // just show static view
//        }

//        // =======================
//        // CREATE (GET)
//        // =======================
//        public IActionResult Create()
//        {
//            return View(); // just show static view
//        }

//        // =======================
//        // EDIT (GET)
//        // =======================
//        public IActionResult Edit()
//        {
//            return View(); // just show static view
//        }

//        // =======================
//        // VIEW DETAILS
//        // =======================
//        public IActionResult Viewdetails()
//        {
//            return View(); // just show static view
//        }
//    }
//}