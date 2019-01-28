using SafeApp;
#if SAFE_APP_MOCK
using SafeApp.MockAuthBindings;
#endif
using System;
using System.Threading.Tasks;
using SafeApp.Utilities;
using SharedDemoCode;

namespace App.Network
{
    public class Authentication
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#if SAFE_APP_MOCK
        public static async Task<Session> MockAuthenticationAsync()
        {
            try
            {
                // Create a mock safe account to perform authentication.
                // We use this method while developing the app or working with tests.
                // This way we don't have to authenticate using safe-browser.

                // Generating random mock account

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                throw ex;
            }
        }
#endif

        public static async Task AuthenticationWithBrowserAsync()
        {
            try
            {
                // Generate and send auth request to safe-browser for authentication.
                Console.WriteLine("Requesting authentication from Safe browser");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                throw;
            }
        }

        public static async Task ProcessAuthenticationResponse(string authResponse)
        {
            try
            {
                // Decode auth response and initialise a new session
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
