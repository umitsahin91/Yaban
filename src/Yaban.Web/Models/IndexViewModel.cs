using Yaban.Web.Models.Seo;

namespace Yaban.Web.Models;

public class IndexViewModel :  ISeoEnabledViewModel
{
    public SeoData SeoInfo { get; set; } = new SeoData();

}
