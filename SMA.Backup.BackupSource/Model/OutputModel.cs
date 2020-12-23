using System;

namespace SMA.Backup.BackupSource.Model
{
    public class OutputModel
    {
        public DateTime FileCreationDate
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string FileExtension
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public long FileSize
        {
            get;
            set;
        }

        public string FileHash
        {
            get;
            set;
        }
    }
}
