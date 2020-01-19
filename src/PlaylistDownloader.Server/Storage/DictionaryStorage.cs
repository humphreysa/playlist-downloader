namespace PlaylistDownloader.Server.Storage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DictionaryStorage : IStorageService
    {
        private readonly Dictionary<string, string> state = new Dictionary<string, string>();

        public bool IsPersistent => false;

        public Task AddOrUpdateUser(string spotifyUserId, string refreshToken)
        {
            state[spotifyUserId] = refreshToken;

            return Task.CompletedTask;
        }

        public Task<string> GetRefreshTokenForUser(string spotifyUserId)
        {
            if (state.TryGetValue(spotifyUserId, out string refreshToken))
            {
                return Task.FromResult(refreshToken);
            }
            else
            {
                throw new System.Exception("User id not added. AddOrUpdateUser(...) must be called first.");
            }
        }
    }
}