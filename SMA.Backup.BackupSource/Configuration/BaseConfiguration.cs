using SMA.Backup.Source.Framework;

namespace SMA.Backup.Source.Configuration
{
    public abstract class BaseConfiguration : ISourceConfiguration
    {
        public string Name
        {
            get;
            set;
        }
    }
}
