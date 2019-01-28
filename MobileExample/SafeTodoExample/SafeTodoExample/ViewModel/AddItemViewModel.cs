using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using SafeTodoExample.Helpers;
using SafeTodoExample.Model;
using SafeTodoExample.ViewModel.Base;
using Xamarin.Forms;

namespace SafeTodoExample.ViewModel
{
    public class AddItemViewModel : BaseViewModel
    {
        private string _title;
        private string _detail;
        private bool _isCompleted;
        private DateTime _createdOn;

        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public string Details { get => _detail; set => SetProperty(ref _detail, value); }

        public bool IsCompleted { get => _isCompleted; set => SetProperty(ref _isCompleted, value); }

        public DateTime CreatedOn { get => _createdOn; set => SetProperty(ref _createdOn, value); }

        public bool Edit { get; }

        public ICommand SaveItemCommand { get; private set; }

        public ICommand CloseDialogCommand { get; private set; }

        public ICommand DeleteItemCommand { get; private set; }

        public AddItemViewModel()
        {
            InitializeCommands();
        }

        public AddItemViewModel(TodoItem item, bool edit)
        {
            Edit = edit;
            Title = item.Title ?? string.Empty;
            Details = item.Detail ?? string.Empty;
            IsCompleted = item.IsCompleted;
            CreatedOn = item.CreatedOn;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveItemCommand = new Command(async () => await OnSaveItemCommand());
            DeleteItemCommand = new Command(async (item) => await OnDeleteItemsCommand((TodoItem)item));
            CloseDialogCommand = new Command(async () => await OnCloseDialogCommand());
        }

        private async Task OnCloseDialogCommand()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync(true);
        }

        public async Task OnSaveItemCommand()
        {
            try
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading("Adding/Updating entry"))
                {
                    if (Edit)
                    {
                        await AppService.UpdateItemAsync(
                            new TodoItem
                            {
                                Title = Title,
                                Detail = Details,
                                CreatedOn = CreatedOn,
                                IsCompleted = IsCompleted
                            });
                    }
                    else
                    {
                        await AppService.AddItemAsync(
                            new TodoItem
                            {
                                Title = Title,
                                Detail = Details,
                                CreatedOn = DateTime.Now
                            });
                    }
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                MessagingCenter.Send(this, MessengerConstants.RefreshItemList);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Add Item Failed: {ex.Message}", "OK");
            }
        }

        public async Task OnDeleteItemsCommand(TodoItem item)
        {
            try
            {
                var result = await Application.Current.MainPage.DisplayAlert(
                    "Delete item", "Are you sure you want to delete this item from list", "Delete", "Cancel");
                if (result)
                {
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading("Deleting entry"))
                    {
                        await DeleteItemAsync(item);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    MessagingCenter.Send(this, MessengerConstants.RefreshItemList);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Delete Item Failed: {ex.Message}", "OK");
            }
        }

        public async Task DeleteItemAsync(TodoItem item)
        {
            await AppService.DeleteItemAsync(item);
        }
    }
}
