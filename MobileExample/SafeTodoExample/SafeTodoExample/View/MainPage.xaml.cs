using SafeTodoExample.Helpers;
using SafeTodoExample.Service;
using SafeTodoExample.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeTodoExample.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        public void MessageCenterUnsubscribe()
        {
            MessagingCenter.Unsubscribe<MainPageViewModel>(this, MessengerConstants.NavigateToItemPage);
            MessagingCenter.Unsubscribe<AppService>(this, MessengerConstants.NavigateToItemPage);
        }

        public void MessageCenterSubscribe()
        {
            MessagingCenter.Subscribe<MainPageViewModel>(
               this,
               MessengerConstants.NavigateToItemPage,
               sender =>
               {
                   Application.Current.MainPage = new NavigationPage(new ToDoItemsPage());
               });

            MessagingCenter.Subscribe<AppService>(
               this,
               MessengerConstants.NavigateToItemPage,
               sender =>
               {
                   Application.Current.MainPage = new NavigationPage(new ToDoItemsPage());
               });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessageCenterUnsubscribe();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel == null)
            {
                _viewModel = new MainPageViewModel();
            }

            BindingContext = _viewModel;

            MessageCenterSubscribe();
        }
    }
}
