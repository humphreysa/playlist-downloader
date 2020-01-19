namespace PlaylistDownloader.Server.Csv
{
    using System.Threading.Tasks;

    public interface ICsvService
    {
        Task<byte[]> GetUserSavedTracksCsv(string spotifyUserId);

        Task<byte[]> GetPlaylistTracksCsv(string spotifyUserId, string playlistId);
    }
}