using Yaban.Web.Models.Seo;

namespace Yaban.Web.Models;

public abstract class BaseViewModel : ISeoEnabledViewModel
{
    public SeoData SeoInfo { get; set; } = new SeoData();
}
