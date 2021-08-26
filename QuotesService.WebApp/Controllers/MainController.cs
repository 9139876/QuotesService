using Microsoft.AspNetCore.Mvc;

namespace QuotesService.WebApp.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
