namespace SMA.Backup.Destination.Framework
{
    public interface IBackupDestinationConfiguration
    {
        string Name
        {
            get;
            set;
        }

        string FileName
        {
            get;
            set;
        }
    }
}
