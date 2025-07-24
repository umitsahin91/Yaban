using Yaban.Web.Services.Alert;

namespace Yaban.Web.Services.Notification;

public interface IAlertService
{
    void Alert(AlertType type, string message);

    void SuccessAlert(string message);

    void WarningAlert(string message);

    void ErrorAlert(string message);
}

