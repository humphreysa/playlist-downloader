namespace PlaylistDownloader.Server.Api.Model
{
    using System;

    public class Track
    {
        public DateTime AddedAt { get; set; }

        public int TrackNumber { get; set; }

        public string Name { get; set; }

        public int DurationMs { get; set; }

        public int DiscNumber { get; set; }

        public string[] Artists { get; set; }

        public string Album { get; set; }
    }
}