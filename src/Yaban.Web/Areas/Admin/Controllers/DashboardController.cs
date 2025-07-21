using Microsoft.AspNetCore.Mvc;

namespace Yaban.Web.Areas.Admin.Controllers
{
   
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
