using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp;
using SafeApp.Utilities;

namespace App.Network
{
    public class DataOperations
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static Session _session;

#pragma warning disable 169
        private MDataInfo _mdinfo;
#pragma warning restore 169

        public static void InitialiseSession(Session session)
        {
            try
            {
                _session = session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        internal async Task CreateMutableData()
        {
            try
            {
                Console.WriteLine("\nCreating new mutable data");

                // Create a random private mutable data

                // Insert Permission Sets

                Console.WriteLine("Mutable data created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        internal async Task AddEntry(string key, string value)
        {
            try
            {
                // Add entry to mutable data

                Console.WriteLine("Entry Added");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        internal async Task<List<MDataEntry>> GetEntries()
        {
            // Create an MDataEntry list to hold entries
            try
            {
                // Fetch and decrypt entries

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        internal async Task UpdateEntry(string key, string newValue)
        {
            try
            {
                // Update an existing mutable data entry
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        internal async Task DeleteEntry(string key)
        {
            try
            {
                // Delete an existing mutable data entry
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
