using Microsoft.AspNetCore.Mvc;
using QuotesService.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Controllers
{
    public class LoadFromFileController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
    }
}
