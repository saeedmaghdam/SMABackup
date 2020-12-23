using Autofac;
using SMA.Backup.Destination;
using SMA.Backup.Destination.Framework;

namespace SMA.Backup.Runtime
{
    public class DestinationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<GoogleDriveDestination>().As<IGoogleDriveDestination>().InstancePerLifetimeScope();
        }
    }
}
