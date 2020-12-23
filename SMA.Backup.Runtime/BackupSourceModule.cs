using Autofac;
using SMA.Backup.BackupSource;
using SMA.Backup.BackupSource.Framework;

namespace SMA.Backup.Runtime
{
    public class BackupSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SqlServerBackupSource>().As<ISqlServerBackupSource>().InstancePerLifetimeScope();
            builder.RegisterType<MongodbBackupSource>().As<IMongoDbBackupSource>().InstancePerLifetimeScope();
        }
    }
}
