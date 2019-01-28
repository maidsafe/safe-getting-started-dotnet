using System;
using System.Linq;
using System.Threading.Tasks;
using App.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeApp.Utilities;
using Helpers = App.Helpers;

namespace NetworkExampleTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task MockAuthenticationTestAsync()
        {
            try
            {
                var session = await Authentication.MockAuthenticationAsync();
                Assert.IsNotNull(session);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task PerformMutableDataOptionsTestAsync()
        {
            try
            {
                var session = await Authentication.MockAuthenticationAsync();
                Assert.IsNotNull(session);

                DataOperations.InitialiseSession(session);

                var mdOperations = new DataOperations();
                await mdOperations.CreateMutableData();

                await mdOperations.AddEntry(Helpers.GenerateRandomString(10), Helpers.GenerateRandomString(10));
                var enteries = await mdOperations.GetEntries();
                Assert.AreEqual(1, enteries.Count);

                var key = Helpers.GenerateRandomString(10);
                var value = Helpers.GenerateRandomString(10);
                await mdOperations.AddEntry(key, value);
                enteries = await mdOperations.GetEntries();
                Assert.AreEqual(2, enteries.Count);

                var newValue = Helpers.GenerateRandomString(10);
                await mdOperations.UpdateEntry(key, newValue);
                enteries = await mdOperations.GetEntries();
                Assert.AreEqual(
                    newValue,
                    enteries.Where(e => e.Key.Key.ToUtfString() == key).FirstOrDefault().Value.Content.ToUtfString());

                await mdOperations.DeleteEntry(key);
                enteries = await mdOperations.GetEntries();
                Assert.AreEqual(1, enteries.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
