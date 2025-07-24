using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using Yaban.Web.Services.Alert;

namespace Yaban.Web.Services.Notification;

public class AlertService : IAlertService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    
    public AlertService(IHttpContextAccessor httpContextAccessor,
                       ITempDataDictionaryFactory tempDataDictionaryFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
    }
 
    public void Alert(AlertType type, string message)
    {
        PrepareTempData(type, message);
    }

    public void ErrorAlert(string message)
    {
        PrepareTempData(AlertType.Error, message);
    }

    public void SuccessAlert(string message)
    {
        PrepareTempData(AlertType.Success, message);
    }

    public void WarningAlert(string message)
    {
        PrepareTempData(AlertType.Warning, message);
    }


    private void PrepareTempData(AlertType type, string message)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return; // Güvenlik kontrolü

        var tempData = _tempDataDictionaryFactory.GetTempData(context);
        List<AlertData> messages;

        // TempData'dan mevcut değeri güvenli bir şekilde oku
        if (tempData.TryGetValue(AlertDefaults.AlertListKey, out object? value) && value is string existingJson && !string.IsNullOrEmpty(existingJson))
            // Değer dolu bir string ise deserialize et, değilse yeni liste oluştur
            messages = JsonSerializer.Deserialize<List<AlertData>>(existingJson) ?? new List<AlertData>();
        else
            // Değer yoksa veya boşsa, boş bir liste ile başla
            messages = new List<AlertData>();

        // Yeni mesajı listeye ekle
        messages.Add(new AlertData
        {
            Message = message,
            Type = type,
        });

        // Güncellenmiş listeyi tekrar JSON'a çevirerek TempData'ya kaydet
        tempData[AlertDefaults.AlertListKey] = JsonSerializer.Serialize(messages);
    }

}