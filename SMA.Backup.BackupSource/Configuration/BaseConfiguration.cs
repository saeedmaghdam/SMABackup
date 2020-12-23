using SMA.Backup.BackupSource.Framework;

namespace SMA.Backup.BackupSource.Configuration
{
    public abstract class BaseConfiguration : IBackupSourceConfiguration
    {
        public string Name
        {
            get;
            set;
        }
    }
}
