namespace PlaylistDownloader.Server.Settings
{
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task<Settings> GetSettings();
    }
}