using SafeTodoExample.Model;
using SafeTodoExample.Service;
using Xamarin.Forms;

namespace SafeTodoExample.ViewModel.Base
{
    public class BaseViewModel : ObservableObject
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                {
                    IsNotBusy = !isBusy;
                }
            }
        }

        private bool isNotBusy = true;

        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                {
                    IsBusy = !isNotBusy;
                }
            }
        }

        public AppService AppService => DependencyService.Get<AppService>();
    }
}
