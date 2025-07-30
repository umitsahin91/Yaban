namespace Yaban.Web.Models.Seo;

public class SeoData
{
    // --- TEMEL SEO ETİKETLERİ ---
    public string Title { get; set; } = "Varsayılan Proje Başlığı";
    public string Description { get; set; } = "Projemiz hakkında varsayılan açıklama.";
    public string Robots { get; set; } = "index, follow";
    public string? CanonicalUrl { get; set; }
    public string? Keywords { get; set; }

    // --- OPEN GRAPH (Facebook, LinkedIn vb. için) ---
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string OgType { get; set; } = "website";
    public string? OgSiteName { get; set; }

    // --- TWITTER CARDS ---
    public string TwitterCard { get; set; } = "summary_large_image";
    public string? TwitterSite { get; set; }
}
