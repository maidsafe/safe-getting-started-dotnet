using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using SafeTodoExample.Helpers;
using SafeTodoExample.ViewModel.Base;
using Xamarin.Forms;

namespace SafeTodoExample.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public const string AuthInProgressMessage = "Connecting to SAFE Network...";

        public bool IsMock
        {
            get
            {
#if SAFE_APP_MOCK
                return true;
#else
                return false;
#endif
            }
        }

        public string BuildMode
        {
            get
            {
#if SAFE_APP_MOCK
                return "MOCK";
#else
                return "Non-Mock";
#endif
            }
        }

        public string WelcomeText
        {
            get
            {
#if SAFE_APP_MOCK
                return "You are running the mock build of the application. " +
                    "The button below will perform mock authentication for you.";
#else
                return "You are runnning the non-mock build of the application." +
                "Before hitting the Authenticate button please make sure that " +
                "you have your IP updated, SAFE Authenticator Application installed and you are logged in. You know the drill!";
#endif
            }
        }

        public ICommand ConnectCommand => new Command(async () => await ConnectToNetworkAsync());

        public async Task ConnectToNetworkAsync()
        {
            try
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading("Authenticating"))
                {
#if SAFE_APP_MOCK
                    // Create mock account and test app
                    await AppService.ProcessMockAuthentication();
                    MessagingCenter.Send(this, MessengerConstants.NavigateToItemPage);
#else
                    // Sending AuthReq to the authenticator
                    await AppService.ProcessNonMockAuthentication();
#endif
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
