using System;
using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace App
{
    public static class PipeComm
    {
        /// <summary>
        /// Starts a new pipe server and read response from client
        /// </summary>
        public static string ReceiveNamedPipeServerMessage()
        {
            try
            {
                PipeSecurity ps = new PipeSecurity();
                PipeAccessRule psRule = new PipeAccessRule(@"Everyone", PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
                ps.AddAccessRule(psRule);

                using (NamedPipeServerStream namedPipeServer =
                    new NamedPipeServerStream("test-pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous, 1, 1, ps))
                {
                    namedPipeServer.WaitForConnection();

                    IFormatter f = new BinaryFormatter();
                    var namedPipePayload = (NamedPipePayload)f.Deserialize(namedPipeServer);

                    return namedPipePayload.SignalQuit ? null : namedPipePayload.Arguments;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Uses a named pipe client to send the currently parsed options to an already running instance.
        /// </summary>
        /// <param name="namedPipePayload"></param>
        public static void SendNamedPipeClient(NamedPipePayload namedPipePayload)
        {
            try
            {
                using (NamedPipeClientStream namedPipeClient = new NamedPipeClientStream("test-pipe"))
                {
                    namedPipeClient.Connect();

                    IFormatter f = new BinaryFormatter();
                    f.Serialize(namedPipeClient, namedPipePayload);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
