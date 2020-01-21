namespace PlaylistDownloader.Azure.Storage
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;

    public interface ICloudStorageAccountProvider
    {
        Task<CloudStorageAccount> GetCloudStorageAccount();
    }
}