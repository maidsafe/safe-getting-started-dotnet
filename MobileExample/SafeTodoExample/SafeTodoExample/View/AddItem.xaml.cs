using SafeTodoExample.Helpers;
using SafeTodoExample.Model;
using SafeTodoExample.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeTodoExample.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItem : ContentPage
    {
        private readonly TodoItem _item;
        private readonly bool _edit;
        private AddItemViewModel _viewModel;

        public AddItem()
        {
            InitializeComponent();
        }

        public AddItem(TodoItem item)
        {
            InitializeComponent();
            _edit = true;
            _item = item;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel == null)
            {
                if (_edit)
                {
                    _viewModel = new AddItemViewModel(_item, _edit);
                }
                else
                {
                    _viewModel = new AddItemViewModel();
                }
            }

            BindingContext = _viewModel;
            AddToolbarItems();
        }

        private void AddToolbarItems()
        {
            if (ToolbarItems.Count == 0)
            {
                if (_edit)
                {
                    ToolbarItems.Add(new ToolbarItem()
                    {
                        Command = _viewModel.DeleteItemCommand,
                        CommandParameter = _item,
                        Icon = "deleteToolbarIcon",
                        Text = "Delete"
                    });
                }
                ToolbarItems.Add(new ToolbarItem() { Command = _viewModel.SaveItemCommand, Icon = "checkToolbarIcon", Text = "Save" });
            }
        }

        public void MessageCenterUnsubscribe()
        {
            MessagingCenter.Unsubscribe<AddItemViewModel>(this, MessengerConstants.HidePopUp);
        }

        public void MessageCenterSubscribe()
        {
            MessagingCenter.Subscribe<AddItemViewModel>(
               this,
               MessengerConstants.HidePopUp,
               async sender =>
               {
                   await Navigation.PopAsync();
               });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
