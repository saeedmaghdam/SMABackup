using SMA.Backup.BackupSource.Framework;

namespace SMA.Backup.BackupSource.Configuration
{
    public class SqlServerConfiguration : BaseConfiguration
    {
        public string ServerName
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

        public string DatabaseName
        {
            get;
            set;
        }
    }
}
