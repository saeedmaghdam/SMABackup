using SMA.Backup.Source.Framework;

namespace SMA.Backup.Source.Configuration
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
