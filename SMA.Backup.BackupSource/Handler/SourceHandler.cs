using SMA.Backup.Source.Model;
using SMA.Backup.Source.Framework;
using System.Threading.Tasks;

namespace SMA.Backup.Source.Handler
{
    public class SourceHandler : ISourceHandler
    {
        private readonly ISqlServerSource _sqlServerSource;
        private readonly IMongoDbSource _mongoDbSource;

        public SourceHandler(ISqlServerSource sqlServerSource, IMongoDbSource mongoDbSource)
        {
            _sqlServerSource = sqlServerSource;
            _mongoDbSource = mongoDbSource;
        }

        public Task<OutputModel> CreateBackup(ISourceConfiguration configuration)
        {
            if (configuration is SqlServerSource)
                return _sqlServerSource.Backup(configuration);
            else if (configuration is MongodbBackupSource)
                return _mongoDbSource.Backup(configuration);

            return new Task<OutputModel>(() =>
            {
                return NullOutputModel.Create();
            });
        }
    }
}
