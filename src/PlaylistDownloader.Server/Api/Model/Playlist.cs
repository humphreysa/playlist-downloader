namespace PlaylistDownloader.Server.Api.Model
{
    public class Playlist
    {
        public string Name { get; set; }

        public string OwnerDisplayName { get; set; }

        public int Tracks { get; set; }

        public string Id { get; set; }

        public string[] Images { get; set; }
    }
}