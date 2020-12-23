namespace SMA.Backup.Common
{
    public class SystemConfiguration : ISystemConfiguration
    {
        public string BackupPath
        {
            get;
        }

        public SystemConfiguration()
        {
            BackupPath = "C:\\SMABackupService"; // Static configuration
        }
    }
}
