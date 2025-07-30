using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Yaban.Web.Models;
using Yaban.Web.Models.Seo;

namespace Yaban.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() 
    {
        var model = new IndexViewModel
        {
            SeoInfo = new SeoData
            {
                Title = "Yaban Web - Anasayfa",
                Description = "Yaban Web'in anasayfası.",
                Keywords = "anasayfa, yaban, web"
            }
        };
        return View(model);
    }
    public IActionResult About() => View();       // Hakkımızda
    public IActionResult Contact() => View();     // İletişim
    public IActionResult Services() => View();    // Hizmetler
    public IActionResult Projects() => View();
}
