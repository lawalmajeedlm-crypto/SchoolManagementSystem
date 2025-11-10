using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
