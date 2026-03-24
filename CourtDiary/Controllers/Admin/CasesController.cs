using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class CasesController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/Cases/Index.cshtml");
        }
        public IActionResult Create()
        {
            return View("~/Views/Admin/Cases/Create.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/Admin/Cases/Edit.cshtml");
        }

        public IActionResult Viewdetails()
        {
            return View("~/Views/Admin/Cases/Viewdetails.cshtml");
        }
    }
}
