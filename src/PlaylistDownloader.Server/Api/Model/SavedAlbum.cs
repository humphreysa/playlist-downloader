namespace PlaylistDownloader.Server.Api.Model
{
    using System;

    public class SavedAlbum
    {
        public DateTime AddedAt { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string[] Genres { get; set; }

        public string[] Artists { get; set; }
    }
}