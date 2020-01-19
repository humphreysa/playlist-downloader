namespace PlaylistDownloader.Server.Csv
{
    using CsvHelper.Configuration;
    using PlaylistDownloader.Server.Api.Model;
    
    internal class TrackMap : ClassMap<Track>
    {
        public TrackMap()
        {
            Map(m => m.AddedAt).Index(0);
            Map(m => m.Name).Index(1);
            Map(m => m.Artists).ConvertUsing(a => string.Join(", ", a.Artists)).Index(2);
            Map(m => m.Album).Index(3);
            Map(m => m.DurationMs).Index(4);
        }
    }
}