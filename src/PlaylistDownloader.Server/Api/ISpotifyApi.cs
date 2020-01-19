namespace PlaylistDownloader.Server.Api
{
    using System.Threading.Tasks;
    using PlaylistDownloader.Server.Api.Model;

    public interface ISpotifyApi
    {
        Task<Playlist[]> GetUserPlaylists();

        Task<PrivateProfile> GetPrivateProfile();

        Task<Track[]> GetPlaylistTracks(string playlistId);

        Task<Track[]> GetUserSavedTracks();

        Task<SavedAlbum[]> GetUserSavedAlbums();
    }
}