using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class LawyersController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/Lawyers/Index.cshtml");
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/Lawyers/Create.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/Admin/Lawyers/Edit.cshtml");
        }

        public IActionResult Viewlawyer()
        {
            return View("~/Views/Admin/Lawyers/Viewlawyer.cshtml");
        }
    }
}
