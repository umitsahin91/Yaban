using Microsoft.AspNetCore.Mvc;
using Yaban.Web.Models.Seo;

namespace Yaban.Web.ViewComponents;

public class SeoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(SeoData seoData)
    {
        // Gelen modeli doğrudan kendi View'ına gönderir.
        return View(seoData ?? new SeoData());
    }
}
