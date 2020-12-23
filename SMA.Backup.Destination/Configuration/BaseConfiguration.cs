using SMA.Backup.Destination.Framework;

namespace SMA.Backup.Destination.Configuration
{
    public abstract class BaseConfiguration : IDestinationConfiguration
    {
        public string Name
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }
    }
}
