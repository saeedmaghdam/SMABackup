namespace SMA.Backup.Destination.Configuration
{
    public class GoogleDriveConfiguration : BaseConfiguration
    {
        public string ClientId
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }

        public string ApplicationName
        {
            get;
            set;
        }
    }
}
