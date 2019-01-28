using System;
using System.Threading.Tasks;
using App.Network;
using SafeApp.Utilities;

namespace SharedDemoCode.Network
{
    public class UserInput
    {
        private DataOperations _mdOperations;

        public UserInput()
        {
            _mdOperations = new DataOperations();
        }

        public async Task ShowUserOptions()
        {
            await _mdOperations.CreateMutableData();

            var userChoice = ShowUserMenu();
            while (userChoice != -1)
            {
                switch (userChoice)
                {
                    case 1:
                        await AddEntryAsync();
                        break;
                    case 2:
                        await UpdateEntryAsync();
                        break;
                    case 3:
                        await DeleteEntryAsync();
                        break;
                    case 4:
                        await ListEntriesAsync();
                        break;
                    default:
                        Console.WriteLine("Not a valid choice.");
                        break;
                }
                userChoice = ShowUserMenu();
            }
        }

        private int ShowUserMenu()
        {
            Console.WriteLine("\nMutable Data Operation:");
            Console.WriteLine("1. Add entry");
            Console.WriteLine("2. Update entry");
            Console.WriteLine("3. Delete entry");
            Console.WriteLine("4. List all entries");
            Console.Write("Select option: ");
            var input = Console.ReadLine();
            return int.TryParse(input, out int result) ? result : -1;
        }

        private async Task AddEntryAsync()
        {
            Console.Write("\nEnter key: ");
            var key = Console.ReadLine();
            Console.Write("Enter value: ");
            var value = Console.ReadLine();
            await _mdOperations.AddEntry(key, value);
        }

        private async Task UpdateEntryAsync()
        {
            Console.Write("\nEnter key to update: ");
            var key = Console.ReadLine();
            Console.Write("Enter new value: ");
            var value = Console.ReadLine();
            await _mdOperations.UpdateEntry(key, value);
        }

        private async Task DeleteEntryAsync()
        {
            Console.Write("\nEnter entry key to delete : ");
            var key = Console.ReadLine();
            await _mdOperations.DeleteEntry(key);
        }

        private async Task ListEntriesAsync()
        {
            Console.WriteLine("\nEntries in Mutable Data:");
            var entries = await _mdOperations.GetEntries();
            if (entries.Count == 0)
            {
                Console.WriteLine("\n0 entries.");
            }

            foreach (var entry in entries)
            {
                var key = entry.Key.Key;
                var value = entry.Value.Content;

                Console.WriteLine("Key : " + key.ToUtfString());
                Console.WriteLine("Value : " + value.ToUtfString());
            }
        }
    }
}
