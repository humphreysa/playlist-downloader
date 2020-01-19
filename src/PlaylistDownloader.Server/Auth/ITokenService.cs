namespace PlaylistDownloader.Server.Auth
{
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<string> GetAccessTokenForUser(string spotifyUserId);
    }
}