using SMA.Backup.BackupSource.Framework;

namespace SMA.Backup.BackupSource.Configuration
{
    public class MongoDbConfiguration : BaseConfiguration
    {
        public string HostName
        {
            get;
            set;
        }

        public int? Port
        {
            get;
            set;
        }

        public IAuthenticationModel AuthenticationModel
        {
            get;
            set;
        }

        public string CollectionName
        {
            get;
            set;
        }
    }
}
