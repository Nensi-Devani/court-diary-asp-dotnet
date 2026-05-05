//using Microsoft.AspNetCore.Mvc;
//using CourtDiary.Models;
//using System;
//using System.Linq;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;

//namespace CourtDiary.Controllers
//{
//    public class ClientsController : Controller
//    {
//        private readonly CourtDiaryContext db = new CourtDiaryContext();

//        // =======================
//        // LIST CLIENTS
//        // =======================
//        public IActionResult Index()
//        {
//            var clients = db.Clients.ToList();
//            return View(clients);
//        }

//        // =======================
//        // CREATE (GET)
//        // =======================
//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // =======================
//        // CREATE (POST)
//        // =======================
//        [HttpPost]
//        public async Task<IActionResult> Create(Client c, IFormFile avatar)
//        {
//            if (avatar != null && avatar.Length > 0)
//            {
//                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
//                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

//                using (var stream = new FileStream(path, FileMode.Create))
//                {
//                    await avatar.CopyToAsync(stream);
//                }

//                c.ImagePath = "/images/" + fileName;
//            }

//            c.created_at = DateTime.Now;

//            db.Clients.Add(c);
//            db.SaveChanges();

//            return RedirectToAction("Index", "Clients");

//        }

//        // =======================
//        // EDIT (GET)
//        // =======================
//        [HttpGet]
//        public IActionResult Edit(int id)
//        {
//            var client = db.Clients.FirstOrDefault(x => x.client_id == id);

//            if (client == null)
//                return NotFound();

//            return View(client);
//        }

//        // =======================
//        // EDIT (POST)
//        // =======================
//        [HttpPost]
//        public async Task<IActionResult> Edit(Client c, IFormFile avatar)
//        {
//            var client = db.Clients.FirstOrDefault(x => x.client_id == c.client_id);

//            if (client == null)
//                return NotFound();

//            // Update fields
//            client.name = c.name;
//            client.phone = c.phone;
//            client.address = c.address;
//            client.description = c.description;

//            // Image update
//            if (avatar != null && avatar.Length > 0)
//            {
//                // Delete old image
//                if (!string.IsNullOrEmpty(client.ImagePath))
//                {
//                    string oldPath = Path.Combine(
//                        Directory.GetCurrentDirectory(),
//                        "wwwroot",
//                        client.ImagePath.TrimStart('/')
//                    );

//                    if (System.IO.File.Exists(oldPath))
//                    {
//                        System.IO.File.Delete(oldPath);
//                    }
//                }

//                // Save new image
//                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
//                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

//                using (var stream = new FileStream(path, FileMode.Create))
//                {
//                    await avatar.CopyToAsync(stream);
//                }

//                client.ImagePath = "/images/" + fileName;
//            }

//            db.SaveChanges();

//            return RedirectToAction("Index");
//        }

//        // =======================
//        // VIEW DETAILS
//        // =======================
//        public IActionResult ViewClient(int id)
//        {
//            var client = db.Clients.FirstOrDefault(x => x.client_id == id);

//            if (client == null)
//                return NotFound();

//            return View(client);
//        }

//        // =======================
//        // DELETE
//        // =======================
//        public IActionResult Delete(int id)
//        {
//            var client = db.Clients.FirstOrDefault(x => x.client_id == id);

//            if (client != null)
//            {
//                // Delete image also
//                if (!string.IsNullOrEmpty(client.ImagePath))
//                {
//                    string path = Path.Combine(
//                        Directory.GetCurrentDirectory(),
//                        "wwwroot",
//                        client.ImagePath.TrimStart('/')
//                    );

//                    if (System.IO.File.Exists(path))
//                    {
//                        System.IO.File.Delete(path);
//                    }
//                }

//                db.Clients.Remove(client);
//                db.SaveChanges();
//            }

//            return RedirectToAction("Index", "Clients");
//        }
//    }
//}




using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CourtDiary.Controllers
{
    public class ClientsController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        // =======================
        // LIST CLIENTS
        // =======================
        public IActionResult Index()
        {
            var clients = db.Clients.ToList();
            return View(clients);
        }

        // =======================
        // CREATE (GET)
        // =======================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =======================
        // CREATE (POST)
        // =======================
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

            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var uri = new Uri(referer);
                var path = uri.AbsolutePath.ToLower();

                if (path.StartsWith("/admin"))
                    return Redirect("/Admin/Clients/Index");
                else
                    return Redirect("/Clients/Index");
            }

            return Redirect("/Clients/Index");
        }

        // =======================
        // EDIT (GET)
        // =======================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // =======================
        // EDIT (POST)
        // =======================
        [HttpPost]
        public async Task<IActionResult> Edit(Client c, IFormFile avatar)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == c.client_id);

            if (client == null)
                return NotFound();

            client.name = c.name;
            client.phone = c.phone;
            client.address = c.address;
            client.description = c.description;

            if (avatar != null && avatar.Length > 0)
            {
                if (!string.IsNullOrEmpty(client.ImagePath))
                {
                    string oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        client.ImagePath.TrimStart('/')
                    );

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

            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var uri = new Uri(referer);
                var path = uri.AbsolutePath.ToLower();

                if (path.StartsWith("/admin"))
                    return Redirect("/Admin/Clients/Index");
                else
                    return Redirect("/Clients/Index");
            }

            return Redirect("/Clients/Index");
        }

        // =======================
        // VIEW DETAILS
        // =======================
        public IActionResult ViewClient(int id)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == id);

            if (client == null)
                return NotFound();

            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var uri = new Uri(referer);
                var path = uri.AbsolutePath.ToLower();

                if (path.StartsWith("/admin"))
                    ViewBag.BackUrl = "/Admin/Clients/Index";
                else
                    ViewBag.BackUrl = "/Clients/Index";
            }
            else
            {
                ViewBag.BackUrl = "/Clients/Index";
            }

            return View(client);
        }

        // =======================
        // DELETE
        // =======================
        public IActionResult Delete(int id)
        {
            var client = db.Clients.FirstOrDefault(x => x.client_id == id);

            if (client != null)
            {
                if (!string.IsNullOrEmpty(client.ImagePath))
                {
                    string path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        client.ImagePath.TrimStart('/')
                    );

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                db.Clients.Remove(client);
                db.SaveChanges();
            }

            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var uri = new Uri(referer);
                var path = uri.AbsolutePath.ToLower();

                if (path.StartsWith("/admin"))
                    return Redirect("/Admin/Clients/Index");
                else
                    return Redirect("/Clients/Index");
            }

            return Redirect("/Clients/Index");
        }
    }
}