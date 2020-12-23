using Autofac;
using SMA.Backup.Common;

namespace SMA.Backup.Runtime
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SystemConfiguration>().As<ISystemConfiguration>().InstancePerLifetimeScope();
        }
    }
}
