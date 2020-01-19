namespace PlaylistDownloader.Server.Settings
{
    public class Settings
    {
        public string SpotifyClientId { get; set; }

        public string SpotifyClientSecret { get; set; }

        public string SpotifyTokenEndpoint => "https://accounts.spotify.com/api/token";

        public string SpotifyAuthorizationEndpoint => "https://accounts.spotify.com/authorize";

        public string SpotifyUserInformationEndpoint => "https://api.spotify.com/v1/me";
    }
}