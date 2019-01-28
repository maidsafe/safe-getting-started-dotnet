using System;
using System.Threading;
using System.Threading.Tasks;
using App;
using App.Network;
using SharedDemoCode;
using SharedDemoCode.Network;

namespace LiveNetworkExample
{
    internal class Program
    {
        private static Mutex _mutex;
        private static bool _firstApplicationInstance;

        private static async Task Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            Console.WriteLine("SafeNetwork Console Application");

            if (IsApplicationFirstInstance())
            {
                // args[0] is always the path to the application
                // update system registry
                Helpers.RegisterAppProtocol(args[0]);

                // Authentication with the SAFE browser
                await Authentication.AuthenticationWithBrowserAsync();

                // Start named pipe server and listen for message
                var authResponse = PipeComm.ReceiveNamedPipeServerMessage();

                if (!string.IsNullOrEmpty(authResponse))
                {
                    // Create session from response
                    await Authentication.ProcessAuthenticationResponse(authResponse);

                    // Show user menu
                    UserInput userInput = new UserInput();
                    await userInput.ShowUserOptions();
                }
            }
            else
            {
                // We are not the first instance, send the named pipe message with our payload and stop loading
                if (args.Length >= 2)
                {
                    // We are not the first instance, send the named pipe message with our payload and stop loading
                    var namedPipePayload = new NamedPipePayload
                    {
                        SignalQuit = false,
                        Arguments = args[1]
                    };

                    // Send the message
                    PipeComm.SendNamedPipeClient(namedPipePayload);
                }

                // Close app
                return;
            }
            Console.ReadLine();
        }

        private static bool IsApplicationFirstInstance()
        {
            // Allow for multiple runs but only try and get the mutex once
            if (_mutex == null)
            {
                _mutex = new Mutex(true, ConsoleAppConstants.AppName, out _firstApplicationInstance);
            }
            return _firstApplicationInstance;
        }
    }
}
