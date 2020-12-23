using SMA.Backup.Source.Model;
using SMA.Backup.Source.Framework;
using System.Threading.Tasks;
using SMA.Backup.Source.Configuration;

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
            if (configuration is SqlServerConfiguration)
                return _sqlServerSource.Backup(configuration);
            
            if (configuration is MongoDbConfiguration)
                return _mongoDbSource.Backup(configuration);

            return new Task<OutputModel>(SourceNullOutputModel.Instance);
        }
    }
}
