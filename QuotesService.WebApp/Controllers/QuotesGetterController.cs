using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Controllers
{
    public class QuotesGetterController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
    }
}
