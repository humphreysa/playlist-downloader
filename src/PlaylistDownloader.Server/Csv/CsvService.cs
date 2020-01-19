namespace PlaylistDownloader.Server.Csv
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CsvHelper;
    using CsvHelper.Configuration;
    using PlaylistDownloader.Server.Api;

    public class CsvService : ICsvService
    {
        private static readonly Configuration csvHelperConfiguration = GetCsvHelperConfiguration();
        
        private readonly ISpotifyApiFactory spotifyApiFactory;

        public CsvService(ISpotifyApiFactory spotifyApiFactory)
        {
            this.spotifyApiFactory = spotifyApiFactory;
        }

        public async Task<byte[]> GetPlaylistTracksCsv(string spotifyUserId, string playlistId)
        {
            var api = await spotifyApiFactory.GetSpotifyApiForUser(spotifyUserId);

            var playListTracks = await api.GetPlaylistTracks(playlistId);

            return ConvertToCsv(playListTracks);
        }

        public async Task<byte[]> GetUserSavedTracksCsv(string spotifyUserId)
        {
            var api = await spotifyApiFactory.GetSpotifyApiForUser(spotifyUserId);

            var savedTracks = await api.GetUserSavedTracks();

            return ConvertToCsv(savedTracks);
        }

        private byte[] ConvertToCsv<T>(IEnumerable<T> records)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var csvWriter = new CsvWriter(writer, csvHelperConfiguration))
            {
                csvWriter.WriteRecords(records);

                writer.Flush();

                return stream.ToArray();
            }
        }

        private static Configuration GetCsvHelperConfiguration()
        {
            var configuration = new Configuration();

            configuration.RegisterClassMap<TrackMap>();

            return configuration;
        }
    }
}