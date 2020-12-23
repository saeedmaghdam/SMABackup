using SMA.Backup.Model;
using System.Threading.Tasks;

namespace SMA.Backup.Framework
{
    public interface IBackupHandler
    {
        Task<OutputModel> Backup(string configFile);
    }
}
