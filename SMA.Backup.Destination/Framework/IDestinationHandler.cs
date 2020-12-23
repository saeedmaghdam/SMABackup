using SMA.Backup.Destination.Model;
using System.Threading.Tasks;

namespace SMA.Backup.Destination.Framework
{
    public interface IDestinationHandler
    {
        Task<OutputModel> CopyBackup(IDestinationConfiguration configuration);
    }
}
