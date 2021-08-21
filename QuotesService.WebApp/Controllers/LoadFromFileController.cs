using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.WebApp.Controllers
{
    public class LoadFromFileController : Controller
    {
        public IActionResult Main(string marketName, string tickerName)
        {
            ViewBag.MarketName = marketName ?? "Unknown market";
            ViewBag.TickerName = tickerName ?? "Unknown ticker";

            return View();
        }
    }
}
