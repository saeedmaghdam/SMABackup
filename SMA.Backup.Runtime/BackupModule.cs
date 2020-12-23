using Autofac;
using SMA.Backup.Framework;

namespace SMA.Backup.Runtime
{
    public class BackupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<BackupHandler>().As<IBackupHandler>().InstancePerLifetimeScope();
        }
    }
}
