//using Microsoft.AspNetCore.Mvc;

//namespace CourtDiary.Controllers.Admin
//{
//    [Route("Admin/[controller]/[action]")]
//    public class LawyersController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View("~/Views/Admin/Lawyers/Index.cshtml");
//        }

//        public IActionResult Create()
//        {
//            return View("~/Views/Admin/Lawyers/Create.cshtml");
//        }

//        public IActionResult Edit()
//        {
//            return View("~/Views/Admin/Lawyers/Edit.cshtml");
//        }

//        public IActionResult Viewlawyer()
//        {
//            return View("~/Views/Admin/Lawyers/Viewlawyer.cshtml");
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class LawyersController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        // =======================
        // INDEX
        // =======================
        public IActionResult Index()
        {
            var lawyers = db.Users.Where(x => x.role == 1).ToList();
            return View("~/Views/Admin/Lawyers/Index.cshtml", lawyers);
        }

        // =======================
        // CREATE (GET)
        // =======================
        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Lawyers/Create.cshtml");
        }

        // =======================
        // CREATE (POST)
        // =======================
        [HttpPost]
        public async Task<IActionResult> Create(User u, IFormFile avatarFile)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Lawyers/Create.cshtml", u);
            }

            // IMAGE UPLOAD
            if (avatarFile != null && avatarFile.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                u.avatar = "/images/" + fileName;
            }

            // DEFAULT VALUES
            u.role = 1; // Lawyer
            u.created_at = DateTime.Now;
            u.is_varified = true;

            db.Users.Add(u);
            db.SaveChanges();

            return Redirect("/Admin/Lawyers/Index");
        }

        // =======================
        // EDIT (GET)
        // =======================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var lawyer = db.Users.FirstOrDefault(x => x.user_id == id);

            if (lawyer == null)
                return NotFound();

            return View("~/Views/Admin/Lawyers/Edit.cshtml", lawyer);
        }

        // =======================
        // EDIT (POST)
        // =======================
        [HttpPost]
        public async Task<IActionResult> Edit(User u, IFormFile avatarFile)
        {
            var lawyer = db.Users.FirstOrDefault(x => x.user_id == u.user_id);

            if (lawyer == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Lawyers/Edit.cshtml", u);
            }

            // UPDATE FIELDS
            lawyer.name = u.name;
            lawyer.email = u.email;
            lawyer.password = u.password;
            lawyer.office_email = u.office_email;
            lawyer.office_address = u.office_address;
            lawyer.office_phone_no = u.office_phone_no;

            // IMAGE UPDATE
            if (avatarFile != null && avatarFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(lawyer.avatar))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        lawyer.avatar.TrimStart('/')
                    );

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                lawyer.avatar = "/images/" + fileName;
            }

            db.SaveChanges();

            return Redirect("/Admin/Lawyers/Index");
        }

        // =======================
        // VIEW
        // =======================
        public IActionResult Viewlawyer(int id)
        {
            var lawyer = db.Users.FirstOrDefault(x => x.user_id == id);

            if (lawyer == null)
                return NotFound();

            return View("~/Views/Admin/Lawyers/Viewlawyer.cshtml", lawyer);
        }

        // =======================
        // DELETE
        // =======================
        public IActionResult Delete(int id)
        {
            var lawyer = db.Users.FirstOrDefault(x => x.user_id == id);

            if (lawyer != null)
            {
                if (!string.IsNullOrEmpty(lawyer.avatar))
                {
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        lawyer.avatar.TrimStart('/')
                    );

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                db.Users.Remove(lawyer);
                db.SaveChanges();
            }

            return Redirect("/Admin/Lawyers/Index");
        }
    }
}