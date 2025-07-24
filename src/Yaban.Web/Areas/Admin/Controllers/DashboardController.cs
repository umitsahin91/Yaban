using Microsoft.AspNetCore.Mvc;
using Yaban.Web.Services.Notification;

namespace Yaban.Web.Areas.Admin.Controllers;


public class DashboardController(IAlertService alertService) : AdminBaseController
{
    
    public IActionResult Index()
    {
        alertService.SuccessAlert("Dashboard loaded successfully!");
        return View();
    }
}
