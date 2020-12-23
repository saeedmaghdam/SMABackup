using Autofac;
using SMA.Backup.Source;
using SMA.Backup.Source.Framework;

namespace SMA.Backup.Runtime
{
    public class BackupSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SqlServerSource>().As<ISqlServerSource>().InstancePerLifetimeScope();
            builder.RegisterType<MongodbBackupSource>().As<IMongoDbSource>().InstancePerLifetimeScope();
        }
    }
}
