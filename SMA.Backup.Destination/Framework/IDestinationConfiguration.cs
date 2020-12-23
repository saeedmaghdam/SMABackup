namespace SMA.Backup.Destination.Framework
{
    public interface IDestinationConfiguration
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
