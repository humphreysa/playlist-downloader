namespace PlaylistDownloader.Server.Api
{
    using System.Threading.Tasks;

    public interface ISpotifyApiFactory
    {
        Task<ISpotifyApi> GetSpotifyApiForUser(string spotifyId);
    }
}