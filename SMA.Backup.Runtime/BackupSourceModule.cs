using Autofac;
using SMA.Backup.Source;
using SMA.Backup.Source.Framework;
using SMA.Backup.Source.Handler;

namespace SMA.Backup.Runtime
{
    public class BackupSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SourceHandler>().As<ISourceHandler>().InstancePerLifetimeScope();
            builder.RegisterType<SqlServerSource>().As<ISqlServerSource>().InstancePerLifetimeScope();
            builder.RegisterType<MongodbBackupSource>().As<IMongoDbSource>().InstancePerLifetimeScope();
        }
    }
}
