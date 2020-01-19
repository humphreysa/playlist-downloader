namespace PlaylistDownloader.Server.Settings
{
    using System;
    using System.Threading.Tasks;

    public class EnvironmentVariablesSettings : ISettingsService
    {
        private readonly Lazy<Settings> settings = new Lazy<Settings>(() => new Settings
        {
            SpotifyClientId = GetAndValidateEnvironmentVariable("SPOTIFY_CLIENTID"),
            SpotifyClientSecret = GetAndValidateEnvironmentVariable("SPOTIFY_CLIENTSECRET")
        });

        public Task<Settings> GetSettings()
        {
            return Task.FromResult(settings.Value);
        }

        private static string GetAndValidateEnvironmentVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"Environment variable {name} has not been set.");
            }

            return value;
        }
    }
}