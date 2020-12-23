using System.Threading.Tasks;
using SMA.Backup.BackupSource.Model;

namespace SMA.Backup.BackupSource.Framework
{
    public interface IBackupSource
    {
        Task<OutputModel> Backup(IBackupSourceConfiguration backupSourceConfiguration);
    }
}
