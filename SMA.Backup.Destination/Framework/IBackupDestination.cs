using System.Threading.Tasks;
using SMA.Backup.Destination.Model;

namespace SMA.Backup.Destination.Framework
{
    public interface IBackupDestination
    {
        Task<OutputModel> Upload(IBackupDestinationConfiguration destinationConfiguration);
    }
}
