using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Yaban.Web.Models;

namespace Yaban.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();       // Ana sayfa
    public IActionResult About() => View();       // Hakkımızda
    public IActionResult Contact() => View();     // İletişim
    public IActionResult Services() => View();    // Hizmetler
    public IActionResult Projects() => View();
}
