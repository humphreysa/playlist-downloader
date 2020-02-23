# playlist-downloader
Server side application that interacts with the Spotify public API.

## Usage

Configuring dependency injection:

```csharp
// ASP.NET Core DI example
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ISettingsService, EnvironmentVariablesSettings>();
    services.AddSingleton<IStorageService, DictionaryStorage>();
    services.AddSingleton<ITokenService, TokenService>();
    services.AddTransient<ICsvService, CsvService>();
    services.AddTransient<ISpotifyApiFactory, SpotifyApiFactory>();
}
```

Registering a user:

```csharp
// Call this method from your OAuth 2.0 redirect
// https://developer.spotify.com/documentation/general/guides/authorization-guide/
public Task RegisterUser(IStorageService storage, string spotifyUserId, string refreshToken)
{
    return storage.AddOrUpdateUser(spotifyUserId, refreshToken);
}
```

Calling the API:

```csharp
public async Task Example(ISpotifyApiFactory factory, string spotifyUserId)
{
    var api = await factory.GetSpotifyApiForUser(spotifyUserId);

    // Get the users profile
    var profile = await api.GetPrivateProfile();

    Console.WriteLine("Hello {0}", profile.DisplayName);
    
    // Get the users playlists
    var playlists = await api.GetUserPlaylists();

    // Get tracks in a playlist
    var playlist = await api.GetPlaylistTracks(playlists[0].Id);

    // Get the users saved tracks
    var savedTracks = await api.GetUserSavedTracks();

    // Get the users saved albums
    var savedAlbums = await api.GetUserSavedAlbums();
}
```