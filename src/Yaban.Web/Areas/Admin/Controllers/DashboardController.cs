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

    [HttpPost] // DataTables genellikle POST metodu ile istek yapar
    public IActionResult GetData()
    {
        // DataTables'ın her istekte gönderdiği ve bizim de geri göndermemiz gereken 'draw' parametresi.
        // Bu, isteklerin doğru sırada işlendiğinden emin olmak için kullanılır.
        var draw = Request.Form["draw"].FirstOrDefault();

        // Test amaçlı statik bir kategori listesi oluşturalım.
        var kategoriler = new List<string> { "Yazılım", "Tasarım", "Pazarlama", "Destek", "İnsan Kaynakları" };

        // 15 adet sahte kayıt oluşturacağımız liste.
        var testVerisi = new List<object>();

        for (int i = 1; i <= 15; i++)
        {
            testVerisi.Add(new
            {
                // JSON'da bu alan adlarını kullanacağız: 'adSoyad' ve 'kategori'
                // Bunlar 'columns' attribute'ünde belirteceğimiz isimlerle eşleşmelidir.
                adSoyad = $"Test Kullanıcı {i}",
                kategori = kategoriler[i % kategoriler.Count] // Kategorileri sırayla dağıt
            });
        }

        // DataTables'ın sunucu taraflı işleme için beklediği formatta bir JSON nesnesi oluşturuyoruz.
        // Gerçek bir uygulamada recordsTotal ve recordsFiltered değerleri veritabanı sorgularından gelir.
        var jsonData = new
        {
            draw = Convert.ToInt32(draw),
            recordsTotal = 15,
            recordsFiltered = 15,
            data = testVerisi
        };

        return Json(jsonData);
    }
}
