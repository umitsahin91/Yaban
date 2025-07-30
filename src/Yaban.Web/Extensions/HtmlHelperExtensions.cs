using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yaban.Web.Extensions;

public static class HtmlHelperExtensions
{
    public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action)
    {
        // ViewContext üzerinden o anki route (rota) verilerine ulaşıyoruz.
        var routeData = htmlHelper.ViewContext.RouteData;

        var routeAction = routeData.Values["action"]?.ToString();
        var routeController = routeData.Values["controller"]?.ToString();

        // Büyük/küçük harf duyarlılığı olmadan karşılaştırma yapıyoruz.
        var isCurrentRoute = string.Equals(routeController, controller, StringComparison.OrdinalIgnoreCase) &&
                             string.Equals(routeAction, action, StringComparison.OrdinalIgnoreCase);

        return isCurrentRoute ? "active" : string.Empty;
    }
}
