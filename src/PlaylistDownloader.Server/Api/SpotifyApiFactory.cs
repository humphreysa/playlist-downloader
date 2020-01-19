namespace PlaylistDownloader.Server.Api
{
    using System.Threading.Tasks;
    using PlaylistDownloader.Server.Auth;
    using SpotifyAPI.Web;

    public class SpotifyApiFactory : ISpotifyApiFactory
    {
        private readonly ITokenService tokenService;

        public SpotifyApiFactory(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<ISpotifyApi> GetSpotifyApiForUser(string spotifyId)
        {
            var api = new SpotifyWebAPI
            {
                AccessToken = await tokenService.GetAccessTokenForUser(spotifyId),
                TokenType = "Bearer"
            };

            return new SpotifyApi(spotifyId, api);
        }
    }
}