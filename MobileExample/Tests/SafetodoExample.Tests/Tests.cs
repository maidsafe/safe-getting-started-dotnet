using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeTodoExample.Helpers;
using SafeTodoExample.ViewModel;

namespace SafetodoExample.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task MockAuthenticationTest()
        {
            try
            {
                var authViewModel = new MainPageViewModel();
                await authViewModel.ConnectToNetworkAsync();
                Assert.True(authViewModel.AppService.IsSessionAvailable);
                authViewModel.AppService.Dispose();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Test]
        public async Task MutableOperationsTest()
        {
            try
            {
                var authViewModel = new MainPageViewModel();
                await authViewModel.ConnectToNetworkAsync();
                Assert.True(authViewModel.AppService.IsSessionAvailable);
                await authViewModel.AppService.GetMdInfoAsync();

                var todoItemsViewModel = new TodoItemsPageViewModel();

                // Test get mdata entries
                await todoItemsViewModel.OnRefreshItemsCommand();
                Assert.Zero(todoItemsViewModel.ToDoItems.Count);

                var addItemViewModel = new AddItemViewModel();

                // Test add todo item
                addItemViewModel.Title = Misc.GetRandomString(10);
                addItemViewModel.Details = Misc.GetRandomString(10);

                // Test fetch todo items
                await addItemViewModel.OnSaveItemCommand();
                await todoItemsViewModel.OnRefreshItemsCommand();
                Assert.NotZero(todoItemsViewModel.ToDoItems.Count);

                // Test add second todo item
                addItemViewModel.Title = Misc.GetRandomString(10);
                addItemViewModel.Details = Misc.GetRandomString(10);

                // Test fetch todo items
                await addItemViewModel.OnSaveItemCommand();
                await todoItemsViewModel.OnRefreshItemsCommand();
                Assert.NotZero(todoItemsViewModel.ToDoItems.Count);
                Assert.AreEqual(2, todoItemsViewModel.ToDoItems.Count);

                // Test delete todo item
                await addItemViewModel.DeleteItemAsync(todoItemsViewModel.ToDoItems[0]);

                await todoItemsViewModel.OnRefreshItemsCommand();
                Assert.AreEqual(1, todoItemsViewModel.ToDoItems.Count);

                // Test update todo item
                var updateViewModel = new AddItemViewModel(todoItemsViewModel.ToDoItems[0], true);
                var newDetails = "UpdatedDetails";
                updateViewModel.Details = newDetails;

                await updateViewModel.OnSaveItemCommand();
                await todoItemsViewModel.OnRefreshItemsCommand();
                Assert.AreEqual(1, todoItemsViewModel.ToDoItems.Count);
                Assert.AreEqual(newDetails, todoItemsViewModel.ToDoItems[0].Detail);

                authViewModel.AppService.Dispose();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
