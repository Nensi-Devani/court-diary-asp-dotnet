
using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class ClientsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/Clients/Index.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/Admin/Clients/Edit.cshtml");
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/Clients/Create.cshtml");
        }

        public IActionResult Viewdetails()
        {
            return View("~/Views/Admin/Clients/Viewdetails.cshtml");
        }
    }
}
