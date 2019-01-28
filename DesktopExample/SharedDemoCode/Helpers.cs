using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using SafeApp;
using SafeApp.Utilities;
using SharedDemoCode;

namespace App
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public static class Helpers
    {
        // Add safe-auth:// in encoded auth request
        public static string UrlFormat(string encodedString, bool toAuthenticator)
        {
            var scheme = toAuthenticator ? "safe-auth" : $"{ConsoleAppConstants.AppId}";
            return $"{scheme}://{encodedString}";
        }

        public static string GetRequestData(string url)
        {
            return new Uri(url).PathAndQuery.Replace("/", string.Empty);
        }

        // Generating encoded app request using appname, appid, vendor
        public static async Task<(uint, string)> GenerateEncodedAppRequestAsync()
        {
            Console.WriteLine("\nGenerating application authentication request");

            // Generate an AuthReq

            return (0, null);
        }

        // Registering URL Protocol in System Registry using full path of the application
        public static void RegisterAppProtocol(string appPath)
        {
            Console.WriteLine("\nRegistering Apps URL Protocol in Registry");

            // open App's protocol's subkey
            RegistryKey mainKey = Registry.CurrentUser.OpenSubKey("Software", true)?.OpenSubKey("Classes", true);

            char[] padding = { '=' };
            string appUrl = "safe-" + Convert.ToBase64String(ConsoleAppConstants.AppId.ToUtfBytes().ToArray())
                .TrimEnd(padding).Replace('+', '-').Replace('/', '_');

            var key = mainKey?.OpenSubKey(appUrl, true);

            // because two apps are using same registry key so
            // we are deleting the already present registry key, and then adding a new one
            if (key != null)
            {
                mainKey.DeleteSubKeyTree(appUrl);
                key = mainKey.OpenSubKey(appUrl, true);
            }

            // if the protocol is not registered yet...we register it
            if (key == null)
            {
                key = mainKey.CreateSubKey(appUrl);
                key.SetValue(string.Empty, "URL: dotUrlRegister Protocol");
                key.SetValue("URL Protocol", string.Empty);

                // %1 represents the argument - this tells windows to open this program with an argument / parameter
                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, appPath + " " + "%1");
            }
            key.Close();
        }

        // Used to generate random string of any length
        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
