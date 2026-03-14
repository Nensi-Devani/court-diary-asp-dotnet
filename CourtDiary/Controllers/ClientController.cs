using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult ViewClient()
        {
            return View();
        }
    }
}
