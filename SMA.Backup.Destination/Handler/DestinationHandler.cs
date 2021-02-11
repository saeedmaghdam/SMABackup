using SMA.Backup.Destination.Framework;
using SMA.Backup.Destination.Model;
using System.Threading.Tasks;
using SMA.Backup.Destination.Configuration;

namespace SMA.Backup.Destination.Handler
{
    public class DestinationHandler : IDestinationHandler
    {
        private readonly IGoogleDriveDestination _googleDriveDestination;
        private readonly IEmailDestination _emailDestination;

        public DestinationHandler(IGoogleDriveDestination googleDriveDestination, IEmailDestination emailDestination)
        {
            _googleDriveDestination = googleDriveDestination;
            _emailDestination = emailDestination;
        }

        public async Task<OutputModel> CopyBackup(IDestinationConfiguration configuration)
        {
            if (configuration is GoogleDriveConfiguration)
                return await _googleDriveDestination.Upload(configuration);
            else if (configuration is EmailConfiguration)
                return await _emailDestination.Upload(configuration);

            return new Task<OutputModel>(() => new NullOutputModel()).Result;
        }
    }
}
