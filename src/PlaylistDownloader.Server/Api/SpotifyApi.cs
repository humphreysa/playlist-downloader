namespace PlaylistDownloader.Server.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SpotifyAPI.Web;
    using SpotifyAPI.Web.Models;

    public class SpotifyApi : ISpotifyApi
    {
        private const int defaultPageSize = 50;

        private readonly string spotifyUserId;
        private readonly SpotifyWebAPI spotifyWebApi;

        public SpotifyApi(string spotifyUserId, SpotifyWebAPI spotifyWebApi)
        {
            this.spotifyUserId = spotifyUserId;
            this.spotifyWebApi = spotifyWebApi;
        }

        public async Task<Model.Track[]> GetPlaylistTracks(string playlistId)
        {
            var playlistTracks = await PageAllResults(() => Task.FromResult(spotifyWebApi.GetPlaylistTracks(playlistId)));

            return playlistTracks.Select(p => new Model.Track
            {
                AddedAt = p.AddedAt,
                Album = p.Track.Album.Name,
                Artists = p.Track.Artists.Select(a => a.Name).ToArray(),
                DiscNumber = p.Track.DiscNumber,
                DurationMs = p.Track.DurationMs,
                Name = p.Track.Name,
                TrackNumber = p.Track.TrackNumber
            }).ToArray();
        }

        public async Task<Model.PrivateProfile> GetPrivateProfile()
        {
            var privateProfile = await spotifyWebApi.GetPrivateProfileAsync();

            return new Model.PrivateProfile
            {
                DisplayName = privateProfile.DisplayName,
                Id = privateProfile.Id
            };
        }

        public async  Task<Model.Playlist[]> GetUserPlaylists()
        {
            var playlists = await PageAllResults(() => spotifyWebApi.GetUserPlaylistsAsync(spotifyUserId, defaultPageSize));

            return playlists.Select(p => new Model.Playlist
            {
                Id = p.Id,
                Name = p.Name,
                OwnerDisplayName = p.Owner.DisplayName,
                Tracks = p.Tracks.Total,
                Images = p.Images.Select(i => i.Url).ToArray()
            }).ToArray();
        }

        public async Task<Model.SavedAlbum[]> GetUserSavedAlbums()
        {
            var savedAlbums = await PageAllResults(() => spotifyWebApi.GetSavedAlbumsAsync(defaultPageSize));

            return savedAlbums.Select(s => new Model.SavedAlbum
            {
                AddedAt = s.AddedAt,
                Artists = s.Album.Artists.Select(a => a.Name).ToArray(),
                Genres = s.Album.Genres.ToArray(),
                Label = s.Album.Label,
                Name = s.Album.Name
            }).ToArray();
        }

        public async Task<Model.Track[]> GetUserSavedTracks()
        {
            var savedTracks = await PageAllResults(() => spotifyWebApi.GetSavedTracksAsync(defaultPageSize));

            return savedTracks.Select(s => new Model.Track
            {
                AddedAt = s.AddedAt,
                Album = s.Track.Album.Name,
                Artists = s.Track.Artists.Select(a => a.Name).ToArray(),
                DiscNumber = s.Track.DiscNumber,
                DurationMs = s.Track.DurationMs,
                Name = s.Track.Name,
                TrackNumber = s.Track.TrackNumber
            }).ToArray();
        }

        private async Task<T[]> PageAllResults<T>(Func<Task<Paging<T>>> factory)
        {
            var results = new List<T>();

            var apiResponse = await factory();

            do
            {
                results.AddRange(apiResponse.Items);

                if (apiResponse.HasNextPage())
                {
                    apiResponse = await spotifyWebApi.GetNextPageAsync(apiResponse);
                }
            } while (apiResponse.HasNextPage());

            return results.ToArray();
        }
    }
}