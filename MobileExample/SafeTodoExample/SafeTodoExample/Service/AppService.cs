using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SafeApp;
using SafeApp.Utilities;
using SafeTodoExample.Helpers;
using SafeTodoExample.Model;
using SafeTodoExample.Service;
using Xamarin.Forms;
#if SAFE_APP_MOCK
using SafeApp.MockAuthBindings;
#endif

[assembly: Dependency(typeof(AppService))]

namespace SafeTodoExample.Service
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class AppService : ObservableObject, IDisposable
    {
        private const string AppContainerListKey = "MySafeTodo";
        public const string AuthDeniedMessage = "Failed to receive Authentication.";
        private Session _session;
#pragma warning disable CS0414
        private MDataInfo _mDataInfo;
#pragma warning restore CS0414

        public bool IsSessionAvailable => _session != null;

        public AppService()
        {
            Session.Disconnected += OnSessionDisconnected;
        }

        public void Dispose()
        {
            FreeState();
            GC.SuppressFinalize(this);
        }

        ~AppService()
        {
            FreeState();
        }

        public void FreeState()
        {
            Session.Disconnected -= OnSessionDisconnected;
            _session?.Dispose();
            _session = null;
            _mDataInfo.Name = null;
        }

        private void OnSessionDisconnected(object obj, EventArgs e)
        {
            if (!obj.Equals(_session))
            {
                return;
            }

            Device.BeginInvokeOnMainThread(
              () =>
              {
                  _session?.Dispose();
              });
        }

        public async Task<(uint, string)> GenerateEncodedAuthReqAsync()
        {
            // Create an AuthReq

            // Return encoded AuthReq
            return (0, null);
        }

        #region MutableData Operation

        public async Task GetMdInfoAsync()
        {
            try
            {
                // Retrieve MDataInfo from App container and deserialise
            }
            catch (FfiException ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                await CreateMutableData();
                await StoreMdInfoAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                throw;
            }
        }

        public async Task StoreMdInfoAsync()
        {
            try
            {
                // Serialise and store MDataInfo in App container
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                throw;
            }
        }

        private async Task CreateMutableData()
        {
            // Create a new random private mutable data

            // Insert permission set
        }

        public async Task AddItemAsync(TodoItem todoItem)
        {
            try
            {
                // Add entries to mutable data
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                throw;
            }
        }

        public async Task<List<TodoItem>> GetItemsAsync()
        {
            // Create a list to hold TodoItems fetched from the network
            try
            {
                // Retrieve the mutable data entries from the network and decrypt
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                throw;
            }
            return null;
        }

        public async Task UpdateItemAsync(TodoItem todoItem)
        {
            try
            {
                // Update mutable data entry
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteItemAsync(TodoItem todoItem)
        {
            try
            {
                // Delete mutable data entry
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
                throw;
            }
        }

        #endregion

        #region Test Network Authentication

        public async Task ProcessNonMockAuthentication()
        {
            // Generate encoded authReq and send to authenticator
        }

        public async Task HandleUrlActivationAsync(string url)
        {
            try
            {
                // Decode auth response and Initialise a new session
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Description: {ex.Message}", "OK");
            }
        }

        #endregion

        #region Mock Authentication
#if SAFE_APP_MOCK
#pragma warning disable CS0649
        private Authenticator _authenticator;
#pragma warning restore CS0649

        private async Task CreateAccountAsync()
        {
            try
            {
                // Create a mock account
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task<Session> CreateTestAppAsync()
        {
            // Authenticate using mock authentication API

            return null;
        }

        public async Task ProcessMockAuthentication()
        {
            await CreateAccountAsync();
            _session = await CreateTestAppAsync();
        }

        public async Task LogoutAsync()
        {
            await Task.Run(() =>
            {
                _authenticator.Dispose();
                Dispose();
            });
        }
#else
        public async Task LogoutAsync()
        {
            await Task.Run(() => { Dispose(); });
        }
#endif
        #endregion
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
