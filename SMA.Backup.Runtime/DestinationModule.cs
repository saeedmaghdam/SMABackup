using Autofac;
using SMA.Backup.Destination;
using SMA.Backup.Destination.Framework;
using SMA.Backup.Destination.Handler;

namespace SMA.Backup.Runtime
{
    public class DestinationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DestinationHandler>().As<IDestinationHandler>().InstancePerLifetimeScope();
            builder.RegisterType<GoogleDriveDestination>().As<IGoogleDriveDestination>().InstancePerLifetimeScope();
            builder.RegisterType<EmailDestination>().As<IEmailDestination>().InstancePerLifetimeScope();
        }
    }
}
