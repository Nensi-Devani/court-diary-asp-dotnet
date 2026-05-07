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
                    return RedirectToAction("Index", "AdminDashboard");
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            var user = db.Users.FirstOrDefault(x => x.email == Email);
            if (user != null)
            {
                // Generate a random 6-character alphanumeric password
                string newPassword = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                // Update the user's password in the database
                user.password = newPassword;
                db.SaveChanges();

                ViewBag.Success = true;
                ViewBag.NewPassword = newPassword;
                return View();
            }

            ViewBag.Message = "Email address not found.";
            return View();
        }
    }
}
