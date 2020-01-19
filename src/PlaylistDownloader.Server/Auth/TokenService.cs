namespace PlaylistDownloader.Server.Auth
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using PlaylistDownloader.Server.Settings;
    using PlaylistDownloader.Server.Storage;

    public class TokenService : ITokenService
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly ConcurrentDictionary<string, CachedToken> cache = new ConcurrentDictionary<string, CachedToken>();
        private readonly IStorageService storageService;
        private readonly ISettingsService settingsService;

        public TokenService(IStorageService storageService, ISettingsService settingsService)
        {
            this.storageService = storageService;
            this.settingsService = settingsService;
        }

        public async Task<string> GetAccessTokenForUser(string spotifyUserId)
        {
            if (cache.TryGetValue(spotifyUserId, out CachedToken cachedToken) && cachedToken.ExpiresAt > DateTime.UtcNow)
            {
                return cachedToken.AccessToken;
            }

            var refreshToken = await storageService.GetRefreshTokenForUser(spotifyUserId);

            var spotifyToken = await ExchangeRefreshTokenForAccess(refreshToken);

            // Cache token for less time than it is valid for so we don't return a token that is about to expire.
            var tokenExpiresAt = DateTime.UtcNow.AddSeconds(spotifyToken.ExpiresIn * 0.8);

            AddOrUpdateCache(spotifyUserId, spotifyToken.AccessToken, tokenExpiresAt);

            return spotifyToken.AccessToken;
        }

        private async Task<SpotifyToken> ExchangeRefreshTokenForAccess(string refreshToken)
        {
            var settings = await settingsService.GetSettings();

            var body = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, settings.SpotifyTokenEndpoint)
            {
                Content = new FormUrlEncodedContent(body)
            };

            request.Headers.Authorization = GetAuthHeader(settings.SpotifyClientId, settings.SpotifyClientSecret);

            var responseMessage = await httpClient.SendAsync(request);

            responseMessage.EnsureSuccessStatusCode();

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SpotifyToken>(content);
        }

        private void AddOrUpdateCache(string spotifyUserId, string accessToken, DateTime tokenExpiresAt)
        {
            cache.AddOrUpdate(spotifyUserId,
                new CachedToken(accessToken, tokenExpiresAt),
                (key, ct) =>
                {
                    ct.AccessToken = accessToken;
                    ct.ExpiresAt = tokenExpiresAt;

                    return ct;
                });
        }

        private static System.Net.Http.Headers.AuthenticationHeaderValue GetAuthHeader(string spotifyClientId, string spotifyClientSecret)
        {
            var bytes = Encoding.UTF8.GetBytes(spotifyClientId + ":" + spotifyClientSecret);

            var base64 = Convert.ToBase64String(bytes);

            return new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64);
        }

        private struct CachedToken
        {
            public string AccessToken;

            public DateTime ExpiresAt;

            public CachedToken(string accessToken, DateTime expiresAt)
            {
                AccessToken = accessToken;
                ExpiresAt = expiresAt;
            }
        }
    }
}