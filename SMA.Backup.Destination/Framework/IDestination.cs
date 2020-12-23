using System.Threading.Tasks;
using SMA.Backup.Destination.Model;

namespace SMA.Backup.Destination.Framework
{
    public interface IDestination
    {
        Task<OutputModel> Upload(IDestinationConfiguration destinationConfiguration);
    }
}
