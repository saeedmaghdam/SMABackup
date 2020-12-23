using SMA.Backup.Destination.Framework;
using SMA.Backup.Destination.Model;
using System.Threading.Tasks;

namespace SMA.Backup.Destination.Handler
{
    public class DestinationHandler : IDestinationHandler
    {
        private readonly IGoogleDriveDestination _googleDriveDestination;

        public DestinationHandler(IGoogleDriveDestination googleDriveDestination)
        {
            _googleDriveDestination = googleDriveDestination;
        }

        public Task<OutputModel> CopyBackup(IDestinationConfiguration configuration)
        {
            if (configuration is GoogleDriveDestination)
                return _googleDriveDestination.Upload(configuration);

            return new Task<OutputModel>(() =>
            {
                return new NullOutputModel();
            });
        }
    }
}
