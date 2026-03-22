
using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/Client/Index.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/Admin/Client/Edit.cshtml");
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/Client/Create.cshtml");
        }

        public IActionResult Viewdetails()
        {
            return View("~/Views/Admin/Client/Viewdetails.cshtml");
        }
    }
}
