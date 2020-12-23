using SMA.Backup.Source.Model;
using System.Threading.Tasks;

namespace SMA.Backup.Source.Framework
{
    public interface ISourceHandler
    {
        Task<OutputModel> CreateBackup(ISourceConfiguration configuration);
    }
}
