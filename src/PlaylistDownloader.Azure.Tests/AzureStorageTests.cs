namespace PlaylistDownloader.Azure.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PlaylistDownloader.Azure.Storage;

    [TestClass]
    public class AzureStorageTests
    {
        [TestMethod]
        public async Task WhenGettingRefreshToken_ForExistingUser_ThenReturnExpectedValue()
        {
            var cloudStorageAccountProvider = new TestStorageAccount();

            var azureStorage = new AzureStorage(cloudStorageAccountProvider);

            var userId = Guid.NewGuid().ToString().Split('-')[0];
            var refreshToken = Guid.NewGuid().ToString().Split('-')[0];

            // Act
            await azureStorage.AddOrUpdateUser(userId, refreshToken);

            var storageResponse = await azureStorage.GetRefreshTokenForUser(userId);

            // Assert
            Assert.AreEqual(refreshToken, storageResponse);
        }

        [TestMethod]
        public async Task WhenGettingRefreshToken_ForUserNotAdded_ThenThrowsException()
        {
            Exception expectedException = null;
            
            var cloudStorageAccountProvider = new TestStorageAccount();

            var azureStorage = new AzureStorage(cloudStorageAccountProvider);

            var userId = Guid.NewGuid().ToString().Split('-')[0];

            // Act
            try
            {
                await azureStorage.GetRefreshTokenForUser(userId);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Assert
            Assert.IsNotNull(expectedException);
            Assert.AreEqual("User id not added. AddOrUpdateUser(...) must be called first.", expectedException.Message);
        }

        private class TestStorageAccount : ICloudStorageAccountProvider
        {
            public Task<CloudStorageAccount> GetCloudStorageAccount()
            {
                var connectionString = Environment.GetEnvironmentVariable("TestStorageConnectionString");

                return Task.FromResult(CloudStorageAccount.Parse(connectionString));
            }
        }
    }
}
