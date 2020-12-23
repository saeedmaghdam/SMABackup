using SMA.Backup.Source.Model;
using System.Threading.Tasks;

namespace SMA.Backup.Source.Framework
{
    public interface ISource
    {
        Task<OutputModel> Backup(ISourceConfiguration backupSourceConfiguration);
    }
}
