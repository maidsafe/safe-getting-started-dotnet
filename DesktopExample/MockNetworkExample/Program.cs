using System;
using System.Threading;
using System.Threading.Tasks;
using App;
using App.Network;
using SharedDemoCode;
using SharedDemoCode.Network;

namespace MockNetworkExample
{
    internal class Program
    {
        private static Mutex _mutex;
        private static bool _firstApplicationInstance;

        private static async Task Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            Console.WriteLine("SafeNetwork Console Application");
            try
            {
                if (IsApplicationFirstInstance())
                {
                    Console.Write("Press Y/y to use mock safe-browser for authentication otherwise a random mock account will be used : ");
                    var input = Console.ReadLine();

                    if (input.Equals("Y") || input.Equals("y"))
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
                        // Create session from mock authentication
                        var session = await Authentication.MockAuthenticationAsync();

                        // Initialise session for Mutable Data operations
                        DataOperations.InitialiseSession(session);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
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
