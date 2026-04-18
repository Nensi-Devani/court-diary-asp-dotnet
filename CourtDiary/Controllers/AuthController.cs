//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

//namespace CourtDiary.Controllers
//{
//    public class AuthController : Controller
//    {
//        public IActionResult Login()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult Login(string Email, string Password)
//        {
//            // (optional: save data)
//            if (Email == "ndevani894@rku.ac.in")
//            {
//                HttpContext.Session.SetString("UserRole", "Admin");
//            }
//            else
//            {
//                HttpContext.Session.SetString("UserRole", "User");
//            }

//            HttpContext.Session.SetString("UserEmail", Email);

//            return RedirectToAction("Index", "Dashboard");
//            //return RedirectToAction("Index", "Dashboard"); // ✅ go to dashboard
//        }

//        public IActionResult SignUp()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult SignUp(string FullName, string Email, string Password, string ConfirmPassword)
//        {
//            // (optional: save data)

//            return RedirectToAction("Index", "Dashboard"); // ✅ go to dashboard
//        }
//        public IActionResult ForgotPassword()
//        {
//            return View();
//        }
//    }
//}





//using Microsoft.AspNetCore.Mvc;
//using CourtDiary.Models;
//using System;
//using System.Linq;

//namespace CourtDiary.Controllers
//{
//    public class AuthController : Controller
//    {   
//        private readonly CourtDiaryContext db = new CourtDiaryContext();

//        // GET: Login page
//        public IActionResult Login()
//        {
//            return View();
//        }

//        // POST: Login
//        [HttpPost]
//        public IActionResult Login(string Email, string Password)
//        {
//            var user = db.Users
//                         .FirstOrDefault(x => x.email == Email
//                                           && x.password == Password);

//            if (user != null)
//            {
//                HttpContext.Session.SetString("UserEmail", user.email);

//                if (user.role == 1)
//                {
//                    HttpContext.Session.SetString("UserRole", "Admin");
//                }
//                else
//                {
//                    HttpContext.Session.SetString("UserRole", "User");
//                }

//                return RedirectToAction("Index", "Dashboard");
//            }

//            ViewBag.Message = "Invalid Email or Password";
//            return View();
//        }

//        // GET: SignUp page
//        public IActionResult SignUp()
//        {
//            return View();
//        }

//        // POST: SignUp
//        [HttpPost]
//        public IActionResult SignUp(string FullName, string Email, string Password, string ConfirmPassword)
//        {
//            if (Password != ConfirmPassword)
//            {
//                ViewBag.Message = "Passwords do not match";
//                return View();
//            }

//            var existingUser = db.Users.FirstOrDefault(x => x.email == Email);

//            if (existingUser != null)
//            {
//                ViewBag.Message = "Email already exists";
//                return View();
//            }

//            User newUser = new User
//            {
//                name = FullName,
//                email = Email,
//                password = Password,
//                created_at = DateTime.Now,
//                role = 0,
//                is_varified = false
//            };

//            db.Users.Add(newUser);
//            db.SaveChanges();

//            return RedirectToAction("Login");
//        }

//        public IActionResult ForgotPassword()
//        {
//            return View();
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System;
using System.Linq;

namespace CourtDiary.Controllers
{
    public class AuthController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        // GET: Login page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var user = db.Users
                         .FirstOrDefault(x => x.email == Email
                                           && x.password == Password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.email);

                if (user.role == 1)
                {
                    HttpContext.Session.SetString("UserRole", "Admin");

                    // FIXED: Admin goes to Admin Dashboard
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    HttpContext.Session.SetString("UserRole", "User");

                    // FIXED: User goes to User Dashboard
                    return RedirectToAction("Index", "Dashboard", new { area = "" });
                }
            }

            ViewBag.Message = "Invalid Email or Password";
            return View();
        }

        // GET: SignUp page
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: SignUp
        [HttpPost]
        public IActionResult SignUp(string FullName, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.Message = "Passwords do not match";
                return View();
            }

            var existingUser = db.Users.FirstOrDefault(x => x.email == Email);
            if (existingUser != null)
            {
                ViewBag.Message = "Email already exists";
                return View();
            }

            User newUser = new User
            {
                name = FullName,
                email = Email,
                password = Password,
                created_at = DateTime.Now,
                role = 0,
                is_varified = false
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
