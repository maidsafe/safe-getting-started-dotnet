using Acr.UserDialogs;
using Xamarin.Forms;

namespace SafeTodoExample.Helpers
{
    internal class DialogHelper
    {
        public static void ShowToast(string message, DialogType type)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(3000);
            switch (type)
            {
                case DialogType.Success:
                    toastConfig.SetBackgroundColor(Color.Green);
                    break;
                case DialogType.Information:
                    toastConfig.SetBackgroundColor(Color.Blue);
                    break;
                case DialogType.Warning:
                    toastConfig.SetBackgroundColor(Color.LightBlue);
                    break;
                case DialogType.Error:
                    toastConfig.SetBackgroundColor(Color.Red);
                    break;
                default:
                    break;
            }

            toastConfig.SetMessageTextColor(Color.White);

            UserDialogs.Instance.Toast(toastConfig);
        }
    }

    public enum DialogType
    {
        Success,
        Information,
        Warning,
        Error
    }
}
