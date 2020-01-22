namespace PlaylistDownloader.Azure.Storage
{
    using Microsoft.Azure.Cosmos.Table;

    internal class SpotifyUserEntity : TableEntity
    {
        public SpotifyUserEntity()
        {
        }

        public SpotifyUserEntity(string partitionKey, string spotifyUserId, string refreshToken)
        {
            PartitionKey = partitionKey;
            RowKey = spotifyUserId;
            SpotifyUserId = spotifyUserId;
            RefreshToken = refreshToken; 
        }

        public string SpotifyUserId { get; set; }

        public string RefreshToken { get; set; }
    }
}