namespace PlaylistDownloader.Server.Storage
{
    using System.Threading.Tasks;

    public interface IStorageService
    {
        bool IsPersistent { get; }

        Task AddOrUpdateUser(string spotifyUserId, string refreshToken);

        Task<string> GetRefreshTokenForUser(string spotifyUserId);
    }
}