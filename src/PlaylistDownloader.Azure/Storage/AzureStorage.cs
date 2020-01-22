namespace PlaylistDownloader.Azure.Storage
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Server.Storage;

    public class AzureStorage : IStorageService
    {
        private const string UsersTableName = "SpotifyUsers";
        private const string UsersTablePartitionKey = "USER";

        private readonly ICloudStorageAccountProvider cloudStorageAccountProvider;

        public bool IsPersistent => true;

        public AzureStorage(ICloudStorageAccountProvider cloudStorageAccountProvider)
        {
            this.cloudStorageAccountProvider = cloudStorageAccountProvider;
        }

        public async Task AddOrUpdateUser(string spotifyUserId, string refreshToken)
        {
            var cloudTable = await GetCloudTableClient(UsersTableName);

            var entity = new SpotifyUserEntity(UsersTablePartitionKey, spotifyUserId, refreshToken);

            var operation = TableOperation.InsertOrMerge(entity);

            await cloudTable.ExecuteAsync(operation);
        }

        public async Task<string> GetRefreshTokenForUser(string spotifyUserId)
        {
            var cloudTable = await GetCloudTableClient(UsersTableName);

            var operation = TableOperation.Retrieve<SpotifyUserEntity>(UsersTablePartitionKey, spotifyUserId);

            var result = await cloudTable.ExecuteAsync(operation);

            if (result.Result is null)
            {
                throw new Exception("User id not added. AddOrUpdateUser(...) must be called first.");
            }
            
            var userEntity = result.Result as SpotifyUserEntity;

            return userEntity.RefreshToken;
        }

        private async Task<CloudTable> GetCloudTableClient(string tableName)
        {
            var account = await cloudStorageAccountProvider.GetCloudStorageAccount();

            var tableClient = account.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}