using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SafeTodoExample.Helpers;
using SafeTodoExample.Model;
using SafeTodoExample.ViewModel.Base;
using Xamarin.Forms;

namespace SafeTodoExample.ViewModel
{
    public class TodoItemsPageViewModel : BaseViewModel
    {
        public ICommand AddItemCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand RefreshItemCommand { get; }

        public ICommand MarkCompletedCommand { get; private set; }

        private ObservableCollection<TodoItem> _todoItems;

        public ObservableCollection<TodoItem> ToDoItems
        {
            get => _todoItems;
            set => SetProperty(ref _todoItems, value);
        }

        public TodoItemsPageViewModel()
        {
            AddItemCommand = new Command(async () => await OnAddItemCommand());
            LogoutCommand = new Command(async () => await OnLogoutCommandAsync());
            RefreshItemCommand = new Command(async () => await OnRefreshItemsCommand());
            MarkCompletedCommand = new Command(async item => await OnCompleteItemsCommand((TodoItem)item));
            ToDoItems = new ObservableCollection<TodoItem>();
            Task.Run(async () =>
            {
                await AppService.GetMdInfoAsync();
                await OnRefreshItemsCommand();
            });
        }

        private async Task OnCompleteItemsCommand(TodoItem item)
        {
            try
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading("Updating task"))
                {
                    item.IsCompleted = !item.IsCompleted;
                    await AppService.UpdateItemAsync(item);
                    await OnRefreshItemsCommand();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Add Item Failed: {ex.Message}", "OK");
            }
        }

        public async Task OnRefreshItemsCommand()
        {
            IsBusy = true;
            try
            {
                var todoItem = await AppService.GetItemsAsync();
                ToDoItems = new ObservableCollection<TodoItem>(todoItem.OrderByDescending(i => i.IsCompleted).Reverse());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Fetch Ids Failed: {ex.Message}", "OK");
            }

            IsBusy = false;
        }

        private async Task OnAddItemCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new View.AddItem());
        }

        private async Task OnLogoutCommandAsync()
        {
            var result = await Application.Current.MainPage.DisplayAlert(
               "Logout", "Are you sure you want to logout.", "Yes", "No");
            if (result)
            {
                await AppService.LogoutAsync();
                MessagingCenter.Send(this, MessengerConstants.NavigateToAuthPage);
            }
        }
    }
}
